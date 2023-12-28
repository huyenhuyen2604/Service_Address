using MongoDB.Bson.IO;
using Service_Address.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using static Service_Address.Service.FileProcessingService;

namespace Service_Address.Service
{
    public class FileProcessingService : IFileAddressService
    {
        private readonly List<Countries> _Countries;
        private readonly List<Subregions> _Subregions;
        private readonly List<Regions> _Regions;
        private readonly List<States> _States;
        private readonly List<Cities> _Cities;
        //private FileStream _fs;
        //private BinaryFormatter _bf;

        #region Convert từ json sang object

        /// <summary>
        /// Convert từ json regions sang object
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        //public Regions ConvertJsonToObject(string jsonString)
        //{
        //    Regions regions = Newtonsoft.Json.JsonConvert.DeserializeObject<Regions>(jsonString);
        //    return regions;
        //}
        #endregion

        #region upload file
        //public interface IBufferedFileUploadService
        //{
        //    Task<bool> UploadFile(IFormFile file);
        //}

        //public class BufferedFileUploadLocalService : IBufferedFileUploadService
        //{
        //    public async Task<bool> UploadFile(IFormFile file)
        //    {
        //        string path = "C:\\Users\\HuyenVu\\OneDrive\\Desktop\\LenfulCode\\Service_Address\\Service_Address\\regions.json";
        //        try
        //        {
        //            if (file.Length > 0)
        //            {
        //                path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "UploadedFiles"));
        //                if (!Directory.Exists(path))
        //                {
        //                    Directory.CreateDirectory(path);
        //                }
        //                using (var fileStream = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
        //                {
        //                    await file.CopyToAsync(fileStream);
        //                }
        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new Exception("File Copy Failed", ex);
        //        }
        //    }
        //}


        //public async Task<string> UploadFileAsync(IFormFile file)
        //{
        //    if (file == null || file.Length == 0)
        //        return "File not selected or empty";

        //    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

        //    if (!Directory.Exists(uploadPath))
        //        Directory.CreateDirectory(uploadPath);

        //    var filePath = Path.Combine(uploadPath, file.FileName);

        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await file.CopyToAsync(stream);
        //    }

        //    return "File uploaded successfully";
        //}


        /// <summary>
        /// Upload file
        /// </summary>
        /// <returns></returns>

        public async Task<string> UploadFileAsync(IFormFile file, string jsonData)
        {
            // Deserialize JSON data
            var additionalData = Newtonsoft.Json.JsonConvert.DeserializeObject<Regions>(jsonData);

            if (file == null || file.Length == 0)
                return "File not selected or empty";

            // Use additionalData as needed...

            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var filePath = Path.Combine(uploadPath, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return "File uploaded successfully";
        }

        #endregion


        /// <summary>
        /// Đọc từ file json, service này được sử dụng để lấy dữ liệu và trả về nó dưới dạng JSON
        /// </summary>
        /// <param name="jsonFilePath"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<Regions> GetAndRegions(string jsonFilePath)
        {
            //List<Regions> regions = new List<Regions>();
            //_fs = new FileStream(jsonContent, FileMode.Open);
            //_bf = new BinaryFormatter();
            //var data = _bf.Deserialize(_fs);
            //regions = new List<Regions>();
            string jsonContent = File.ReadAllText(jsonFilePath);
         
            List<Regions> regions = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Regions>>(jsonContent);
            return regions;
        }

        public List<Countries> GetAndSCountries(string jsonFilePath)
        {
             string jsonContent = File.ReadAllText(jsonFilePath);
           

            List<Countries> countries = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Countries>>(jsonContent);

            return countries;
        }

        public List<States> GetAndStates(string jsonFilePath)
        {
            string jsonContent = File.ReadAllText(jsonFilePath);


            List<States> states = Newtonsoft.Json.JsonConvert.DeserializeObject<List<States>>(jsonContent);

            return states;
        }

        public List<Cities> GetAndCities(string jsonFilePath)
        {
            string jsonContent = File.ReadAllText(jsonFilePath);


            List<Cities> cities = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Cities>>(jsonContent);

            return cities;
        }

        public List<Subregions> GetSubregions(string jsonFilePath)
        {
            string jsonContent = File.ReadAllText(jsonFilePath);


            List<Subregions> subregions = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Subregions>>(jsonContent);

            return subregions;



        }


        // tạo Interface cho Service

        public interface IFileAddressService
        {
            List<Regions> GetAndRegions(string jsonFilePath);
            List<States> GetAndStates(string jsonFilePath);
            List<Cities> GetAndCities(string jsonFilePath);
            List<Subregions> GetSubregions(string jsonFilePath);
            List<Countries> GetAndSCountries(string jsonFilePath);


        }

        
        // Đọc file 
        //public async Task<string> ProcessFileAsync(IFormFile file)
        //{
        //    if (file != null && file.Length > 0)
        //    {
        //        // Đọc dữ liệu từ tệp tin ở đây
        //        using (var streamReader = new StreamReader(file.OpenReadStream()))
        //        {
        //            var fileContent = await streamReader.ReadToEndAsync();

        //            // Thực hiện các thao tác xử lý với nội dung tệp tin
        //            // Ví dụ: lưu trữ nó, xử lý nó, vv.

        //            return "File processed successfully";
        //        }
        //    }

        //    return "No file to process";
        //}

    }
}
