using Aspose.Pdf.Cloud.Sdk.Api;
using Aspose.Pdf.Cloud.Sdk.Model;
using Aspose.Storage.Cloud.Sdk.Api;
using Aspose.Storage.Cloud.Sdk.Model.Requests;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace Pre_populate_Pdf
{
    [TestFixture]
    public class PrePopulatePdfTests
    {
        private string _fileName = "Boot_camp_form _v2.pdf";
        [Test]
        public void AddFileToCloudStorage_GivenAnExistingPdfFile_ShouldUploadTheFileToCloudStorage()
        {
            var apiKey = "f544165e1e73730ef3216cfa484e682e";
            var appSid = "68a6e9bf-cab2-43d6-82cd-39c351a018b3";

            var baseDirectory = TestContext.CurrentContext.TestDirectory;
            var pdfPath = Path.Combine(baseDirectory, _fileName);

            using (var stream = new FileStream(pdfPath, FileMode.Open))
            {
                var storageApi = new StorageApi(apiKey, appSid);
                var request = new PutCreateRequest(_fileName, stream);
                var actual = storageApi.PutCreate(request);

                Assert.AreEqual(200, actual.Code);
            }

        }

        [Test]
        public void AddValuesToTexboxes_GivenAnExistingPdfFile_ShouldAddGivenValueToItsTextboxAndReturnOk()
        {
            //---------------Arrange------------------
            var value = new Fields
            {
                List = new List<Field>
               {
                   new Field
                   {
                       Name="FirstName",
                       Values=new List<string>
                       {
                           "Mcebisi"
                       }
                   },
            }
            };

            var sut = CreatePdfManipulatorService();

            //---------------Act------------------
            var actual = sut.AddValuesToTexboxes(_fileName, value);

            //---------------Assert------------------
            var expected = "OK";
            Assert.AreEqual(expected, actual);

        }

        [Test]
        public void AddValuesToTexboxes_GivenAnExistingPdfFile_ShouldAddGivenValuesToTheirTextboxesAndReturnOk()
        {
            //---------------Arrange------------------
            var values = new Fields
            {
                List = new List<Field>
               {
                   new Field
                   {
                       Name="Surname",
                       Values=new List<string>
                       {
                           "Mkhohliwe"
                       }
                   },
                   new Field
                   {
                       Name="DateOfBirth",
                       Values=new List<string>
                       {
                           "1900-02-09"
                       }
                   }
            }
            };

            var sut = CreatePdfManipulatorService();

            //---------------Act------------------
            var actual = sut.AddValuesToTexboxes(_fileName, values);

            //---------------Assert------------------
            var expected = "OK";
            Assert.AreEqual(expected, actual);

        }

        [TestCase("FirstName", "Mcebisi")]
        [TestCase("Surname", "Mkhohliwe")]
        [TestCase("DateOfBirth", "1900-02-09")]
        public void GetPopulatedPdfValues_GivenAnExistingPdfFile_ShouldGetRequestedField(string fieldName, string expected)
        {
            //---------------Arrange------------------
            string filename = "Boot_camp_form _v2.pdf";

            var sut = new PdfReaderService();

            //---------------Act------------------
            var actual = sut.GetPopulatedPdfValues(filename, fieldName);

            //---------------Assert------------------
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SetTextboxesToReadonly_GivenAnExistingPdfFile_ShouldSetTheProtertyToReadonly()
        {
            //---------------------------Arrange--------------------------------------
            var inputStream = @"M:\Visual Studio\Pre-Populated-Pdf-Document\Boot_camp_form_v2.pdf";
            var outputStrem = @"M:\Visual Studio\Pre-Populated-Pdf-Document\Boot_camp_form_v2(Readonly).pdf";
            var fieldsToDisable = new string[] { "FirstName", "Surname", "DateOfBirth" };

            var sut = CreatePdfManipulatorService();

            //---------------------------Act------------------------------------------
            var actual = sut.SetTextboxesToReadOnly(inputStream, outputStrem, fieldsToDisable);

            //---------------------------Assert---------------------------------------
            var expectedBytes = 110124;
            Assert.AreEqual(expectedBytes, actual.Length);

        }

        [TestCase("")]
        [TestCase(" ")]
        public void DownloadDocument_GivenInvalidFileName_ShouldReturnErrorMessage(string fileName)
        {
            //---------------------------Arrange--------------------------------------
            var path = @"M:\Visual Studio\Pre-Populated-Pdf-Document\Boot_camp_form_v2.pdf";

            var sut = CreatePdfDownloaderService();

            //---------------------------Act------------------------------------------
            var actual = sut.DownloadDocument(fileName, path);

            //---------------------------Assert---------------------------------------
            var expectedMessage = "You have supplied invalid path, please try again.";
            Assert.AreEqual(expectedMessage, actual);

        }

        [Test]
        public void DownloadDocument_GivenThatFileDoesNotExists_ShouldReturnErrorMessage()
        {
            //---------------------------Arrange--------------------------------------
            var path = @"M:\Visual Studio\Pre-Populated-Pdf-Document\document.pdf";
            var fileName = "Boot_camp_form_v1";

            var sut = CreatePdfDownloaderService();

            //---------------------------Act------------------------------------------
            var actual = sut.DownloadDocument(fileName, path);

            //---------------------------Assert---------------------------------------
            var expectedMessage = "The file you're trying to download does not exist.";
            Assert.AreEqual(expectedMessage, actual);

        }

        [Test]
        public void DownloadDocument_GivenThatFileIsAvailableOnCloud_ShouldReturnSuccessMessage()
        {
            //---------------------------Arrange--------------------------------------
            var path = @"M:\Visual Studio\Pre-Populated-Pdf-Document\Boot_camp_form_v2.pdf";

            var sut = CreatePdfDownloaderService();

            //---------------------------Act------------------------------------------
            var actual = sut.DownloadDocument(_fileName, path);

            //---------------------------Assert---------------------------------------
            var expectedMessage = $"You have successfully downloaded {_fileName}.";
            Assert.AreEqual(expectedMessage, actual);

        }

        private static PdfDownloaderService CreatePdfDownloaderService()
        {
            return new PdfDownloaderService();
        }

        private static PdfManipulatorService CreatePdfManipulatorService()
        {
            return new PdfManipulatorService();
        }
    }
}