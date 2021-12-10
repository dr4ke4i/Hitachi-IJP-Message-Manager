using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace Hitachi_IJP_Message_Manager
{
    class CSettings
    {
        public const string filepath = @".\settings.txt";

        #region Error codes

        private Error lasterror;

        public enum Error
        {
            OK = 0,
            FileDoesNotExist = 1,
            FileReadError = 2,
            FileWriteError = 3,
            ValueParseError = 4
        }

        public Error GetLastError
        {
            get { return lasterror; }
        }

        #endregion

        #region Settings Structure
        
        private enum Index {
            PrinterIPAddress,
            PrinterPort,
            CurrentBatchStartCounter,
            CurrentBatchLastKnownCounter,
            CurrentBatchMaintBags,
            CurrentBatchBrokenBags,
            CurrentBatchVolume,
            CurrentBatchStartDate,
            CurrentBatchLot,
            NextBatchStartCounter,
            NextBatchLastKnownCounter,
            NextBatchMaintBags,
            NextBatchBrokenBags,
            NextBatchVolume,
            NextBatchStartDate,
            NextBatchLot,
            BatchPossibleVolumeList,
            FormConnectOnLaunch,
            FormAutoDate,
            FormAutoLot
        };

        private sett_entry[] Settings = new sett_entry[Enum.GetValues(typeof(Index)).Length];

        private struct sett_entry
        {
            public string name;
            public object value;
            public char type;   // 's' - string , 'i' - int, 'b' - bool, 'd' - date, 'l' - List<int>
            public void Init(string _name, object _defaultvalue, char _type)
                { name = _name; value = _defaultvalue; type = _type; }
        }

        private void FillSettingsArray()
        {
            IPAddress.TryParse("192.168.0.1", out IPAddress ipaddr);
            Settings[(int)Index.PrinterIPAddress].Init              ("PrinterIPAddress",                ipaddr.ToString(),      's');
            Settings[(int)Index.PrinterPort].Init                   ("PrinterPort",                     502,                    'i');

            Settings[(int)Index.CurrentBatchStartCounter].Init      ("CurrentBatchStartCounter",        -1,                     'i');
            Settings[(int)Index.CurrentBatchLastKnownCounter].Init  ("CurrentBatchLastKnownCounter",    -1,                     'i');
            Settings[(int)Index.CurrentBatchMaintBags].Init         ("CurrentBatchMaintBags",           0,                      'i');
            Settings[(int)Index.CurrentBatchBrokenBags].Init        ("CurrentBatchBrokenBags",          0,                      'i');
            Settings[(int)Index.CurrentBatchVolume].Init            ("CurrentBatchVolume",              1600,                   'i');
            Settings[(int)Index.CurrentBatchStartDate].Init         ("CurrentBatchStartDate",           default(DateTime),      'd');
            Settings[(int)Index.CurrentBatchLot].Init               ("CurrentBatchLot",                 666,                    'i');
            
            Settings[(int)Index.NextBatchStartCounter].Init         ("NextBatchStartCounter",           -1,                     'i');
            Settings[(int)Index.NextBatchLastKnownCounter].Init     ("NextBatchLastKnownCounter",       -1,                     'i');
            Settings[(int)Index.NextBatchMaintBags].Init            ("NextBatchMaintBags",              0,                      'i');
            Settings[(int)Index.NextBatchBrokenBags].Init           ("NextBatchBrokenBags",             0,                      'i');
            Settings[(int)Index.NextBatchVolume].Init               ("NextBatchVolume",                 1600,                   'i');
            Settings[(int)Index.NextBatchStartDate].Init            ("NextBatchStartDate",              default(DateTime),      'd');
            Settings[(int)Index.NextBatchLot].Init                  ("NextBatchLot",                    666,                    'i');

            Settings[(int)Index.BatchPossibleVolumeList].Init       ("BatchPossibleVolumeItem",         new List<int> { 1600 }, 'l');
            Settings[(int)Index.FormConnectOnLaunch].Init           ("FormConnectOnLaunch",             false,                  'b');
            Settings[(int)Index.FormAutoDate].Init                  ("FormAutoDate",                    true,                   'b');
            Settings[(int)Index.FormAutoLot].Init                   ("FormAutoLot",                     true,                   'b');
        }

        #endregion

        #region Properties

        public string IPAddr
        { get { return (string)Getter(Index.PrinterIPAddress); } set { Setter(Index.PrinterIPAddress, value); } }

        public int Port
        { get { return (int)Getter(Index.PrinterPort); } set { Setter(Index.PrinterPort, value); } }

        public int CurrentBatchStartCounter
        { get { return (int)Getter(Index.CurrentBatchStartCounter); } set { Setter(Index.CurrentBatchStartCounter, value); } }

        public int CurrentBatchLastKnownCounter
        { get { return (int)Getter(Index.CurrentBatchLastKnownCounter); } set { Setter(Index.CurrentBatchLastKnownCounter, value); } }

        public int CurrentBatchMaintBags
        { get { return (int)Getter(Index.CurrentBatchMaintBags); } set { Setter(Index.CurrentBatchMaintBags, value); } }

        public int CurrentBatchBrokenBags
        { get { return (int)Getter(Index.CurrentBatchBrokenBags); } set { Setter(Index.CurrentBatchBrokenBags, value); } }

        public int CurrentBatchVolume
        { get { return (int)Getter(Index.CurrentBatchVolume); } set { Setter(Index.CurrentBatchVolume, value); } }

        public DateTime CurrentBatchStartDate
        { get { return (DateTime)Getter(Index.CurrentBatchStartDate); } set { Setter(Index.CurrentBatchStartDate, value); } }

        public string   CurrentBatchStartDateStr
        {
            get { return ((DateTime)Getter(Index.CurrentBatchStartDate)).Date.ToString("dd.MM.yy"); }
            set {
                    if (DateTime.TryParse(value, out DateTime datetime))
                        Setter(Index.CurrentBatchStartDate, datetime);
                    else
                        lasterror = Error.ValueParseError;
                }
        }

        public int CurrentBatchLot
        { get { return (int)Getter(Index.CurrentBatchLot); } set { Setter(Index.CurrentBatchLot, value); } }

        public string CurrentBatchLotStr
        {
            get { return Convert.ToString((int)Getter(Index.CurrentBatchLot)); }
            set {
                    if (int.TryParse(value, out int lot))
                        Setter(Index.CurrentBatchLot, lot);
                    else
                        lasterror = Error.ValueParseError;
                }
        }

        public int NextBatchStartCounter
        { get { return (int)Getter(Index.NextBatchStartCounter); } set { Setter(Index.NextBatchStartCounter, value); } }

        public int NextBatchLastKnownCounter
        { get { return (int)Getter(Index.NextBatchLastKnownCounter); } set { Setter(Index.NextBatchLastKnownCounter, value); } }

        public int NextBatchMaintBags
        { get { return (int)Getter(Index.NextBatchMaintBags); } set { Setter(Index.NextBatchMaintBags, value); } }

        public int NextBatchBrokenBags
        { get { return (int)Getter(Index.NextBatchBrokenBags); } set { Setter(Index.NextBatchBrokenBags, value); } }

        public int NextBatchVolume
        { get { return (int)Getter(Index.NextBatchVolume); } set { Setter(Index.NextBatchVolume, value); } }

        public DateTime NextBatchStartDate
        { get { return (DateTime)Getter(Index.NextBatchStartDate); } set { Setter(Index.NextBatchStartDate, value); } }

        public string NextBatchStartDateStr
        {
            get { return ((DateTime)Getter(Index.NextBatchStartDate)).Date.ToString("dd.MM.yy"); }
            set
            {
                if (DateTime.TryParse(value, out DateTime datetime))
                    Setter(Index.NextBatchStartDate, datetime);
                else
                    lasterror = Error.ValueParseError;
            }
        }

        public int NextBatchLot
        { get { return (int)Getter(Index.NextBatchLot); } set { Setter(Index.NextBatchLot, value); } }

        public string NextBatchLotStr
        {
            get { return Convert.ToString((int)Getter(Index.NextBatchLot)); }
            set
            {
                if (int.TryParse(value, out int lot))
                    Setter(Index.NextBatchLot, lot);
                else
                    lasterror = Error.ValueParseError;
            }
        }

        public List<int> PossibleVolumeList
        { get { return (List<int>)Getter(Index.BatchPossibleVolumeList); } set { Setter(Index.BatchPossibleVolumeList, value); } }

        public bool FormConnectOnLaunch
        { get { return (bool)Getter(Index.FormConnectOnLaunch); } set { Setter(Index.FormConnectOnLaunch, value); } }

        public bool FormAutoDate
        { get { return (bool)Getter(Index.FormAutoDate); } set { Setter(Index.FormAutoDate, value); } }

        public bool FormAutoLot
        { get { return (bool)Getter(Index.FormAutoLot); } set { Setter(Index.FormAutoLot, value); } }

        #endregion

        #region Constructor

        public CSettings()
        {
            lasterror = Error.OK;
            FillSettingsArray();
            if (System.IO.File.Exists(filepath))
            {
                ReadSettingsFromFile();
            }
                else
            {
                SaveSettingsToFile();
                if (lasterror==Error.OK)
                {
                    lasterror = Error.FileDoesNotExist;
                }
            }
        }

        #endregion

        #region Private Methods

        public void SaveSettingsToFile()
        {
            System.IO.StreamWriter writefile = null;
            try
            {
                writefile = new StreamWriter(filepath);
                foreach (int i in Enum.GetValues(typeof(Index)))
                {
                    if (Settings[i].type == 'l')        // List<int> conversion
                    {
                        List<int> temp_listint = (List<int>)Settings[i].value;
                        foreach (int value in temp_listint)
                        {
                            writefile.WriteLine(Settings[i].name + "=" + value.ToString());
                        }
                    }
                    else if (Settings[i].type == 'd')   // DateTime conversion
                    {
                        DateTime temp_dt = (DateTime)Settings[i].value;
                        writefile.WriteLine(Settings[i].name + "=" + temp_dt.Date.ToString("dd.MM.yy"));
                    }
                    else                                // other types use .ToString() method
                        writefile.WriteLine( Settings[i].name + "=" + Convert.ToString( Settings[i].value) );
                }
            }
            catch (Exception e)
            {
                lasterror = Error.FileWriteError;
                System.Windows.Forms.MessageBox.Show(e.Message, "Error occured during settings file write");
            }
            finally
            {
                if (writefile != null) writefile.Close();
            }
        }   

        private void ReadSettingsFromFile()
        {
            System.IO.StreamReader readfile = null;
            try
            {
                readfile = new StreamReader(filepath);
                string line;
                while (!readfile.EndOfStream)
                {
                    line = readfile.ReadLine();
                    if (line.IndexOf("=")>=0)
                    {
                        string name = line.Substring(0, line.IndexOf("="));
                        string value = line.Substring(line.IndexOf("=") + 1);
                        foreach (int i in Enum.GetValues(typeof(Index)))
                        {
                            if (Settings[i].name == name)
                            {
                                switch (Settings[i].type)
                                {
                                    case 's':
                                        Settings[i].value = value;
                                        break;
                                    case 'i':
                                        if (int.TryParse(value, out int output_int))
                                            Settings[i].value = output_int;
                                        break;
                                    case 'b':
                                        if (bool.TryParse(value, out bool output_bool))
                                            Settings[i].value = output_bool;
                                        break;
                                    case 'd':
                                        if (DateTime.TryParse(value, out DateTime output_dt))
                                            Settings[i].value = output_dt;
                                        break;
                                    case 'l':
                                        if (int.TryParse(value, out int output_listint_element))
                                        {
                                            List<int> temp_listint = (List<int>)Settings[i].value;
                                            if (!temp_listint.Contains(output_listint_element))
                                            {
                                                temp_listint.Add(output_listint_element);
                                            }
                                            temp_listint.Sort();
                                            Settings[i].value = temp_listint;
                                        }
                                        break;
                                    default:
                                        Settings[i].value = value;
                                        break;
                                }
                            }
                        }
                    }                    
                }
            }
            catch (Exception e)
            {
                lasterror = Error.FileReadError;
                System.Windows.Forms.MessageBox.Show(e.Message, "Error occured during settings file read");
            }
            finally
            {
                if (readfile != null) readfile.Close();
            }
        }

        // TODO Simplify: remove 'if (Settings[i].value != _value) ...'
        private void Setter(Index _index, object _value)
        {
            int i = (int)_index;
            switch (Settings[i].type)
            {
                case 's':  // String conversion
                    if (i == (int)Index.PrinterIPAddress)
                    {
                        if (System.Net.IPAddress.TryParse(Convert.ToString(_value), out System.Net.IPAddress ipaddr))
                        {
                            lasterror = Error.OK;
                            if (Convert.ToString(Settings[i].value) != ipaddr.ToString())
                            {
                                Settings[i].value = ipaddr.ToString();
                                // SaveSettingsToFile();
                            }
                        }
                        else
                            lasterror = Error.ValueParseError;
                    }
                    else
                    {
                        lasterror = Error.OK;
                        if ((string)Settings[i].value != (string)_value)
                        {
                            Settings[i].value = _value;
                            // SaveSettingsToFile();
                        }
                    }
                    break;
                case 'i':   // Int conversion
                    lasterror = Error.OK;
                    if ((int)Settings[i].value != (int)_value)
                    {
                        Settings[i].value = _value;
                        // SaveSettingsToFile();
                    }
                    break;
                case 'b':   // Bool conversion
                    lasterror = Error.OK;
                    if ((bool)Settings[i].value != (bool)_value)
                    {
                        Settings[i].value = _value;
                        // SaveSettingsToFile();
                    }
                    break;
                case 'd':   // DateTime conversion
                    lasterror = Error.OK;
                    if ((DateTime)Settings[i].value != (DateTime)_value)
                    {
                        Settings[i].value = _value;
                        // SaveSettingsToFile();
                    }
                    break;
                case 'l':   // List<int> conversion
                    lasterror = Error.OK;
                    if ((List<int>)Settings[i].value != (List<int>)_value)
                    {
                        Settings[i].value = _value;
                        // SaveSettingsToFile();
                    }
                    break;
            }
        }

        private object Getter(Index _index)
        {
            return Settings[(int)_index].value;
        }

    }

    #endregion

}