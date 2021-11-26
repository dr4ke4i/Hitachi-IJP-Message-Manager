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

        private enum Index { IPAddress, Port, /*WorkingMessageNo,*/ StartCounter, MaintBags, BrokenBags,
            Volume, StartDate, Lot, PossibleVolume, ConnectOnLaunch, AutoDate, AutoLot };
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
            Settings[(int)Index.IPAddress].Init         ("IP_Address",       ipaddr.ToString(),      's');
            Settings[(int)Index.Port].Init              ("Port",             502,                    'i');
            //Settings[(int)Index.WorkingMessageNo].Init  ("WorkingMessageNo", 1,                      'i');
            Settings[(int)Index.StartCounter].Init      ("Start_Counter",   -1,                      'i');
            Settings[(int)Index.MaintBags].Init         ("Maintenance_Bags", 0,                      'i');
            Settings[(int)Index.BrokenBags].Init        ("Broken_Bags",      0,                      'i');
            Settings[(int)Index.Volume].Init            ("Batch_Volume",     1600,                   'i');
            Settings[(int)Index.StartDate].Init         ("Batch_Start_Date", default(DateTime),      'd');
            Settings[(int)Index.Lot].Init               ("Lot_Number",       666,                    'i');
            Settings[(int)Index.PossibleVolume].Init    ("PossibleVolume",   new List<int> { 1600 }, 'l');
            Settings[(int)Index.ConnectOnLaunch].Init   ("ConnectOnLaunch",  false,                  'b');
            Settings[(int)Index.AutoDate].Init          ("AutoDate",         true,                   'b');
            Settings[(int)Index.AutoLot].Init           ("AutoLot",          true,                   'b');
        }

        #endregion

        #region Properties

        public string IPAddr
        {
            get { return (string)Getter(Index.IPAddress); }
            set { Setter(Index.IPAddress, value); }
        }

        public string PortStr
        {
            get { return Convert.ToString((int)Getter(Index.Port));            }
            set
            {
                if (int.TryParse(value, out int port))
                    Setter(Index.Port, port);
                else
                    lasterror = Error.ValueParseError;
            }
        }

        public int    PortInt
        {
            get { return (int)Getter(Index.Port); }
            set { Setter(Index.Port, value); }
        }

        /*public int    WorkingMessageNo
        {
            get { return (int)Getter(Index.WorkingMessageNo); }
            set { Setter(Index.WorkingMessageNo, value); }
        }
        */

        public int    StartCounter
        {
            get { return (int)Getter(Index.StartCounter); }
            set { Setter(Index.StartCounter, value); }
        }

        public int    MaintBags
        {
            get { return (int)Getter(Index.MaintBags); }
            set { Setter(Index.MaintBags, value); }
        }

        public int    BrokenBags
        {
            get { return (int)Getter(Index.BrokenBags); }
            set { Setter(Index.BrokenBags, value); }
        }

        public int    Volume
        {
            get { return (int)Getter(Index.Volume); }
            set { Setter(Index.Volume, value); }
        }

        public DateTime StartDate
        {
            get { return (DateTime)Getter(Index.StartDate); }
            set { Setter(Index.StartDate, value); }
        }

        public string   StartDateStr
        {
            get { return ((DateTime)Getter(Index.StartDate)).Date.ToString("dd.MM.yy"); }
            set
            {
                if (DateTime.TryParse(value, out DateTime datetime))
                    Setter(Index.StartDate, datetime);
                else
                    lasterror = Error.ValueParseError;
            }
        }

        public string LotStr
        {
            get { return Convert.ToString((int)Getter(Index.Lot)); }
            set
            {
                if (int.TryParse(value, out int lot))
                    Setter(Index.Lot, lot);
                else
                    lasterror = Error.ValueParseError;
            }
        }

        public int    LotInt
        {
            get { return (int)Getter(Index.Lot); }
            set { Setter(Index.Lot, value); }
        }

        public List<int> PossibleVolumeList
        {
            get { return (List<int>)Getter(Index.PossibleVolume); }
            set { Setter(Index.PossibleVolume, value); }
        }

        public bool   ConnectOnLaunch
        {
            get { return (bool)Getter(Index.ConnectOnLaunch); }
            set { Setter(Index.ConnectOnLaunch, value); }
        }

        public bool   AutoDate
        {
            get { return (bool)Getter(Index.AutoDate); }
            set { Setter(Index.AutoDate, value); }
        }

        public bool   AutoLot
        {
            get { return (bool)Getter(Index.AutoLot); }
            set { Setter(Index.AutoLot, value); }
        }

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

        private void SaveSettingsToFile()
        {
            System.IO.StreamWriter writefile = null;
            try
            {
                writefile = new StreamWriter(filepath);
                foreach (int i in Enum.GetValues(typeof(Index)))
                {
                    if (Settings[i].type=='l')
                    {
                        List<int> temp_listint = (List<int>)Settings[i].value;
                        foreach(int value in temp_listint)
                        {
                            writefile.WriteLine( Settings[i].name + "=" + value.ToString() );
                        }
                    }
                    else if (Settings[i].type=='d')
                    {
                        DateTime temp_dt = (DateTime)Settings[i].value;
                        writefile.WriteLine(Settings[i].name + "=" + temp_dt.Date.ToString("dd.MM.yy"));
                    }
                    else
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

        private void Setter(Index _index, object _value)
        {
            int i = (int)Enum.GetValues(typeof(Index)).GetValue((int)_index);
            switch (Settings[i].type)
            {
                case 's':
                    if (i == (int)Index.IPAddress)
                    {
                        if (System.Net.IPAddress.TryParse(Convert.ToString(_value), out System.Net.IPAddress ipaddr))
                        {
                            lasterror = Error.OK;
                            if (Convert.ToString(Settings[i].value) != ipaddr.ToString())
                            {
                                Settings[i].value = ipaddr.ToString();
                                SaveSettingsToFile();
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
                            SaveSettingsToFile();
                        }
                    }
                    break;
                case 'i':
                    lasterror = Error.OK;
                    if ((int)Settings[i].value != (int)_value)
                    {
                        Settings[i].value = _value;
                        SaveSettingsToFile();
                    }
                    break;
                case 'b':
                    lasterror = Error.OK;
                    if ((bool)Settings[i].value != (bool)_value)
                    {
                        Settings[i].value = _value;
                        SaveSettingsToFile();
                    }
                    break;
                case 'd':
                    lasterror = Error.OK;
                    if ((DateTime)Settings[i].value != (DateTime)_value)
                    {
                        Settings[i].value = _value;
                        SaveSettingsToFile();
                    }
                    break;
                case 'l':
                    lasterror = Error.OK;
                    if ((List<int>)Settings[i].value != (List<int>)_value)
                    {
                        Settings[i].value = _value;
                        SaveSettingsToFile();
                    }
                    break;
            }
        }

        private object Getter(Index _index)
        {
            return Settings[(int)Enum.GetValues(typeof(Index)).GetValue((int)_index)].value;
        }

    }

    #endregion

}
