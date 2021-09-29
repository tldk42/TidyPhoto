using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using TidyPhoto.Model;
using TidyPhoto.ViewModel.Command;
using Directory = System.IO.Directory;

namespace TidyPhoto.ViewModel
{
    public class ClassifyFolder
    {
        public ClassifyFolderCommand CFCommand { get; set; }

        private ProgressInformation ProgressInfo = Application.Current.MainWindow.Resources["ProgressInfo"] as ProgressInformation;
        private FolderInformation FolderInfo = Application.Current.MainWindow.Resources["FolderInfo"] as FolderInformation;
        private ClsfFolderInformation ClsfFolderInfo = Application.Current.MainWindow.Resources["ClsfFolderInfo"] as ClsfFolderInformation;
        private BackgroundWorker worker = new BackgroundWorker();
        private List<String> metaDataList = new List<String>();
        private List<string> imgList = new List<string>();
        private int menuNum = 0;

        public ClassifyFolder()
        {
            CFCommand = new ClassifyFolderCommand(this);

            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = true;
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Worker_RunWorkerCompleted);
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressInfo.ProgressNum = e.ProgressPercentage;
            ProgressInfo.StateText = ProgressInfo.ProgressNum.ToString() + "%";
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("  실행 완료!", $"폴더 분류 - {FolderInfo.DestFolder}",
                MessageBoxButton.OK, MessageBoxImage.Asterisk);
            Process.Start(FolderInfo.DestFolder);
            ProgressInfo.StateText = "준비 중...";
            ProgressInfo.ProgressNum = 0;
        }

        public void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            menuNum = GetMenuNum();
            if (menuNum == 0) { worker.ReportProgress(100); return; }
            imgList = new FileManager().GetFile(FolderInfo.WorkFolder);
            if (imgList.Count == 0) { worker.ReportProgress(100); return; }
            worker.ReportProgress(25);
            metaDataList = new FileManager().GetMetaData(imgList, menuNum, null);
            worker.ReportProgress(50);
            MakeFolder(FolderInfo.DestFolder);
            MoveFile(imgList, FolderInfo.WorkFolder);
            metaDataList.Clear();
            imgList.Clear();
            return;
        }

        public void DoClassify()
        {
            worker.RunWorkerAsync();
        }
        
        public int GetMenuNum()
        {
            if (ClsfFolderInfo.YearOpt)
                return 1;
            else if (ClsfFolderInfo.MonthOpt)
                return 2;
            else if (ClsfFolderInfo.DayOpt)
                return 3;
            else if (ClsfFolderInfo.CameraOpt)
                return 4;
            else if (ClsfFolderInfo.LocateOpt)
                return 5;
            else
                return 0;
        }

        public void MakeFolder(string destFolder)
        {
            List<string> makeFolderPathList = new List<string>();

            for (int i = 0; i < metaDataList.Count; i++)
            {
                if (metaDataList[i] == null)
                {
                    makeFolderPathList.Add($"{destFolder}\\데이터 없음");
                }
                else if (menuNum == 1) // 연도
                {
                    makeFolderPathList.Add($"{destFolder}\\{metaDataList[i].Substring(0, 4)}년");
                }
                else if(menuNum == 2) // 연,월
                {
                    makeFolderPathList.Add($"{destFolder}\\{metaDataList[i].Substring(0, 4)}년" +
                        $"_{metaDataList[i].Substring(5, 2)}월");
                }
                else if(menuNum == 3) // 연,월,일
                {
                    makeFolderPathList.Add($"{destFolder}\\{metaDataList[i].Substring(0, 4)}년" +
                        $"_{metaDataList[i].Substring(5, 2)}월_{metaDataList[i].Substring(8, 2)}일");
                }
                else if(menuNum == 4 || menuNum == 5) // 카메라, 위치
                {
                    makeFolderPathList.Add($"{destFolder}\\{metaDataList[i]}");
                }
            }

            makeFolderPathList = makeFolderPathList.Distinct().ToList();

            for (int i = 0; i < makeFolderPathList.Count; i++)
            {
                if (Directory.Exists(makeFolderPathList[i]) == false)
                    Directory.CreateDirectory(makeFolderPathList[i]);
                worker.ReportProgress(((i + 1) * 25 / imgList.Count) + 50);
            }
         }

        public void MoveFile(List<string> imgList, string workFolder)
        {
            List<string> moveFolderPathList = new List<string>();

            for (int i = 0; i < metaDataList.Count; i++)
            {
                if (metaDataList[i] == null)
                {
                    moveFolderPathList.Add($"{workFolder}\\데이터 없음\\{Path.GetFileName(imgList[i])}");
                }
                else if (menuNum == 1) // 연도
                {
                    moveFolderPathList.Add($"{workFolder}\\{metaDataList[i].Substring(0, 4)}년" +
                        $"\\{Path.GetFileName(imgList[i])}");
                }
                else if (menuNum == 2) // 연,월
                {
                    moveFolderPathList.Add($"{workFolder}\\{metaDataList[i].Substring(0, 4)}년" +
                        $"_{metaDataList[i].Substring(5, 2)}월\\{Path.GetFileName(imgList[i])}");
                }
                else if (menuNum == 3) // 연,월,일
                {
                    moveFolderPathList.Add($"{workFolder}\\{metaDataList[i].Substring(0, 4)}년" +
                        $"_{metaDataList[i].Substring(5, 2)}월_{metaDataList[i].Substring(8, 2)}일" +
                        $"\\{Path.GetFileName(imgList[i])}");
                }
                else if (menuNum == 4 || menuNum == 5) // 카메라, 위치
                {
                    moveFolderPathList.Add($"{workFolder}\\{metaDataList[i]}\\{Path.GetFileName(imgList[i])}");
                }
            }

            for (int i = 0; i < metaDataList.Count; i++)
            {
                if (File.Exists(imgList[i]))
                    System.IO.File.Move(imgList[i], moveFolderPathList[i]);
                worker.ReportProgress(((i + 1) * 25 / imgList.Count)+75);
            }
        }
    }
}
