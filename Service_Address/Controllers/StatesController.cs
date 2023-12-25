using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

                // Thêm mơi
                await _statesService.InsertOneAsync(states);
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
                // Gán lại dữ liêu
                states.id = id;
                var _filter = _statesService.BuilderFilter(new StatesFilter { id = id });
                // Update dữ liệu
                await _statesService.UpdateOneAsync(_filter, states);
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
