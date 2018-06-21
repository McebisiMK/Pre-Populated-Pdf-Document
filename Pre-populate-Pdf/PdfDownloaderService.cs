using Aspose.Storage.Cloud.Sdk.Api;
using Aspose.Storage.Cloud.Sdk.Model.Requests;
using RestSharp.Extensions;
using System.Configuration;
using System.IO;

namespace Pre_populate_Pdf
{
    public class PdfDownloaderService
    {
        private readonly string _apiKey;
        private readonly string _appSid;

        public PdfDownloaderService()
        {
            _apiKey = ConfigurationManager.AppSettings["APIKEY"];
            _appSid = ConfigurationManager.AppSettings["APPSID"];
        }

        public object DownloadDocument(string fileName, string pdfPath)
        {
            if (fileName.Trim().Equals(""))
            {
                return "You have supplied invalid path, please try again.";
            }

            var storage = new StorageApi(_apiKey, _appSid);
            var request = new GetDownloadRequest(fileName);

            using (var response = storage.GetDownload(request))
            {
                if (response == null)
                {
                    return "The file you're trying to download does not exist.";
                }
                var bytes = response.ReadAsBytes();
                File.WriteAllBytes(pdfPath, bytes);

                return $"You have successfully downloaded {fileName}.";
            };
        }
    }
}
