using System;
using System.Windows.Input;
using Microsoft.WindowsAPICodePack.Dialogs;
using TidyPhoto.Model;

namespace TidyPhoto.ViewModel.Command
{
    public class DestFolderCommand : ICommand
    {
        public FolderInformation FolderInfo { get; set; }

        public DestFolderCommand(FolderInformation FolderInfo)
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
                FolderInfo.DestFolder = folderDialog.FileName;
                if (FolderInfo.DestFolder != FolderInfo.WorkFolder)
                    FolderInfo.IsFolderEqual = false;
            }
        }
    }
}
