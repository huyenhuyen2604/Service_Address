using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service_Address.Models;
using Service_Address.Service;

namespace Service_Address.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private FileProcessingService _fileProcessingService1;


        public AddressController(FileProcessingService fileProcessingService)
        {
            _fileProcessingService1 = fileProcessingService;
        }

        /// <summary>
        /// đọc dữ liệu từ file json  này được sử dụng để lấy dữ liệu và trả về nó dưới dạng JSON
        /// </summary>
        /// <returns></returns>
        [HttpGet("regions")]
        public IActionResult GetRegions([FromBody] string jsonString)
        {
            string jsonFilePath = "C:\\Users\\HuyenVu\\OneDrive\\Desktop\\LenfulCode\\Service_Address\\Service_Address\\regions.json"; // Đặt đường dẫn thực tế đến tệp JSON của bạn

            List<Regions> regions = _fileProcessingService1.GetAndRegions(jsonFilePath);

            return Ok(regions);
        }


        /// <summary>
        /// đọc dữ liệu từ file json  này được sử dụng để lấy dữ liệu và trả về nó dưới dạng JSON
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
        /// đọc dữ liệu từ file json  này được sử dụng để lấy dữ liệu và trả về nó dưới dạng JSON
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
        /// đọc dữ liệu từ file json  này được sử dụng để lấy dữ liệu và trả về nó dưới dạng JSON
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
        /// đọc dữ liệu từ file json  này được sử dụng để lấy dữ liệu và trả về nó dưới dạng JSON
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
