using Helpers;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Service_Address.Models;

namespace Service_Address.Service
{
    public class StatesService
    {
        private readonly string CollectionName = "States";

        //--Thực thể sẽ làm việc
        private readonly IMongoCollection<States> _Collection;
        //--Dữ liệu hiển thị mặc định
        private ProjectionDefinition<States> _FieldsDefault;

        //--Khởi tạo kết nối đễn colection (table) Product

        public StatesService(IOptions<List<Models.ConnectionInfo>> Connections)
        {
            //-- Cấu hình kết nối đến mongoDB
            var conn = Connections.Value?.Find(x => x.DatabaseName == "Address")
            ?? throw new Exception("Connection DB Test Failed");

            // Tạo kết nối tới DB student
            MongoClient client = new(conn.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(conn.DatabaseName);
            _Collection = database.GetCollection<States>(CollectionName);


        }

        ///<summary>
        ///Tìm kiếm theo bộ lọc
        /// </summary>
        public FilterDefinition<States> BuilderFilter(StatesFilter statesFilter )
        {
            var _builder = Builders<States>.Filter;
            var _filter = _builder.Ne(c => c.id, null);
            // Tìm kiếm theo id
            if (!string.IsNullOrEmpty(statesFilter.id)) _filter &= _builder.Eq(c => c.id, statesFilter.id);
            // Tìm kiếm theo name
            if (!string.IsNullOrEmpty(statesFilter.name)) _filter &= _builder.Eq(c => c.name, statesFilter.name);
            // Tìm kiếm theo country_code
            if(!string.IsNullOrEmpty(statesFilter.country_code)) _filter &= _builder.Eq(c=>c.country_code, statesFilter.country_code);
            return _filter;
        }


        ///<summary>
        /// Tạo mới Subregions
        /// </summary>
        public Task InsertOneAsync(States states)
        {
         
                return _Collection.InsertOneAsync(states);
          
        }

        ///<summary>
        /// Cập nhật subregions
        /// </summary>
        public Task<UpdateResult> UpdateOneAsync(FilterDefinition<States> filter, States states )
        {
           
                var update = MongoHelper.ApplyMultiFields(Builders<States>.Update, states);
                return _Collection.UpdateOneAsync(filter, update);
           
        }


        ///<summary>
        /// Xóa 1 bản ghi Subregions 
        /// </summary>
        public Task<DeleteResult> DeleteOneAsync(FilterDefinition<States> filter)
        {
          
                return _Collection.DeleteOneAsync(filter);
        }
    }
}
