using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using TidyPhoto.Model;
using TidyPhoto.ViewModel.Command;

namespace TidyPhoto.ViewModel
{
    public class TextInput
    {
        public TextInputCommand TXTCommand { get; set; }

        private ProgressInformation ProgressInfo = Application.Current.MainWindow.Resources["ProgressInfo"] as ProgressInformation;
        private TextInputInfomation TextInputInfo = Application.Current.MainWindow.Resources["TextInputInfo"] as TextInputInfomation;
        private FolderInformation FolderInfo = Application.Current.MainWindow.Resources["FolderInfo"] as FolderInformation;
        private BackgroundWorker worker = new BackgroundWorker();
        private List<String> metaDataList = new List<String>();
        private List<string> imgList = new List<string>();
        private List<int> rotateList = new List<int>();
        private int menuNum = 0;
        private int location;

        public TextInput()
        {
            TXTCommand = new TextInputCommand(this);

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
            MessageBox.Show("  실행 완료!", $"문구 삽입 - {FolderInfo.DestFolder}",
                MessageBoxButton.OK, MessageBoxImage.Asterisk);
            Process.Start(FolderInfo.DestFolder);
            ProgressInfo.StateText = "준비 중...";
            ProgressInfo.ProgressNum = 0;
        }

        public void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            menuNum = GetMenuNum();
            if (menuNum == 0) { worker.ReportProgress(100); return; }
            location = GetLocation();
            imgList = new FileManager().GetFile(FolderInfo.WorkFolder);
            if (imgList.Count == 0) { worker.ReportProgress(100); return; }
            worker.ReportProgress(25);
            metaDataList = new FileManager().GetMetaData(imgList, menuNum, TextInputInfo.UserText);
            rotateList = new FileManager().GetRotateInfo(imgList);
            worker.ReportProgress(50);
            System.Windows.Media.Color InputColor = TextInputInfo.FontData.SelectedFontcolor;
            System.Drawing.Color color = System.Drawing.Color.FromArgb(InputColor.A, InputColor.R, InputColor.G, InputColor.B);
            Edit(FolderInfo.DestFolder, location, imgList, metaDataList, color);
            metaDataList.Clear();
            rotateList.Clear();
            imgList.Clear();
        }

        public void DoEdit()  
        {
            worker.RunWorkerAsync(); 
        }

        public int GetMenuNum()
        {
            if (TextInputInfo.DateOpt)
                return 1;
            else if (TextInputInfo.CameraOpt)
                return 4;
            else if (TextInputInfo.LocateOpt)
                return 5;
            else if (TextInputInfo.UserTextOpt)
            {
                return 6;
            }
            else
                return 0;
        }

        public int GetLocation()
        {
            if (TextInputInfo.Alignment1)
                return 1;
            else if (TextInputInfo.Alignment2)
                return 2;
            else if (TextInputInfo.Alignment3)
                return 3;
            else if (TextInputInfo.Alignment4)
                return 4;
            else if (TextInputInfo.Alignment5)
                return 5;
            else if (TextInputInfo.Alignment6)
                return 6;
            else
                return 0;
        }

        public void Edit(string destFolder, int location, List<string> imgList, List<string> metaDataList, Color color)
        {
            for (int i = 0; i < imgList.Count; i++)
            {
                String text;

                if (metaDataList.Count == 1)
                    text = metaDataList[0];

                else
                {
                    if (metaDataList[i] == null)
                        text = "";
                    else
                        text = metaDataList[i];
                }

                var EXFileName = Path.GetFileName(imgList[i]).Replace(Path.GetExtension(imgList[i]), "");
                var CopyNmae = EXFileName + "의 사본" + Path.GetExtension(imgList[i]);
                var FileName = EXFileName + Path.GetExtension(imgList[i]);

                Bitmap b = new Bitmap(imgList[i]);
                Graphics g = Graphics.FromImage(b);

                var myFontStyle = new System.Drawing.FontStyle();
                if (TextInputInfo.FontData.SelectedFontStyle == "굵게")
                    myFontStyle = System.Drawing.FontStyle.Bold;
                else if (TextInputInfo.FontData.SelectedFontStyle == "기울임꼴")
                    myFontStyle = System.Drawing.FontStyle.Italic;
                else if (TextInputInfo.FontData.SelectedFontStyle == "굵은 기울임꼴")
                    myFontStyle = System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic;
                else
                    myFontStyle = System.Drawing.FontStyle.Regular;

                Brush MyBrush = new SolidBrush(color);
                Font myFont 
                    = new Font(new FontFamily(TextInputInfo.FontData.SelectedFont), 
                    TextInputInfo.FontData.SelectedFontSize * 10, myFontStyle, GraphicsUnit.Pixel);

                StringFormat st = new StringFormat();
                Rectangle colRec = new Rectangle(0, 0, b.Width, b.Height);
                if (location == 1)
                {
                    st.Alignment = StringAlignment.Near;
                    st.LineAlignment = StringAlignment.Near;
                }
                else if (location == 2)
                {
                    st.Alignment = StringAlignment.Far;
                    st.LineAlignment = StringAlignment.Near;
                }
                else if (location == 3)
                {
                    st.Alignment = StringAlignment.Near;
                    st.LineAlignment = StringAlignment.Far;
                }
                else if (location == 4)
                {
                    st.Alignment = StringAlignment.Far;
                    st.LineAlignment = StringAlignment.Far;
                }
                else if (location == 5)
                {
                    st.Alignment = StringAlignment.Center;
                    st.LineAlignment = StringAlignment.Near;
                }
                else
                {
                    st.Alignment = StringAlignment.Center;
                    st.LineAlignment = StringAlignment.Far;
                }

                if (rotateList[i] == 6)
                {
                    g.TranslateTransform(b.Width / 2, b.Height / 2);
                    g.RotateTransform(270);
                    g.TranslateTransform(-(b.Width / 2), -(b.Height / 2));
                    colRec = new Rectangle(((b.Width - b.Height) / 2), -((b.Width - b.Height) / 2), b.Height, b.Width);
                }
                else if (rotateList[i] == 3)
                {
                    g.TranslateTransform(b.Width / 2, b.Height / 2);
                    g.RotateTransform(180);
                    g.TranslateTransform(-(b.Width / 2), -(b.Height / 2));
                }
                else if (rotateList[i] == 8)
                {
                    g.TranslateTransform(b.Width / 2, b.Height / 2);
                    g.RotateTransform(90);
                    g.TranslateTransform(-(b.Width / 2), -(b.Height / 2));
                    colRec = new Rectangle(((b.Width - b.Height) / 2), -((b.Width - b.Height) / 2), b.Height, b.Width);
                }

                g.DrawString(text, myFont, MyBrush, colRec, st);

                if (FolderInfo.WorkFolder == destFolder)
                    b.Save(destFolder + "\\" + CopyNmae);
                else
                    b.Save(destFolder + "\\" + FileName);

                g.Flush();
                b.Dispose();
                worker.ReportProgress(((i + 1) * 50 / imgList.Count) + 50);
            }
        }
    }
}
