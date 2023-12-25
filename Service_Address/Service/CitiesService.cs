using Helpers;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Service_Address.Models;

namespace Service_Address.Service
{
    public class CitiesService
    {
        private readonly string CollectionName = "Cities";

        //--Thực thể sẽ làm việc
        private readonly IMongoCollection<Cities> _Collection;
        //--Dữ liệu hiển thị mặc định
        private ProjectionDefinition<Cities> _FieldsDefault;

        //--Khởi tạo kết nối đễn colection (table) Product

        public CitiesService(IOptions<List<Models.ConnectionInfo>> Connections)
        {
            //-- Cấu hình kết nối đến mongoDB
            var conn = Connections.Value?.Find(x => x.DatabaseName == "Address")
            ?? throw new Exception("Connection DB Test Failed");

            // Tạo kết nối tới DB student
            MongoClient client = new(conn.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(conn.DatabaseName);
            _Collection = database.GetCollection<Cities>(CollectionName);

        
        }


        /// <summary>
        /// Kiểm tra dữ liệu tồn tại hay không
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Task<bool> AnyAsync(FilterDefinition<Cities> filter)
        {
            return _Collection.Find(filter).AnyAsync();
        }


        /// Lấy danh sách student
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="fields"></param>
        /// <param name="sort_by"></param>
        public Task<IAsyncCursor<Cities>> FindAsync(FilterDefinition<Cities> filter, int? page = 1, int? limit = 250, List<string>? fields = null, string? sort_by = "id_desc")
        {



            // Lấy trường nào
            if (fields != null && fields.Count > 0)
            {
                _FieldsDefault = Builders<Cities>.Projection.Include(fields.First());
                foreach (var field in fields.Skip(1)) _FieldsDefault = _FieldsDefault.Include(field);
            }

            // Sắp xếp kiểu gì
            var _sort_builder = Builders<Cities>.Sort;
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
            return _Collection.FindAsync(filter, new FindOptions<Cities, Cities>
            {
                AllowDiskUse = true,
                Limit = limit,
                Skip = (page - 1) * limit,
                Projection = _FieldsDefault,
                Sort = _sort,
            });

        }




        ///<summary>
        ///Tìm kiếm theo bộ lọc
        /// </summary>
        public FilterDefinition<Cities> BuilderFilter(CitiesFilter citiesFilter)
        {
            var _builder = Builders<Cities>.Filter;
            var _filter = _builder.Ne(c => c.id, null);
            // Tìm kiếm theo id
            if (!string.IsNullOrEmpty(citiesFilter.id)) _filter &= _builder.Eq(c => c.id, citiesFilter.id);
            // Tìm kiếm theo name
            if (!string.IsNullOrEmpty(citiesFilter.name)) _filter &= _builder.Eq(c => c.name, citiesFilter.name);

            return _filter;
        }

        /// <summary>
        /// Tạo mới 1 bản ghi
        /// </summary>

        public Task InsertOneAsync(Cities cities)
        {
                return _Collection.InsertOneAsync(cities);
        }

        /// <summary>
        /// Cập nhật City
        /// <param name="filter"></param>
        /// <param name="cities"></param>
        /// </summary>
        public Task<UpdateResult> UpdateOneAsync(FilterDefinition<Cities> filter, Cities cities )
        {
                var update = MongoHelper.ApplyMultiFields(Builders<Cities>.Update, cities);
                return _Collection.UpdateOneAsync(filter, update);
        }

        /// <summary>
        /// Xóa 1 bản ghi theo bộ lọc
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Task<DeleteResult> DeleteOneAsync(FilterDefinition<Cities> filter)
        {
                return _Collection.DeleteOneAsync(filter);
        }
    }
}
