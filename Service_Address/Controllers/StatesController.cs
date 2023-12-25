using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Service_Address.Models;
using Service_Address.Service;

namespace Service_Address.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatesController : ControllerBase
    {
        private readonly StatesService _statesService;
        public StatesController(StatesService statesService)
        {
            _statesService = statesService;
        }

        [HttpPost]
        public async Task<IActionResult> InsertOneAsync(States states )
        {

            try
            {
                var _filter = _statesService.BuilderFilter(new StatesFilter
                {
                    id = states.id //id này là duy nhất
                });
                var _exists = await _statesService.AnyAsync(_filter);
                // Thêm mơi
                await _statesService.InsertOneAsync(states);
                //// Lấy dữ liệu
                var rs = (await _statesService.FindAsync(
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
        /// Cập nhật theo state 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="states"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOneAsync(string id, States states )
        {
            try
            {
                // Gán lại dữ liệu
                states.id = id;
                var _filter = _statesService.BuilderFilter(new StatesFilter { id = id });
                // Update dữ liệu
                await _statesService.UpdateOneAsync(_filter, states);
                // Lấy lại chi tiết
                var rs = (await _statesService.FindAsync(filter: _filter, fields: typeof(States).GetProperties().Select(x => x.Name).ToList())).FirstOrDefault();
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
        /// Xóa 1 bản ghi
        /// </summary>
        /// <param name="statesFilter"></param>
        /// <returns></returns>
        [HttpDelete]
        public  async Task<IActionResult> DeleteManyAsync(StatesFilter statesFilter)
        {
            try
            {
                var _filter = _statesService.BuilderFilter(statesFilter);
                await _statesService.DeleteOneAsync(_filter);
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
