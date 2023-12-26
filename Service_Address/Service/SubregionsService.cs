using Helpers;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Service_Address.Models;

namespace Service_Address.Service
{
    public class SubregionsService
    {
        private readonly string CollectionName = "Subregions";

        //--Thực thể sẽ làm việc
        private readonly IMongoCollection<Subregions> _Collection;
        //--Dữ liệu hiển thị mặc định
        private ProjectionDefinition<Subregions> _FieldsDefault;

        //--Khởi tạo kết nối đễn colection (table) Product
        public SubregionsService(IOptions<List<Models.ConnectionInfo>> Connections)
        {
            //-- Cấu hình kết nối đến mongoDB
            var conn = Connections.Value?.Find(x => x.DatabaseName == "Address")
            
            ??throw new Exception("Connection DB Test Failed");

            // Tạo kết nối tới DB student
            MongoClient client = new(conn.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(conn.DatabaseName);
            _Collection = database.GetCollection<Subregions>(CollectionName);
        }

        /// <summary>
        /// Kiểm tra dữ liệu tồn tại hay không
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Task<bool> AnyAsync(FilterDefinition<Subregions> filter)
        {
            return _Collection.Find(filter).AnyAsync();
        }


        ///<summary>
        ///Tìm kiếm theo bộ lọc
        /// </summary>
        public FilterDefinition<Subregions> BuilderFilter(SubregionsFitler subregionsFitler )
        {
            var _builder = Builders<Subregions>.Filter;
            var _filter = _builder.Ne(c => c.id, null);
            // Tìm kiếm theo id
            if (!string.IsNullOrEmpty(subregionsFitler.id)) _filter &= _builder.Eq(c => c.id, subregionsFitler.id);
            // Tìm kiếm theo name
            if (!string.IsNullOrEmpty(subregionsFitler.name)) _filter &= _builder.Eq(c => c.name, subregionsFitler.name);

            return _filter;
        }



        /// Lấy danh sách student
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="fields"></param>
        /// <param name="sort_by"></param>
        public Task<IAsyncCursor<Subregions>> FindAsync(FilterDefinition<Subregions> filter, int? page = 1, int? limit = 250, List<string>? fields = null, string? sort_by = "id_desc")
        {

            // Lấy trường nào, kiểm tra các trường có bị null không và điều kiện các trường lớn hơn 0
            if (fields != null && fields.Count > 0)
            {
                _FieldsDefault = Builders<Subregions>.Projection.Include(fields.First());
                foreach (var field in fields.Skip(1)) _FieldsDefault = _FieldsDefault.Include(field);
            }

            // sắp xếp các trường theo kiểu tăng dần giảm dần theo yêu cầu cần sắp xếp
            var _sort_builder = Builders<Subregions>.Sort;
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
            return _Collection.FindAsync(filter, new FindOptions<Subregions, Subregions>
            {
                AllowDiskUse = true,
                Limit = limit,
                Skip = (page - 1) * limit,
                Projection = _FieldsDefault,
                Sort = _sort,
            });

        }


        ///<summary>
        /// Tạo mới Subregions
        /// </summary>
        public Task InsertOneAsync(Subregions subregions )
        {
            return _Collection.InsertOneAsync(subregions);
        }


        ///<summary>
        /// Cập nhật subregions
        /// </summary>
        public Task<UpdateResult> UpdateOneAsync(FilterDefinition<Subregions> filter, Subregions subregions)
        {
            var update = MongoHelper.ApplyMultiFields(Builders<Subregions>.Update, subregions);
            return _Collection.UpdateOneAsync(filter, update);
        }


        ///<summary>
        /// Xóa 1 bản ghi Subregions 
        /// </summary>
        public Task<DeleteResult> DeleteOneAsync(FilterDefinition<Subregions> filter)
        {
            return _Collection.DeleteOneAsync(filter);
        }

    }
}
