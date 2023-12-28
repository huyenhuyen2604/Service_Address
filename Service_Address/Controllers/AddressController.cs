using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service_Address.Models;
using Service_Address.Service;
using static Service_Address.Service.FileProcessingService;

namespace Service_Address.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private FileProcessingService _fileProcessingService1;
        //private IBufferedFileUploadService _bufferedFileUploadService;


        public AddressController(FileProcessingService fileProcessingService/*, IBufferedFileUploadService bufferedFileUploadService*/)
        {
            _fileProcessingService1 = fileProcessingService;
            //_bufferedFileUploadService = bufferedFileUploadService;
        }
        /// <summary>
        /// convert sang object
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        //public Region GetMyObject(string jsonString)
        //{
        //    return (Region)_fileProcessingService1.ConvertJsonToObject(jsonString);
        //}

        /// <summary>
        /// đọc dữ liệu từ file json
        /// </summary>
        /// <returns></returns>
        [HttpGet("regions")]
        public IActionResult GetRegions(/*[FromBody] string jsonString, Region region*/)
        {
            string jsonFilePath = "C:\\Users\\HuyenVu\\OneDrive\\Desktop\\LenfulCode\\Service_Address\\Service_Address\\regions.json"; // Đặt đường dẫn thực tế đến tệp JSON của bạn

            List<Regions> regions = _fileProcessingService1.GetAndRegions(jsonFilePath);
          

            return Ok(regions);
        }
        #region đọc file
        ///// <summary>
        ///// Upload file
        ///// </summary>
        ///// <param name="file"></param>
        ///// <returns></returns>
        //[HttpPost("single")]
        //public async Task<IActionResult> UploadSingleFile(IFormFile file)
        //{
        //    if (file == null || file.Length == 0)
        //        return BadRequest("File not selected");

        //    var uploads = Path.Combine(_fileProcessingService1.ContentRootPath);
        //    var filePath = Path.Combine(uploads, file.FileName);

        //    using (var fileStream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await file.CopyToAsync(fileStream);
        //    }

        //    return Ok($"File uploaded successfully: {file.FileName}");


        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="files"></param>
        ///// <returns></returns>
        //[HttpPost("multiple")]
        //public async Task<IActionResult> UploadMultipleFiles(List<IFormFile> files)
        //{
        //    if (files == null || files.Count == 0)
        //        return BadRequest("Files not selected");

        //    var uploads = Path.Combine(_fileProcessingService1.ContentRootPath);

        //    foreach (var file in files)
        //    {
        //        var filePath = Path.Combine(uploads, file.FileName);

        //        using (var fileStream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await file.CopyToAsync(fileStream);
        //        }
        //    }

        //    return Ok($"Files uploaded successfully: {string.Join(", ", files.Select(f => f.FileName))}");
        //}




        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File not selected or empty");

            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var filePath = Path.Combine(uploadPath, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok("File uploaded successfully");
        }


        //[HttpPost("upload")]
        //public async Task<IActionResult> UploadFile(IFormFile file)
        //{
        //    var result = await _fileProcessingService1.UploadFileAsync(file);
        //    return Ok(result);
        //}

        //[HttpPost("upload")]
        //public async Task<IActionResult> UploadFile(IFormFile file, [FromBody] string jsonData)
        //{

        //    var result = await _fileProcessingService1.UploadFileAsync(file, jsonData);
        //    return Ok(result);
        //}


        #endregion

        /// <summary>
        /// đọc lấy dữ liệu từ file json
        /// </summary>
        /// <returns></returns>
        [HttpGet("Countries")]
        public IActionResult GetCountries()
        {
            string jsonFilePath = "C:\\Users\\HuyenVu\\OneDrive\\Desktop\\LenfulCode\\Service_Address\\Service_Address\\countries.json"; // Đặt đường dẫn thực tế đến tệp JSON của bạn

            List<Countries> countries = _fileProcessingService1.GetAndSCountries(jsonFilePath);

            return Ok(countries);
        }
        /// <summary>
        /// đọc lấy dữ liệu từ file json
        /// </summary>
        /// <returns></returns>
        [HttpGet("States")]
        public IActionResult GetStates()
        {
            string jsonFilePath = "C:\\Users\\HuyenVu\\OneDrive\\Desktop\\LenfulCode\\Service_Address\\Service_Address\\states.json"; // Đặt đường dẫn thực tế đến tệp JSON của bạn

            List<States> states = _fileProcessingService1.GetAndStates(jsonFilePath);

            return Ok(states);
        }
        /// <summary>
        /// đọc lấy dữ liệu từ file json
        /// </summary>
        /// <returns></returns>
        [HttpGet("Cities")]
        public IActionResult GetCities()
        {
            string jsonFilePath = "C:\\Users\\HuyenVu\\OneDrive\\Desktop\\LenfulCode\\Service_Address\\Service_Address\\cities.json"; // Đặt đường dẫn thực tế đến tệp JSON của bạn

            List<Cities> cities = _fileProcessingService1.GetAndCities(jsonFilePath);

            return Ok(cities);
        }
        /// <summary>
        /// đọc dữ liệu từ file json
        /// </summary>
        /// <returns></returns>
        [HttpGet("Subregions")]
        public IActionResult GetSubregions()
        {
            string jsonFilePath = "C:\\Users\\HuyenVu\\OneDrive\\Desktop\\LenfulCode\\Service_Address\\Service_Address\\subregions.json"; // Đặt đường dẫn thực tế đến tệp JSON của bạn

            List<Subregions> subregions = _fileProcessingService1.GetSubregions(jsonFilePath);

            return Ok(subregions);
        }
      
    }
}
