using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using static System.Console;

namespace BlobDropper
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteLine("BlobDropper: backup public blob data from a azure blob container");
            WriteLine("================================================================\r\n");

            if(args.Length != 3)
            {
                WriteLine("Usage:\r\nBlobDropper [blob connection string] [container name] [local directory to save to]");
            }

            var connectionString = args[0];
            var containerName = args[1];
            var localDirectory = args[2];

            try
            {
                var cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
                var blobClient = cloudStorageAccount.CreateCloudBlobClient();
                var blobContainer = blobClient.GetContainerReference(containerName);

                Write($"Getting list of blobs in container '{containerName}'...");
                var blobs = blobContainer.ListBlobs().ToArray();
                WriteLine($"{blobs.Length} found.");

                using (var http = new HttpClient())
                    for (var i = 0; i < blobs.Length; i++)
                    {
                        var blob = blobs[i];
                        var filename = Path.GetFileName(blob.Uri.AbsolutePath);

                        Write($"Processing {i + 1}: {filename}...");
                        using (var file = File.OpenWrite(Path.Combine(localDirectory, filename)))
                        using (var download = http.GetStreamAsync(blob.Uri).GetAwaiter().GetResult())
                            download.CopyTo(file);
                        WriteLine("Done");
                    }

                WriteLine("\r\nJob Complete");
            }
            catch(Exception ex)
            {
                Error.WriteLine($"Failed to copy down blob data.\r\nError was: {ex.Message}");
            }
            

            //CloudBlockBlob blockBlob = cloudBlobContainer.GetBlockBlobReference("uploadedfilename.ext");


            //MemoryStream memStream = new MemoryStream();

            //blockBlob.DownloadToStream(memStream);

            //HttpContext.Current.Response.ContentType = blockBlob.Properties.ContentType.ToString();
            //HttpContext.Current.Response.AddHeader("Content-Disposition", "Attachment; filename=" + blockBlob.ToString());

            //HttpContext.Current.Response.AddHeader("Content-Length", blockBlob.Properties.Length.ToString());
            //HttpContext.Current.Response.BinaryWrite(memStream.ToArray());
            //HttpContext.Current.Response.Flush();
            //HttpContext.Current.Response.Close();
        }
    }
}
