using System;
using System.Windows;
using System.Windows.Input;
using TidyPhoto.Model;

namespace TidyPhoto.ViewModel.Command
{
    public class TextInputCommand : ICommand
    {
        private FolderInformation FolderInfo = Application.Current.MainWindow.Resources["FolderInfo"] as FolderInformation;
        private TextInputInfomation TextInputInfo = Application.Current.MainWindow.Resources["TextInputInfo"] as TextInputInfomation;

        public TextInput Text { get; set; }

        public TextInputCommand(TextInput Text)
        {
            this.Text = Text;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            if (FolderInfo.WorkFolder != "" && FolderInfo.DestFolder != "")
                return true;
            return false;
        }

        public void Execute(object parameter)
        {
            System.IO.DirectoryInfo workFolderInfo = new System.IO.DirectoryInfo(FolderInfo.WorkFolder);
            System.IO.DirectoryInfo destFolderInfo = new System.IO.DirectoryInfo(FolderInfo.DestFolder);

            if (workFolderInfo.Exists && destFolderInfo.Exists)
            {
                Text.DoEdit();
            }
            else if (!workFolderInfo.Exists && !destFolderInfo.Exists)
            {
                MessageBox.Show("  작업 폴더, 대상폴더가 존재하지 않습니다.", "실행 오류!",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (!workFolderInfo.Exists)
            {
                MessageBox.Show("  작업 폴더가 존재하지 않습니다.", "실행 오류!",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (!destFolderInfo.Exists)
            {
                MessageBox.Show("  대상 폴더가 존재하지 않습니다.", "실행 오류!",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }
    }
}
