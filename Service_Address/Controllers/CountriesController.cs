using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service_Address.Models;
using Service_Address.Service;

namespace Service_Address.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class CountriesController : ControllerBase
    {
        private CountriesService _countriesService;

        public CountriesController(CountriesService countriesService)
        {
            _countriesService = countriesService;
        }
        /// <summary>
        /// Thêm mới country
        /// </summary>
        /// <param name="countries"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> InsertOneAsync(Countries countries)
        {

            try
            {
                //var _filter = _countriesService.BuilderFilter(new CountriesFitler
                //{
                //    id = countries.id //id này là duy nhất
                //});
                // Thêm mơi
                await _countriesService.InsertOneAsync(countries);            
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new RsMessage
                {
                    status = false,
                    message = ex.Message
                });
            }


        }
        /// <summary>
        /// Cập nhật lại country
        /// </summary>
        /// <param name="id"></param>
        /// <param name="countries"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOneAsync(string id, Countries countries)
        {
            try
            {
                // Gán lại dữ liêu
                countries.id =id;
                var _filter = _countriesService.BuilderFilter(new CountriesFitler { id = id });
                // Update dữ liệu
                await _countriesService.UpdateOneAsync(_filter, countries);
                // Lấy lại chi tiết
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new RsMessage
                {
                    status = false,
                    message = ex.Message
                });
            }
        }
        /// <summary>
        /// Xóa 1 bản ghi country
        /// </summary>
        /// <param name="countriesFitler"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteManyAsync(CountriesFitler countriesFitler)
        {
            try
            {
                var _filter = _countriesService.BuilderFilter(countriesFitler);
                await _countriesService.DeleteOneAsync(_filter);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new RsMessage
                {
                    status = false,
                    message = ex.Message
                });
            }
        }
    }
}
