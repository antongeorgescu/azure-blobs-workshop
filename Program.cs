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
            string storageAccountName = "<storage account name>";
            Uri accountUri = new Uri($"https://{storageAccountName}.blob.core.windows.net/");
            try
            {
                switch (args[0])
                {
                    case "UPN":
                        ManageBlobsWithUPN(accountUri, "nslsc", storageAccountName);
                        break;
                    case "KEY":
                        ManageBlobsWithKey(accountUri, "cal", storageAccountName);
                        break;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }

        static void ManageBlobsWithUPN(Uri storageAccountUri, string lob, string storageAccount)
        {
            var credential = new DefaultAzureCredential(includeInteractiveCredentials: true);
            var token = credential.GetToken(
                new Azure.Core.TokenRequestContext(
                    new[] { "https://graph.microsoft.com/.default" }));

            var accessToken = token.Token;

            BlobServiceClient client = new BlobServiceClient(storageAccountUri, credential);

            var blobContainerUri = new Uri($"https://{storageAccount}.blob.core.windows.net/{lob}");
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

        static void ManageBlobsWithKey(Uri storageAccountUri,string lob,string storageAccount)
        {
            BlobServiceClient client = new BlobServiceClient(storageAccountUri);

            // Get a connection string to our Azure Storage account.
            string secret = "<secret key>";
            string connectionString = $"DefaultEndpointsProtocol=https;AccountName={storageAccount};AccountKey={secret};EndpointSuffix=core.windows.net";
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
