using System.IO;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using TransactionStore.Client.Contracts;

namespace TransactionStore.Api.Tests.Helpers
{
    public static class FileExtensions
    {
        public static FileParameter ToFormFile(this FileInfo physicalFile)
        {
            var stream = physicalFile.OpenRead();
            var file = new FormFile(stream, 0, stream.Length, physicalFile.Name, physicalFile.Name);

            return new FileParameter(file.OpenReadStream(), physicalFile.Name);
        }

        public static FileParameter ToXmlFormFile(this FileInfo physicalFile)
        {
            var xmlDoc = XDocument.Load(physicalFile.OpenRead(), LoadOptions.SetLineInfo);
            var xmlStream = new MemoryStream();
            xmlDoc.Save(xmlStream);
            xmlStream.Flush();
            xmlStream.Position = 0;
            return new FileParameter(xmlStream, physicalFile.Name);
        }
    }
}