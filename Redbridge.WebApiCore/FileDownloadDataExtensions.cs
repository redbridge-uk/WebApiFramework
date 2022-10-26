using System.IO.Compression;
using System.Net;
using System.Net.Http.Headers;
using Redbridge.Data;
using Redbridge.IO;

namespace Redbridge.WebApiCore
{
    public static class FileDownloadDataExtensions
    {
        public static HttpResponseMessage AsFileResult(this FileDownloadData downloadData)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(downloadData.FileStream)
            };

            result.Content.Headers.ContentType = new MediaTypeHeaderValue(downloadData.ContentType);
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = downloadData.FileName
            };

            return result;
        }

        public static HttpResponseMessage AsZippedFileResult(this FileDownloadsData downloadData)
        {
            using (var archiveStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
                {
                    foreach (var download in downloadData.Files)
                    {
                        var zipArchiveEntry = archive.CreateEntry(download.FileName, CompressionLevel.Fastest);
                        using var zipStream = zipArchiveEntry.Open();
                        zipStream.Write(download.FileStream.ToByteArray(), 0, (int)download.FileStream.Length);
                    }
                }

                var archiveFile = archiveStream.ToArray();
                return ZipContentResult(archiveFile, downloadData.FileName);
            }
        }

        private static HttpResponseMessage ZipContentResult( byte[] archiveFile, string filename)
        {
            var pushStreamContent = new PushStreamContent((stream, content, context) =>
            {
                stream.Write(archiveFile);
                stream.Close(); // After save we close the stream to signal that we are done writing.
            }, "application/zip");

            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = pushStreamContent };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/zip");
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = filename
            };
            return response;
        }
    }
}
