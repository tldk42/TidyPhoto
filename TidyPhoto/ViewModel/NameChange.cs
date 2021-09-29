using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using TidyPhoto.Model;
using TidyPhoto.ViewModel.Command;

namespace TidyPhoto.ViewModel
{
    public class NameChange
    {
        public NameChangeCommand NCCommand { get; set; }

        private ProgressInformation ProgressInfo = Application.Current.MainWindow.Resources["ProgressInfo"] as ProgressInformation;
        private FolderInformation FolderInfo = Application.Current.MainWindow.Resources["FolderInfo"] as FolderInformation;
        private NameChangeInformation NameChangeInfo = Application.Current.MainWindow.Resources["NameChangeInfo"] as NameChangeInformation;
        private BackgroundWorker worker = new BackgroundWorker();
        private List<string> imgList = new List<string>();
        private List<List<String>> metaDataTypeList = new List<List<string>>();
        private List<int> menuNumList = new List<int>();
        private List<string> userTextList = new List<string>(); 

        public NameChange()
        {
            NCCommand = new NameChangeCommand(this);

            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = true;
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Worker_RunWorkerCompleted);
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("  실행 완료!", $"이름 변경 - {FolderInfo.DestFolder}",
                MessageBoxButton.OK, MessageBoxImage.Asterisk);
            Process.Start(FolderInfo.DestFolder);
            ProgressInfo.StateText = "준비 중...";
            ProgressInfo.ProgressNum = 0;
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressInfo.ProgressNum = e.ProgressPercentage;
            ProgressInfo.StateText = ProgressInfo.ProgressNum.ToString() + "%";
        }

        public void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            AddDataToList();
            if (menuNumList.Count == 0) { worker.ReportProgress(100); return; }
            imgList = new FileManager().GetFile(FolderInfo.WorkFolder);
            if (imgList.Count == 0) { worker.ReportProgress(100); return; }
            worker.ReportProgress(25);
            MakemetaDataList(menuNumList, userTextList);
            MakeChangeNameLsit(imgList, menuNumList);
            Rename(FolderInfo.DestFolder, imgList);
            metaDataTypeList.Clear();
            imgList.Clear();
        }

        public void DoNameChange()
        {
            worker.RunWorkerAsync();
        }

        public void AddDataToList()
        {
            //첫번째 콤보 
            if (NameChangeInfo.SelectedOpt1 == "촬영 날짜") //날짜
            {
                menuNumList.Add(3);
                userTextList.Add(null);
            }
            else if (NameChangeInfo.SelectedOpt1 == "촬영 위치") //위치
            {
                menuNumList.Add(5);
                userTextList.Add(null);
            }
            else if (NameChangeInfo.SelectedOpt1 == "촬영 기기") //기기
            {
                menuNumList.Add(4);
                userTextList.Add(null);
            }
            else if (NameChangeInfo.SelectedOpt1 == "직접 입력") //직접 입력
            {
                menuNumList.Add(6);
                userTextList.Add(NameChangeInfo.UserText1);
            }

            //두번째 콤보 박스
            if (NameChangeInfo.SelectedOpt2 == "촬영 날짜") //날짜
            {
                menuNumList.Add(3);
                userTextList.Add(null);
            }
            else if (NameChangeInfo.SelectedOpt2 == "촬영 위치") //위치
            {
                menuNumList.Add(5);
                userTextList.Add(null);
            }
            else if (NameChangeInfo.SelectedOpt2 == "촬영 기기") //기기
            {
                menuNumList.Add(4);
                userTextList.Add(null);
            }
            else if (NameChangeInfo.SelectedOpt2 == "직접 입력") //직접 입력
            {
                menuNumList.Add(6);
                userTextList.Add(NameChangeInfo.UserText2);
            }

            //세번째 콤보 박스
            if (NameChangeInfo.SelectedOpt3 == "촬영 날짜") //날짜
            {
                menuNumList.Add(3);
                userTextList.Add(null);
            }
            else if (NameChangeInfo.SelectedOpt3 == "촬영 위치") //위치
            {
                menuNumList.Add(5);
                userTextList.Add(null);
            }
            else if (NameChangeInfo.SelectedOpt3 == "촬영 기기") //기기
            {
                menuNumList.Add(4);
                userTextList.Add(null);
            }
            else if (NameChangeInfo.SelectedOpt3 == "직접 입력") //직접 입력
            {
                menuNumList.Add(6);
                userTextList.Add(NameChangeInfo.UserText3);
            }

            //네번째 콤보 박스
            if (NameChangeInfo.SelectedOpt4 == "촬영 날짜") //날짜
            {
                menuNumList.Add(3);
                userTextList.Add(null);
            }
            else if (NameChangeInfo.SelectedOpt4 == "촬영 위치") //위치
            {
                menuNumList.Add(5);
                userTextList.Add(null);
            }
            else if (NameChangeInfo.SelectedOpt4 == "촬영 기기") //기기
            {
                menuNumList.Add(4);
                userTextList.Add(null);
            }
            else if (NameChangeInfo.SelectedOpt4 == "직접 입력") //직접 입력
            {
                menuNumList.Add(6);
                userTextList.Add(NameChangeInfo.UserText4);
            }
        }

        public void MoveFile(string sourceFileName, string destFileName)
        {
            int count = 1;
            string FileName = System.IO.Path.GetFileNameWithoutExtension(destFileName);
            string Extension = System.IO.Path.GetExtension(destFileName);
            string path = System.IO.Path.GetDirectoryName(destFileName);
            string newPath = destFileName;
            while (File.Exists(newPath))
            {
                string tmpFileNM = string.Format("{0} ({1})", FileName, count++);
                newPath = System.IO.Path.Combine(path, tmpFileNM + Extension);
            }
            try
            {
                File.Move(sourceFileName, newPath); destFileName = newPath;
            }
            catch (Exception ex) { throw ex; }
        }

        public void MakemetaDataList(List<int> menuNumList, List<string> userTextList)
        {
            for (int i = 0; i < menuNumList.Count; i++) 
            {
                metaDataTypeList.Add(new FileManager().GetMetaData(imgList, menuNumList[i], userTextList[i]));
                worker.ReportProgress(((i + 1) * 25 / imgList.Count) + 25);
            }
        }

        public void MakeChangeNameLsit(List<string> imgList, List<int> menuNumList)
        {
            for(int i = 0; i < metaDataTypeList.Count; i++)
            {
                for (int j = 0; j < imgList.Count; j++) 
                {
                    if (menuNumList[i] == 3) //날짜 
                    {
                        if (metaDataTypeList[i][j] == null)
                            metaDataTypeList[i][j] = ("날짜없음");
                        else
                            metaDataTypeList[i][j] = $"{metaDataTypeList[i][j].Substring(0, 4)}" + //년
                                                     $"{metaDataTypeList[i][j].Substring(5, 2)}" + //월
                                                     $"{metaDataTypeList[i][j].Substring(8, 2)}_" + //일
                                                     $"{metaDataTypeList[i][j].Substring(11, 2)}" + //시
                                                     $"{metaDataTypeList[i][j].Substring(14, 2)}" + //분
                                                     $"{metaDataTypeList[i][j].Substring(17, 2)}"; //초
                    }
                    else if (menuNumList[i] == 4) //기기 
                    {
                        if (metaDataTypeList[i][j] == null)
                            metaDataTypeList[i][j] = ("기기없음");
                    }
                    else if (menuNumList[i] == 5) //위치 
                    {
                        if (metaDataTypeList[i][j] == null)
                            metaDataTypeList[i][j] = ("위치없음");
                    }
                    if (metaDataTypeList[i][j] == null)
                        metaDataTypeList[i][j] = ("데이터 없음");
                }
                worker.ReportProgress(((i + 1) * 25 / imgList.Count) + 50);
            }
        }

        public void Rename(string destFolder, List<string> imgList)
        {
            List<string> changeNamePathList = new List<string>();
            if (metaDataTypeList.Count == 1)
            {
                for (int i = 0; i < imgList.Count; i++)
                {
                    changeNamePathList.Add($"{destFolder}\\{metaDataTypeList[0][i]}.jpg");
                }
            }
            else if (metaDataTypeList.Count == 2)
            {
                for (int i = 0; i < imgList.Count; i++)
                {
                    changeNamePathList.Add($"{destFolder}\\{metaDataTypeList[0][i]}_{metaDataTypeList[1][i]}.jpg");
                }
            }
            else if (metaDataTypeList.Count == 3)
            {
                for (int i = 0; i < imgList.Count; i++)
                {
                    changeNamePathList.Add($"{destFolder}\\{metaDataTypeList[0][i]}_{metaDataTypeList[1][i]}" +
                                           $"_{metaDataTypeList[2][i]}.jpg");
                }       
            }
            else if (metaDataTypeList.Count == 4)
            {
                for (int i = 0; i < imgList.Count; i++)
                {
                    changeNamePathList.Add($"{destFolder}\\{metaDataTypeList[0][i]}_{metaDataTypeList[1][i]}" +
                                           $"_{metaDataTypeList[2][i]}_{metaDataTypeList[3][i]}.jpg");
                }
            }
            for (int i = 0; i < imgList.Count; i++)
            {
                MoveFile(imgList[i], changeNamePathList[i]);
                worker.ReportProgress(((i + 1) * 25 / imgList.Count) + 75);
            }
        }
    }
}
