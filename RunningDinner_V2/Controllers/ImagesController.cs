using System;
using System.Collections.Generic;
using System.IO;
using Azure;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RunningDinner.Extensions;
using RunningDinner.Helpers;
using Microsoft.AspNetCore.Identity;
using RunningDinner.Models.DatabaseModels;

namespace RunningDinner.Controllers
{
    [Route("Identity/Account/api/[controller]")]
    public class ImagesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IConfiguration _configuration;

        public ImagesController(IConfiguration configuration,
            UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        // POST /api/images/upload
        [HttpPost("[action]")]
        public async Task<IActionResult> Upload()
        {
            var files = HttpContext.Request.Form.Files;
            Response<BlobContentInfo> response = null;
            var accountKey = _configuration?.GetAzureStorageSettings("AccountKey");
            var baseUrl = _configuration?.GetAzureStorageSettings("BaseUrl");
            var accountName = _configuration?.GetAzureStorageSettings("AccountName");
            var imageContainer = _configuration?.GetAzureStorageSettings("ImageContainer");
            var thumbnailContainer = _configuration?.GetAzureStorageSettings("ThumbnailContainer");
            var fileName = string.Empty;
            try
            {
                if (files.Count == 0)
                {
                    return BadRequest("No files received from the upload");
                }

                if (string.IsNullOrEmpty(accountKey) || string.IsNullOrEmpty(accountName))
                {
                    return BadRequest("sorry, can't retrieve your azure storage details from appsettings.js, make sure that you add azure storage details there");
                }

                if (string.IsNullOrEmpty(imageContainer))
                {
                    return BadRequest("Please provide a name for your image container in the azure blob storage");
                }

                foreach (var formFile in files)
                {
                    if (StorageHelper.IsImage(formFile))
                    {
                        if (formFile.Length > 0)
                        {
                            using Stream stream = formFile.OpenReadStream();
                            fileName = Guid.NewGuid() + formFile.FileName;
                            response = await StorageHelper.UploadFileToStorage(stream, fileName, _configuration).ConfigureAwait(false);
                        }
                    }

                    else
                    {
                        return new UnsupportedMediaTypeResult();
                    }
                }

                if (response != null)
                {
                    if (!string.IsNullOrEmpty(thumbnailContainer))
                    {
                        var user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
                        user.TemporaryProfilePicture = baseUrl + imageContainer + "/" + fileName;
                        await _userManager.UpdateAsync(user).ConfigureAwait(false);
                        return new AcceptedResult();
                    }

                    else
                    {
                        return new AcceptedResult();
                    }
                }

                else
                {
                    return BadRequest("Look like the image couldnt upload to the storage");
                }
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET /api/images/thumbnails
        [HttpGet("thumbnails")]
        public async Task<IActionResult> GetThumbNails()
        {
            var accountKey = _configuration?.GetAzureStorageSettings("AccountKey");
            var accountName = _configuration?.GetAzureStorageSettings("AccountName");
            var imageContainer = _configuration?.GetAzureStorageSettings("ImageContainer");
            try
            {
                if (string.IsNullOrEmpty(accountKey) || string.IsNullOrEmpty(accountName))
                {
                    return BadRequest("sorry, can't retrieve your azure storage details from appsettings.js, make sure that you add azure storage details there");
                }

                if (string.IsNullOrEmpty(imageContainer))
                {
                    return BadRequest("Please provide a name for your image container in the azure blob storage");
                }

                List<string> thumbnailUrls = await StorageHelper.GetThumbNailUrls(_configuration).ConfigureAwait(false);
                return new ObjectResult(thumbnailUrls);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}