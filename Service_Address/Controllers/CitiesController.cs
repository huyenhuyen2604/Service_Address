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
    public class CitiesController : ControllerBase
    {
        private readonly CitiesService _citiesService;
        public CitiesController(CitiesService citiesService)
        {
            _citiesService = citiesService;
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
                    var _rs = fileContent.JsonToObject<List<Cities>>();
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
        private async Task InsertMultiCountry(List<Cities>? cities)
        {
            if (cities == null || cities.Count == 0) return;
            foreach (var citie in cities)
            {
                try
                {
                    await _citiesService.InsertOneAsync(citie);
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
        /// Thêm mới city
        /// </summary>
        /// <param name="cities"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> InsertOneAsync(Cities cities)
        {
            try
            {
                var _filter = _citiesService.BuilderFilter(new CitiesFilter
                {
                    id = cities.id //id này là duy nhất
                });
                // Thêm mới dữ liệu gọi lại từ servieccountries
                await _citiesService.InsertOneAsync(cities);
                // lấy dữ liệu
                var rs = (await _citiesService.FindAsync(
                    filter: _filter,
                    fields: typeof(Cities).GetProperties().Select(x => x.Name).ToList()
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
