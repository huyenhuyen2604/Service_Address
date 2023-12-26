using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Service_Address.Models;
using Service_Address.Service;

namespace Service_Address.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubregionsController : ControllerBase
    {
        private readonly SubregionsService _subregionsService; // khai báo gọi lại class serviec
        public SubregionsController(SubregionsService subregionsService)
        {
            _subregionsService = subregionsService;
        }

        /// <summary>
        /// Thêm mới 1 bản ghi subregion
        /// </summary>
        /// <param name="subregions"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> InsertOneAsync(Subregions subregions)
        {

            try
            {
                var _filter = _subregionsService.BuilderFilter(new SubregionsFitler
                {
                    id = subregions.id //id này là duy nhất
                });
                // Thêm mới dữ liệu gọi lại từ RegionsServiec
                await _subregionsService.InsertOneAsync(subregions);
                // lấy dữ liệu
                var rs = (await _subregionsService.FindAsync(
                    filter: _filter,
                    fields: typeof(Subregions).GetProperties().Select(x => x.Name).ToList()
                    )).FirstOrDefault();
                return Ok(rs);
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
        /// Cập nhật lại bản ghi
        /// </summary>
        /// <param name="id"></param>
        /// <param name="subregions"></param>
        /// <returns></returns>
        [HttpPut]

        public async Task<IActionResult> UpdateOneAsync(string id, Subregions subregions)
        {
           
            try
            {
                // Gán lại dữ liệu
                subregions.id = id;
                var _filter = _subregionsService.BuilderFilter(new SubregionsFitler { id = id });
                // update dữ liệu 
                await _subregionsService.UpdateOneAsync(id, subregions);
                // lấy lại chi tiết đã sửa
                var rs = (await _subregionsService.FindAsync(filter: _filter, fields: typeof(Subregions).GetProperties().Select(x => x.Name).ToList())).FirstOrDefault();
                return Ok(rs);
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