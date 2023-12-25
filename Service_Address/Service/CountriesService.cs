using Service_Address.Models;
using Helpers;
using Microsoft.Extensions.Options;
using MongoDB.Driver;


namespace Service_Address.Service
{
    public class CountriesService
    {
        private readonly string CollectionName = "Countries";

        //--Thực thể sẽ làm việc
        private readonly IMongoCollection<Countries> _Collection;
        //--Dữ liệu hiển thị mặc định
        private ProjectionDefinition<Countries> _FieldsDefault;

        //--Khởi tạo kết nối đễn colection (table) Product
     
        public CountriesService(IOptions<List<Models.ConnectionInfo>> Connections)
        {
            //-- Cấu hình kết nối đến mongoDB
            var conn = Connections.Value?.Find(x => x.DatabaseName == "Address")
           ?? throw new Exception("Connection DB Test Failed");

            // Tạo kết nối tới DB student
            MongoClient client = new(conn.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(conn.DatabaseName);
            _Collection = database.GetCollection<Countries>(CollectionName);
        }



        /// <summary>
        /// Kiểm tra dữ liệu tồn tại hay không
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Task<bool> AnyAsync(FilterDefinition<Countries> filter)
        {
            return _Collection.Find(filter).AnyAsync();
        }

        /// <summary>
        /// Lấy danh sách student
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="fields"></param>
        /// <param name="sort_by"></param>
        public Task<IAsyncCursor<Countries>> FindAsync(FilterDefinition<Countries> filter, int? page = 1, int? limit = 250, List<string>? fields = null, string? sort_by = "id_desc")
        {



            // Lấy trường nào
            if (fields != null && fields.Count > 0)
            {
                _FieldsDefault = Builders<Countries>.Projection.Include(fields.First());
                foreach (var field in fields.Skip(1)) _FieldsDefault = _FieldsDefault.Include(field);
            }

            // Sắp xếp kiểu gì
            var _sort_builder = Builders<Countries>.Sort;
            var _sort = _sort_builder.Descending("id");
            switch (sort_by)
            {
                case "id_desc":
                    _sort = _sort_builder.Descending("id");
                    break;
                case "id_asc":
                    _sort = _sort_builder.Ascending("id");
                    break;
                case "name_asc":
                    _sort = _sort_builder.Ascending("name");
                    break;
                case "name_desc":
                    _sort = _sort_builder.Descending("name");
                    break;


                default: break;
            }
            return _Collection.FindAsync(filter, new FindOptions<Countries, Countries>
            {
                AllowDiskUse = true,
                Limit = limit,
                Skip = (page - 1) * limit,
                Projection = _FieldsDefault,
                Sort = _sort,
            });

        }

        /// <summary>
        /// Tìm kiếm theo bộ lọc
        /// </summary>
        public FilterDefinition<Countries> BuilderFilter(CountriesFitler countriesFitler)
        {
            var _builder = Builders<Countries>.Filter;
            var _filter = _builder.Ne(c => c.id, null);

            // Tìm kiếm theo id
            if (!string.IsNullOrEmpty(countriesFitler.id)) _filter &= _builder.Eq(c => c.id, countriesFitler.id);

            // Tìm kiếm theo ids
            if (countriesFitler.ids != null && countriesFitler.ids.Count > 0) _filter &= _builder.In(c => c.id, countriesFitler.ids);

            // Tìm kiếm theo name
            if (!string.IsNullOrEmpty(countriesFitler.name)) _filter &= _builder.Eq(c => c.name, countriesFitler.name);

            // Tìm kiếm theo iso2
            if (!string.IsNullOrEmpty(countriesFitler.iso2)) _filter &= _builder.Eq(c => c.iso2, countriesFitler.iso2);

            // Tìm kiếm theo iso3
            if (!string.IsNullOrEmpty(countriesFitler.iso3)) _filter &= _builder.Eq(c => c.iso3, countriesFitler.iso3);

            // Tìm kiếm theo numeric_code
            if (!string.IsNullOrEmpty(countriesFitler.numeric_code)) _filter &= _builder.Eq(c => c.numeric_code, countriesFitler.numeric_code);

            // Tìm kiếm theo sdt
            if (!string.IsNullOrEmpty(countriesFitler.phone_code)) _filter &= _builder.Eq(c => c.phone_code, countriesFitler.phone_code);


            return _filter;
        }

        /// <summary>
        /// Tạo mới 1 bản ghi
        /// </summary>

        public Task InsertOneAsync(Countries countries)
        {
           
                return _Collection.InsertOneAsync(countries);
           
        }


        /// <summary>
        /// Cập nhật 1 bản ghi
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="countries"></param>
        /// <returns></returns>
        public Task<UpdateResult> UpdateOneAsync(FilterDefinition<Countries> filter, Countries countries)
        {
            
                countries.id = null;

                var update = MongoHelper.ApplyMultiFields(Builders<Countries>.Update, countries);
                return _Collection.UpdateOneAsync(filter, update);
            
           
        }

        /// <summary>
        /// Xóa 1 bản ghi theo bộ lọc
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Task<DeleteResult> DeleteOneAsync(FilterDefinition<Countries> filter)
        {
           
                return _Collection.DeleteOneAsync(filter);
        }

        
    }





}



