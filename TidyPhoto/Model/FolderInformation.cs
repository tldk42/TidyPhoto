using System.ComponentModel;
using TidyPhoto.ViewModel.Command;

namespace TidyPhoto.Model
{
    public class FolderInformation : INotifyPropertyChanged
    {
        public WorkFolderCommand WorkFolderCommand { get; set; }
        public DestFolderCommand DestFolderCommand { get; set; }

        private string workFolder;
        public string WorkFolder
        {
            get { return workFolder; }
            set { workFolder = value; OnPropertyChaneged("WorkFolder"); }
        }
        private string destFolder;
        public string DestFolder
        {
            get { return destFolder; }
            set { destFolder = value; OnPropertyChaneged("DestFolder"); }
        }

        private bool isFolderEqual;
        public bool IsFolderEqual
        {
            get { return isFolderEqual; }
            set 
            { 
                isFolderEqual = value; 
                if(isFolderEqual == true)
                    DestFolder = WorkFolder;
                OnPropertyChaneged("IsFolderEqual");
            }
        }

        public FolderInformation()
        {
            WorkFolder = "";
            DestFolder = "";
            isFolderEqual = true;
            WorkFolderCommand = new WorkFolderCommand(this);
            DestFolderCommand = new DestFolderCommand(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChaneged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
