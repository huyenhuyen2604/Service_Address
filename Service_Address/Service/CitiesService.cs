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
            try
            {
                if (_Collection == null) throw new Exception("Collection null");
                return _Collection.InsertOneAsync(cities);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Cập nhật City
        /// <param name="filter"></param>
        /// <param name="cities"></param>
        /// </summary>
        public Task<UpdateResult> UpdateOneAsync(FilterDefinition<Cities> filter, Cities cities )
        {
            try
            {
                if (_Collection == null) throw new Exception("Collection null");
                cities.id = null;

                var update = MongoHelper.ApplyMultiFields(Builders<Cities>.Update, cities);
                return _Collection.UpdateOneAsync(filter, update);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Xóa 1 bản ghi theo bộ lọc
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public Task<DeleteResult> DeleteOneAsync(FilterDefinition<Cities> filter)
        {
            try
            {
                if (_Collection == null) throw new Exception("Collection null");
                return _Collection.DeleteOneAsync(filter);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
