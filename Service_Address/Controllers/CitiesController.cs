using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service_Address.Models;
using Service_Address.Service;

namespace Service_Address.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly CitiesService _citiesService;
        public CitiesController(CitiesService citiesService)
        {
            _citiesService = citiesService;
        }



        /// <summary>
        /// Thêm mới city
        /// </summary>
        /// <param name="cities"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> InsertOneAsync(Cities cities)
        {

            try
            {

                await _citiesService.InsertOneAsync(cities);
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
        /// Cập nhật City
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cities"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOneAsync(string id, Cities cities)
        {
            try
            {
                // Gán lại dữ liêu
                cities.id = id;
                // update dữ liệu
                await _citiesService.UpdateOneAsync(Convert.ToString(id), cities);

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
        /// Xóa 1 bản ghi theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteManyAsync(CitiesFilter citiesFilter  )
        {
            var _filter = _citiesService.BuilderFilter(citiesFilter);
            await _citiesService.DeleteOneAsync(_filter);
            return Ok();

        }
    }
}
