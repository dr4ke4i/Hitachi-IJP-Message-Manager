using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hitachi_IJP_Message_Manager
{
    class CBatch
    {
        public int StartCounter { get; private set; }
        public int EndCounter { get { return StartCounter + Delta.Sum; } }        
        public int LastKnownCounter { get; set; }

        private CDataTypes.structDelta pDelta;
        public CDataTypes.structDelta Delta { get { return pDelta; } }
        public class CData
        {
            public CData(DateTime _StartDate, int _Lot = 1)
            {
                Info = new CDataTypes.structInfo( _StartDate, _Lot);
            }

            public List<int> PossibleVolume = new List<int> { 1600 };
            public void AddToPossibleVolumeList(int bags)
            {
                bool is_new = true;
                foreach(int value in PossibleVolume)
                {
                    if (value == bags)
                        is_new = false;
                }
                if (is_new)
                {
                    PossibleVolume.Add(bags);
                    PossibleVolume.Sort();

                }
            }

            public CDataTypes.structInfo Info;
        }
        public CData Data;

        public class CDataTypes
        {
            public struct structDelta
            {
                public int Maint;
                public int Broken;
                public int Volume;
                public int Sum
                {
                    get
                    {
                        return Maint + Broken + Volume;
                    }
                }
            }

            public struct structInfo
            {
                public DateTime StartDate;
                public string StartDateStr
                {
                    get { return StartDate.Date.ToString("dd.MM.yy"); }
                    set { DateTime.TryParse(value, out StartDate); }
                }
                public int Lot;
                public string LotStr
                {
                    get { return Convert.ToString(Lot); }
                    set { int.TryParse(value, out Lot); }
                }
                public structInfo(DateTime _StartDate, int _Lot = 1 )
                {
                    StartDate = _StartDate;
                    Lot = _Lot;
                }
            }
        }

        public CBatch(int sc = -1, int lc = -1, int mb = -1, int bb = -1, int vb = -1,
            DateTime dt = default(DateTime), int lot = 1)
        {
            StartCounter = sc > 0 ? sc : 0;
            LastKnownCounter = lc > 0 ? (lc > StartCounter ? lc : StartCounter) : StartCounter;
            pDelta.Maint = mb > 0 ? mb : 0;
            pDelta.Broken = bb > 0 ? bb : 0;
            pDelta.Volume = vb > 0 ? vb : 0;
            MaintBeginCounter = -1;
            Data = new CData(dt, lot);
        }

        public void StartBatch(int count)
        {
            StartCounter = count;
            LastKnownCounter = count;
            pDelta.Broken = 0;
            pDelta.Maint = 0;
            pDelta.Volume = GetClosestPossibleVolume(pDelta.Volume);
            MaintBeginCounter = -1;
        }

        public bool CheckCounter(int counter)
        {
            if (counter >= EndCounter)
                return true;
            return false;
        }

        public bool IsFinished(int counter = -1)
        {
            if (counter != -1)
            {
                LastKnownCounter = counter;
            }
            if (LastKnownCounter >= EndCounter)
                return true;
            return false;
        }

        public bool IsOverthrown()
        {
            if (LastKnownCounter > EndCounter)
                return true;
            return false;
        }

        public bool SetBrokenBags(int bags)
        {
            int new_endcounter = StartCounter + pDelta.Maint + pDelta.Volume + bags;
            if ((bags>=0) && (LastKnownCounter <= new_endcounter))
            {
                pDelta.Broken = bags;
                return true;
            }                
            return false;
        }

        public bool SetVolume(int bags)
        {
            int new_endcounter = StartCounter + pDelta.Maint + pDelta.Broken + bags;
            if ((bags>0) && (LastKnownCounter <= new_endcounter))
            {
                pDelta.Volume = bags;
                return true;
            }
            return false;
        }

        private int MaintBeginCounter = -1;

        public void MaintBegin(int counter)
        {
            if (MaintBeginCounter == -1)
                MaintBeginCounter = counter;
        }
        
        public void MaintEnd(int counter)
        {
            if (MaintBeginCounter != -1)
            {
                pDelta.Maint += counter - MaintBeginCounter;
                MaintBeginCounter = -1;
            }
        }

        private int GetClosestPossibleVolume(int _volume)
        {
            if (_volume != 0)
            {
                int v_min = Data.PossibleVolume[0];
                foreach (int possible in Data.PossibleVolume)
                {
                    if (v_min > Math.Abs(_volume - possible)) v_min = possible;
                }
                return v_min;
            }
            return Data.PossibleVolume[0];
        }
    }
}
