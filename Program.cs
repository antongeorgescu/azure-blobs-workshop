using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.IO;

namespace TestDocumentManager
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Azure Blob Storage!");

            Uri accountUri = new Uri("https://p01d12202014001.blob.core.windows.net/");
            try
            {
                switch (args[0])
                {
                    case "UPN":
                        ManageBlobsWithUPN(accountUri, "nslsc");
                        break;
                    case "KEY":
                        ManageBlobsWithKey(accountUri, "cal");
                        break;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }

        static void ManageBlobsWithUPN(Uri storageAccountUri, string lob)
        {
            var credential = new DefaultAzureCredential(includeInteractiveCredentials: true);
            var token = credential.GetToken(
                new Azure.Core.TokenRequestContext(
                    new[] { "https://graph.microsoft.com/.default" }));

            var accessToken = token.Token;

            BlobServiceClient client = new BlobServiceClient(storageAccountUri, credential);

            var blobContainerUri = new Uri($"https://p01d12202014001.blob.core.windows.net/{lob}");
            BlobContainerClient container = new BlobContainerClient(blobContainerUri, credential);

            Console.WriteLine("=== List of current blobs ===");
            foreach (BlobItem blob in container.GetBlobs())
            {
                Console.WriteLine(blob.Name);
            }
            
            string blob_path = "studentonline/2022/march";

            string filePath = @"Files/S3_data_protection.pdf";
            // Upload a few blobs so we have something to list
            container.UploadBlob($"{blob_path}/S3DataProtection.pdf", File.OpenRead(filePath));

            Console.WriteLine("=== List of updated blobs ===");
            foreach (BlobItem blob in container.GetBlobs())
            {
                Console.WriteLine(blob.Name);
            }
            return;
        }

        static void ManageBlobsWithKey(Uri storageAccountUri,string lob)
        {
            BlobServiceClient client = new BlobServiceClient(storageAccountUri);

            // Get a connection string to our Azure Storage account.
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=p01d12202014001;AccountKey=y3bJ0P/HVYhWjQKTJypjedeJsSmFRAtiyhsUumUkN3VhA81g2QSUIMW0mlEe4vRIc4VK60MvzBKiwCDZZmgayg==;EndpointSuffix=core.windows.net";
            string containerName = lob;

            BlobContainerClient container = new BlobContainerClient(connectionString, containerName);

            Console.WriteLine("=== List of current blobs ===");
            foreach (BlobItem blob in container.GetBlobs())
            {
                Console.WriteLine(blob.Name);
            }

            var blob_path = "agentonline/2022/july";

            var filePath = @"Files/S3_data_protection.pdf";
            // Upload a few blobs so we have something to list
            container.UploadBlob($"{blob_path}/S3DataProtection.pdf", File.OpenRead(filePath));

            Console.WriteLine("=== List of updated blobs ===");
            foreach (BlobItem blob in container.GetBlobs())
            {
                Console.WriteLine(blob.Name);
            }
            return;
        }

        
    }
}
