
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Service_Address.Models;
using Service_Address.Service;

namespace Service_Address.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private RegionsServiec _regionsServiec;

        public RegionsController(RegionsServiec regionsServiec)
        {
            _regionsServiec = regionsServiec;
        }

        /// <summary>
        /// Thêm mới 1 bản ghi regions
        /// </summary>
        /// <param name="regions"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<IActionResult> InsertOneAsync(Regions regions)
        {

            try
            {
                var _filter = _regionsServiec.BuilderFilter(new RegionsFitler
                {
                    id = regions.id //id này là duy nhất
                });
                var _exists = await _regionsServiec.AnyAsync(_filter);
                // Thêm mơi
                await _regionsServiec.InsertOneAsync(regions);
                /// Lấy dữ liệu
                var rs = (await _regionsServiec.FindAsync(
                    filter: _filter,
                    fields: typeof(States).GetProperties().Select(x => x.Name).ToList()
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
        /// <param name="regions"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOneAsync(string id, Regions regions)
        {
           
            try
            {
                // Gán lại dữ liêu
                regions.id = id;
                var _filter = _regionsServiec.BuilderFilter(new RegionsFitler { id = id });
                // Update dữ liệu
                await _regionsServiec.UpdateOneAsync(Convert.ToString(id), regions);
                // Lấy lại chi tiết
                var rs = (await _regionsServiec.FindAsync(filter: _filter, fields: typeof(States).GetProperties().Select(x => x.Name).ToList())).FirstOrDefault();
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
        /// Xóa 1 bản ghi theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteManyAsync(RegionsFitler regionsFitler)
        {
            var _filter = _regionsServiec.BuilderFilter(regionsFitler);
            await _regionsServiec.DeleteOneAsync(_filter);
            return Ok();

        }
    }
}
