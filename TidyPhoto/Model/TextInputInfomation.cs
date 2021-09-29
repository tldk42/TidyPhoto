using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;

namespace TidyPhoto.Model
{
    public class FontData : INotifyPropertyChanged
    {
        private ObservableCollection<string> fontStyleList;
        public ObservableCollection<string> FontStyleList
        {
            get { return fontStyleList; }
            set { fontStyleList = value; OnPropertyChaneged("FontStyleList"); }
        }

        private ObservableCollection<float> fontSizeList;
        public ObservableCollection<float> FontSizeList
        {
            get { return fontSizeList; }
            set { fontSizeList = value; OnPropertyChaneged("FontSizeList"); }
        }

        private string selectedFont;
        public string SelectedFont
        {
            get { return selectedFont; }
            set { selectedFont = value; OnPropertyChaneged("SelectedFont"); }
        }

        private string selectedFontStyle;
        public string SelectedFontStyle
        {
            get { return selectedFontStyle; }
            set { selectedFontStyle = value; OnPropertyChaneged("SelectedFontStyle"); }
        }

        private float selectedFontSize;
        public float SelectedFontSize
        {
            get { return selectedFontSize; }
            set { selectedFontSize = value; OnPropertyChaneged("SelectedFontSize"); }
        }

        private Color selectedFontcolor;
        public Color SelectedFontcolor
        {
            get { return selectedFontcolor; }
            set { selectedFontcolor = value; OnPropertyChaneged("SelectedFontcolor"); }
        }

        public FontData()
        {
            FontStyleList = new ObservableCollection<string>
            {
                "보통", "기울임꼴", "굵게", "굵은 기울임꼴"
            };
            FontSizeList = new ObservableCollection<float>
            {
                6, 7, 8, 9, 10, 11, 12, 14, 16, 18,
                20, 22, 24, 26, 28, 36, 48, 72
            };
            SelectedFont = "Arial";
            SelectedFontStyle = "보통";
            SelectedFontSize = 8;
            selectedFontcolor = Color.FromArgb(255,0,0,0);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChaneged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class TextInputInfomation : INotifyPropertyChanged
    {
        private bool dateOpt;
        public bool DateOpt
        {
            get { return dateOpt; }
            set { dateOpt = value; OnPropertyChaneged("DateOpt"); }
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

        private bool userTextOpt;
        public bool UserTextOpt
        {
            get { return userTextOpt; }
            set { userTextOpt = value; OnPropertyChaneged("UserTextOpt"); }
        }

        private string userText;
        public string UserText
        {
            get { return userText; }
            set { userText = value; OnPropertyChaneged("UserText"); }
        }

        private bool alignment1;
        public bool Alignment1
        {
            get { return alignment1; }
            set { alignment1 = value; OnPropertyChaneged("Alignment1"); }
        }

        private bool alignment2;
        public bool Alignment2
        {
            get { return alignment2; }
            set { alignment2 = value; OnPropertyChaneged("Alignment2"); }
        }

        private bool alignment3;
        public bool Alignment3
        {
            get { return alignment3; }
            set { alignment3 = value; OnPropertyChaneged("Alignment3"); }
        }

        private bool alignment4;
        public bool Alignment4
        {
            get { return alignment4; }
            set { alignment4 = value; OnPropertyChaneged("Alignment4"); }
        }

        private bool alignment5;
        public bool Alignment5
        {
            get { return alignment5; }
            set { alignment5 = value; OnPropertyChaneged("Alignment5"); }
        }
        
        private bool alignment6;
        public bool Alignment6
        {
            get { return alignment6; }
            set { alignment6 = value; OnPropertyChaneged("Alignment6"); }
        }

        private FontData fontData;
        public FontData FontData
        {
            get { return fontData; }
            set { fontData = value; OnPropertyChaneged("FontData"); }
        }

        public TextInputInfomation()
        {
            FontData = new FontData();
            DateOpt = true;
            CameraOpt = false;
            LocateOpt = false;
            UserTextOpt = false;
            userText = "";
            alignment1 = false;
            alignment2 = false;
            alignment3 = false;
            alignment4 = true;
            alignment5 = false;
            alignment6 = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChaneged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
