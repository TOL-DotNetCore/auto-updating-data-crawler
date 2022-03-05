using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace auto_updating_data_crawler.Services
{
    public interface IS3Service
    {
        Task UploadFileS3(IFormFile file);
        Task<bool> DeleteFileS3(string fileName);
    }
}
