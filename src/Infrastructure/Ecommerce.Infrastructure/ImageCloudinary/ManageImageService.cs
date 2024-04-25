using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Models.ImageMangement;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.ImageCloudinary
{
    public class ManageImageService : IManageImageService
    {
        public CloudinarySettings cloudinarySettings { get; }

        public ManageImageService(IOptions<CloudinarySettings> cloudinarySettings)
        {
            this.cloudinarySettings = cloudinarySettings.Value;
        }

        //the follow method is for upload an image
        public async Task<ImageResponse> UploadImage(ImageData imageStream)
        {
            //initialize the cloudinay account
            var account=new Account(cloudinarySettings.CloudName, 
                                    cloudinarySettings.ApiKey, 
                                    cloudinarySettings.ApiSecret);

            //Cloudinary Client

            var cloudinary = new Cloudinary(account);

            //Upload Object

            var uploadImage = new ImageUploadParams()
            {
                File=new FileDescription(imageStream.Name, imageStream.ImageStream)
            };

            var uploadResult=await cloudinary.UploadAsync(uploadImage);

            if (uploadResult.StatusCode==HttpStatusCode.OK)
            {
                var response= new ImageResponse() { 
                    PublicId=uploadResult.PublicId,
                     Url=uploadResult.Url.ToString(),
                };
                return response; 
            }

            throw new Exception("Couldn´t to image upload");
        }
    }
}
