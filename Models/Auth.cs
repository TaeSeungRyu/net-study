using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MemberApi.Models
{
    [BsonIgnoreExtraElements]
    public class Auth
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }

        [BsonElement("code")]
        public string code { get; set; } = "";

        [BsonElement("name")]
        public string? name { get; set; }

        [BsonElement("desc")]
        public string? desc { get; set; }

        [BsonElement("createDate")]
        public DateTime? createDate { get; set; }

        [BsonElement("createdAt")]
        public DateTime? createdAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime? updatedAt { get; set; }
    }
}