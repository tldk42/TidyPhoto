using System;
using System.Windows.Input;
using Microsoft.WindowsAPICodePack.Dialogs;
using TidyPhoto.Model;

namespace TidyPhoto.ViewModel.Command
{
    public class WorkFolderCommand : ICommand
    {
        public FolderInformation FolderInfo { get; set; }

        public WorkFolderCommand(FolderInformation FolderInfo)
        {
            this.FolderInfo = FolderInfo;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            CommonOpenFileDialog folderDialog = new CommonOpenFileDialog()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                IsFolderPicker = true,
                ShowPlacesList = true
            };

            if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {   
                FolderInfo.WorkFolder = folderDialog.FileName;
                if (FolderInfo.IsFolderEqual)
                    FolderInfo.DestFolder = folderDialog.FileName;
            }
        }
    }
}
