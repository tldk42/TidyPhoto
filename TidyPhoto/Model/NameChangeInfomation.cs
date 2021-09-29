using System.Collections.ObjectModel;
using System.ComponentModel;

namespace TidyPhoto.Model
{

    public class NameChangeInformation : INotifyPropertyChanged
    {
        private ObservableCollection<string> opt1List;
        public ObservableCollection<string> Opt1List
        {
            get { return opt1List; }
            set { opt1List = value; OnPropertyChaneged("Opt1List"); }
        }

        private ObservableCollection<string> opt2List;
        public ObservableCollection<string> Opt2List
        {
            get { return opt2List; }
            set { opt2List = value; OnPropertyChaneged("Opt2List"); }
        }

        private ObservableCollection<string> opt3List;
        public ObservableCollection<string> Opt3List
        {
            get { return opt3List; }
            set { opt3List = value; OnPropertyChaneged("Opt3List"); }
        }

        private ObservableCollection<string> opt4List;
        public ObservableCollection<string> Opt4List
        {
            get { return opt4List; }
            set { opt4List = value; OnPropertyChaneged("Opt4List"); }
        }

        private string selectedOpt1;
        public string SelectedOpt1
        {
            get { return selectedOpt1; }
            set { selectedOpt1 = value; OnPropertyChaneged("SelectedOpt1"); }
        }

        private string selectedOpt2;
        public string SelectedOpt2
        {
            get { return selectedOpt2; }
            set { selectedOpt2 = value; OnPropertyChaneged("SelectedOpt2"); }
        }

        private string selectedOpt3;
        public string SelectedOpt3
        {
            get { return selectedOpt3; }
            set { selectedOpt3 = value; OnPropertyChaneged("SelectedOpt3"); }
        }

        private string selectedOpt4;
        public string SelectedOpt4
        {
            get { return selectedOpt4; }
            set { selectedOpt4 = value; OnPropertyChaneged("SelectedOpt4"); }
        }

        private string userText1;
        public string UserText1
        {
            get { return userText1; }
            set { userText1 = value; OnPropertyChaneged("UserText1"); }
        }

        private string userText2;
        public string UserText2
{
            get { return userText2; }
            set { userText2 = value; OnPropertyChaneged("UserText2"); }
        }

        private string userText3;
        public string UserText3
{
            get { return userText3; }
            set { userText3 = value; OnPropertyChaneged("UserText3"); }
        }

        private string userText4;
        public string UserText4
{
            get { return userText4; }
            set { userText4 = value; OnPropertyChaneged("UserText4"); }
        }

        public NameChangeInformation()
        {
            opt1List = new ObservableCollection<string>
            {
                 "촬영 날짜", "촬영 위치", "촬영 기기", "직접 입력"
            };
            opt2List = new ObservableCollection<string>
            {
                "촬영 날짜", "촬영 위치", "촬영 기기", "직접 입력", "선택 안함"
            };
            opt3List = new ObservableCollection<string>
            {
                "촬영 날짜", "촬영 위치", "촬영 기기", "직접 입력", "선택 안함"
            };
            opt4List = new ObservableCollection<string>
            {
                "촬영 날짜", "촬영 위치", "촬영 기기", "직접 입력", "선택 안함"
            };
            SelectedOpt1 = "촬영 날짜"; 
            SelectedOpt2 = "선택 안함"; 
            SelectedOpt3 = "선택 안함"; 
            SelectedOpt4 = "선택 안함";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChaneged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
