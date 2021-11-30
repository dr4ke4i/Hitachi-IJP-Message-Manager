
namespace Hitachi_IJP_Message_Manager
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.lbl_IP = new System.Windows.Forms.Label();
            this.lbl_Port = new System.Windows.Forms.Label();
            this.chboxConnectOnLaunch = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblMarkedBags = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblBrokenBags = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblMaintBags = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmboxVolume = new System.Windows.Forms.ComboBox();
            this.btnBrokenDEC = new System.Windows.Forms.Button();
            this.btnBrokenINC = new System.Windows.Forms.Button();
            this.tboxBrokenBags = new System.Windows.Forms.TextBox();
            this.btn_tbBrokenBagsConfirm = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.lblBatchStartCounter = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblPrinterPrintCount = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblBatchEndCounter = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblVolume = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblBatchLotNo = new System.Windows.Forms.Label();
            this.btn_tbBatchLotNoConfirm = new System.Windows.Forms.Button();
            this.tboxBatchLotNo = new System.Windows.Forms.TextBox();
            this.chboxBatchLotNoAutoINC = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.lblBatchNextLotNo = new System.Windows.Forms.Label();
            this.chboxBatchDateAUTO = new System.Windows.Forms.CheckBox();
            this.lblBatchDate = new System.Windows.Forms.Label();
            this.lblBatchNextDate = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.dtpickBatchDate = new System.Windows.Forms.DateTimePicker();
            this.tboxPrinterPCString = new System.Windows.Forms.TextBox();
            this.lboxLog = new System.Windows.Forms.ListBox();
            this.cmstrip_lbLog = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tlstr_miSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.tlstr_miClear = new System.Windows.Forms.ToolStripMenuItem();
            this.tboxTimerTickPeriod = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.btnStartNewBatch = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.lblPrinterStatusOnline = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.btnStartMaintenance = new System.Windows.Forms.Button();
            this.label19 = new System.Windows.Forms.Label();
            this.lblPrinterOnlineOffline = new System.Windows.Forms.Label();
            this.btnReWriteStringToPrinter = new System.Windows.Forms.Button();
            this.label20 = new System.Windows.Forms.Label();
            this.lblPrinterACKs = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.lblPrinterNACKs = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.lblPCStringLastUpdateTime = new System.Windows.Forms.Label();
            this.tboxBatchNextLotNo = new System.Windows.Forms.TextBox();
            this.btn_tbBatchNextLotNoConfirm = new System.Windows.Forms.Button();
            this.dtpickBatchNextDate = new System.Windows.Forms.DateTimePicker();
            this.btnStorePrintingMessageIntoMemory = new System.Windows.Forms.Button();
            this.btnSetRemoteOperationStart = new System.Windows.Forms.Button();
            this.btnSetRemoteOperationStop = new System.Windows.Forms.Button();
            this.btnExpandWindowForm = new System.Windows.Forms.Button();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.lblLastCycleMillis = new System.Windows.Forms.Label();
            this.lblPrinterStatusErrorText = new System.Windows.Forms.Label();
            this.lblPrinterStatusWarningText = new System.Windows.Forms.Label();
            this.btnStopCalendarTimeResume = new System.Windows.Forms.Button();
            this.lblPrinterRemoteOperation = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.lblPrinterHoldOnActive = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.lblPrinterStatusClockStopped = new System.Windows.Forms.Label();
            this.lblProgramHoldOn = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.btn_cmboxNewBatchVolumeConfirm = new System.Windows.Forms.Button();
            this.btn_cmboxDeleteBatchVolumeItem = new System.Windows.Forms.Button();
            this.cmstrip_lbLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmrUpdate
            // 
            this.tmrUpdate.Interval = 1000;
            this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Tick);
            // 
            // btnConnect
            // 
            this.btnConnect.AutoSize = true;
            this.btnConnect.Location = new System.Drawing.Point(889, 33);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(121, 27);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Подключение";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.AutoSize = true;
            this.btnDisconnect.Location = new System.Drawing.Point(1016, 33);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(135, 27);
            this.btnDisconnect.TabIndex = 1;
            this.btnDisconnect.Text = "Отключение";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // lbl_IP
            // 
            this.lbl_IP.AutoSize = true;
            this.lbl_IP.Location = new System.Drawing.Point(886, 13);
            this.lbl_IP.Name = "lbl_IP";
            this.lbl_IP.Size = new System.Drawing.Size(42, 17);
            this.lbl_IP.TabIndex = 2;
            this.lbl_IP.Text = "lbl_IP";
            // 
            // lbl_Port
            // 
            this.lbl_Port.AutoSize = true;
            this.lbl_Port.Location = new System.Drawing.Point(1013, 13);
            this.lbl_Port.Name = "lbl_Port";
            this.lbl_Port.Size = new System.Drawing.Size(56, 17);
            this.lbl_Port.TabIndex = 2;
            this.lbl_Port.Text = "lbl_Port";
            // 
            // chboxConnectOnLaunch
            // 
            this.chboxConnectOnLaunch.Location = new System.Drawing.Point(889, 66);
            this.chboxConnectOnLaunch.Name = "chboxConnectOnLaunch";
            this.chboxConnectOnLaunch.Size = new System.Drawing.Size(281, 21);
            this.chboxConnectOnLaunch.TabIndex = 3;
            this.chboxConnectOnLaunch.Text = "Подключать при старте приложения";
            this.chboxConnectOnLaunch.UseVisualStyleBackColor = true;
            this.chboxConnectOnLaunch.CheckedChanged += new System.EventHandler(this.chboxConnectOnLaunch_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Маркированные:";
            // 
            // lblMarkedBags
            // 
            this.lblMarkedBags.AutoSize = true;
            this.lblMarkedBags.Location = new System.Drawing.Point(180, 52);
            this.lblMarkedBags.Name = "lblMarkedBags";
            this.lblMarkedBags.Size = new System.Drawing.Size(101, 17);
            this.lblMarkedBags.TabIndex = 5;
            this.lblMarkedBags.Text = "lblMarkedBags";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Испорченные:";
            // 
            // lblBrokenBags
            // 
            this.lblBrokenBags.AutoSize = true;
            this.lblBrokenBags.Location = new System.Drawing.Point(180, 84);
            this.lblBrokenBags.Name = "lblBrokenBags";
            this.lblBrokenBags.Size = new System.Drawing.Size(99, 17);
            this.lblBrokenBags.TabIndex = 5;
            this.lblBrokenBags.Text = "lblBrokenBags";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(158, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Пропущенные при ТО:";
            // 
            // lblMaintBags
            // 
            this.lblMaintBags.AutoSize = true;
            this.lblMaintBags.Location = new System.Drawing.Point(180, 116);
            this.lblMaintBags.Name = "lblMaintBags";
            this.lblMaintBags.Size = new System.Drawing.Size(88, 17);
            this.lblMaintBags.TabIndex = 5;
            this.lblMaintBags.Text = "lblMaintBags";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 148);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 17);
            this.label4.TabIndex = 4;
            this.label4.Text = "Объём партии:";
            // 
            // cmboxVolume
            // 
            this.cmboxVolume.FormattingEnabled = true;
            this.cmboxVolume.Location = new System.Drawing.Point(180, 145);
            this.cmboxVolume.Name = "cmboxVolume";
            this.cmboxVolume.Size = new System.Drawing.Size(121, 24);
            this.cmboxVolume.TabIndex = 6;
            this.cmboxVolume.SelectionChangeCommitted += new System.EventHandler(this.cmboxVolume_SelectionChangeCommitted);
            this.cmboxVolume.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmboxVolume_KeyDown);
            this.cmboxVolume.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmboxVolume_KeyPress);
            // 
            // btnBrokenDEC
            // 
            this.btnBrokenDEC.AutoSize = true;
            this.btnBrokenDEC.Location = new System.Drawing.Point(300, 79);
            this.btnBrokenDEC.Name = "btnBrokenDEC";
            this.btnBrokenDEC.Size = new System.Drawing.Size(31, 27);
            this.btnBrokenDEC.TabIndex = 7;
            this.btnBrokenDEC.Text = "-1";
            this.btnBrokenDEC.UseVisualStyleBackColor = true;
            this.btnBrokenDEC.Click += new System.EventHandler(this.btnBrokenDEC_Click);
            // 
            // btnBrokenINC
            // 
            this.btnBrokenINC.AutoSize = true;
            this.btnBrokenINC.Location = new System.Drawing.Point(337, 79);
            this.btnBrokenINC.Name = "btnBrokenINC";
            this.btnBrokenINC.Size = new System.Drawing.Size(34, 27);
            this.btnBrokenINC.TabIndex = 7;
            this.btnBrokenINC.Text = "+1";
            this.btnBrokenINC.UseVisualStyleBackColor = true;
            this.btnBrokenINC.Click += new System.EventHandler(this.btnBrokenINC_Click);
            // 
            // tboxBrokenBags
            // 
            this.tboxBrokenBags.Location = new System.Drawing.Point(425, 81);
            this.tboxBrokenBags.Name = "tboxBrokenBags";
            this.tboxBrokenBags.Size = new System.Drawing.Size(62, 22);
            this.tboxBrokenBags.TabIndex = 8;
            this.tboxBrokenBags.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tboxBrokenBags_KeyDown);
            this.tboxBrokenBags.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tboxBrokenBags_KeyPress);
            // 
            // btn_tbBrokenBagsConfirm
            // 
            this.btn_tbBrokenBagsConfirm.AutoSize = true;
            this.btn_tbBrokenBagsConfirm.Location = new System.Drawing.Point(493, 79);
            this.btn_tbBrokenBagsConfirm.Name = "btn_tbBrokenBagsConfirm";
            this.btn_tbBrokenBagsConfirm.Size = new System.Drawing.Size(38, 27);
            this.btn_tbBrokenBagsConfirm.TabIndex = 7;
            this.btn_tbBrokenBagsConfirm.Text = "OK";
            this.btn_tbBrokenBagsConfirm.UseVisualStyleBackColor = true;
            this.btn_tbBrokenBagsConfirm.Click += new System.EventHandler(this.btn_tbBrokenBagsConfirm_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(886, 102);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(113, 17);
            this.label5.TabIndex = 4;
            this.label5.Text = "Начало партии:";
            // 
            // lblBatchStartCounter
            // 
            this.lblBatchStartCounter.AutoSize = true;
            this.lblBatchStartCounter.Location = new System.Drawing.Point(1013, 102);
            this.lblBatchStartCounter.Name = "lblBatchStartCounter";
            this.lblBatchStartCounter.Size = new System.Drawing.Size(138, 17);
            this.lblBatchStartCounter.TabIndex = 5;
            this.lblBatchStartCounter.Text = "lblBatchStartCounter";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(886, 136);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 17);
            this.label6.TabIndex = 4;
            this.label6.Text = "Текущий:";
            // 
            // lblPrinterPrintCount
            // 
            this.lblPrinterPrintCount.AutoSize = true;
            this.lblPrinterPrintCount.Location = new System.Drawing.Point(1013, 136);
            this.lblPrinterPrintCount.Name = "lblPrinterPrintCount";
            this.lblPrinterPrintCount.Size = new System.Drawing.Size(130, 17);
            this.lblPrinterPrintCount.TabIndex = 5;
            this.lblPrinterPrintCount.Text = "lblPrinterPrintCount";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(886, 170);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(104, 17);
            this.label7.TabIndex = 4;
            this.label7.Text = "Конец партии:";
            // 
            // lblBatchEndCounter
            // 
            this.lblBatchEndCounter.AutoSize = true;
            this.lblBatchEndCounter.Location = new System.Drawing.Point(1013, 170);
            this.lblBatchEndCounter.Name = "lblBatchEndCounter";
            this.lblBatchEndCounter.Size = new System.Drawing.Size(133, 17);
            this.lblBatchEndCounter.TabIndex = 5;
            this.lblBatchEndCounter.Text = "lblBatchEndCounter";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(297, 52);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(12, 17);
            this.label8.TabIndex = 4;
            this.label8.Text = "/";
            // 
            // lblVolume
            // 
            this.lblVolume.AutoSize = true;
            this.lblVolume.Location = new System.Drawing.Point(315, 52);
            this.lblVolume.Name = "lblVolume";
            this.lblVolume.Size = new System.Drawing.Size(69, 17);
            this.lblVolume.TabIndex = 5;
            this.lblVolume.Text = "lblVolume";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 213);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(106, 17);
            this.label9.TabIndex = 9;
            this.label9.Text = "Номер партии:";
            // 
            // lblBatchLotNo
            // 
            this.lblBatchLotNo.AutoSize = true;
            this.lblBatchLotNo.Location = new System.Drawing.Point(177, 213);
            this.lblBatchLotNo.Name = "lblBatchLotNo";
            this.lblBatchLotNo.Size = new System.Drawing.Size(96, 17);
            this.lblBatchLotNo.TabIndex = 5;
            this.lblBatchLotNo.Text = "lblBatchLotNo";
            // 
            // btn_tbBatchLotNoConfirm
            // 
            this.btn_tbBatchLotNoConfirm.AutoSize = true;
            this.btn_tbBatchLotNoConfirm.Location = new System.Drawing.Point(366, 208);
            this.btn_tbBatchLotNoConfirm.Name = "btn_tbBatchLotNoConfirm";
            this.btn_tbBatchLotNoConfirm.Size = new System.Drawing.Size(38, 27);
            this.btn_tbBatchLotNoConfirm.TabIndex = 7;
            this.btn_tbBatchLotNoConfirm.Text = "OK";
            this.btn_tbBatchLotNoConfirm.UseVisualStyleBackColor = true;
            this.btn_tbBatchLotNoConfirm.Click += new System.EventHandler(this.btn_tbBatchLotNoConfirm_Click);
            // 
            // tboxBatchLotNo
            // 
            this.tboxBatchLotNo.Location = new System.Drawing.Point(298, 210);
            this.tboxBatchLotNo.Name = "tboxBatchLotNo";
            this.tboxBatchLotNo.Size = new System.Drawing.Size(62, 22);
            this.tboxBatchLotNo.TabIndex = 8;
            this.tboxBatchLotNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tboxBatchLotNo_KeyDown);
            this.tboxBatchLotNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tboxBatchLotNo_KeyPress);
            // 
            // chboxBatchLotNoAutoINC
            // 
            this.chboxBatchLotNoAutoINC.AutoSize = true;
            this.chboxBatchLotNoAutoINC.Location = new System.Drawing.Point(12, 276);
            this.chboxBatchLotNoAutoINC.Name = "chboxBatchLotNoAutoINC";
            this.chboxBatchLotNoAutoINC.Size = new System.Drawing.Size(185, 21);
            this.chboxBatchLotNoAutoINC.TabIndex = 10;
            this.chboxBatchLotNoAutoINC.Text = "Автоинкремент номера";
            this.chboxBatchLotNoAutoINC.UseVisualStyleBackColor = true;
            this.chboxBatchLotNoAutoINC.CheckedChanged += new System.EventHandler(this.chboxBatchLotNoAutoINC_CheckedChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 246);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(118, 17);
            this.label10.TabIndex = 11;
            this.label10.Text = "След. № партии:";
            // 
            // lblBatchNextLotNo
            // 
            this.lblBatchNextLotNo.AutoSize = true;
            this.lblBatchNextLotNo.Location = new System.Drawing.Point(175, 246);
            this.lblBatchNextLotNo.Name = "lblBatchNextLotNo";
            this.lblBatchNextLotNo.Size = new System.Drawing.Size(124, 17);
            this.lblBatchNextLotNo.TabIndex = 5;
            this.lblBatchNextLotNo.Text = "lblBatchNextLotNo";
            // 
            // chboxBatchDateAUTO
            // 
            this.chboxBatchDateAUTO.AutoSize = true;
            this.chboxBatchDateAUTO.Location = new System.Drawing.Point(12, 402);
            this.chboxBatchDateAUTO.Name = "chboxBatchDateAUTO";
            this.chboxBatchDateAUTO.Size = new System.Drawing.Size(174, 21);
            this.chboxBatchDateAUTO.TabIndex = 10;
            this.chboxBatchDateAUTO.Text = "Автоматическая дата";
            this.chboxBatchDateAUTO.UseVisualStyleBackColor = true;
            this.chboxBatchDateAUTO.CheckedChanged += new System.EventHandler(this.chboxBatchDateAUTO_CheckedChanged);
            // 
            // lblBatchDate
            // 
            this.lblBatchDate.AutoSize = true;
            this.lblBatchDate.Location = new System.Drawing.Point(177, 338);
            this.lblBatchDate.Name = "lblBatchDate";
            this.lblBatchDate.Size = new System.Drawing.Size(88, 17);
            this.lblBatchDate.TabIndex = 5;
            this.lblBatchDate.Text = "lblBatchDate";
            // 
            // lblBatchNextDate
            // 
            this.lblBatchNextDate.AutoSize = true;
            this.lblBatchNextDate.Location = new System.Drawing.Point(175, 371);
            this.lblBatchNextDate.Name = "lblBatchNextDate";
            this.lblBatchNextDate.Size = new System.Drawing.Size(116, 17);
            this.lblBatchNextDate.TabIndex = 5;
            this.lblBatchNextDate.Text = "lblBatchNextDate";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(12, 338);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(97, 17);
            this.label13.TabIndex = 9;
            this.label13.Text = "Дата партии:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(12, 371);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(135, 17);
            this.label14.TabIndex = 11;
            this.label14.Text = "След. дата партии:";
            // 
            // dtpickBatchDate
            // 
            this.dtpickBatchDate.CustomFormat = "HH:mm:ss dd.MM.yyyy";
            this.dtpickBatchDate.Location = new System.Drawing.Point(299, 333);
            this.dtpickBatchDate.Name = "dtpickBatchDate";
            this.dtpickBatchDate.Size = new System.Drawing.Size(202, 22);
            this.dtpickBatchDate.TabIndex = 12;
            this.dtpickBatchDate.ValueChanged += new System.EventHandler(this.dtpickBatchDate_ValueChanged);
            // 
            // tboxPrinterPCString
            // 
            this.tboxPrinterPCString.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tboxPrinterPCString.Location = new System.Drawing.Point(12, 461);
            this.tboxPrinterPCString.Name = "tboxPrinterPCString";
            this.tboxPrinterPCString.ReadOnly = true;
            this.tboxPrinterPCString.Size = new System.Drawing.Size(486, 30);
            this.tboxPrinterPCString.TabIndex = 13;
            // 
            // lboxLog
            // 
            this.lboxLog.ContextMenuStrip = this.cmstrip_lbLog;
            this.lboxLog.FormattingEnabled = true;
            this.lboxLog.ItemHeight = 16;
            this.lboxLog.Location = new System.Drawing.Point(12, 540);
            this.lboxLog.Name = "lboxLog";
            this.lboxLog.ScrollAlwaysVisible = true;
            this.lboxLog.Size = new System.Drawing.Size(841, 148);
            this.lboxLog.TabIndex = 14;
            // 
            // cmstrip_lbLog
            // 
            this.cmstrip_lbLog.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmstrip_lbLog.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlstr_miSaveAs,
            this.tlstr_miClear});
            this.cmstrip_lbLog.Name = "cmstrip_lbLog";
            this.cmstrip_lbLog.Size = new System.Drawing.Size(162, 52);
            // 
            // tlstr_miSaveAs
            // 
            this.tlstr_miSaveAs.Name = "tlstr_miSaveAs";
            this.tlstr_miSaveAs.Size = new System.Drawing.Size(161, 24);
            this.tlstr_miSaveAs.Text = "Сохранить...";
            this.tlstr_miSaveAs.Click += new System.EventHandler(this.tlstr_miSaveAs_Click);
            // 
            // tlstr_miClear
            // 
            this.tlstr_miClear.Name = "tlstr_miClear";
            this.tlstr_miClear.Size = new System.Drawing.Size(161, 24);
            this.tlstr_miClear.Text = "Очистить";
            this.tlstr_miClear.Click += new System.EventHandler(this.tlstr_miClear_Click);
            // 
            // tboxTimerTickPeriod
            // 
            this.tboxTimerTickPeriod.Location = new System.Drawing.Point(1049, 618);
            this.tboxTimerTickPeriod.Name = "tboxTimerTickPeriod";
            this.tboxTimerTickPeriod.Size = new System.Drawing.Size(100, 22);
            this.tboxTimerTickPeriod.TabIndex = 15;
            this.tboxTimerTickPeriod.Text = "1000";
            this.tboxTimerTickPeriod.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tboxTimerTickPeriod.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tboxTimerTickPeriod_KeyDown);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(886, 621);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(121, 17);
            this.label11.TabIndex = 16;
            this.label11.Text = "Цикл опроса, мс:";
            // 
            // btnStartNewBatch
            // 
            this.btnStartNewBatch.AutoSize = true;
            this.btnStartNewBatch.Location = new System.Drawing.Point(12, 12);
            this.btnStartNewBatch.Name = "btnStartNewBatch";
            this.btnStartNewBatch.Size = new System.Drawing.Size(163, 27);
            this.btnStartNewBatch.TabIndex = 17;
            this.btnStartNewBatch.Text = "Начать новую партию";
            this.btnStartNewBatch.UseVisualStyleBackColor = true;
            this.btnStartNewBatch.Click += new System.EventHandler(this.btnStartNewBatch_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(886, 239);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(57, 17);
            this.label12.TabIndex = 18;
            this.label12.Text = "Статус:";
            // 
            // lblPrinterStatusOnline
            // 
            this.lblPrinterStatusOnline.Location = new System.Drawing.Point(1009, 238);
            this.lblPrinterStatusOnline.Name = "lblPrinterStatusOnline";
            this.lblPrinterStatusOnline.Size = new System.Drawing.Size(145, 17);
            this.lblPrinterStatusOnline.TabIndex = 5;
            this.lblPrinterStatusOnline.Text = "lblPrinterStatusOnline";
            this.lblPrinterStatusOnline.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(942, 239);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(49, 17);
            this.label15.TabIndex = 18;
            this.label15.Text = "Online";
            // 
            // btnStartMaintenance
            // 
            this.btnStartMaintenance.AutoSize = true;
            this.btnStartMaintenance.Location = new System.Drawing.Point(892, 455);
            this.btnStartMaintenance.Name = "btnStartMaintenance";
            this.btnStartMaintenance.Size = new System.Drawing.Size(262, 27);
            this.btnStartMaintenance.TabIndex = 17;
            this.btnStartMaintenance.Text = "Начать тех.обслуживание";
            this.btnStartMaintenance.UseVisualStyleBackColor = true;
            this.btnStartMaintenance.Click += new System.EventHandler(this.btnStartMaintenance_Click);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(886, 210);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(63, 17);
            this.label19.TabIndex = 18;
            this.label19.Text = "Онлайн:";
            // 
            // lblPrinterOnlineOffline
            // 
            this.lblPrinterOnlineOffline.Location = new System.Drawing.Point(1008, 210);
            this.lblPrinterOnlineOffline.Name = "lblPrinterOnlineOffline";
            this.lblPrinterOnlineOffline.Size = new System.Drawing.Size(146, 17);
            this.lblPrinterOnlineOffline.TabIndex = 5;
            this.lblPrinterOnlineOffline.Text = "lblPrinterOnlineOffline";
            this.lblPrinterOnlineOffline.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnReWriteStringToPrinter
            // 
            this.btnReWriteStringToPrinter.AutoSize = true;
            this.btnReWriteStringToPrinter.Location = new System.Drawing.Point(12, 497);
            this.btnReWriteStringToPrinter.Name = "btnReWriteStringToPrinter";
            this.btnReWriteStringToPrinter.Size = new System.Drawing.Size(486, 27);
            this.btnReWriteStringToPrinter.TabIndex = 19;
            this.btnReWriteStringToPrinter.Text = "Отправить выбранную дату и партию в принтер";
            this.btnReWriteStringToPrinter.UseVisualStyleBackColor = true;
            this.btnReWriteStringToPrinter.Click += new System.EventHandler(this.btnReWriteStringToPrinter_Click);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(886, 647);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(46, 17);
            this.label20.TabIndex = 20;
            this.label20.Text = "ACKs:";
            // 
            // lblPrinterACKs
            // 
            this.lblPrinterACKs.Location = new System.Drawing.Point(941, 647);
            this.lblPrinterACKs.Name = "lblPrinterACKs";
            this.lblPrinterACKs.Size = new System.Drawing.Size(70, 20);
            this.lblPrinterACKs.TabIndex = 21;
            this.lblPrinterACKs.Text = "lblPrinterACKs";
            this.lblPrinterACKs.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(1017, 647);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(56, 17);
            this.label21.TabIndex = 20;
            this.label21.Text = "NACKs:";
            // 
            // lblPrinterNACKs
            // 
            this.lblPrinterNACKs.Location = new System.Drawing.Point(1079, 647);
            this.lblPrinterNACKs.Name = "lblPrinterNACKs";
            this.lblPrinterNACKs.Size = new System.Drawing.Size(76, 17);
            this.lblPrinterNACKs.TabIndex = 21;
            this.lblPrinterNACKs.Text = "lblPrinterACKs";
            this.lblPrinterNACKs.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(12, 441);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(188, 17);
            this.label22.TabIndex = 22;
            this.label22.Text = "Строка печати в принтере:";
            // 
            // lblPCStringLastUpdateTime
            // 
            this.lblPCStringLastUpdateTime.Location = new System.Drawing.Point(317, 441);
            this.lblPCStringLastUpdateTime.Name = "lblPCStringLastUpdateTime";
            this.lblPCStringLastUpdateTime.Size = new System.Drawing.Size(181, 17);
            this.lblPCStringLastUpdateTime.TabIndex = 23;
            this.lblPCStringLastUpdateTime.Text = "lblPCStringLastUpdateTime";
            this.lblPCStringLastUpdateTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tboxBatchNextLotNo
            // 
            this.tboxBatchNextLotNo.Location = new System.Drawing.Point(298, 241);
            this.tboxBatchNextLotNo.Name = "tboxBatchNextLotNo";
            this.tboxBatchNextLotNo.Size = new System.Drawing.Size(62, 22);
            this.tboxBatchNextLotNo.TabIndex = 8;
            this.tboxBatchNextLotNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tboxBatchNextLotNo_KeyDown);
            this.tboxBatchNextLotNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tboxBatchNextLotNo_KeyPress);
            // 
            // btn_tbBatchNextLotNoConfirm
            // 
            this.btn_tbBatchNextLotNoConfirm.AutoSize = true;
            this.btn_tbBatchNextLotNoConfirm.Location = new System.Drawing.Point(366, 238);
            this.btn_tbBatchNextLotNoConfirm.Name = "btn_tbBatchNextLotNoConfirm";
            this.btn_tbBatchNextLotNoConfirm.Size = new System.Drawing.Size(38, 27);
            this.btn_tbBatchNextLotNoConfirm.TabIndex = 7;
            this.btn_tbBatchNextLotNoConfirm.Text = "OK";
            this.btn_tbBatchNextLotNoConfirm.UseVisualStyleBackColor = true;
            this.btn_tbBatchNextLotNoConfirm.Click += new System.EventHandler(this.btn_tbBatchNextLotNoConfirm_Click);
            // 
            // dtpickBatchNextDate
            // 
            this.dtpickBatchNextDate.CustomFormat = "HH:mm:ss dd.MM.yyyy";
            this.dtpickBatchNextDate.Location = new System.Drawing.Point(299, 366);
            this.dtpickBatchNextDate.Name = "dtpickBatchNextDate";
            this.dtpickBatchNextDate.Size = new System.Drawing.Size(200, 22);
            this.dtpickBatchNextDate.TabIndex = 12;
            this.dtpickBatchNextDate.ValueChanged += new System.EventHandler(this.dtpickBatchNextDate_ValueChanged);
            // 
            // btnStorePrintingMessageIntoMemory
            // 
            this.btnStorePrintingMessageIntoMemory.AutoSize = true;
            this.btnStorePrintingMessageIntoMemory.Location = new System.Drawing.Point(892, 493);
            this.btnStorePrintingMessageIntoMemory.Name = "btnStorePrintingMessageIntoMemory";
            this.btnStorePrintingMessageIntoMemory.Size = new System.Drawing.Size(262, 44);
            this.btnStorePrintingMessageIntoMemory.TabIndex = 17;
            this.btnStorePrintingMessageIntoMemory.Text = "Сохранить сообщение на\r\nFLASH-память принтера";
            this.btnStorePrintingMessageIntoMemory.UseVisualStyleBackColor = true;
            this.btnStorePrintingMessageIntoMemory.Click += new System.EventHandler(this.btnStorePrintingMessageIntoMemory_Click);
            // 
            // btnSetRemoteOperationStart
            // 
            this.btnSetRemoteOperationStart.AutoSize = true;
            this.btnSetRemoteOperationStart.Location = new System.Drawing.Point(892, 348);
            this.btnSetRemoteOperationStart.Name = "btnSetRemoteOperationStart";
            this.btnSetRemoteOperationStart.Size = new System.Drawing.Size(262, 44);
            this.btnSetRemoteOperationStart.TabIndex = 24;
            this.btnSetRemoteOperationStart.Text = "Запуск циркуляции чернил\r\n(включение принтера)";
            this.btnSetRemoteOperationStart.UseVisualStyleBackColor = true;
            this.btnSetRemoteOperationStart.Click += new System.EventHandler(this.btnSetRemoteOperationStart_Click);
            // 
            // btnSetRemoteOperationStop
            // 
            this.btnSetRemoteOperationStop.AutoSize = true;
            this.btnSetRemoteOperationStop.Location = new System.Drawing.Point(892, 398);
            this.btnSetRemoteOperationStop.Name = "btnSetRemoteOperationStop";
            this.btnSetRemoteOperationStop.Size = new System.Drawing.Size(262, 44);
            this.btnSetRemoteOperationStop.TabIndex = 24;
            this.btnSetRemoteOperationStop.Text = "Останов циркуляции чернил\r\n(выключение на ночь)";
            this.btnSetRemoteOperationStop.UseVisualStyleBackColor = true;
            this.btnSetRemoteOperationStop.Click += new System.EventHandler(this.btnSetRemoteOperationStop_Click);
            // 
            // btnExpandWindowForm
            // 
            this.btnExpandWindowForm.AutoSize = true;
            this.btnExpandWindowForm.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnExpandWindowForm.Location = new System.Drawing.Point(778, 461);
            this.btnExpandWindowForm.Name = "btnExpandWindowForm";
            this.btnExpandWindowForm.Size = new System.Drawing.Size(75, 39);
            this.btnExpandWindowForm.TabIndex = 25;
            this.btnExpandWindowForm.Text = " > > ";
            this.btnExpandWindowForm.UseVisualStyleBackColor = true;
            this.btnExpandWindowForm.Click += new System.EventHandler(this.btnExpandWindowForm_Click);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(886, 675);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(97, 17);
            this.label23.TabIndex = 26;
            this.label23.Text = "Время цикла:";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(1127, 675);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(24, 17);
            this.label24.TabIndex = 27;
            this.label24.Text = "мс";
            // 
            // lblLastCycleMillis
            // 
            this.lblLastCycleMillis.Location = new System.Drawing.Point(989, 675);
            this.lblLastCycleMillis.Name = "lblLastCycleMillis";
            this.lblLastCycleMillis.Size = new System.Drawing.Size(132, 17);
            this.lblLastCycleMillis.TabIndex = 28;
            this.lblLastCycleMillis.Text = "lblLastCycleMillis";
            this.lblLastCycleMillis.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblPrinterStatusErrorText
            // 
            this.lblPrinterStatusErrorText.Location = new System.Drawing.Point(196, 9);
            this.lblPrinterStatusErrorText.Name = "lblPrinterStatusErrorText";
            this.lblPrinterStatusErrorText.Size = new System.Drawing.Size(657, 23);
            this.lblPrinterStatusErrorText.TabIndex = 29;
            this.lblPrinterStatusErrorText.Text = "lblPrinterStatusErrorText";
            this.lblPrinterStatusErrorText.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblPrinterStatusWarningText
            // 
            this.lblPrinterStatusWarningText.Location = new System.Drawing.Point(385, 33);
            this.lblPrinterStatusWarningText.Name = "lblPrinterStatusWarningText";
            this.lblPrinterStatusWarningText.Size = new System.Drawing.Size(468, 23);
            this.lblPrinterStatusWarningText.TabIndex = 30;
            this.lblPrinterStatusWarningText.Text = "lblPrinterStatusWarningText";
            this.lblPrinterStatusWarningText.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnStopCalendarTimeResume
            // 
            this.btnStopCalendarTimeResume.Location = new System.Drawing.Point(892, 549);
            this.btnStopCalendarTimeResume.Name = "btnStopCalendarTimeResume";
            this.btnStopCalendarTimeResume.Size = new System.Drawing.Size(262, 27);
            this.btnStopCalendarTimeResume.TabIndex = 31;
            this.btnStopCalendarTimeResume.Text = "Вернуть текущее время на печати";
            this.btnStopCalendarTimeResume.UseVisualStyleBackColor = true;
            this.btnStopCalendarTimeResume.Click += new System.EventHandler(this.btnStopCalendarTimeResume_Click);
            // 
            // lblPrinterRemoteOperation
            // 
            this.lblPrinterRemoteOperation.Location = new System.Drawing.Point(859, 328);
            this.lblPrinterRemoteOperation.Name = "lblPrinterRemoteOperation";
            this.lblPrinterRemoteOperation.Size = new System.Drawing.Size(295, 17);
            this.lblPrinterRemoteOperation.TabIndex = 32;
            this.lblPrinterRemoteOperation.Text = "lblPrinterRemoteOperation";
            this.lblPrinterRemoteOperation.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(886, 311);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(182, 17);
            this.label25.TabIndex = 18;
            this.label25.Text = "Управление циркуляцией:";
            // 
            // lblPrinterHoldOnActive
            // 
            this.lblPrinterHoldOnActive.AutoSize = true;
            this.lblPrinterHoldOnActive.ForeColor = System.Drawing.Color.Red;
            this.lblPrinterHoldOnActive.Location = new System.Drawing.Point(180, 1);
            this.lblPrinterHoldOnActive.Name = "lblPrinterHoldOnActive";
            this.lblPrinterHoldOnActive.Size = new System.Drawing.Size(296, 51);
            this.lblPrinterHoldOnActive.TabIndex = 33;
            this.lblPrinterHoldOnActive.Text = "<- Устраните возможные ошибки готовости\r\nпринтера и/или начните новую партию.\r\nПр" +
    "интер сам партию не переведёт.";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(889, 579);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(84, 17);
            this.label16.TabIndex = 18;
            this.label16.Text = "Календарь:";
            // 
            // lblPrinterStatusClockStopped
            // 
            this.lblPrinterStatusClockStopped.Location = new System.Drawing.Point(979, 579);
            this.lblPrinterStatusClockStopped.Name = "lblPrinterStatusClockStopped";
            this.lblPrinterStatusClockStopped.Size = new System.Drawing.Size(175, 17);
            this.lblPrinterStatusClockStopped.TabIndex = 32;
            this.lblPrinterStatusClockStopped.Text = "lblPrinterStatusClockStopped";
            this.lblPrinterStatusClockStopped.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblProgramHoldOn
            // 
            this.lblProgramHoldOn.Location = new System.Drawing.Point(1066, 267);
            this.lblProgramHoldOn.Name = "lblProgramHoldOn";
            this.lblProgramHoldOn.Size = new System.Drawing.Size(89, 17);
            this.lblProgramHoldOn.TabIndex = 5;
            this.lblProgramHoldOn.Text = "lblProgramHoldOn";
            this.lblProgramHoldOn.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(886, 267);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(174, 17);
            this.label18.TabIndex = 18;
            this.label18.Text = "Бездействие программы:";
            // 
            // btn_cmboxNewBatchVolumeConfirm
            // 
            this.btn_cmboxNewBatchVolumeConfirm.AutoSize = true;
            this.btn_cmboxNewBatchVolumeConfirm.Location = new System.Drawing.Point(307, 143);
            this.btn_cmboxNewBatchVolumeConfirm.Name = "btn_cmboxNewBatchVolumeConfirm";
            this.btn_cmboxNewBatchVolumeConfirm.Size = new System.Drawing.Size(82, 27);
            this.btn_cmboxNewBatchVolumeConfirm.TabIndex = 7;
            this.btn_cmboxNewBatchVolumeConfirm.Text = "Добавить";
            this.btn_cmboxNewBatchVolumeConfirm.UseVisualStyleBackColor = true;
            this.btn_cmboxNewBatchVolumeConfirm.Visible = false;
            this.btn_cmboxNewBatchVolumeConfirm.Click += new System.EventHandler(this.btn_cmboxNewBatchVolumeConfirm_Click);
            // 
            // btn_cmboxDeleteBatchVolumeItem
            // 
            this.btn_cmboxDeleteBatchVolumeItem.AutoSize = true;
            this.btn_cmboxDeleteBatchVolumeItem.Location = new System.Drawing.Point(307, 143);
            this.btn_cmboxDeleteBatchVolumeItem.Name = "btn_cmboxDeleteBatchVolumeItem";
            this.btn_cmboxDeleteBatchVolumeItem.Size = new System.Drawing.Size(73, 27);
            this.btn_cmboxDeleteBatchVolumeItem.TabIndex = 7;
            this.btn_cmboxDeleteBatchVolumeItem.Text = "Убрать";
            this.btn_cmboxDeleteBatchVolumeItem.UseVisualStyleBackColor = true;
            this.btn_cmboxDeleteBatchVolumeItem.Click += new System.EventHandler(this.btn_cmboxDeleteBatchVolumeItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1166, 700);
            this.Controls.Add(this.lblPrinterHoldOnActive);
            this.Controls.Add(this.lblPrinterStatusClockStopped);
            this.Controls.Add(this.lblPrinterRemoteOperation);
            this.Controls.Add(this.btnStopCalendarTimeResume);
            this.Controls.Add(this.lblPrinterStatusWarningText);
            this.Controls.Add(this.lblPrinterStatusErrorText);
            this.Controls.Add(this.lblLastCycleMillis);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.btnExpandWindowForm);
            this.Controls.Add(this.btnSetRemoteOperationStop);
            this.Controls.Add(this.btnSetRemoteOperationStart);
            this.Controls.Add(this.lblPCStringLastUpdateTime);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.lblPrinterNACKs);
            this.Controls.Add(this.lblPrinterACKs);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.btnReWriteStringToPrinter);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.btnStorePrintingMessageIntoMemory);
            this.Controls.Add(this.btnStartMaintenance);
            this.Controls.Add(this.btnStartNewBatch);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.tboxTimerTickPeriod);
            this.Controls.Add(this.lboxLog);
            this.Controls.Add(this.tboxPrinterPCString);
            this.Controls.Add(this.dtpickBatchNextDate);
            this.Controls.Add(this.dtpickBatchDate);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.chboxBatchDateAUTO);
            this.Controls.Add(this.chboxBatchLotNoAutoINC);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tboxBatchNextLotNo);
            this.Controls.Add(this.tboxBatchLotNo);
            this.Controls.Add(this.tboxBrokenBags);
            this.Controls.Add(this.btn_tbBatchNextLotNoConfirm);
            this.Controls.Add(this.btn_tbBatchLotNoConfirm);
            this.Controls.Add(this.btn_cmboxNewBatchVolumeConfirm);
            this.Controls.Add(this.btn_tbBrokenBagsConfirm);
            this.Controls.Add(this.btnBrokenINC);
            this.Controls.Add(this.btnBrokenDEC);
            this.Controls.Add(this.lblBatchNextDate);
            this.Controls.Add(this.cmboxVolume);
            this.Controls.Add(this.lblBatchDate);
            this.Controls.Add(this.lblBatchNextLotNo);
            this.Controls.Add(this.lblBatchLotNo);
            this.Controls.Add(this.lblMaintBags);
            this.Controls.Add(this.lblBrokenBags);
            this.Controls.Add(this.lblProgramHoldOn);
            this.Controls.Add(this.lblPrinterOnlineOffline);
            this.Controls.Add(this.lblPrinterStatusOnline);
            this.Controls.Add(this.lblBatchEndCounter);
            this.Controls.Add(this.lblPrinterPrintCount);
            this.Controls.Add(this.lblBatchStartCounter);
            this.Controls.Add(this.lblVolume);
            this.Controls.Add(this.lblMarkedBags);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chboxConnectOnLaunch);
            this.Controls.Add(this.lbl_Port);
            this.Controls.Add(this.lbl_IP);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btn_cmboxDeleteBatchVolumeItem);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Hitachi UX-D161W Message Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.cmstrip_lbLog.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer tmrUpdate;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Label lbl_IP;
        private System.Windows.Forms.Label lbl_Port;
        private System.Windows.Forms.CheckBox chboxConnectOnLaunch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblMarkedBags;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblBrokenBags;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblMaintBags;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmboxVolume;
        private System.Windows.Forms.Button btnBrokenDEC;
        private System.Windows.Forms.Button btnBrokenINC;
        private System.Windows.Forms.TextBox tboxBrokenBags;
        private System.Windows.Forms.Button btn_tbBrokenBagsConfirm;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblBatchStartCounter;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblPrinterPrintCount;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblBatchEndCounter;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblVolume;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblBatchLotNo;
        private System.Windows.Forms.Button btn_tbBatchLotNoConfirm;
        private System.Windows.Forms.TextBox tboxBatchLotNo;
        private System.Windows.Forms.CheckBox chboxBatchLotNoAutoINC;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblBatchNextLotNo;
        private System.Windows.Forms.CheckBox chboxBatchDateAUTO;
        private System.Windows.Forms.Label lblBatchDate;
        private System.Windows.Forms.Label lblBatchNextDate;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.DateTimePicker dtpickBatchDate;
        private System.Windows.Forms.TextBox tboxPrinterPCString;
        private System.Windows.Forms.ListBox lboxLog;
        private System.Windows.Forms.TextBox tboxTimerTickPeriod;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnStartNewBatch;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblPrinterStatusOnline;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btnStartMaintenance;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label lblPrinterOnlineOffline;
        private System.Windows.Forms.Button btnReWriteStringToPrinter;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label lblPrinterACKs;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label lblPrinterNACKs;
        private System.Windows.Forms.ContextMenuStrip cmstrip_lbLog;
        private System.Windows.Forms.ToolStripMenuItem tlstr_miSaveAs;
        private System.Windows.Forms.ToolStripMenuItem tlstr_miClear;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label lblPCStringLastUpdateTime;
        private System.Windows.Forms.TextBox tboxBatchNextLotNo;
        private System.Windows.Forms.Button btn_tbBatchNextLotNoConfirm;
        private System.Windows.Forms.DateTimePicker dtpickBatchNextDate;
        private System.Windows.Forms.Button btnStorePrintingMessageIntoMemory;
        private System.Windows.Forms.Button btnSetRemoteOperationStart;
        private System.Windows.Forms.Button btnSetRemoteOperationStop;
        private System.Windows.Forms.Button btnExpandWindowForm;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label lblLastCycleMillis;
        private System.Windows.Forms.Label lblPrinterStatusErrorText;
        private System.Windows.Forms.Label lblPrinterStatusWarningText;
        private System.Windows.Forms.Button btnStopCalendarTimeResume;
        private System.Windows.Forms.Label lblPrinterRemoteOperation;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label lblPrinterHoldOnActive;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label lblPrinterStatusClockStopped;
        private System.Windows.Forms.Label lblProgramHoldOn;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button btn_cmboxNewBatchVolumeConfirm;
        private System.Windows.Forms.Button btn_cmboxDeleteBatchVolumeItem;
    }
}

