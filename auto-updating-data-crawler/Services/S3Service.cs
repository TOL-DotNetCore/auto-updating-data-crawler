using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace auto_updating_data_crawler.Services
{
    public class S3Service : IS3Service
    {
        private readonly string AwsAccessKey = "AKIATYC5BMXR5PKO6AGO";
        private readonly string AwsSecretAccessKey = "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";
        private readonly string _bucketName = "chello";
        private readonly IAmazonS3 _awsS3Client;
        public S3Service()
        {
            _awsS3Client = new AmazonS3Client(AwsAccessKey, AwsSecretAccessKey, Amazon.RegionEndpoint.APSoutheast1);
        }
        public async Task<bool> DeleteFileS3(string fileName)
        {
            if (IsFileExists(fileName))
            {
                DeleteObjectRequest request = new DeleteObjectRequest()
                {
                    BucketName = _bucketName,
                    Key = fileName
                };
                await _awsS3Client.DeleteObjectAsync(request);
                return true;
            }
            return false;
        }

        public bool IsFileExists(string fileName)
        {
            try
            {
                GetObjectMetadataRequest request = new GetObjectMetadataRequest()
                {
                    BucketName = _bucketName,
                    Key = fileName
                };
                var response = _awsS3Client.GetObjectMetadataAsync(request).Result;
                return true;

            }
            catch(Exception ex)
            {
                if(ex.InnerException != null &&  ex.InnerException is AmazonS3Exception awsEx)
                {
                    if (string.Equals(awsEx.ErrorCode, "NoSuchBucket")) return false;
                    else if (string.Equals(awsEx.ErrorCode, "NotFound")) return false;
                }
                throw;
            }
        }
        public async Task UploadFileS3(byte[] blob)
        {
            var ms = new System.IO.MemoryStream();
            ms.Write(blob, 0, blob.Length);
            ms.Position = 0;
            var now = DateTime.Now.ToString().Replace("/", "-");
            var upLoadRequest = new TransferUtilityUploadRequest
            {
                InputStream = ms,
                Key = "weather_" + now + ".xlsx",
                BucketName = _bucketName,
                CannedACL = S3CannedACL.BucketOwnerFullControl
            };

            var fileTransferUtility = new TransferUtility(_awsS3Client);
            await fileTransferUtility.UploadAsync(upLoadRequest);   
        }
    }
}
