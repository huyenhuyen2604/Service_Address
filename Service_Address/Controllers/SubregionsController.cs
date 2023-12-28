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
    public class SubregionsController : ControllerBase
    {
        private readonly SubregionsService _subregionsService; // khai báo gọi lại class serviec
        public SubregionsController(SubregionsService subregionsService)
        {
            _subregionsService = subregionsService;
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
                    var _rs = fileContent.JsonToObject<List<Subregions>>();
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
        private async Task InsertMultiCountry(List<Subregions>? subregions)
        {
            if (subregions == null || subregions.Count == 0) return;
            foreach (var subregion in subregions)
            {
                try
                {
                    await _subregionsService.InsertOneAsync(subregion);
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