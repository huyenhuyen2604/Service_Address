using Helpers;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Service_Address.Models;

namespace Service_Address.Service
{
    public class RegionsServiec
    {
        private readonly string CollectionName = "Regions";

        //--Thực thể sẽ làm việc
        private readonly IMongoCollection<Regions> _Collection;
        //--Dữ liệu hiển thị mặc định
        private ProjectionDefinition<Regions> _FieldsDefault;

        //--Khởi tạo kết nối đễn colection (table) Product
        public RegionsServiec(IOptions<List<Models.ConnectionInfo>> Connections)
        {
            //-- Cấu hình kết nối đến mongoDB
            var conn = Connections.Value?.Find(x => x.DatabaseName == "Address")
                ?? throw new Exception("Connection DB Test Failed");

            // Tạo kết nối tới DB student
            MongoClient client = new(conn.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(conn.DatabaseName);
            _Collection = database.GetCollection<Regions>(CollectionName);
        }

        /// <summary>
        /// Kiểm tra dữ liệu tồn tại hay không
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Task<bool> AnyAsync(FilterDefinition<Regions> filter)
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
        public Task<IAsyncCursor<Regions>> FindAsync(FilterDefinition<Regions> filter, int? page = 1, int? limit = 250, List<string>? fields = null, string? sort_by = "id_desc")
        {
            
               

                // Lấy trường nào
                if (fields != null && fields.Count > 0)
                {
                    _FieldsDefault = Builders<Regions>.Projection.Include(fields.First());
                    foreach (var field in fields.Skip(1)) _FieldsDefault = _FieldsDefault.Include(field);
                }

                // Sắp xếp kiểu gì
                var _sort_builder = Builders<Regions>.Sort;
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
                return _Collection.FindAsync(filter, new FindOptions<Regions, Regions>
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

        public FilterDefinition<Regions> BuilderFilter(RegionsFitler regionsFitler )
        {
            var _builder = Builders<Regions>.Filter;
            var _filter = _builder.Ne(c => c.id, null);
            // Tìm kiếm theo id
            if(!string.IsNullOrEmpty(regionsFitler.id)) _filter = _builder.Ne(c => c.id, regionsFitler.id);
            ///Tìm kiếm theo tên
            if (!string.IsNullOrEmpty(regionsFitler.name)) _filter &= _builder.Eq(c => c.name, regionsFitler.name);
            return Convert.ToString(_filter);
        }
        /// <summary>
        /// Tạo mới 1 bản ghi
        /// </summary>

        public  Task InsertOneAsync(Regions regions )
        {
            
                return _Collection.InsertOneAsync(regions);
            
        }

        ///<summary>
        ///Cập nhật region
        ///</summary>
        public Task<UpdateResult> UpdateOneAsync(FilterDefinition<Regions> filter, Regions regions )
        {
           
                regions.id = null;
                var update = MongoHelper.ApplyMultiFields(Builders<Regions>.Update, regions);
                return _Collection.UpdateOneAsync(filter, update);
        }

        ///<summary>
        ///Xóa 1 bản ghi theo bộ lọc
        ///</summary>
        public Task<DeleteResult> DeleteOneAsync(FilterDefinition<Regions> filter)
        {
         
                return _Collection.DeleteOneAsync(filter);
        }

    }
}
