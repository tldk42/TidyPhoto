using System.ComponentModel;

namespace TidyPhoto.Model
{
    public class ClsfFolderInformation : INotifyPropertyChanged
    {
        private bool yearOpt;
        public bool YearOpt
        {
            get { return yearOpt; }
            set { yearOpt = value; OnPropertyChaneged("YearOpt"); }
        }

        private bool monthOpt;
        public bool MonthOpt
        {
            get { return monthOpt; }
            set { monthOpt = value; OnPropertyChaneged("MonthOpt"); }
        }

        private bool dayOpt;
        public bool DayOpt
        {
            get { return dayOpt; }
            set { dayOpt = value; OnPropertyChaneged("DayOpt"); }
        }

        private bool cameraOpt;
        public bool CameraOpt
        {
            get { return cameraOpt; }
            set { cameraOpt = value; OnPropertyChaneged("CameraOpt"); }
        }

        private bool locateOpt;
        public bool LocateOpt
        {
            get { return locateOpt; }
            set { locateOpt = value; OnPropertyChaneged("LocateOpt"); }
        }
        public ClsfFolderInformation()
        {
            YearOpt = true;
            MonthOpt = false;
            DayOpt = false;
            CameraOpt = false;
            LocateOpt = false;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChaneged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
