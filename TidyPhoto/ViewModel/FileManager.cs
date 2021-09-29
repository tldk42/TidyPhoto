using System;
using System.Collections.Generic;
using System.Linq;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;

namespace TidyPhoto.ViewModel
{
    public class FileManager
    {
        public List<String> GetFile(string workFolder)
        {
            List<string> imgList = new List<string>();
            imgList = System.IO.Directory.GetFiles(workFolder).Where(
                                   x => x.IndexOf(".jpg", StringComparison.OrdinalIgnoreCase) >= 0
                                     || x.IndexOf(".jpeg", StringComparison.OrdinalIgnoreCase) >= 0
                                     || x.IndexOf(".tiff", StringComparison.OrdinalIgnoreCase) >= 0
                                     || x.IndexOf(".png", StringComparison.OrdinalIgnoreCase) >= 0)
                                         .Select(x => x).ToList();
            return imgList;
        }

        public List<String> GetMetaData(List<string> imgList, int menuNum, string userText)
        {
            List<string> metaDataList = new List<string>();
            for (int i = 0; i < imgList.Count; i++)
            {
                var image = System.Drawing.Image.FromFile(imgList[i]);
                if (menuNum == 1 || menuNum == 2 || menuNum == 3) // 날짜
                {
                    if (image.PropertyIdList.Contains(ExifSubIfdDirectory.TagDateTimeOriginal))
                    {
                        var directories = ImageMetadataReader.ReadMetadata(imgList[i]);
                        var subIfDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
                        var Date = subIfDirectory?.GetDescription(ExifSubIfdDirectory.TagDateTimeOriginal);
                        metaDataList.Add(Date);
                    }
                    else
                        metaDataList.Add(null);
                }
                else if (menuNum == 4) // 기기
                {
                    if (image.PropertyIdList.Contains(ExifDirectoryBase.TagModel))
                    {
                        var directories = ImageMetadataReader.ReadMetadata(imgList[i]);
                        var subIfDirectory = directories.OfType<ExifDirectoryBase>().FirstOrDefault();
                        var Model = subIfDirectory?.GetDescription(ExifDirectoryBase.TagModel);
                        metaDataList.Add(Model);
                    }
                    else
                        metaDataList.Add(null);
                }
                else if (menuNum == 5) // 위치
                {
                    if (image.PropertyIdList.Contains(GpsDirectory.TagLatitude)
                        && image.PropertyIdList.Contains(GpsDirectory.TagLongitude))
                    {
                        var directories = ImageMetadataReader.ReadMetadata(imgList[i]);
                        var subIfDirectory = directories.OfType<GpsDirectory>().FirstOrDefault();
                        var latitude = subIfDirectory?.GetGeoLocation().Latitude.ToString(); // 위도
                        var longitude = subIfDirectory?.GetGeoLocation().Longitude.ToString(); // 경도
                        if (Double.Parse(latitude) < 33.10000000 || Double.Parse(latitude) > 38.45000000 ||
                            Double.Parse(longitude) < 125.06666667 || Double.Parse(longitude) > 131.87222222) // API 서비스 지역 밖
                            metaDataList.Add("해외");
                        else 
                        {
                            var regionName = new KakaoRestApi().GetRegionName(longitude, latitude); // 지역 이름
                            metaDataList.Add(regionName);
                        }
                    }
                    else
                        metaDataList.Add(null);
                }
                else if (menuNum == 6) // 유저 텍스트
                {
                    if (userText != null)
                        metaDataList.Add(userText);
                    else
                        metaDataList.Add(null);
                }
                image.Dispose();
            }
            return metaDataList;
        }

        public List<int> GetRotateInfo(List<string> imgList)
        {
            List<int> rotateList = new List<int>();
            for (int i = 0; i < imgList.Count; i++)
            {
                var image = System.Drawing.Image.FromFile(imgList[i]);
                if (image.PropertyIdList.Contains(ExifDirectoryBase.TagOrientation))
                {
                    var directories = ImageMetadataReader.ReadMetadata(imgList[i]);
                    var subExDirectory = directories.OfType<ExifDirectoryBase>().FirstOrDefault();
                    var RotateInfo = subExDirectory.GetInt16(ExifIfd0Directory.TagOrientation);

                    rotateList.Add(RotateInfo);
                }
                else
                    rotateList.Add(0);
                image.Dispose();
            }
            return rotateList;
        }
    }
}
