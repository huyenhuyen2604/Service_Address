using Helpers;
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("import")]
        public async Task<IActionResult> ImportAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File not found");
            }

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                // Đọc nội dung file
                var fileContent = await reader.ReadToEndAsync();
                if (!string.IsNullOrEmpty(fileContent))
                {
                    // Chuyển dữ liệu text sang dạng List Country
                    var _rs = fileContent.JsonToObject<List<States>>();
                    _ = InsertMultiCountry(_rs);


                }
                // Xử lý dữ liệu tệp văn bản ở đây
                // Ví dụ: Lưu vào cơ sở dữ liệu, xử lý nghiệp vụ, vv.

                //return Ok("Ok nhé");


                ////// Đọc nội dung file
                //var fileContent = await reader.ReadToEndAsync();
                //if (!string.IsNullOrEmpty(fileContent))
                //{
                //    // Chuyển dữ liệu văn bản sang danh sách đối tượng Country
                //    var countries = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Countries>>(fileContent);
                //    await InsertMultiCountry(countries);
                //}
                ////// Xử lý dữ liệu tệp văn bản ở đây
                ////// Ví dụ: Lưu vào cơ sở dữ liệu, xử lý nghiệp vụ, vv.

                return Ok("Import successful");
            }
        }



        /// <summary>
        /// Xử lý sự kiện thêm nhiều Cities
        /// </summary>
        /// <param name="countries"></param>
        /// <returns></returns>
        private async Task InsertMultiCountry(List<States>? states)
        {
            if (states == null || states.Count == 0) return;
            foreach (var state in states)
            {
                try
                {
                    await _statesService.InsertOneAsync(state);
                    // cho nó nghỉ 50ms không oẳng giờ
                    await Task.Delay(100);
                }
                catch
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// Thêm mới 1 bản ghi
        /// </summary>
        /// <param name="states"></param>
        /// <returns></returns>
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
