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
