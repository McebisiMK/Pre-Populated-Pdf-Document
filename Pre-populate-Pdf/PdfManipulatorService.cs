using Aspose.Pdf.Cloud.Sdk.Api;
using Aspose.Pdf.Cloud.Sdk.Model;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pre_populate_Pdf
{
    public class PdfManipulatorService
    {
        private readonly string _apiKey;
        private readonly string _appSid;

        public PdfManipulatorService()
        {
            _apiKey = ConfigurationManager.AppSettings["APIKEY"];
            _appSid = ConfigurationManager.AppSettings["APPSID"];
        }
        
        public string AddValuesToTexboxes(string fileName, Fields values)
        {
            var target = new PdfApi(_apiKey, _appSid);
            var apiResponse = target.PutUpdateFields(fileName, values);

            return apiResponse.Status;
        }

        public byte[] SetTextboxesToReadOnly(string inputStream, string outputStrem, string[] fieldsToDisable)
        {
            var reader = new PdfReader(inputStream);
            var ms = new MemoryStream();
            var stamper = new PdfStamper(reader, new FileStream(outputStrem, FileMode.Create));

            foreach (var field in fieldsToDisable)
            {
                stamper.AcroFields.SetFieldProperty(field, "setfflags", PdfFormField.FF_READ_ONLY, null);
            }
            stamper.Close();
            ms.Position = 0;
            var fileBytes = ms.ToArray();
            ms.Flush();
            reader.Close();

            var readOnlyBytes = File.ReadAllBytes(outputStrem);

            return readOnlyBytes;
        }
    }
}
