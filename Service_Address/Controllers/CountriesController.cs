using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using Service_Address.Models;
using Service_Address.Service;
using System.Collections.Generic;


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
                var _filter = _countriesService.BuilderFilter(new CountriesFitler
                {
                    id = countries.id //id này là duy nhất
                });
                // Thêm mới dữ liệu gọi lại từ servieccountries
                await _countriesService.InsertOneAsync(countries);
                // lấy dữ liệu
                var rs = (await _countriesService.FindAsync(
                    filter: _filter,
                    fields: typeof(Countries).GetProperties().Select(x => x.Name).ToList()
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
                    var _rs = fileContent.JsonToObject<List<Countries>>();
                    _ = InsertMultiCountry(_rs);


                }
                // Xử lý dữ liệu tệp văn bản ở đây
                // Ví dụ: Lưu vào cơ sở dữ liệu, xử lý nghiệp vụ, vv.

                //return Ok("Ok nhé");


                //// Đọc nội dung file
                //var fileContent = await reader.ReadToEndAsync();
                //if (!string.IsNullOrEmpty(fileContent))
                //{
                //    // Chuyển dữ liệu văn bản sang danh sách đối tượng Country
                //    var countries = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Countries>>(fileContent);
                //    await InsertMultiCountry(countries);
                //}
                //// Xử lý dữ liệu tệp văn bản ở đây
                //// Ví dụ: Lưu vào cơ sở dữ liệu, xử lý nghiệp vụ, vv.

                return Ok("Import successful");
            }
        }

  

        /// <summary>
        /// Xử lý sự kiện thêm nhiều country
        /// </summary>
        /// <param name="countries"></param>
        /// <returns></returns>
        private async Task InsertMultiCountry(List<Countries>? countries)
        {
            if (countries == null || countries.Count == 0) return;
            foreach (var country in countries)
            {
                try
                {
                    await _countriesService.InsertOneAsync(country);
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
                var rs = (await _countriesService.FindAsync(filter: _filter, fields: typeof(States).GetProperties().Select(x => x.Name).ToList())).FirstOrDefault();
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
