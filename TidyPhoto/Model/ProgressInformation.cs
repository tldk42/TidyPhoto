using System.ComponentModel;

namespace TidyPhoto.Model
{
    public class ProgressInformation : INotifyPropertyChanged
    {
        private int progressNum;
        public int ProgressNum
        {
            get { return progressNum; }
            set { progressNum = value; OnPropertyChaneged("ProgressNum"); }
        }

        private string stateText;
        public string StateText
        {
            get { return stateText; }
            set { stateText = value; OnPropertyChaneged("StateText"); }
        }

        public ProgressInformation()
        {
            ProgressNum = 0;
            StateText = "준비 중...";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChaneged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
