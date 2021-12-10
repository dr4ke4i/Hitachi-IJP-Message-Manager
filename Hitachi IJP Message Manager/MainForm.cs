using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using Modbus_DLL;

namespace Hitachi_IJP_Message_Manager
{
    public partial class MainForm : Form
    {
        CSettings Settings;
        CBatch Batch;
        CBatch NextBatch;
        CPrinter Printer;
        CLog Log;
        public bool HoldOn = true;
        public bool MaintenanceOn = false;

        public class CWindowForm
        {
            public int FullWidth;
            public int SmallWidth;
            public int Height;
            public bool AppStarting;
        }
        private CWindowForm MyForm;

        public MainForm()
        {
            InitializeComponent();

            MyForm = new CWindowForm();
            MyForm.FullWidth = this.Width;
            MyForm.SmallWidth = MyForm.FullWidth * 3 / 4;
            MyForm.Height = this.Height;
            this.AutoScaleMode = AutoScaleMode.Dpi;
            MyForm.AppStarting = true;

            Log = new CLog(@".\log.txt.csv");
            Log.AddLine("");

            Settings = new CSettings();

            if (Settings.GetLastError != CSettings.Error.OK)
            {
                Log.AddObject(Enum.GetName(typeof(CSettings.Error), Settings.GetLastError));
                AddListBoxLine(Enum.GetName(typeof(CSettings.Error), Settings.GetLastError));
                MessageBox.Show(Enum.GetName(typeof(CSettings.Error), Settings.GetLastError), "Ошибка чтения настроек");
            }
            LogAddCheckpoint(null, Settings, null, null, _outputlistbox: true);

            Printer = new CPrinter(this, Settings.IPAddr, Settings.Port);

            Batch = new CBatch(Settings.CurrentBatchStartCounter, Settings.CurrentBatchLastKnownCounter, Settings.CurrentBatchMaintBags,
                Settings.CurrentBatchBrokenBags, Settings.CurrentBatchVolume, Settings.CurrentBatchStartDate, Settings.CurrentBatchLot);
            NextBatch = new CBatch(Settings.NextBatchStartCounter, Settings.NextBatchLastKnownCounter, Settings.NextBatchMaintBags,
                Settings.NextBatchBrokenBags, Settings.NextBatchVolume, Settings.NextBatchStartDate, Settings.NextBatchLot);
            if (Settings.FormAutoLot == true)
                NextBatch.Data.Info.Lot = Batch.Data.Info.Lot + 1;
            if (Settings.FormAutoDate == true)
                NextBatch.Data.Info.StartDate = System.DateTime.Now;

            List<int> cmbox_list = new List<int>();
            cmbox_list.Add(Batch.Delta.Volume);
            if (Batch.Delta.Volume != NextBatch.Delta.Volume)
                cmbox_list.Add(NextBatch.Delta.Volume);

            foreach (int _value in Settings.PossibleVolumeList)
            {
                bool duplicate = false;
                foreach (int _item in cmbox_list)
                {
                    if (_item == _value)
                        duplicate = true;
                }
                if (!duplicate)
                    cmbox_list.Add(_value);
                Batch.Data.AddToPossibleVolumeList(_value);
                NextBatch.Data.AddToPossibleVolumeList(_value);
            }
            cmbox_list.Sort();

            foreach (int _value in cmbox_list)
            {
                cmboxCurrentBatchVolume.Items.Add(_value);
                cmboxNextBatchVolume.Items.Add(_value);
            }
            cmboxCurrentBatchVolume.SelectedIndex = ClosestIndex(cmboxCurrentBatchVolume.Items, Batch.Delta.Volume);
            cmboxNextBatchVolume.SelectedIndex = ClosestIndex(cmboxNextBatchVolume.Items, NextBatch.Delta.Volume);

            chboxBatchDateAUTO.Checked = Settings.FormAutoDate;
            chboxBatchLotNoAutoINC.Checked = Settings.FormAutoLot;
            chboxConnectOnLaunch.Checked = Settings.FormConnectOnLaunch;

            btnDisconnect.Enabled = false;

            HoldOn = true;
            MaintenanceOn = false;
            MyForm.AppStarting = false;
            UpdateComponents();
            this.Width = MyForm.SmallWidth;
        }

        public class CTimerData
        {
            public long startmillis;
            public long millis;
            public long cyclemillis;
            public UInt16 update_switch = 1;
            public int lastknowncounter = -1;
            public bool running = false;
        }
        private CTimerData tmr = new CTimerData();

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            if (tmr.running)
                return;
            tmr.running = true;

            tmr.startmillis = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            if (MaintenanceOn)
            {
                Printer.ReloadOnlineStatus();
            }
            else if (Printer.ReloadOnlineStatus() && Printer.IsOnline)
            {
                Printer.ReloadCounter();
                if (tmr.lastknowncounter != Printer.Counter)
                {
                    if (Printer.Counter > tmr.lastknowncounter + 1)
                    {
                        AddLineToLogs($"ВНИМАНИЕ: пропуск более одного мешка в цикле опроса счётчика принтера.");
                        AddLineToLogs($"tmr.lastknowncounter={tmr.lastknowncounter}; Printer.Counter={Printer.Counter}", _outputlistbox: true);
                    }
                }

                tmr.lastknowncounter = Printer.Counter;

                if (HoldOn)
                {
                    Printer.ReloadOnlineStatus();
                    Printer.ReloadStatus();
                    Printer.ReloadString();
                    if (tmr.update_switch % 19 == 0) Printer.ReloadCalendarTime();
                    if (tmr.update_switch % 49 == 0) Printer.SetCurrentTime(DateTime.Now);
                    tmr.update_switch++;
                }
                else //!HoldOn
                {
                    if (Batch.IsFinished(Printer.Counter))
                    {
                        if (Batch.IsOverthrown())
                        {
                            // Printer comms error. Unfortunatelly, can't cause printer error to stop the line
                            AddLineToLogs($"Ошибка! Счётчик принтера Printer.Counter={Printer.Counter} перескочил за конец партии Batch.EndCounter={Batch.EndCounter}",
                                            _outputlistbox: true);
                            // TODO: MessageBox
                        }
                        // Start next Batch normally
                        Batch.Data.Info.Lot = NextBatch.Data.Info.Lot;
                        Batch.Data.Info.StartDate = NextBatch.Data.Info.StartDate;
                        StartNewBatch();
                        // NextBatch should be modified on next UpdateComponents() call
                    }
                    else
                    {
                        // do some things in freetime
                        tmr.cyclemillis = DateTimeOffset.Now.ToUnixTimeMilliseconds() - tmr.startmillis;
                        if (tmr.cyclemillis <= tmrUpdate.Interval / 2)
                        {
                            if (tmr.update_switch % 5 == 0)
                            {
                                Printer.ReloadString();
                            }
                            else if (tmr.update_switch % 19 == 0)
                            {
                                Printer.ReloadCalendarTime();
                            }
                            else
                            {
                                Printer.ReloadStatus();
                                //Printer.ReloadOnlineStatus();
                            }
                            tmr.update_switch++;
                        }
                        else
                        {
                            AddLineToLogs($"Предупреждение: время цикла превысило половину цикла опроса. tmr.cyclemillis={tmr.cyclemillis}; tmrUpdate.Interval={tmrUpdate.Interval}",
                                          _outputlistbox: true);
                        }
                    }
                }
            }
            else  //(!Printer.IsOnline)
            {
                bool result = Printer.SetOnline();
                result &= Printer.ReloadOnlineStatus();
                AddLineToLogs($"Принтер в режиме оффлайн. HoldOn={HoldOn}; Printer.IsOnline={Printer.IsOnline}. Переподключение... {(result ? "Успех" : "Ошибка")}",
                              _outputlistbox: true);
            }
            tmr.cyclemillis = DateTimeOffset.Now.ToUnixTimeMilliseconds() - tmr.startmillis;
            tmr.running = false;
            AddLineToLogs(tmr.cyclemillis + ", ms", _outputlistbox: false);
            UpdateComponents();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            AddLineToLogs("Приложение запущено", _outputlistbox: true);
            if (Settings.FormConnectOnLaunch)
            {
                AddLineToLogs("Подключение при старте приложения (активно)", _outputlistbox: true);
                ConnectToPrinter();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            AddLineToLogs($"Приложение закрывается", _outputlistbox: true);
            bool result = Printer.IsConnected && Printer.IsOnline;
            if (result)
            {
                result &= Printer.End();
                AddLineToLogs($"Принтер - отключение: {(result ? "успех" : "ошибка")}", _outputlistbox: true);
            }
            //SaveSettingsToFile();
            Log.CloseFile();
        }


        #region Service Routines

        private int ClosestIndex(ComboBox.ObjectCollection _items, int _value)
        {
            int item_value;
            int index_min = 0;
            int delta_min = int.MaxValue;
            bool check = true;
            for (int i = 0; i < _items.Count; i++)
            {
                check &= int.TryParse(_items[i].ToString(), out item_value);
                if (check)
                {
                    if (delta_min > Math.Abs(item_value - _value))
                    {
                        index_min = i;
                        delta_min = Math.Abs(item_value - _value);
                    }
                }
                else
                    return 0;
            }
            return index_min;
        }

        private void UpdateComponents()
        {
            btnStartMaintenance.Text = !MaintenanceOn ? "Начать тех.обслуживание" : "Завершить тех.обслуживание";
            if (HoldOn || !Printer.Status.StatusReady) lblPrinterHoldOnActive.Visible = true; else lblPrinterHoldOnActive.Visible = false;

            lblMarkedBags.Text = Convert.ToString(Printer.Counter - Batch.StartCounter - Batch.Delta.Maint - Batch.Delta.Broken);
            lblVolume.Text = Batch.Delta.Volume.ToString();
            lblBrokenBags.Text = Batch.Delta.Broken.ToString();
            lblMaintBags.Text = Batch.Delta.Maint.ToString();

            // Current batch
            lblBatchLotNo.Text = Batch.Data.Info.LotStr;
            lblMarkedBagsCurrB.Text = Convert.ToString(Batch.LastKnownCounter - Batch.StartCounter - Batch.Delta.Maint - Batch.Delta.Broken);
            lblVolumeCurrB.Text = Batch.Delta.Volume.ToString();
            lblBrokenBagsCurrB.Text = Batch.Delta.Broken.ToString();
            lblMaintBagsCurrB.Text = Batch.Delta.Maint.ToString();
            lblBatchDate.Text = Batch.Data.Info.StartDateStr;

            // Next batch
            lblBatchNextLotNo.Text = NextBatch.Data.Info.LotStr;
            lblMarkedBagsNextB.Text = Convert.ToString(NextBatch.LastKnownCounter - NextBatch.StartCounter - NextBatch.Delta.Maint - NextBatch.Delta.Broken);
            lblVolumeNextB.Text = NextBatch.Delta.Volume.ToString();
            lblBrokenBagsNextB.Text = NextBatch.Delta.Broken.ToString();
            lblMaintBagsNextB.Text = NextBatch.Delta.Maint.ToString();
            if (chboxBatchDateAUTO.Checked) NextBatch.Data.Info.StartDate = System.DateTime.Now;
            lblBatchNextDate.Text = NextBatch.Data.Info.StartDateStr;

            if (Printer != null && Printer.PCString.Value != null)
            {
                tboxPrinterPCString.Text = $"[{Printer.PCString.Length}]: {Printer.PCString.Value}";
                lblPCStringLastUpdateTime.Text = Printer.PCString.LastUpdate.ToString();
            }

            lbl_IP.Text = "IP: " + Printer.Settings.IP;
            lbl_Port.Text = "Port: " + Printer.Settings.Port.ToString();

            lblBatchStartCounter.Text = Batch.StartCounter.ToString();
            lblPrinterPrintCount.Text = Printer.Counter.ToString();
            lblBatchEndCounter.Text = Batch.EndCounter.ToString();

            if (Printer != null)
            {
                if ((!Printer.IsConnected) || (!Printer.IsOnline))
                {
                    lblPrinterStatusOnline.Text = "X"; lblPrinterStatusOnline.ForeColor = Label.DefaultForeColor;
                    lblPrinterStatusErrorText.Text = "X"; lblPrinterStatusErrorText.ForeColor = Label.DefaultForeColor;
                    lblPrinterStatusWarningText.Text = "X"; lblPrinterStatusWarningText.ForeColor = Label.DefaultForeColor;
                    lblPrinterRemoteOperation.Text = "X"; lblPrinterRemoteOperation.ForeColor = Label.DefaultForeColor;
                    lblPrinterStatusClockStopped.Text = "X"; lblPrinterStatusClockStopped.ForeColor = Label.DefaultForeColor;
                }

                if (Printer.IsConnected)
                {
                    if (Printer.IsOnline)
                    {
                        lblPrinterStatusOnline.Text = Printer.Status.Online ? "OK" : "нет";
                        lblPrinterStatusOnline.ForeColor = Printer.Status.Online ? Color.Green : Color.Red;
                        lblPrinterStatusErrorText.Text = Printer.Status.StatusString + " (0x" + Printer.Status.StatusRaw.ToString("X") + ")";
                        lblPrinterStatusErrorText.ForeColor = Printer.Status.StatusReady ? Color.Green : Color.Red;
                        lblPrinterStatusWarningText.Text = Printer.Status.WarningString + " (0x" + Printer.Status.WarningRaw.ToString("X") + ")";
                        lblPrinterStatusWarningText.ForeColor = Printer.Status.WarningNoAlarms ? Color.Green : Color.Red;
                        lblPrinterRemoteOperation.Text = Printer.Status.RemoteOperationString + " (" + Printer.Status.RemoteOperationRAW.ToString() + ")";
                        lblPrinterRemoteOperation.ForeColor = Printer.Status.RemoteOperationRAW == 0x00 ? Color.Green : Color.Red;
                        lblPrinterStatusClockStopped.Text = Printer.Status.ClockStoppedString;
                        lblPrinterStatusClockStopped.ForeColor = Printer.Status.ClockStoppedRAW ? Color.Red : Color.Green;
                    }

                    // This status can be read regardless if we're online or not
                    lblPrinterOnlineOffline.Text = Printer.IsOnline ? "OK" : "нет";
                    lblPrinterOnlineOffline.ForeColor = Printer.IsOnline ? Color.Green : Color.Red;
                }
                else
                {
                    lblPrinterOnlineOffline.Text = "X"; lblPrinterOnlineOffline.ForeColor = Label.DefaultForeColor;
                }
            }

            lblProgramHoldOn.Text = HoldOn ? "да" : "нет"; lblProgramHoldOn.ForeColor = HoldOn ? Color.Red : Color.Green;

            lblPrinterACKs.Text = Printer.Status.ACKs.ToString();
            lblPrinterNACKs.Text = Printer.Status.NACKs.ToString();
            lblLastCycleMillis.Text = tmr.cyclemillis.ToString();

            if (!Printer.IsConnected || HoldOn || MaintenanceOn || !Printer.IsOnline)
            {
                btnReWriteStringToPrinter.Enabled = false;
                btnSetRemoteOperationStart.Enabled = false;
                btnSetRemoteOperationStop.Enabled = false;
                btnStartMaintenance.Enabled = (MaintenanceOn && Printer.IsConnected) ? true : false;
                btnStartNewBatch.Enabled = false;
                btnStopCalendarTimeResume.Enabled = false;
                btnStorePrintingMessageIntoMemory.Enabled = false;
            }
            else    // Printer.IsConnected && !HoldOn && !MaintenanceOn && Printer.IsOnline
            {
                btnReWriteStringToPrinter.Enabled = true;
                btnSetRemoteOperationStart.Enabled = true;
                btnSetRemoteOperationStop.Enabled = true;
                btnStartMaintenance.Enabled = true;
                btnStartNewBatch.Enabled = true;
                btnStopCalendarTimeResume.Enabled = true;
                btnStorePrintingMessageIntoMemory.Enabled = true;
            }
        }

        private string GeneratePrintString(CBatch _batch)
        {
            if (_batch.Data.Info.Lot <= 999)
            {
                return $"DATE:{_batch.Data.Info.StartDateStr} LOT:{_batch.Data.Info.Lot.ToString("000")}";
            }
            else
            {
                return $"DATE:{_batch.Data.Info.StartDateStr} LOT:{_batch.Data.Info.Lot.ToString("0000")}";
            }
        }

        private void AddLineToLogs(string _newline, bool _outputlistbox = false)
        {
            if (_outputlistbox)
                AddListBoxLine(_newline);
            if (Log != null)
            {
                Log.AddLine(_newline);
            }
        }

        private void AddListBoxLine(string _newline)
        {
            while (lboxLog.Items.Count >= 3000)
            {
                lboxLog.Items.RemoveAt(0);
            }
            lboxLog.Items.Add(DateTime.Now.ToString() + ": " + _newline);
            lboxLog.SelectedIndex = lboxLog.Items.Count - 1;
        }

        private void LogAddCheckpoint(CBatch _batch = null, CSettings _settings = null, CPrinter _printer_settings = null,
                                        CPrinter _printer = null, bool _outputlistbox = false)
        {
            if (_batch != null)
            {
                string[] output =
                {
                    $"{ ( _batch == NextBatch ? "NextBatch" : "Batch" ) } = {{ .SC={_batch.StartCounter}     " +
                    $"\t.MB={_batch.Delta.Maint}    .BB={_batch.Delta.Broken}    .VB={_batch.Delta.Volume} ",
                    $"\t.Lot={_batch.Data.Info.LotStr}    .StartDate={_batch.Data.Info.StartDateStr}}}"
                };
                if (_outputlistbox)
                    foreach (string str in output) AddListBoxLine(str);
                if (Log != null)
                {
                    Log.AddLine(output[0] + output[1]);
                }
            }
            if (_settings != null)
            {
                string[] output =
                {
                    $"Settings = {{ .IP={_settings.IPAddr}    .Port={_settings.Port}    .AutoConnect={_settings.FormConnectOnLaunch}    " +
                    $".AutoDate={_settings.FormAutoDate}    .AutoLot={_settings.FormAutoLot}    ",
                    $"\t.currSC={_settings.CurrentBatchStartCounter}    .currMB={_settings.CurrentBatchMaintBags}    " +
                    $".currBB={_settings.CurrentBatchBrokenBags}    .currVB={_settings.CurrentBatchVolume}    " +
                    $".currLot={_settings.CurrentBatchLotStr}    .currStartDate={_settings.CurrentBatchStartDateStr}    ",
                    $"\t.nextSC={_settings.NextBatchStartCounter}    .nextMB={_settings.NextBatchMaintBags}    " +
                    $".nextBB={_settings.NextBatchBrokenBags}    .nextVB={_settings.NextBatchVolume}    " +
                    $".nextLot={_settings.NextBatchLotStr}    .nextStartDate={_settings.NextBatchStartDateStr}    ",
                    $"\tPossibleVolumeList {{ {string.Join(", ", _settings.PossibleVolumeList.ToArray())} }}     }}"
                };
                if (_outputlistbox)
                    foreach (string str in output) AddListBoxLine(str);
                if (Log != null)
                {
                    Log.AddLine(output[0] + output[1] + output[2]);
                    Log.AddLine(output[output.Length - 1]);
                }
            }
            if (_printer_settings != null)
            {
                string[] output =
                    {
                        $"Printer = {{ .IP={_printer_settings.Settings.IP}    .Port={_printer_settings.Settings.Port} ",
                        $"\t.GrpNo={_printer_settings.Settings.GrpNo}     .MsgNo={_printer_settings.Settings.MsgNo}    " +
                        $"\t.MsgName=\"{_printer_settings.Settings.MsgName.Value}\"",
                        $"\t.MsgNameRAW=\"" + BitConverter.ToString(_printer_settings.Settings.MsgName.RawData).Replace('-',' ') + @" }"
                    };
                if (_outputlistbox)
                    foreach (string str in output) AddListBoxLine(str);
                if (Log != null)
                    Log.AddLine(output[0] + output[1] + output[2]);
            }
            if (_printer != null)   // printer status
            {
                string[] output =
                    {
                        $"Printer = {{ .Number_of_print_items={_printer.Settings.NumberOfPrintItems}     .Counter={_printer.Counter} ",
                        $"\t.IsConnected={_printer.IsConnected}     .IsOnline={_printer.IsOnline}",
                        $"\t.ACKs={_printer.Status.ACKs}    .NACKs={_printer.Status.NACKs}     .Online={_printer.Status.Online}    .Reception={_printer.Status.Reception}",
                        $"\t.Status={_printer.Status.StatusRaw}      .Warning={_printer.Status.WarningRaw} ",
                        $"\t.PrinterString=[{_printer.PCString.Length}]\"{_printer.PCString.Value}\"",
                        $"\t.PrinterStringRAW=\"" + BitConverter.ToString(_printer.PCString.RawData).Replace('-',' ') + $"\" }}"
                    };
                if (_outputlistbox)
                    foreach (string str in output) AddListBoxLine(str);
                if (Log != null)
                    Log.AddLine(output[0] + output[1] + output[2] + output[3] + output[4] + output[5]);
            }
        }

        private void SaveSettingsToFile(CBatch _batch = null, CPrinter _printer = null, bool _form = false)
        {
            if (_batch != null)
            {
                if (_batch == Batch)
                {
                    Settings.CurrentBatchStartCounter = _batch.StartCounter;
                    Settings.CurrentBatchLastKnownCounter = _batch.LastKnownCounter;
                    Settings.CurrentBatchStartDate = _batch.Data.Info.StartDate;
                    Settings.CurrentBatchBrokenBags = _batch.Delta.Broken;
                    Settings.CurrentBatchMaintBags = _batch.Delta.Maint;
                    Settings.CurrentBatchVolume = _batch.Delta.Volume;
                    Settings.CurrentBatchLot = _batch.Data.Info.Lot;
                    Settings.PossibleVolumeList = _batch.Data.PossibleVolume;
                }
                else if (_batch == NextBatch)
                {
                    Settings.NextBatchStartCounter = _batch.StartCounter;
                    Settings.NextBatchLastKnownCounter = _batch.LastKnownCounter;
                    Settings.NextBatchStartDate = _batch.Data.Info.StartDate;
                    Settings.NextBatchBrokenBags = _batch.Delta.Broken;
                    Settings.NextBatchMaintBags = _batch.Delta.Maint;
                    Settings.NextBatchVolume = _batch.Delta.Volume;
                    Settings.NextBatchLot = _batch.Data.Info.Lot;
                    Settings.PossibleVolumeList = _batch.Data.PossibleVolume;
                }
            }
            if (_printer != null)
            {
                Settings.IPAddr = _printer.Settings.IP;
                Settings.Port = _printer.Settings.Port;
            }
            if (_form)
            {
                Settings.FormAutoDate = chboxBatchDateAUTO.Checked;
                Settings.FormAutoLot = chboxBatchLotNoAutoINC.Checked;
                Settings.FormConnectOnLaunch = chboxConnectOnLaunch.Checked;
            }
            Settings.SaveSettingsToFile();
        }

        #endregion

        private void StartNewBatch()
        {
            // Store new Start Batch Counter
            AddLineToLogs($"Начало новой партии...", _outputlistbox: true);
            Batch.StartBatch(Printer.Counter);
            LogAddCheckpoint(Batch, null, null, null, _outputlistbox: true);
            SaveSettingsToFile(Batch, null, _form: false);

            // Send new Print String to Printer
            AddLineToLogs($"Отправка новой строки печати \"{GeneratePrintString(Batch)}\" в принтер.", _outputlistbox: true);
            bool result = Printer.SetNewFormattedString(Batch.Data.Info.Lot, Batch.Data.Info.StartDate);
            AddLineToLogs($"{(result ? "Успешно" : "Ошибка")} ");

            // Retry if failed
            if (!result)
            {
                result = Printer.SetOnline();
                result &= Printer.ReloadOnlineStatus();
                if (result)
                {
                    result &= Printer.SetNewFormattedString(Batch.Data.Info.Lot, Batch.Data.Info.StartDate);
                }
                AddLineToLogs($"Повторная попытка... {(result ? "Успех" : "Ошибка")}", _outputlistbox: true);
            }

            if (!result)
            {
                HoldOn = true;
                // todo: MessageBox in a separate thread
            }
            else // Everything's good
            {
                HoldOn = false;
                LogAddCheckpoint(null, null, null, Printer, _outputlistbox: true);
                cmboxCurrentBatchVolume.Items.Clear();
                foreach (int _item in Batch.Data.PossibleVolume)
                    cmboxCurrentBatchVolume.Items.Add(_item);
                cmboxCurrentBatchVolume.SelectedIndex = ClosestIndex(cmboxCurrentBatchVolume.Items, Batch.Delta.Volume);
            }
            MaintenanceOn = false;  // ? ? ?
        }

        private void ConnectToPrinter()
        {
            if (!Printer.IsConnected)
            {
                // Log begin
                AddLineToLogs("Подключение принтера...", _outputlistbox: false);

                if (Printer.Begin())
                {
                    btnConnect.Enabled = false;
                    btnDisconnect.Enabled = true;

                    Batch.LastKnownCounter = Printer.Counter;
                    tmr.lastknowncounter = Printer.Counter;

                    // Log success
                    AddLineToLogs("Принтер успешно подключен", _outputlistbox: true);
                    LogAddCheckpoint(null, null, Printer, Printer, _outputlistbox: true);

                    if (Printer.Status.RemoteOperationRAW == 0x01 || Printer.Status.CirculationInProgress)
                    {
                        HoldOn = true;
                    }
                    else if (Batch.IsOverthrown())
                    {
                        // Log warning
                        AddLineToLogs($"Счётчик принтера Printer.Counter={Printer.Counter} перескочил за конец партии Batch.EndCounter={Batch.EndCounter}", _outputlistbox: true);
                        HoldOn = true;
                    }
                    else
                    {
                        HoldOn = false;
                    }
                    UpdateComponents();
                    tmrUpdate.Enabled = true;
                }
                else
                {
                    // Log failure
                    AddLineToLogs("Невозможно установить соединение с принтером", _outputlistbox: true);
                }
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            // Log inside routine
            ConnectToPrinter();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            tmrUpdate.Enabled = false;
            tmr.running = true;
            System.Threading.Thread.Sleep(tmrUpdate.Interval);
            tmr.running = false;

            if (Printer.ReloadOnlineStatus())
            {
                // Log begin
                AddLineToLogs("Отключение принтера...", _outputlistbox: true);
                if (Printer.IsOnline)
                {
                    bool result = Printer.End();
                    LogAddCheckpoint(null, null, Printer, Printer, _outputlistbox: true);
                    // Log success / failure
                    AddLineToLogs($"Принтер - отключение: {(result ? "успех" : "ошибка")}", _outputlistbox: true);
                }
            }
            btnConnect.Enabled = true;
            btnDisconnect.Enabled = false;
            UpdateComponents();
        }

        private void tboxTimerTickPeriod_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                if (int.TryParse(tboxTimerTickPeriod.Text, out int new_value))
                {
                    // Log success
                    AddLineToLogs($"Старое время цикла опроса: {tmrUpdate.Interval}. Новое время опроса: {new_value}", _outputlistbox: true);
                    tmrUpdate.Interval = new_value;
                }
        }

        private void btnStartNewBatch_Click(object sender, EventArgs e)
        {
            if (Printer.ReloadOnlineStatus() && Printer.IsOnline)
            {
                Printer.ReloadString();
                DialogResult user_answer = MessageBox.Show($"Хотите начать новую партию сейчас?\n" +
                                                        $"Внимание! Будет произведена попытка записи новой строки печати в принтер.\n\n" +
                                                        $"Текущая строка печати в принтере: \"{Printer.PCString.Value}\"\n" +
                                                        $"Новая строка печати в принтере: \"{GeneratePrintString(Batch)}\"\n" +
                                                        $"Счётчик маркированных мешков будет обнулён.", "Вопрос",
                                                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                if (user_answer == DialogResult.Yes)
                {
                    // Log begin
                    AddLineToLogs($"Нажатие кнопки \"{btnStartNewBatch.Text}\"...", _outputlistbox: true);
                    Printer.ReloadCounter();
                    // Log success (inside routine)
                    StartNewBatch();
                }
            }
            else    // Log failure
            {
                AddLineToLogs($"Принтер - отсутствует подключение, или принтер оффлайн", _outputlistbox: true);
                MessageBox.Show("Требуется подключение к принтеру, чтобы произвести изменения", "Ошибка");
            }
        }

        private void btnBrokenDEC_Click(object sender, EventArgs e)
        {
            // Log begin
            AddLineToLogs($"Уменьшение счётчика испорченных мешков на 1...", _outputlistbox: true);
            if (Batch.SetBrokenBags(Batch.Delta.Broken - 1))
            {
                SaveSettingsToFile(Batch);
                UpdateComponents();
                // Log success
                LogAddCheckpoint(Batch, null, null, null, _outputlistbox: true);
            }
            else    // Log failure
            {
                AddLineToLogs($"Невозможно уменьшить количество испорченных мешков на 1", _outputlistbox: true);
                MessageBox.Show("Невозможно уменьшить количество испорченных мешков", "Ошибка");
            }
        }

        private void btnBrokenINC_Click(object sender, EventArgs e)
        {
            // Log begin
            AddLineToLogs($"Увеличение счётчика испорченных мешков на 1...", _outputlistbox: true);
            if (Batch.SetBrokenBags(Batch.Delta.Broken + 1))
            {
                SaveSettingsToFile(Batch);
                UpdateComponents();
                // Log success
                LogAddCheckpoint(Batch, null, null, null, _outputlistbox: true);
            }
            else    // Log failure
            {
                AddLineToLogs($"Невозможно увеличить количество испорченных мешков на 1", _outputlistbox: true);
                MessageBox.Show("Невозможно увеличить количество испорченных мешков", "Ошибка");
            }
        }

        private void tboxBrokenBags_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void btn_tbBrokenBagsConfirm_Click(object sender, EventArgs e)
        {
            // Log begin
            AddLineToLogs($"Изменение количества использованных мешков на данные от пользователя...", _outputlistbox: true);
            if (int.TryParse(tboxBrokenBags.Text, out int new_value))
                if (Batch.SetBrokenBags(new_value))
                {
                    SaveSettingsToFile(Batch);
                    UpdateComponents();
                    // Log success
                    LogAddCheckpoint(Batch, null, null, null, _outputlistbox: true);
                }
                else    // Log failure
                {
                    AddLineToLogs($"Невозможно изменить количество испорченных мешков (значение не прошло проверку)", _outputlistbox: true);
                    MessageBox.Show("Невозможно изменить количество испорченных мешков", "Ошибка");
                }
            else    // Log incorrect data
            {
                AddLineToLogs($"Некорректные данные: \"{tboxBrokenBags.Text}\"", _outputlistbox: true);
                MessageBox.Show("Введено некорректное число", "Ошибка");
            }
        }

        private void tboxBrokenBags_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_tbBrokenBagsConfirm.PerformClick();
            }
        }

        private void cmboxVolume_KeyPress(object sender, KeyPressEventArgs e)
        {
            ComboBox _cmbox = (ComboBox)sender;
            if (char.IsDigit(e.KeyChar))
            {
                if (_cmbox == cmboxCurrentBatchVolume)
                    btn_cmboxAddCurrentBatchVolumeItem.Visible = true;
                if (_cmbox == cmboxNextBatchVolume)
                    btn_cmboxAddNextBatchVolumeItem.Visible = true;
            }
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void cmboxVolume_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox _cmbox = (ComboBox)sender;
            CBatch _batch = (_cmbox == cmboxNextBatchVolume ? NextBatch : Batch);
            string _subst = (_cmbox == cmboxNextBatchVolume ? "следующей" : "текущей");

            // Log begin
            AddLineToLogs($"Изменение объёма {_subst} партии на {_cmbox.SelectedItem.ToString()}...", _outputlistbox: true);
            if (_batch.SetVolume((int)_cmbox.SelectedItem))
            {
                // Log success
                if (_cmbox == cmboxCurrentBatchVolume)
                {
                    SaveSettingsToFile(_batch, null, _form: false);
                }
                UpdateComponents();
                LogAddCheckpoint(_batch, null, null, null, _outputlistbox: true);
            }
            else    // Log failure
            {
                AddLineToLogs($"Невозможно изменить объём {_subst} партии на \"{_cmbox.SelectedItem.ToString()}\"", _outputlistbox: true);
                _cmbox.SelectedIndex = ClosestIndex(_cmbox.Items, _batch.Delta.Volume);
                _cmbox.Update();
                UpdateComponents();
                MessageBox.Show("Невозможно изменить объём партии", "Ошибка");
            }
            if (_cmbox == cmboxCurrentBatchVolume)
                btn_cmboxAddCurrentBatchVolumeItem.Visible = false;
            if (_cmbox == cmboxNextBatchVolume)
                btn_cmboxAddNextBatchVolumeItem.Visible = false;
        }

        private void cmboxVolume_KeyDown(object sender, KeyEventArgs e)
        {
            ComboBox _cmbox = (ComboBox)sender;

            if (e.KeyCode == Keys.Enter)
            {
                AddNewBatchVolume(_cmbox);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                if (_cmbox == cmboxCurrentBatchVolume)
                    btn_cmboxAddCurrentBatchVolumeItem.Visible = false;
                if (_cmbox == cmboxNextBatchVolume)
                    btn_cmboxAddNextBatchVolumeItem.Visible = false;
                _cmbox.SelectedIndex = ClosestIndex(_cmbox.Items, (_cmbox == cmboxNextBatchVolume ? NextBatch : Batch).Delta.Volume);
                _cmbox.Update();
            }
        }

        private void AddNewBatchVolume(ComboBox _cmbox)
        {
            CBatch _batch = (_cmbox == cmboxNextBatchVolume ? NextBatch : Batch);

            // Log begin
            AddLineToLogs($"Добавление объёма партии {_cmbox.Text} (нажата кнопка \"Ввод\" в {_cmbox.Name})...", _outputlistbox: true);
            LogAddCheckpoint(_batch, null, null, null, _outputlistbox: true);
            AddLineToLogs($"PossibleVolume {{ {string.Join(",", Batch.Data.PossibleVolume)} }}", _outputlistbox: true);
            AddLineToLogs($"Объём текущей партии {Batch.Delta.Volume}, объём следующей партии {NextBatch.Delta.Volume}", _outputlistbox: true);
            AddLineToLogs($"Список {_cmbox.Name} {{ {string.Join(",", _cmbox.Items.Cast<int>())} }}", _outputlistbox: true);

            if (int.TryParse(_cmbox.Text, out int new_value))
            {
                bool _is_new = true;
                bool _has_possiblevolume = false;
                bool _has_cmbox = false;

                List<int> new_list_cmbox = new List<int>();

                // Look thru PossibleVolume
                foreach (int _item in Batch.Data.PossibleVolume)
                {
                    if (_item == new_value)
                    {
                        if (!_has_possiblevolume)
                        {
                            new_list_cmbox.Add(_item);
                            _has_possiblevolume = true;
                            _has_cmbox = true;
                            _is_new = false;
                        }
                    }
                    else    // _item != new_value
                    {
                        new_list_cmbox.Add(_item);
                    }
                }

                // Look thru cmbox
                foreach (int _item in _cmbox.Items)
                {
                    bool duplicate = false;
                    foreach (int _value in new_list_cmbox)
                    {
                        if (_item == _value)
                            duplicate = true;
                    }

                    if (!duplicate)
                    {
                        new_list_cmbox.Add(_item);
                    }

                    if (_item == new_value)
                    {
                        _has_cmbox = true;
                        _is_new = false;
                    }
                }

                // Making changes
                int prev_volume = _batch.Delta.Volume;
                if (_batch.SetVolume(new_value))
                {
                    if (_is_new)
                    {
                        if (!_has_possiblevolume)
                        {
                            DialogResult user_answer = MessageBox.Show($"Хотите добавить {new_value} в список постоянных (сохранённых) вариантов объёма партии?\n\n" +
                                                                   $"[Да] - вариант объёма партии {new_value} станет постоянным, даже после перезапуска программы\n" +
                                                                   $"[Нет] - вариант объёма партии {new_value} сохранится только на время текущей партии\n",
                                                                    "Вопрос",
                                                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                            // 'Yes' means we save settings to file
                            if (user_answer == DialogResult.Yes)
                            {
                                Batch.Data.AddToPossibleVolumeList(new_value);
                                NextBatch.Data.AddToPossibleVolumeList(new_value);
                                AddLineToLogs($"Добавлено значение в PossibleVolume, новый список: {{ {string.Join(",", Batch.Data.PossibleVolume)} }}",
                                                _outputlistbox: true);
                            }
                            else
                            {
                                AddLineToLogs($"Ответ пользователя: \"Не добавлять в сохранённый список объёмов\"", _outputlistbox: true);
                            }
                        }

                        if (!_has_cmbox)
                        {
                            new_list_cmbox.Add(new_value);
                            new_list_cmbox.Sort();
                            cmboxCurrentBatchVolume.Items.Clear();
                            cmboxNextBatchVolume.Items.Clear();
                            foreach (int _item in new_list_cmbox)
                            {
                                cmboxCurrentBatchVolume.Items.Add(_item);
                                cmboxNextBatchVolume.Items.Add(_item);
                            }
                            cmboxNextBatchVolume.SelectedIndex = ClosestIndex(cmboxNextBatchVolume.Items, NextBatch.Delta.Volume);
                            cmboxCurrentBatchVolume.SelectedIndex = ClosestIndex(cmboxCurrentBatchVolume.Items, Batch.Delta.Volume);
                            cmboxNextBatchVolume.Update();
                            cmboxCurrentBatchVolume.Update();
                            AddLineToLogs($"Добавлено значение в {_cmbox.Name}. Новый список: {{ {string.Join(",", _cmbox.Items.Cast<int>())} }}",
                                _outputlistbox: true);
                        }

                        if (_cmbox == cmboxCurrentBatchVolume)
                            btn_cmboxAddCurrentBatchVolumeItem.Visible = false;
                        if (_cmbox == cmboxNextBatchVolume)
                            btn_cmboxAddNextBatchVolumeItem.Visible = false;
                        SaveSettingsToFile(_batch, null, _form: false);
                        LogAddCheckpoint(_batch, null, null, null, _outputlistbox: true);
                        UpdateComponents();
                    }
                    else
                    {
                        _batch.SetVolume(prev_volume);
                        AddLineToLogs($"{new_value} уже существует в списоке возможных объёмов партии", _outputlistbox: true);
                    }
                }
                else    // Log failure
                {
                    _batch.SetVolume(prev_volume);
                    AddLineToLogs($"Невозможно изменить объём партии на \"{_cmbox.Text}\"", _outputlistbox: true);
                    MessageBox.Show("Невозможно изменить объём партии", "Ошибка");
                }
            }
            else    // Log incorrect data
            {
                AddLineToLogs($"Некорректные данные: \"{_cmbox.Text}\"", _outputlistbox: true);
                MessageBox.Show("Введено некорректное число", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_cmboxAddCurrentBatchVolumeItem_Click(object sender, EventArgs e)
        {
            AddNewBatchVolume(cmboxCurrentBatchVolume);
        }

        private void btn_cmboxAddNextBatchVolumeItem_Click(object sender, EventArgs e)
        {
            AddNewBatchVolume(cmboxNextBatchVolume);
        }

        private void DeleteBatchVolume(ComboBox _cmbox)
        {
            string _subst = (_cmbox == cmboxNextBatchVolume ? "следующей" : "текущей");
            CBatch _batch = (_cmbox == cmboxNextBatchVolume ? NextBatch : Batch);

            // Log begin
            AddLineToLogs($"Удаление объёма \"{_cmbox.Text}\" (нажата кнопка \"Удалить\" в {_subst} партии)...", _outputlistbox: true);
            LogAddCheckpoint(_batch, null, null, null, _outputlistbox: true);
            AddLineToLogs($"PossibleVolume {{ {string.Join(",", Batch.Data.PossibleVolume)} }}", _outputlistbox: true);
            AddLineToLogs($"Объём текущей партии {Batch.Delta.Volume}, объём следующей партии {NextBatch.Delta.Volume}", _outputlistbox: true);
            AddLineToLogs($"Список {_cmbox.Name} {{ {string.Join(",", _cmbox.Items.Cast<int>())} }}", _outputlistbox: true);

            if (int.TryParse(_cmbox.Text, out int remove_value))
            {
                bool _is_found = false;
                bool _has_possiblevolume = false;
                bool _has_cmbox = false;

                List<int> new_list_cmbox = new List<int>();
                List<int> new_list_possiblevolume = new List<int>();

                // Look thru PossibleVolume
                foreach (int _item in Batch.Data.PossibleVolume)
                {
                    if (_item == remove_value)
                    {
                        _has_possiblevolume = true;
                        _is_found = true;
                    }
                    else    // _item != new_value
                    {
                        new_list_cmbox.Add(_item);
                        new_list_possiblevolume.Add(_item);
                    }
                }

                // Look thru cmbox
                foreach (int _item in _cmbox.Items)
                {
                    bool duplicate = false;
                    foreach (int _value in new_list_cmbox)
                    {
                        if (_item == _value)
                            duplicate = true;
                    }
                    if (_item == remove_value)
                    {
                        _has_cmbox = true;
                        _is_found = true;
                    }
                    else
                    {
                        if (!duplicate)
                        {
                            new_list_cmbox.Add(_item);
                        }
                    }
                }

                if (new_list_cmbox.Count == 0)
                {
                    new_list_cmbox.Add(1600);
                }
                if (new_list_possiblevolume.Count == 0)
                {
                    new_list_possiblevolume.Add(1600);
                }
                new_list_cmbox.Sort();
                new_list_possiblevolume.Sort();

                AddLineToLogs($"В списке PossibleVolume: значение {remove_value} {(_has_possiblevolume ? "найдено" : "не найдено")}", _outputlistbox: true);
                AddLineToLogs($"В списке {_cmbox.Name}: значение {remove_value} {(_has_cmbox ? "найдено" : "не найдено")}", _outputlistbox: true);

                // Making changes
                if (_is_found)
                {
                    DialogResult user_answer = DialogResult.Yes;
                    if (_has_possiblevolume)
                    {
                        user_answer = MessageBox.Show($"Хотите удалить {remove_value} из списка постоянных (сохранённых) вариантов объёма партии?\n\n" +
                                                      $"В случае ошибочного удаления можно будет ввести значение объёма партии заново.",
                                                      "Вопрос",
                                                      MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                        if (user_answer == DialogResult.Yes)
                        {
                            Batch.Data.PossibleVolume = new_list_possiblevolume;
                            NextBatch.Data.PossibleVolume = new_list_possiblevolume;
                            SaveSettingsToFile(_batch, null, _form: false);
                            AddLineToLogs($"Удалено значение из Batch.PossibleVolume, новый список: {{ {string.Join(",", Batch.Data.PossibleVolume)} }}",
                                            _outputlistbox: true);
                        }
                        else
                        {
                            AddLineToLogs($"Ответ пользователя: \"Не удалять\"", _outputlistbox: true);
                        }
                    }
                    if (_has_cmbox && user_answer == DialogResult.Yes)
                    {
                        cmboxCurrentBatchVolume.Items.Clear();
                        cmboxNextBatchVolume.Items.Clear();
                        foreach (int _item in new_list_cmbox)
                        {
                            cmboxCurrentBatchVolume.Items.Add(_item);
                            cmboxNextBatchVolume.Items.Add(_item);
                        }
                        cmboxCurrentBatchVolume.SelectedIndex = ClosestIndex(cmboxCurrentBatchVolume.Items, Batch.Delta.Volume);
                        cmboxNextBatchVolume.SelectedIndex = ClosestIndex(cmboxNextBatchVolume.Items, NextBatch.Delta.Volume);
                        cmboxCurrentBatchVolume.Update();
                        cmboxNextBatchVolume.Update();
                        AddLineToLogs($"Удалено значение из {_cmbox.Name} {{ {string.Join(",", _cmbox.Items.Cast<int>())} }}", _outputlistbox: true);
                    }
                    UpdateComponents();
                }
                else
                {
                    AddLineToLogs($"Ошибка. Невозможно найти значение \"{_cmbox.Text}\"", _outputlistbox: true);
                }
            }
            else    // Log incorrect data
            {
                AddLineToLogs($"Некорректные данные: \"{_cmbox.Text}\"", _outputlistbox: true);
            }
        }

        private void btn_cmboxDeleteCurrentBatchVolumeItem_Click(object sender, EventArgs e)
        {
            DeleteBatchVolume(cmboxCurrentBatchVolume);
        }

        private void btn_cmboxDeleteNextBatchVolumeItem_Click(object sender, EventArgs e)
        {
            DeleteBatchVolume(cmboxNextBatchVolume);
        }

        private void chboxBatchLotNoAutoINC_CheckedChanged(object sender, EventArgs e)
        {
            if (!MyForm.AppStarting)
            {
                if (chboxBatchLotNoAutoINC.Checked)
                {
                    NextBatch.Data.Info.Lot = Batch.Data.Info.Lot + 1;
                }
                else if ((!(tboxBatchNextLotNo.Text.Length == 0)) && int.TryParse(tboxBatchNextLotNo.Text, out int new_value))
                {
                    NextBatch.Data.Info.Lot = new_value;
                }
                UpdateComponents();
                SaveSettingsToFile(NextBatch, null, _form: true);
                // Log success
                AddLineToLogs($"Автоинкремент номера установлен: \"{(chboxBatchLotNoAutoINC.Checked ? "ВКЛ" : "ВЫКЛ")}\"", _outputlistbox: true);
            }
            tboxBatchNextLotNo.Enabled = chboxBatchLotNoAutoINC.Checked ? false : true;
            btn_tbBatchNextLotNoConfirm.Enabled = chboxBatchLotNoAutoINC.Checked ? false : true;
        }

        private void tboxBatchLotNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void tboxBatchLotNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SetBatchLotNo(Batch);
            }
        }

        private void SetBatchLotNo(CBatch _batch)
        {
            TextBox _tbox = (_batch == NextBatch) ? tboxBatchNextLotNo : tboxBatchLotNo;
            string _subst = (_batch == NextBatch) ? "следующей" : "текущей";

            // Log begin
            AddLineToLogs($"Ввод нового номера {_tbox.Text} в {_subst} партии (нажатие \"OK\" или \"Enter\")...", _outputlistbox: true);

            if (int.TryParse(_tbox.Text, out int new_value))
            {
                if (new_value >= 0 && new_value <= 1999)
                {
                    if (_batch.Data.Info.Lot != new_value)
                    {
                        _batch.Data.Info.Lot = new_value;
                        SaveSettingsToFile(_batch, null, _form: false);
                        UpdateComponents();
                        // Log success
                        LogAddCheckpoint(_batch, null, null, null, _outputlistbox: true);
                        AddLineToLogs($"Для обновления информации печати в принтере, отправьте новую строку печати вручную.\n");
                        AddLineToLogs($"Чтобы сделать это, нажмите на кнопку \"{btnReWriteStringToPrinter.Text}\"", _outputlistbox: true);
                    }
                    else    // Log neutral
                    {
                        AddLineToLogs($"Номер {_subst} партии оставлен прежним, \"{_tbox.Text}\"", _outputlistbox: true);
                    }
                }
                else    // Log failure
                {
                    AddLineToLogs($"Невозможно изменить номер {_subst} партии на \"{_tbox.Text}\"", _outputlistbox: true);
                    MessageBox.Show($"Невозможно изменить номер {_subst} партии", "Ошибка");
                }
            }
            else    // Log incorrect data
            {
                AddLineToLogs($"Некорректные данные: \"{_tbox.Text}\"", _outputlistbox: true);
                MessageBox.Show($"Введено некорректное число \"{_tbox.Text}\"", "Ошибка");
            }
        }

        private void btn_tbBatchLotNoConfirm_Click(object sender, EventArgs e)
        {
            SetBatchLotNo(Batch);
        }

        private void tboxBatchNextLotNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void tboxBatchNextLotNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SetBatchLotNo(NextBatch);
            }
        }

        private void btn_tbBatchNextLotNoConfirm_Click(object sender, EventArgs e)
        {
            SetBatchLotNo(NextBatch);
        }

        private void chboxBatchDateAUTO_CheckedChanged(object sender, EventArgs e)
        {
            if (!MyForm.AppStarting)
            {
                if (chboxBatchDateAUTO.Checked)
                {
                    NextBatch.Data.Info.StartDate = System.DateTime.Now;
                }
                else if (dtpickBatchNextDate.Value.Date <= System.DateTime.Now.Date)
                {
                    NextBatch.Data.Info.StartDate = dtpickBatchNextDate.Value;
                }
                SaveSettingsToFile(NextBatch, null, _form: true);
                UpdateComponents();
                // Log success
                AddLineToLogs($"Автоматическая дата установлена: \"{(chboxBatchDateAUTO.Checked ? "ВКЛ" : "ВЫКЛ")}\"", _outputlistbox: true);
            }
            dtpickBatchNextDate.Enabled = chboxBatchDateAUTO.Checked ? false : true;
        }

        private void SetBatchDate(CBatch _batch)
        {
            DateTimePicker _dtpick = (_batch == NextBatch) ? dtpickBatchNextDate : dtpickBatchDate;
            string _subst = (_batch == NextBatch) ? "следующей" : "текущей";

            // Log begin
            AddLineToLogs($"Ввод новой даты {_dtpick.Value.Date.ToString()} в {_subst} партии (выбрана дата в календаре)...", _outputlistbox: true);

            if (DateTime.Now.Date >= _dtpick.Value.Date)
            {
                string new_date = _dtpick.Value.ToString("dd.MM.yy");
                if (new_date != _batch.Data.Info.StartDateStr)
                {
                    // Log success
                    AddLineToLogs($"Изменение даты {_subst} партии \"{_batch.Data.Info.StartDateStr}\" -> \"{new_date}\"", _outputlistbox: true);
                    _batch.Data.Info.StartDate = _dtpick.Value;
                    SaveSettingsToFile(_batch, null, _form: false);
                    UpdateComponents();
                    LogAddCheckpoint(_batch, null, null, null, _outputlistbox: true);
                    if (_batch==Batch)
                    {
                        AddLineToLogs($"Для обновления информации печати в принтере, отправьте новую строку печати вручную.\n");
                        AddLineToLogs($"Чтобы сделать это, нажмите на кнопку \"{btnReWriteStringToPrinter.Text}\"", _outputlistbox: true);
                    }
                }
            }
            else    // Log failure
            {
                AddLineToLogs($"Невозможно выбрать дату {_subst} партии \"{_dtpick.Value.Date.ToString("dd.MM.yy")}\", она больше сегодняшней даты",
                               _outputlistbox: true);
                MessageBox.Show($"Невозможно выбрать дату {_subst} партии \"{_dtpick.Value.Date.ToString("dd.MM.yy")}\", она больше сегодняшней даты",
                                 "Информация", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                _dtpick.Value = _batch.Data.Info.StartDate;
            }
        }

        private void dtpickBatchDate_ValueChanged(object sender, EventArgs e)
        {
            SetBatchDate(Batch);
        }

        private void dtpickBatchNextDate_ValueChanged(object sender, EventArgs e)
        {
            SetBatchDate(NextBatch);
        }

        private void btnStartMaintenance_Click(object sender, EventArgs e)
        {
            // Log begin
            AddLineToLogs($"Техническое обслуживание...", _outputlistbox: true);
            if (Printer.ReloadOnlineStatus())
            {
                AddLineToLogs($"Текущее состояние ТО: {(MaintenanceOn ? "ВКЛ" : "ВЫКЛ")}. Переключение режима ТО.", _outputlistbox: true);
                if (!MaintenanceOn)
                {
                    AddLineToLogs($"Принтер - {(Printer.IsOnline ? "онлайн" : "оффлайн")}", _outputlistbox: true);
                    if (Printer.IsOnline)
                    {
                        bool result = Printer.ReloadCounter();
                        result &= Printer.SetOnline(false);
                        LogAddCheckpoint(null, null, Printer, Printer, _outputlistbox: true);
                        if (result)
                        {
                            Batch.MaintBegin(Printer.Counter);
                            MaintenanceOn = true;
                            // Log successfull Maint On
                            AddLineToLogs($"Техническое обслуживание успешно ВКЛ", _outputlistbox: true);
                        }
                        else    // Log failure to turn Maint On
                        {
                            AddLineToLogs($"Не удалось включить режим ТО (не удалось прочитать счётчик мешков, или перевести принтер в оффлайн)",
                                            _outputlistbox: true);
                            MessageBox.Show("Не удалось перевести принтер в режим ТО", "Ошибка");
                        }
                    }
                    else    // Log failure initial conditions
                    {                        
                        AddLineToLogs($"ТО не может быть начато (принтер в режиме оффлайн)", _outputlistbox: true);
                    }
                }
                else
                {
                    bool result = Printer.SetOnline();
                    result &= Printer.ReloadOnlineStatus();
                    result &= Printer.IsOnline;
                    result &= Printer.ReloadCounter();
                    if (result)
                    {
                        Batch.MaintEnd(Printer.Counter);
                        MaintenanceOn = false;
                        SaveSettingsToFile(Batch);
                        LogAddCheckpoint(Batch, null, Printer, Printer, _outputlistbox: true);
                        // Log successfull Maint Off
                        AddLineToLogs($"Техническое обслуживание успешно завершено (ВЫКЛ).", _outputlistbox: true);                        
                    }
                    else    // Log failure to turn Maint Off
                    {
                        AddLineToLogs($"Не удалось выключить режим ТО (не удалось перевести принтер в режим онлайн, либо прочитать счётчик мешков)",
                            _outputlistbox: true);
                        MessageBox.Show("Не удалось перевести принтер из режима ТО в рабочий режим", "Ошибка");
                    }
                }
            }
            else     // Log failure initial conditions
            {
                AddLineToLogs($"Принтер - не подключен, или не удалось прочитать статус принтера", _outputlistbox: true);
                MessageBox.Show("Требуется подключение к принтеру, чтобы произвести изменения", "Ошибка");
            }
            UpdateComponents();
        }

        private void btnReWriteStringToPrinter_Click(object sender, EventArgs e)
        {
            // Log begin
            AddLineToLogs($"Отправка сообщения печати в принтер...", _outputlistbox: true);
            if (Printer.ReloadOnlineStatus() && Printer.IsOnline)
            {
                tmr.running = true;
                tmrUpdate.Enabled = false;
                if (!Printer.ReloadString())
                {
                    // Log warning
                    AddLineToLogs($"Не удалось перечитать строку печати из принтера", _outputlistbox: true);
                }

                DialogResult user_answer = MessageBox.Show($"Вы хотите заменить строку печати принтера \n" +
                                                            $"\t\"{Printer.PCString.Value}\"\n\n" +
                                                            $"на новую строку \n" +
                                                            $"\t\"{GeneratePrintString(Batch)}\" ?", "Вопрос",
                                                            MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (user_answer == DialogResult.Yes)
                {
                    bool result = Printer.SetNewFormattedString(Batch.Data.Info.Lot, Batch.Data.Info.StartDate);
                    UpdateComponents();
                    // Log success / failure
                    AddLineToLogs($"Отправка новой строки печати: {(result ? "успешно" : "ошибка")}", _outputlistbox: true);
                    AddLineToLogs($"Новая строка печати: [{Printer.PCString.Length}]: \"{Printer.PCString.Value}\"", _outputlistbox: true);
                    MessageBox.Show(result ? "OK" : "Ошибка", "Результат");
                }
                else    // Log User Cancelled
                {
                    AddLineToLogs($"Ответ пользователя: \"No\"", _outputlistbox: false);
                }
                tmr.running = false;
                tmrUpdate.Enabled = true;
            }
            else    // Log failure
            {
                AddLineToLogs($"Принтер - не подключен, или не удалось прочитать статус принтера", _outputlistbox: true);
                MessageBox.Show("Требуется подключение к принтеру, чтобы обновить строку печати в принтере", "Ошибка");
            }                
        }

        private void tlstr_miSaveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog file_savedlg = new System.Windows.Forms.SaveFileDialog();
            file_savedlg.FileName = @"log.txt";
            file_savedlg.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
            if (lboxLog.Items.Count > 0 && file_savedlg.ShowDialog() == DialogResult.OK)
                System.IO.File.WriteAllLines(file_savedlg.FileName, lboxLog.Items.Cast<string>().ToArray());
        }

        private void tlstr_miClear_Click(object sender, EventArgs e)
        {
            lboxLog.Items.Clear();
        }

        private void chboxConnectOnLaunch_CheckedChanged(object sender, EventArgs e)
        {
            if (!MyForm.AppStarting)
            {
                SaveSettingsToFile(null, null, _form: true);
            }
        }

        private void btnStorePrintingMessageIntoMemory_Click(object sender, EventArgs e)
        {
            // Log begin
            AddLineToLogs($"Сохранение текущего сообщение во FLASH-память принтера...", _outputlistbox: true);

            if (Printer.IsConnected && Printer.IsOnline)
            {
                DialogResult user_answer = MessageBox.Show($"Будет произведена попытка записи текущего сообщения в постоянную память принтера.\n" +
                                                        $"Во избежание останова конвейера рекомендуется производить запись при пустом конвейере " +
                                                        $"(запись длится порядка 10 секунд).\n" +
                                                        $"Желаете продолжить?\n\n" +
                                                        $"\tНомер группы сообщения: {Printer.Settings.GrpNo}\n" +
                                                        $"\tНомер сообщения: {Printer.Settings.MsgNo}\n" +
                                                        $"\tИмя сообщения: \"{Printer.Settings.MsgName.Value}\".", "Вопрос",
                                                        MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                if (user_answer == DialogResult.OK)
                {
                    bool result = Printer.StorePrintingMessageIntoFlashMemory();
                    // Log success / failure
                    AddLineToLogs($"Результат... {(result ? "Успех" : "Ошибка")}", _outputlistbox: true);
                }
                else    // Log User Cancelled
                {
                    AddLineToLogs($"Отменено пользователем", _outputlistbox: true);
                }
            }
            else    // Log failure
            {
                AddLineToLogs($"Принтер - не подключен, или не удалось прочитать статус принтера", _outputlistbox: true);
                MessageBox.Show("Требуется подключение к принтеру, чтобы произвести изменения", "Ошибка");
            }    
        }

        private void btnSetRemoteOperationStart_Click(object sender, EventArgs e)
        {
            // Log begin
            AddLineToLogs($"Удалённый запуск циркуляции чернил...", _outputlistbox: true);
            bool result = Printer.ReloadOnlineStatus() && Printer.IsOnline;

            if (result)
            {
                AddLineToLogs($"\tPrinter.Status.RemoteOperationRAW=0x{Printer.Status.RemoteOperationRAW.ToString("X")}", _outputlistbox: true);
                AddLineToLogs($"\tPrinter.Status.StatusRaw=0x{Printer.Status.StatusRaw.ToString("X")}", _outputlistbox: true);
                result &= Printer.ReloadStatus();
                if (!result)
                {
                    AddLineToLogs($"Не удалось обновить статус принтера.", _outputlistbox: true);
                    MessageBox.Show($"Не удалось обновить статус принтера.", "Ошибка");
                }
                else if (Printer.Status.RemoteOperationRAW == 0x00)
                {
                    AddLineToLogs($"Циркуляция чернил уже в работе, или в находится в процессе запуска,", _outputlistbox: true);
                    MessageBox.Show($"Циркуляция чернил уже в работе, или в находится в процессе запуска.", "Ошибка");
                }
                else if (Printer.Status.CirculationInProgress)
                {
                    AddLineToLogs($"Принтер находится в процессе запуска или остановки циркуляции чернил.", _outputlistbox: true);
                    MessageBox.Show($"Принтер находится в процессе запуска или остановки циркуляции чернил.", "Ошибка");
                }
                else
                {
                    DialogResult user_answer = MessageBox.Show($"Будет произведена попытка запуска циркуляции чернил.\n" +
                                                           $"Операция занимает около 1 минуты.\n" +
                                                           $"При успешном выполнении операции будет возможна печать сообщений\n\n" +
                                                           $"Вы желаете продолжить?", "Вопрос",
                                                           MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    if (user_answer == DialogResult.OK)
                    {
                        tmrUpdate.Enabled = false;
                        tmr.running = true;
                        MaintenanceOn = false;
                        HoldOn = true;
                        System.Threading.Thread.Sleep(tmrUpdate.Interval);

                        result &= Printer.SetRemoteOperation(SetOperationStart: true);

                        result &= Printer.ReloadStatus();
                        while (Printer.Status.CirculationInProgress)
                        {
                            System.Threading.Thread.Sleep(tmrUpdate.Interval);
                            result &= Printer.ReloadStatus();
                            UpdateComponents();
                            this.Update();
                        }
                        // Log success/failure
                        AddLineToLogs($"Результат: {(result ? "успех" : "ошибка")}", _outputlistbox: true);
                        tmr.running = false;
                        HoldOn = false;
                        tmrUpdate.Enabled = true;
                    }
                    else    // Log User Cancelled
                    {
                        AddLineToLogs($"Отменено пользователем", _outputlistbox: true);
                    }
                }                
            }
            else
            {
                AddLineToLogs($"Принтер - не подключен, или не удалось прочитать статус принтера", _outputlistbox: true);
                MessageBox.Show("Требуется подключение к принтеру, чтобы произвести изменения", "Ошибка");
            }
        }

        private void btnSetRemoteOperationStop_Click(object sender, EventArgs e)
        {
            // Log begin
            AddLineToLogs($"Удалённый останов циркуляции чернил...", _outputlistbox: true);
            bool result = Printer.ReloadOnlineStatus() && Printer.IsOnline;

            if (result)
            {
                AddLineToLogs($"\tPrinter.Status.RemoteOperationRAW=0x{Printer.Status.RemoteOperationRAW.ToString("X")}", _outputlistbox: true);
                AddLineToLogs($"\tPrinter.Status.StatusRaw=0x{Printer.Status.StatusRaw.ToString("X")}", _outputlistbox: true);
                result &= Printer.ReloadStatus();
                if (!result)
                {
                    AddLineToLogs($"Не удалось обновить статус принтера", _outputlistbox: true);
                    MessageBox.Show($"Не удалось обновить статус принтера.", "Ошибка");
                }
                else if (Printer.Status.CirculationInProgress)
                {
                    AddLineToLogs($"Принтер находится в процессе запуска или остановки циркуляции чернил.", _outputlistbox: true);
                    MessageBox.Show($"Принтер находится в процессе запуска или остановки циркуляции чернил.", "Ошибка");
                }
                else if (Printer.Status.RemoteOperationRAW != 0x00)
                {
                    AddLineToLogs($"Циркуляция чернил не запущена,", _outputlistbox: true);
                    MessageBox.Show($"Циркуляция чернил не запущена.", "Ошибка");
                }
                else if (Printer.Status.StatusPaused)
                {
                    AddLineToLogs($"Принтер находится в режиме паузы (нет циркуляции чернил),", _outputlistbox: true);
                    MessageBox.Show($"Принтер находится в режиме паузы (нет циркуляции чернил).", "Ошибка");
                }
                else
                {
                    DialogResult user_answer = MessageBox.Show($"Будет произведена попытка останова циркуляции чернил.\n" +
                                                           $"Операция занимает 4 минуты, во время операции и после неё- печать будет невозможна.\n" +
                                                           $"Рекомендуется выполнять операцию перед длительным простоем (на ночь),\n" +
                                                           $"Не рекомендуется выполнять операцию чаще 1 раза в 1 час (во избежание снижения вязкости чернил).\n\n" +
                                                           $"Вы желаете продолжить?",
                                                           "Вопрос", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    if (user_answer == DialogResult.OK)
                    {
                        tmr.running = true;
                        if (MaintenanceOn)
                        {
                            btnStartMaintenance.PerformClick();
                        }

                        tmrUpdate.Enabled = false;
                        System.Threading.Thread.Sleep(tmrUpdate.Interval);

                        MaintenanceOn = false;
                        HoldOn = true;
                        tmr.update_switch = 1;

                        result &= Printer.SetRemoteOperation(SetOperationStart: false);
                        // Log success/failure
                        AddLineToLogs($"Результат: {(result ? "успех" : "ошибка")}", _outputlistbox: true);
                        tmr.running = false;
                        tmrUpdate.Enabled = true;
                    }
                    else    // Log User Cancelled
                    {
                        AddLineToLogs($"Отменено пользователем", _outputlistbox: true);
                    }
                }                
            }            
            else
            {
                AddLineToLogs($"Принтер - не подключен, или не удалось прочитать статус принтера", _outputlistbox: true);
                MessageBox.Show("Требуется подключение к принтеру, чтобы произвести изменения", "Ошибка");
            }
        }

        private void btnExpandWindowForm_Click(object sender, EventArgs e)
        {
            if (this.Width >= MyForm.FullWidth)
            {
                this.Width = MyForm.SmallWidth;
                btnExpandWindowForm.Text = " > > ";
            }
            else
            {
                this.Width = MyForm.FullWidth;
                btnExpandWindowForm.Text = " < < ";
            }
        }

        private void btnStopCalendarTimeResume_Click(object sender, EventArgs e)
        {
            Printer.SetClockResume(SetResume: true);
        }

        private void btn_cmboxAddCurrentBatchVolumeItem_VisibleChanged(object sender, EventArgs e)
        {
            if (btn_cmboxAddCurrentBatchVolumeItem.Visible)
            {
                btn_cmboxDeleteCurrentBatchVolumeItem.Visible = false;
            }
            else
            {
                btn_cmboxDeleteCurrentBatchVolumeItem.Visible = true;
            }
        }

        private void btn_cmboxAddNextBatchVolumeItem_VisibleChanged(object sender, EventArgs e)
        {
            if (btn_cmboxAddNextBatchVolumeItem.Visible)
            {
                btn_cmboxDeleteNextBatchVolumeItem.Visible = false;
            }
            else
            {
                btn_cmboxDeleteNextBatchVolumeItem.Visible = true;
            }
        }

        private void ClearBatchBagCounters(CBatch _batch)
        {
            string _subst = (_batch == NextBatch) ? "следующей" : "текущей";
            Button _btn = (_batch == NextBatch) ? btn_NextBatchClear : btn_CurrentBatchClear;
            // Log begin
            AddLineToLogs($"Нажата кнопка \"{_btn.Text}\" для {_subst} партии...", _outputlistbox: true);
            LogAddCheckpoint(_batch, null, null, null, _outputlistbox: true);

            if ( _batch.Delta.Broken != 0 || _batch.Delta.Maint !=0 || (_batch.LastKnownCounter - _batch.StartCounter - _batch.Delta.Broken - _batch.Delta.Maint != 0))
            {
                DialogResult user_answer;
                user_answer = MessageBox.Show($"Вы хотите обнулить счётчики {_subst} партии?\n\n" +
                    $"Номер партии\t\t\t{_batch.Data.Info.LotStr}\n" +
                    $"Дата партии\t\t\t{_batch.Data.Info.StartDateStr}\n" +
                    $"Маркированные мешки:\t\t{_batch.LastKnownCounter - _batch.StartCounter - _batch.Delta.Broken - _batch.Delta.Maint}   -->   0\n" +
                    $"Испорченные мешки:\t\t{_batch.Delta.Broken}   -->   0\n" +
                    $"Мешки пропущенные при ТО:\t{_batch.Delta.Maint}   -->   0\n",
                    "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (user_answer == DialogResult.Yes)
                {
                    _batch.StartBatch(_batch.LastKnownCounter);
                    LogAddCheckpoint(_batch, null, null, null, _outputlistbox: true);
                    UpdateComponents();
                    SaveSettingsToFile(_batch, null, _form: false);
                }
                else
                {
                    AddLineToLogs($"Отменено пользователем", _outputlistbox: true);
                }
            }
            else
            {
                AddLineToLogs($"Счётчики маркированных, испорченых и пропущенных при ТО мешков уже равны нулю", _outputlistbox: true);
            }
        }

        private void btn_CurrentBatchClear_Click(object sender, EventArgs e)
        {
            ClearBatchBagCounters(Batch);
        }
    }

    class CLog
    {
        private System.IO.StreamWriter file_log;

        public CLog(string _path)
        {
            file_log = new System.IO.StreamWriter(_path, append: true);
        }

        public void AddLine(string _str)
        {
            file_log.WriteLine(DateTime.Now.ToString() + ", " + _str);
        }

        public void Add(string _str)
        {
            file_log.Write(DateTime.Now.ToString() + ", " + _str);
        }

        public void AddObject(object _obj)
        {
            file_log.Write(DateTime.Now.ToString() + ", " + nameof(_obj).ToString() + "=" + _obj.ToString());
        }

        public void CloseFile()
        {
            if (file_log != null)
            {
                file_log.WriteLine(DateTime.Now.ToString() + ",");
                file_log.Close();
                file_log = null;
            }
        }
        ~CLog()
        {
            if (file_log != null)   file_log.Close();
        }
    }
}
