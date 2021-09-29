using System.IO;
using System.Net;
using System.Web.Script.Serialization;

namespace TidyPhoto.ViewModel
{
    class KakaoRestApi
    {
        public string GetRegionName(string x, string y)
        {
            string url = "https://dapi.kakao.com/v2/local/geo/coord2regioncode.json";
            url += $"?x={x}"; // 경도
            url += $"&y={y}"; // 위도

            var request = (HttpWebRequest)WebRequest.Create(url);
            string rkey = "5808d016ab8ea5d7a8fc9c24c100a082"; // Rest API 키
            string header = "KakaoAK " + rkey;
            request.Headers.Add("Authorization", header);
            request.Method = "GET";

            string results = string.Empty;
            HttpWebResponse response;

            using (response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                results = reader.ReadToEnd();
            }

            JavaScriptSerializer js = new JavaScriptSerializer();
            dynamic dob = js.Deserialize<dynamic>(results);
            dynamic docs = dob["documents"];
            string RegionName = $"{docs[0]["region_1depth_name"]}_{docs[0]["region_2depth_name"]}";

            return RegionName;
        }
    }
}
