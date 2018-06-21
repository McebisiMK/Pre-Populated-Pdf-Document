using System.IO;
using Aspose.Pdf.Cloud.Sdk.Api;
using Aspose.Storage.Cloud.Sdk.Api;
using System.Configuration;

namespace Pre_populate_Pdf
{
    public class PdfReaderService
    {
        private readonly string _apiKey;
        private readonly string _appSid;

        public PdfReaderService()
        {
            _apiKey = ConfigurationManager.AppSettings["APIKEY"];
            _appSid = ConfigurationManager.AppSettings["APPSID"];
        }

        public string GetPopulatedPdfValues(string fileName, string fieldName)
        {
            var target = new PdfApi(_apiKey, _appSid);
            var apiResponse = target.GetField(fileName, fieldName);

            return apiResponse.Field.Values[0];
        }

    }
}