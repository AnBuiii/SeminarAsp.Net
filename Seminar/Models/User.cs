using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Seminar.Models
{
    public class User
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }  
        public string username { get; set; }  

        public string password { get; set; }
    }
}
