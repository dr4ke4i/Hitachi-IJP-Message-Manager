using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Modbus_DLL;

namespace Hitachi_IJP_Message_Manager
{
    class CPrinter
    {

        #region Public Data

        public int Counter { get; private set; }
        public CDataTypes.structPCString PCString { get { return pcstring; } }
        public CDataTypes.structStatus Status { get { return status; } }

        public CDataTypes.structSettings Settings;
        public bool IsConnected { get { return MB.IsConnected; } }
        public bool IsOnline { get { return isonline; } }

        #endregion


        #region Data Types

        public class CDataTypes
        {
            public struct structStatus
            {
                public bool Online;
                public bool Reception;
                public int StatusRaw;
                public bool StatusReady { get { return StatusRaw == 0x32 ? true : false; } }
                public bool StatusPaused { get { return StatusRaw == 0x30 ? true : false; } }
                public bool CirculationIsStopping { get { return StatusRaw == 0x49 ? true : false; } }
                public bool CirculationIsStarting { get { return StatusRaw == 0x31 ? true : false; } }
                public bool CirculationInProgress { get { return CirculationIsStopping || CirculationIsStarting; } }
                public string StatusString { get
                    {
                        switch (StatusRaw)
                            {
                            case 0x30: return "Пауза";                                  //"Paused";
                            case 0x31: return "Запущен - Не готов";                     //"Running - Not Ready";
                            case 0x32: return "Готов";                                  //"Ready";
                            case 0x33: return "Ошибка отклоняющего напряжения";         //"Deflection Voltage Fault";
                            case 0x34: return "Основная ёмкость чернил переполнена";    //"Main Ink Tank Too Full";
                            case 0x35: return "Пустая строка печати";                   //"Blank Print Items";
                            case 0x36: return "Заряд капли слишком низкий";             //"Ink Drop Charge Too Low";
                            case 0x37: return "Заряд капли слишком высокий";            //"Ink Drop Charge Too High";
                            case 0x38: return "Открыта крышка печатающей головки";      //"Print Head Cover Open";
                            case 0x39: return "Неисправность датчика наличия объекта печати"; //"Target Sensor Fault";
                            case 0x3a: return "Внутренняя ошибка системы (C)";          //"System Operation Error C";
                            case 0x3b: return "Расстояние между объектами печати слишком мало"; //"Target Spacing Too Close";
                            case 0x3c: return "Неправильное положение датчика";         //"Improper Sensor Position";
                            case 0x3d: return "Внутренняя ошибка системы (M)";          //"System Operation Error M";
                            case 0x3e: return "Ошибка заряжающего напряжения";          //"Charge Voltage Fault";
                            case 0x3f: return "Недостаточное количество цифр в штрих-коде"; //"Barcode Short On Numbers";
                            case 0x41: return "Неисправность вентилятора DC блока питания";	//"Multi DC Power Supply Fan Fault";
                            case 0x42: return "Электрическая утечка отклоняющего напряжения"; //"Deflection Voltage Leakage";
                            case 0x43: return "Ошибка - наложение печати друг на друга"; //"Print Overlap Fault";
                            case 0x44: return "Низкий уровень чернил";					//"Ink Low Fault";
                            case 0x45: return "Низкий уровень растворителя";			//"Makeup Ink Low Fault";
                            case 0x46: return "Данные печати в процессе изменения (M)";	//"Print Data Changeover In Progress M";
                            case 0x47: return "Избыточное число форматирований объектов печати";	//"Excessive Format Count";
                            case 0x48: return "Истекло время пополнения растворителя";	//"Makeup Ink Replenishment Time-out";
                            case 0x49: return "Останавливается...";						//"Stopping";
                            case 0x4a: return "Истекло время пополнения чернил";	    //"Ink Replenishment Time-out";
                            case 0x4b: return "Отсутствует заряд капель чернил";		//"No Ink Drop Charge";
                            case 0x4c: return "Очень высокая температура нагревателя чернил";	//"Ink Heating Unit Too High";
                            case 0x4d: return "Неисправность термодатчика подогревателя чернил";	//"Ink Heating Unit Temperature Sensor Fault";
                            case 0x4e: return "Высокий ток подогревателя чернил";		//"Ink Heating Unit Over Current";
                            case 0x4f: return "Внутренняя ошибка связи (C)";			//"Internal Communication Error C";
                            case 0x50: return "Внутренняя ошибка связи (M)";			//"Internal Communication Error M";
                            case 0x51: return "Внутренняя ошибка связи (S)";			//"Internal Communication Error S";
                            case 0x52: return "Внутренняя ошибка системы (S)";   	    //"System Operation Error S";
                            case 0x53: return "Внутренняя ошибка памяти (С)";			//"Memory Fault C";
                            case 0x54: return "Внутренняя ошибка памяти (M)";			//"Memory Fault M";
                            case 0x55: return "Ошибка датчика температуры окружающей среды"; //"Ambient Temperature Sensor Fault";
                            case 0x56: return "Неисправность вентилятора контроллера принтера";	//"Print Controller Cooling Fan Fault";
                            case 0x59: return "Данные печати в процессе изменения (S)";	//"Print Data Changeover In Progress S";
                            case 0x5a: return "Данные печати в процессе изменения (V)"; //"Print Data Changeover In Progress V";
                            case 0x5c: return "Запущено тех.обслуживание";				//"Maint. Running";
                            case 0x5d: return "Внутренняя ошибка памяти (M)";			//"Memory Fault S";
                            case 0x5e: return "Неисправность привода насоса";	        //"Pump Motor Fault";
                            case 0x5f: return "Неисправность датчика вязкости чернил";	//"Viscometer Ink Temperature Sensor Fault";
                            case 0x60: return "Ошибка связи с внешними устройствами";	//"External Communication Error";
                            case 0x61: return "Неисправность сигнала от внешнего устройства";	//"External Signal Error";
                            case 0x62: return "Неисправность памяти (OP)";  			//"Memory Fault OP";
                            case 0x63: return "Низкая температура нагревателя чернил";	//"Ink Heating Unit Temperature Low";
                            case 0x64: return "Ошибка кода модели"; 					//"Model-key Fault";
                            case 0x65: return "Ошибка кода языка";						//"Language-key Fault";
                            case 0x66: return "Неисправность буфера коммуникации";		//"Communication Buffer Fault";
                            case 0x67: return "Неисправность выключения принтера";		//"Shutdown Fault";
                            case 0x68: return "Переполнение счётчика печати";			//"Count Overflow";
                            case 0x69: return "Ошибка тайминга при изменении данных печати";	//"Data changeover timing fault";
                            case 0x6a: return "Ошибка тайминга при изменении счётчика печати";	//"Count changeover timing fault";
                            case 0x6b: return "Ошибка тайминга при старте печати";		//"Print start timing fault";
                            case 0x6c: return "Превышен срок хранения бутыли чернил";	//"Ink Shelf Life Information";
                            case 0x6d: return "Превышен срок хранения бутыли растворителя";	//"Makeup Shelf Life Information";
                            case 0x71: return "Ошибка в процессе изменения данных (C)";	//"Print Data Changeover Error C";
                            case 0x72: return "Ошибка в процессе изменения данных (M)";	//"Print Data Changeover Error M";
                            default: return "Неизвестный код статуса принтера";			//"Unknown Operation Status";
                        }
                    }
                }
                public int WarningRaw;
                public bool WarningNoAlarms { get { return WarningRaw == 0x30 ? true : false; } }
                public string WarningString { get
                    {
                        switch (WarningRaw)
                        {
                            case 0x30: return "Предупреждений нет";                       //"No Alarm";
                            case 0x31: return "Низкий уровень чернил";                    //"Ink Low Warning";
                            case 0x32: return "Низкий уровень растворителя";              //"Makeup ink Low Warning";
                            case 0x33: return "Срок годности чернил закончился";          //"Ink Shelf Life Exceeded";
                            case 0x34: return "Низкий заряд встроенной батареи (M)";      //"Battery Low M";
                            case 0x35: return "Высокое давление чернил";                  //"Ink Pressure High";
                            case 0x36: return "Ошибка подстройки скорости печати под конвейер"; //Product Speed Matching Error";
                            case 0x37: return "Ошибка коммуникации с внешними устройствами"; //"External Communication Error nnn";
                            case 0x38: return "Высокая температура окруж.воздуха";        //"Ambient Temperature Too High";
                            case 0x39: return "Низкая температура окруж.воздуха";         //"Ambient Temperature Too Low";
                            case 0x3a: return "Ошибка подогрева чернил";                  //"Ink heating failure";
                            case 0x3b: return "Ошибка сигнала от внешнего устройства";    //"External Signal Error nnn";
                            case 0x3c: return "Низкое давление чернил";                   //"Ink Pressure Low";
                            case 0x3d: return "Проблема опорного напряжения для возбуждения"; //"Excitation V-ref. Review";
                            case 0x3e: return "Нестабильные показания вязкости";          //"Viscosity Reading Instability";
                            case 0x3f: return "Показания вязкости вне диапазона";         //"Viscosity Readings Out of Range";
                            case 0x40: return "Высокая вязкость чернил";                  //"High Ink Viscosity";
                            case 0x41: return "Низкая вязкость чернил";                   //"Low Ink Viscosity";
                            case 0x42: return "Проблема опорного напряжения для возбуждения 2"; //"Excitation V-ref. Review 2";
                            case 0x44: return "Низкий заряд встроенной батареи (M)";      //"Battery Low C";
                            case 0x45: return "Неправильные данные календаря принтера";   //"Calendar Content Inaccurate";
                            case 0x46: return "Проблема опорного напряжения для возбуждения. Высота символов"; //"Excitation V-ref. Char. height Review";
                            case 0x47: return "См.информацию по срокам хранения чернил";  //"Ink Shelf Life Information";
                            case 0x48: return "См.информацию по срокам хранения растворителя"; //"Makeup Shelf Life Information";
                            case 0x49: return "Ошибка кода модели";                       //"Model-key Failure";
                            case 0x4a: return "Ошибка кода языка";                        //"Language-key Failure";
                            case 0x4c: return "Ошибка кода обновления (апгрейда)";        //"Upgrade-Key Fault";
                            case 0x50: return "Неисправность вентилятора системы циркуляции"; //"Circulation System Cooling Fan Fault";
                            case 0x51: return "Очень высокая температура чернил";         //"Ink Tempurature Too High";
                            default: return "Неизвестный код предупреждения";             //"Unknown Warning Status";
                        }
                    }
                }
                public int ACKs;
                public int NACKs;
                public int RemoteOperationRAW;
                public string RemoteOperationString {  get
                    { switch (RemoteOperationRAW)
                        {
                            case 0x00: return "Циркуляция запущена";                // Operation Start
                            case 0x01: return "Циркуляция остановлена";             // Operation Stop
                            case 0x02: return "Отклоняющее напряжение ВКЛ";         // Deflection Voltage Control (ON)
                            case 0x03: return "Отклюняющее напряжение ВЫКЛ";        // Deflection Voltage Control (OFF)
                            case 0x04: return "Ошибки сброшены";                    // Fault Clear
                            default: return "Неизвестный код RemoteOperation";      // Unknown            
                        }
                    }
                }
                public bool ClockStoppedRAW;
                public string ClockStoppedString { get { return ClockStoppedRAW == true ? "время заморожено" : "текущее время"; } }
            }
            public struct structSettings
            {
                public string IP;
                public int Port;
                public int MsgNo;
                public int GrpNo;
                public structPCString MsgName;
                public int NumberOfPrintItems;
            }
            public struct structPCString
            {
                public string Value;
                public int Length;
                public byte[] RawData;
                public System.DateTime LastUpdate;
            }
        }

        #endregion


        #region Public Methods (w/ Constructor)

        /// <summary>
        /// Initialize Modbus, Logging
        /// </summary>
        /// <param name="_this">Host form handler</param>
        /// <param name="_IP">Printer IPv4 Address</param>
        /// <param name="_Port">Printer Port</param>
        /// <param name="_MsgNo">Deprecated: Working Message Number</param>
        public CPrinter(System.Windows.Forms.Form _this, string _IP = "192.168.0.1", int _Port = 502)
        {
            if (IPAddress.TryParse(_IP, out IPAddress addr))
                Settings.IP = addr.ToString();
            else Settings.IP = "192.168.0.1";
            Settings.Port = _Port;

            LogFile = new StreamWriter(file_path, append: true);
            LogFile.AutoFlush = true;

            MB = new Modbus_DLL.Modbus(_this);
            MB.Log += MB_LogHandler;
            MB.Complete += MB_CompleteHandler;
            MB.LogIO = true;
            MB.LogAllIO = true;
            MB.LogIOSpacer = ",";
        }

        public bool Begin()
        {
            // Connect to the Printer (& set it Online, verify we're connected)
            bool result = MB.Connect(Settings.IP, Settings.Port);
            result &= ReloadOnlineStatus();
            result &= isonline;

            if (result)
            {
                // Store current Message Number, Group and Name into Setings
                result &= ReadMessageStorageData();

                // Get Print State
                result &= ReadPCString(out pcstring);
                result &= ReloadStatus();
                result &= ReloadCounter();
                result &= ReloadCalendarTime();
            }
            return result;
        }

        public bool End()
        {
            bool result = MB.IsConnected;
            if (result)
            {
                result &= ReloadOnlineStatus();
                result &= isonline;
                if (result)
                {
                    // Reload Printer State (bc why not?)
                    result &= ReadPCString(out pcstring);
                    result &= ReloadCounter();

                    // Return control to Local Display (just in case)
                    result &= MB.SetAttribute(ccIJP.Online_Offline, 0);
                    result &= ReloadOnlineStatus();

                    // Disconnect now
                    MB.Disconnect();

                    status.Online = false;
                    status.Reception = false;
                    status.StatusRaw = -1;
                    status.WarningRaw = -1;
                }                
            }
            return result;
        }

        public bool SetOnline(bool SetOnlineState = true)
        {
            bool result = MB.IsConnected;

            if (result)
            {
                if (SetOnlineState)
                {
                    result &= MB.SetAttribute(ccIJP.Online_Offline, 1);
                }
                else
                {
                    result &= MB.SetAttribute(ccIJP.Online_Offline, 0);
                }

                result &= ReloadOnlineStatus();

                if (isonline)
                    status.Online = true;
                else
                    status.Online = false;
            }
            return result;
        }

        public bool SetClockResume(bool SetResume = true)
        {
            bool result = MB.IsConnected && isonline && (!status.CirculationInProgress);
            if (result)
            {
                byte[] value = { 0x00, (byte)(SetResume ? 0x00 : 0x01) };
                result &= MB.SetAttribute(0x01, (int)ccES.Calendar_Time_Control, value);
            }            
            return result;
        }

        /// <summary>
        /// Sync Printer Clock
        /// </summary>
        /// <param name="_dt">use System.DateTime.Now</param>
        /// <returns>true if operation was successful, false otherwise</returns>
        public bool SetCurrentTime(DateTime _dt)
        {
            bool result = MB.IsConnected && isonline && (!status.CirculationInProgress);
            if (result)
            {
                byte[] value = { HiByte(_dt.Year),  LoByte(_dt.Year),
                                 HiByte(_dt.Month), LoByte(_dt.Month),
                                 HiByte(_dt.Day),   LoByte(_dt.Day),
                                 HiByte(_dt.Hour),  LoByte(_dt.Hour),
                                 HiByte(_dt.Minute), LoByte(_dt.Minute),
                                 HiByte(_dt.Second), LoByte(_dt.Second)            };
                result &= MB.SetAttribute(0x01, (int)ccES.Current_Time_Year, value);
            }
            return result;
        }

        public bool SetNewFormattedString(int _lot, DateTime _dt = default(DateTime))
        {
            bool result = MB.IsConnected && isonline && (!status.CirculationInProgress);

            if (result)
            {
                // 1. First of all check and delete all unnecessary printing items
                if (Settings.NumberOfPrintItems > 1)
                {
                    DeleteEverythingInTheCurrentMessage();
                    result &= MB.GetAttribute(Modbus_DLL.Modbus.FunctionCode.ReadHolding, 0x01, (int)ccIDX.Number_Of_Items, 2, out byte[] output);
                    result &= (output != null) && (output.Length == 2);
                    if (result)
                    {
                        Settings.NumberOfPrintItems = (output[0] << 8) + output[1];
                    }
                }

                // 2. Get the template and modify it
                CDataTypes.structPCString convert = new CDataTypes.structPCString();
                GetPrintStringTemplate(_lot, out convert.RawData);

                string str_Lot;
                if (_lot <= 999)
                {
                    str_Lot = _lot.ToString("000");
                }
                else    // if (_lot >=1000)
                {
                    str_Lot = _lot.ToString("0000");
                }

                for (int i = 0; i < str_Lot.Length; i++)
                {
                    convert.RawData[72 + i * 4] = 0x00;
                    convert.RawData[73 + i * 4] = 0x00;
                    convert.RawData[74 + i * 4] = 0x00;
                    convert.RawData[75 + i * 4] = (byte)str_Lot[i];
                }
                convert.Length = convert.RawData.Length / 4;
                convert.Value = GetStringFromRaw(convert.RawData);

                // 3. Send raw data to printer
                result &= WritePCString(convert);

                // 4. Freeze time (in {{DD}.{MM}.{YY}} fields)
                if (!(_dt == default(DateTime)))
                {
                    result &= SetCalendarTime(_dt);
                }
                else
                {
                    result &= SetClockResume(SetResume: true);
                }

                // 5. Check back the string written
                result &= ReadPCString(out pcstring);
                
                //not possible, I think:
                //result &= pcstring.RawData.SequenceEqual(convert.RawData);
            }
            return result;
        }

        public bool SetRemoteOperation(bool SetOperationStart = true, int RemoteOperationRAW = int.MinValue)
        {
            // 0: Operation start       // 2: Deflection voltage on            // 4: Fault clear
            // 1: Operation stop        // 3: Deflection voltage off
            bool result = MB.IsConnected && isonline && (!status.CirculationInProgress);
            if (result)
            {
                byte[] value;
                if (RemoteOperationRAW != int.MinValue)
                    value = new byte[] { 0x00, (byte)RemoteOperationRAW };
                else
                    value = new byte[] { 0x00, (byte)(SetOperationStart ? 0x00 : 0x01) };
                result &= MB.SetAttribute(0x01, (int)ccIJP.Remote_operation, value);
            }
            return result;
        }

        public bool ReloadString()
        {
            bool result = MB.IsConnected && isonline;

            if (result)
            {
                result &= ReadPCString(out pcstring);
            }

            return result;
        }

        public bool ReloadStatus()
        {
            bool result = MB.IsConnected && isonline;

            if (result)
            {
                result &= MB.GetAttribute(Modbus_DLL.Modbus.FunctionCode.ReadInput, 0x01, (int)ccUS.Communication_Status, 8, out byte[] output);
                result &= (output != null) && (output.Length == 8);
                if (result)
                {
                    status.Online = (output[0] << 8) + output[1] == 0x0031 ? true : false;
                    status.Reception = (output[2] << 8) + output[3] == 0x0031 ? true : false;
                    status.StatusRaw = (output[4] << 8) + output[5];
                    status.WarningRaw = (output[6] << 8) + output[7];
                }
                result &= MB.GetAttribute(Modbus_DLL.Modbus.FunctionCode.ReadHolding, 0x01, (int)ccIJP.Remote_operation, 2, out output);
                result &= (output != null) && (output.Length == 2);
                if (result)
                {
                    status.RemoteOperationRAW = (output[0] << 8) + output[1];
                }
            }
            return result;
        }

        public bool ReloadCounter()
        {
            bool result = MB.IsConnected && isonline;

            if (result)
            {
                result &= MB.GetAttribute(Modbus_DLL.Modbus.FunctionCode.ReadHolding, 0x01, (int)ccOM.Print_Count, 4, out byte[] output);
                result &= (output != null) && (output.Length == 4);
                if (result)
                {
                    Counter = (output[0] << 24) + (output[1] << 16) + (output[2] << 8) + (output[3]);
                }
            }
            return result;
        }

        public bool ReloadOnlineStatus()
        {
            bool result = MB.IsConnected;
            if (result)
            {
                result &= MB.GetAttribute(Modbus_DLL.Modbus.FunctionCode.ReadHolding, 0x01, (int)ccIJP.Online_Offline, 2, out byte[] data);
                result &= (data != null) && (data.Length == 2);
                if (result)
                    isonline = ((data[0] << 8) + data[1]) == 0x01 ? true : false;
            }
            return result;
        }

        public bool ReloadCalendarTime()
        {
            bool result = MB.IsConnected && isonline;
            if (result)
            {
                result &= MB.GetAttribute(Modbus_DLL.Modbus.FunctionCode.ReadHolding, 0x01, (int)ccES.Calendar_Time_Control, 2, out byte[] output);
                result &= (output != null) && (output.Length == 2);
                if (result)
                {
                    status.ClockStoppedRAW = ((output[0] << 8) + output[1] == 1) ? true : false;
                }
            }
            return result;
        }

        public bool StorePrintingMessageIntoFlashMemory()
        {
            bool result = MB.IsConnected && isonline && (!status.CirculationInProgress);
            if (result)
            {
                byte[] value = new byte[28];
                value[0] = HiByte(Settings.GrpNo);  value[1] = LoByte(Settings.GrpNo);
                value[2] = HiByte(Settings.MsgNo);  value[3] = LoByte(Settings.MsgNo);
                for (int i = 0; i < Settings.MsgName.RawData.Length; i++)
                    value[4 + i] = Settings.MsgName.RawData[i];
                result &= MB.SetAttribute(ccIDX.Start_Stop_Management_Flag, 1);
                result &= MB.SetAttribute(0x01, (int)ccPDR.Group_Number, value);
                result &= MB.SetAttribute(ccIDX.Start_Stop_Management_Flag, 2);

                //// Additionally try to rename message
                //value = new byte[26];
                //value[0] = HiByte(Settings.MsgNo); value[1] = LoByte(Settings.MsgNo);
                //for (int i = 0; i < 24; i++)
                //    value[2 + i] = Settings.MsgName.RawData[i];
                //result &= MB.SetAttribute(ccIDX.Start_Stop_Management_Flag, 1);
                //result &= MB.SetAttribute(0x01, (int)ccPDM.Change_Message_Name, value);
                //result &= MB.SetAttribute(ccIDX.Start_Stop_Management_Flag, 2);
            }
            return result;
        }

        #endregion


        #region Private Data

        private Modbus_DLL.Modbus MB;
        private string file_path = @"./modbus_log.csv";
        private StreamWriter LogFile;
        private CDataTypes.structStatus status;
        private CDataTypes.structPCString pcstring;
        private bool isonline;

        #endregion


        #region Private Methods

        private void MB_LogHandler(object sender, string msg)
        {
            DateTime dt = DateTime.Now;
            LogFile.WriteLine(dt.ToString("G") + "." + dt.ToString("fff") + "," + msg);
        }

        private void MB_CompleteHandler(object sender, bool Success)
        {
            if (Success)
            {
                LogFile.WriteLine("Success");
                status.ACKs++;
            }
            else
            {
                LogFile.WriteLine("Failure");
                status.NACKs++;
            }
        }

        private bool ReadMessageStorageData()
        {
            // Write Message Number to 0x10 ("0" for currently selected message)
            // to get it's name, group and number from input register address 0x0E40
            bool result = MB.SetAttribute(ccIDX.Message_Number, 0);     
            result &= MB.GetAttribute(Modbus_DLL.Modbus.FunctionCode.ReadInput, 0x01, (int)ccMM.Message_Number, 28, out byte[] data);            
            result &= (data != null) && (data.Length == 28);
            if (result)
            {
                Settings.MsgNo = (data[0] << 8) + data[1];
                Settings.GrpNo = (data[2] << 8) + data[3];
                Settings.MsgName.RawData = new byte[24];
                Settings.MsgName.Value = "";
                Settings.MsgName.Length = 24;
                for (int i = 0; i < 24; i++)
                {
                    Settings.MsgName.RawData[i] = data[i + 4];
                    if (i % 2 == 1)
                        Settings.MsgName.Value += (char)data[i + 4];
                }
            }
            return result;
        }

        private bool ReadPCString(out CDataTypes.structPCString pcstring)
        {
            // 1. Get Number of Print Items (ideally should be 1)
            bool result = MB.GetAttribute(Modbus_DLL.Modbus.FunctionCode.ReadHolding, 0x01, (int)ccIDX.Number_Of_Items, 2, out byte[] data);
            result &= (data != null) && (data.Length == 2);
            if (result)
            {
                Settings.NumberOfPrintItems = (data[0] << 8) + data[1];
            }

            // 2. Count the length of every print item
            CDataTypes.structPCString output = new CDataTypes.structPCString { Length = 0, Value = "", RawData = null };
            result &= MB.GetAttribute(Modbus_DLL.Modbus.FunctionCode.ReadHolding, 0x01, (int)ccPC.Characters_per_Item,
                                                                                    2 * Settings.NumberOfPrintItems, out data);
            result &= (data != null) && (data.Length == 2 * Settings.NumberOfPrintItems);
            if (result)
            {
                for (int i = 0; i < 2 * Settings.NumberOfPrintItems; i += 2)
                {
                    output.Length += (data[i] << 8) + data[i + 1];
                }
            }

            // 3. Read Print Contents
            if (result)
            { 
                result &= MB.GetAttribute(Modbus_DLL.Modbus.FunctionCode.ReadHolding, 0x01, (int)ccPC.Print_Character_String,
                                                                                    output.Length * 4, out output.RawData);
                result &= (output.RawData != null) && (output.RawData.Length == output.Length * 4);
                if (result)
                {
                    output.Value = GetStringFromRaw(output.RawData);
                    output.LastUpdate = System.DateTime.Now;
                }
            }
            pcstring = output;
            return result;
        }

        private bool WritePCString(CDataTypes.structPCString pcstring)
        {
            // To send several attributes at once we need to keep S/S Management Flag "1"
            bool result = MB.SetAttribute(ccIDX.Start_Stop_Management_Flag, 1);
            byte[] value = { HiByte(pcstring.RawData.Length / 4), LoByte(pcstring.RawData.Length / 4) };
            result &= MB.SetAttribute(0x01, (int)ccPC.Characters_per_Item, value);
            result &= MB.SetAttribute(0x01, (int)ccPC.Print_Character_String, pcstring.RawData);
            //result &= MB.SetAttribute(ccPC.Characters_per_Item, pcstring.Length);
            //result &= MB.SetAttribute(ccPC.Print_Character_String, pcstring.Value);
            result &= MB.SetAttribute(ccIDX.Start_Stop_Management_Flag, 2);
            return result;
        }

        private bool SetCalendarTime(DateTime _dt)
        {
            ////SetClockResume(true);
            ////SetClockResume(false);
            ////byte[] value = {    0x00, 0x01,
            ////                    HiByte(_dt.Year),  LoByte(_dt.Year),
            ////                    HiByte(_dt.Month), LoByte(_dt.Month),
            ////                    HiByte(_dt.Day),   LoByte(_dt.Day),
            ////                    HiByte(_dt.Hour),  LoByte(_dt.Hour),
            ////                    HiByte(_dt.Minute), LoByte(_dt.Minute),
            ////                    HiByte(_dt.Second), LoByte(_dt.Second)        };
            ////bool result = MB.SetAttribute(0x01, (int)ccES.Calendar_Time_Control, value);

            bool result = MB.GetAttribute(Modbus_DLL.Modbus.FunctionCode.ReadHolding, 0x01, (int)ccES.Calendar_Time_Control, 2, out byte[] output);
            result &= (output != null) && (output.Length == 2);
            if (result)
            {
                bool clock_stop = ((output[0] << 8) + output[1] == 1) ? true : false;
                if (!clock_stop)
                {
                    SetClockResume(SetResume: false);
                }

                //System.Threading.Thread.Sleep(100);

                //result &= SetClockResume(SetResume: true);

                byte[] value = {
                                    HiByte(_dt.Year),  LoByte(_dt.Year),
                                    HiByte(_dt.Month), LoByte(_dt.Month),
                                    HiByte(_dt.Day),   LoByte(_dt.Day),
                                    0x00, 0x00,
                                    0x00, 0x00,
                                    0x00, 0x00,        };
                result &= MB.SetAttribute(0x01, (int)ccES.Calendar_Time_Year, value);

                //byte[] value = new byte[2];

                //result &= MB.SetAttribute(ccIDX.Start_Stop_Management_Flag, 1);
                //value[0] = HiByte(_dt.Year); value[1] = LoByte(_dt.Year);
                //result &= MB.SetAttribute(0x01, (int)ccES.Calendar_Time_Year, value);
                //value[0] = HiByte(_dt.Month); value[1] = LoByte(_dt.Month);
                //result &= MB.SetAttribute(0x01, (int)ccES.Calendar_Time_Month, value);
                //value[0] = HiByte(_dt.Day); value[1] = LoByte(_dt.Day);
                //result &= MB.SetAttribute(0x01, (int)ccES.Calendar_Time_Day, value);
                //value[0] = 0x00; value[1] = 0x00;
                //result &= MB.SetAttribute(0x01, (int)ccES.Calendar_Time_Hour, value);
                //result &= MB.SetAttribute(0x01, (int)ccES.Calendar_Time_Minute, value);
                ////result &= MB.SetAttribute(0x01, (int)ccES.Calendar_Time_Second, value);
                //result &= MB.SetAttribute(ccIDX.Start_Stop_Management_Flag, 2);




                //SetClockResume(SetResume: false);

                ////byte[] value = {    HiByte(_dt.Year),  LoByte(_dt.Year),
                ////                    HiByte(_dt.Month), LoByte(_dt.Month),
                ////                    HiByte(_dt.Day),   LoByte(_dt.Day),
                ////                    HiByte(_dt.Hour),  LoByte(_dt.Hour),
                ////                    HiByte(_dt.Minute), LoByte(_dt.Minute),
                ////                    HiByte(_dt.Second), LoByte(_dt.Second)        };
                ////result &= MB.SetAttribute(0x01, (int)ccES.Calendar_Time_Year, value);
            }

            ////bool result = MB.SetAttribute(0x01, (int)ccES.Calendar_Time_Control, new byte[] { 0x00, 0x00 });
            ////result &= MB.SetAttribute(ccIDX.Start_Stop_Management_Flag, 1);
            ////result &= MB.SetAttribute(0x01, (int)ccES.Calendar_Time_Year, value);
            ////result &= MB.SetAttribute(ccIDX.Start_Stop_Management_Flag, 2);
            ////result &= MB.SetAttribute(0x01, (int)ccES.Calendar_Time_Control, new byte[] { 0x00, 0x01 });

            ////result &= ReloadStatus();
            ////result &= MB.SetAttribute(0x01, (int)ccES.Calendar_Time_Control, new byte[] { 0x00, 0x01 });
            return result;
        }

        private byte LoByte(int _int)
        {
            return (byte)(_int & 0xff);
        }

        private byte HiByte(int _int)
        {
            return (byte)((_int >> 8) & 0xff);
        }

        private string GetStringFromRaw(byte[] _raw)
        {
            string result = "";
            if (_raw != null && _raw.Length > 2)
            {
                for (int i = 3; i < _raw.Length; i+=4)
                {
                    result += (char)_raw[i];
                }
            }
            return result;
        }

        private bool DeleteEverythingInTheCurrentMessage()
        {
            bool result = MB.SetAttribute(ccPF.Format_Setup, 1);    // 1="Individual"
            if (result && Settings.NumberOfPrintItems > 1)
            {
                // Count Columns    (Column consists of 1..4 lines. Sum of all lines in all columns equals to the Number of Print Items)
                int columns = 0;
                int print_item_index = 0;
                byte[] output;
                while (print_item_index < Settings.NumberOfPrintItems)
                {
                    //lines_in_column = MB.GetDecAttribute(ccPF.Line_Count, print_item_index);
                    result &= MB.GetAttribute(Modbus_DLL.Modbus.FunctionCode.ReadHolding, 0x01,
                        (int)ccPF.Line_Count + print_item_index * MB.GetAttrData(ccPF.Line_Count).Stride,
                        2, out output);
                    int lines_in_column = (output[0] << 8) + output[1];
                    columns++;
                    print_item_index += lines_in_column;
                }

                for (int i = columns; i > 1; i--)   // Delete all Columns, but keep 1 left
                {
                    result &= MB.SetAttribute(ccPF.Delete_Column, i);
                }

                if (result)
                    result &= MB.SetAttribute(ccPC.Print_Erasure, 1);   
            }
            return result;
        }

        private void GetPrintStringTemplate(int _lot, out byte[] _output)
        {
            if (_lot <= 999)
            {
                _output = new byte[] {    0x00, 0x00, 0x00, 0x44,   // 'D'          //  1       // 00
                                          0x00, 0x00, 0x00, 0x41,   // 'A'          //  2       // 04
                                          0x00, 0x00, 0x00, 0x54,   // 'T'          //  3       // 08
                                          0x00, 0x00, 0x00, 0x45,   // 'E'          //  4       // 12
                                          0x00, 0x00, 0x00, 0x3A,   // ':'          //  5       // 16
                                          0xF2, 0x62, 0x00, 0x00,   //{{D}  + '2'   //  6       // 20
                                          0xF2, 0x52, 0x00, 0x00,   // {D}  + '1'   //  7       // 24
                                          0x00, 0x00, 0x00, 0x2E,   // '.'          //  8       // 28
                                          0xF2, 0x51, 0x00, 0x00,   // {M}  + '1'   //  9       // 32
                                          0xF2, 0x51, 0x00, 0x00,   // {M}  + '1'   // 10       // 36
                                          0x00, 0x00, 0x00, 0x2E,   // '.'          // 11       // 40
                                          0xF2, 0x50, 0x00, 0x00,   // {Y}  + '8'   // 12       // 44
                                          0xF2, 0x70, 0x00, 0x00,   // {Y}} + '5'   // 13       // 48
                                          0x00, 0x00, 0x00, 0x20,   // ' '          // 14       // 52
                                          0x00, 0x00, 0x00, 0x4C,   // 'L'          // 15       // 56
                                          0x00, 0x00, 0x00, 0x4F,   // 'O'          // 16       // 60
                                          0x00, 0x00, 0x00, 0x54,   // 'T'          // 17       // 64 . . .
                                          0x00, 0x00, 0x00, 0x3A,   // ':'          // 18       // 68, 69, 70, 71,                              
                                          0x00, 0x00, 0x00, 0x20,   // ' '          // 19       // 72, 73, 74, 75,
                                          0x00, 0x00, 0x00, 0x20,   // ' '          // 20       // 76, 77, 78, 79,
                                          0x00, 0x00, 0x00, 0x20 }; // ' '          // 21       // 80, 81, 82, 83.
            }
            else    // if (_lot >=1000)
            {
                _output = new byte[] {    0x00, 0x00, 0x00, 0x44,   // 'D'          //  1       // 00
                                          0x00, 0x00, 0x00, 0x41,   // 'A'          //  2       // 04
                                          0x00, 0x00, 0x00, 0x54,   // 'T'          //  3       // 08
                                          0x00, 0x00, 0x00, 0x45,   // 'E'          //  4       // 12
                                          0x00, 0x00, 0x00, 0x3A,   // ':'          //  5       // 16
                                          0xF2, 0x62, 0x00, 0x00,   //{{D}  + '2'   //  6       // 20
                                          0xF2, 0x52, 0x00, 0x00,   // {D}  + '1'   //  7       // 24
                                          0x00, 0x00, 0x00, 0x2E,   // '.'          //  8       // 28
                                          0xF2, 0x51, 0x00, 0x00,   // {M}  + '1'   //  9       // 32
                                          0xF2, 0x51, 0x00, 0x00,   // {M}  + '1'   // 10       // 36
                                          0x00, 0x00, 0x00, 0x2E,   // '.'          // 11       // 40
                                          0xF2, 0x50, 0x00, 0x00,   // {Y}  + '8'   // 12       // 44
                                          0xF2, 0x70, 0x00, 0x00,   // {Y}} + '5'   // 13       // 48
                                          0x00, 0x00, 0x00, 0x20,   // ' '          // 14       // 52
                                          0x00, 0x00, 0x00, 0x4C,   // 'L'          // 15       // 56
                                          0x00, 0x00, 0x00, 0x4F,   // 'O'          // 16       // 60
                                          0x00, 0x00, 0x00, 0x54,   // 'T'          // 17       // 64 . . .
                                          0x00, 0x00, 0x00, 0x3A,   // ':'          // 18       // 68, 69, 70, 71,                              
                                          0x00, 0x00, 0x00, 0x20,   // ' '          // 19       // 72, 73, 74, 75,
                                          0x00, 0x00, 0x00, 0x20,   // ' '          // 20       // 76, 77, 78, 79,
                                          0x00, 0x00, 0x00, 0x20,   // ' '          // 21       // 80, 81, 82, 83,
                                          0x00, 0x00, 0x00, 0x20 }; // ' '          // 22       // 84, 85, 86, 87.
            }
                //0x00, 0x00, 0x00, 0x36,   // '6'          // 19       // 72, 73, 74, 75,
                //0x00, 0x00, 0x00, 0x36,   // '6'          // 20       // 76, 77, 78, 79,
                //0x00, 0x00, 0x00, 0x36 }; // '6'          // 21       // 80, 81, 82, 83
        }
        ~CPrinter()
        {
            if (MB.IsConnected)
            {
                MB.SetAttribute(ccIJP.Online_Offline, 0);
            }
            MB.Disconnect();
        }

        #endregion

    }
}
