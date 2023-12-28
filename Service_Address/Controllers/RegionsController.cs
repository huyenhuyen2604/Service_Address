
using Helpers;
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
                // Thêm mới dữ liệu gọi lại từ RegionsServiec
                await _regionsServiec.InsertOneAsync(regions);
                // lấy dữ liệu
                var rs = (await _regionsServiec.FindAsync(
                    filter: _filter,
                    fields: typeof(Regions).GetProperties().Select(x => x.Name).ToList()
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
                    var _rs = fileContent.JsonToObject<List<Regions>>();
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
        /// Xử lý sự kiện thêm nhiều Region
        /// </summary>
        /// <param name="countries"></param>
        /// <returns></returns>
        private async Task InsertMultiCountry(List<Regions>? regions)
        {
            if (regions == null || regions.Count == 0) return;
            foreach (var region in regions)
            {
                try
                {
                    await _regionsServiec.InsertOneAsync(region);
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
                var rs = (await _regionsServiec.FindAsync(filter: _filter, fields: typeof(Regions).GetProperties().Select(x => x.Name).ToList())).FirstOrDefault();
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
            try
            {
                var _filter = _regionsServiec.BuilderFilter(regionsFitler);
                await _regionsServiec.DeleteOneAsync(_filter);
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
