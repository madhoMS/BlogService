namespace BlogService.Common.Utilities
{
    public class Common
    {

        public static string UploadImage(string base64Image, string module)
        {
            string imageURL = "";
            if (!string.IsNullOrEmpty(base64Image))
            {
                string webRootPath = "";
                string imageSavePath = module == "User" ? "//Images//Profile//" : "//Images//Post//";

                //get root path
                webRootPath = Directory.GetCurrentDirectory();
                var base64Data = "";
                if (base64Image.StartsWith("data:image/"))
                {
                    base64Data = base64Image.Split(",")[1];
                }
                else
                {
                    base64Data = base64Image;
                }

                string extension = ".png";

                if (!Directory.Exists(webRootPath + imageSavePath))
                {
                    Directory.CreateDirectory(webRootPath + imageSavePath);
                }
                if (!Directory.Exists(webRootPath + imageSavePath))
                {
                    Directory.CreateDirectory(webRootPath + imageSavePath);
                }

                string picName = Guid.NewGuid().ToString();

                imageURL = webRootPath + imageSavePath + picName + extension;

                byte[] bytes = Convert.FromBase64String(base64Data);


                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    using (FileStream fileStream = new FileStream(imageURL, FileMode.Create))
                    {
                        ms.CopyTo(fileStream);
                    }
                }

                imageURL = imageSavePath + picName + extension;

            }
            else
            {
                imageURL = "";
            }

            return imageURL;

        }

    }
}
