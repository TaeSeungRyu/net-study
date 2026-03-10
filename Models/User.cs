using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MemberApi.Models
{
    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }

        [BsonElement("username")]
        public string username { get; set; } = "";

        [BsonElement("password")]
        public string password { get; set; } = "";

        [BsonElement("name")]
        public string? name { get; set; } = "";

        [BsonElement("accessDate")]
        public string? accessDate { get; set; } = "";

        [BsonElement("role")]
        public List<string>? role { get; set; }

        [BsonElement("email")]
        public string? email { get; set; }

        [BsonElement("phone")]
        public string? phone { get; set; }

        [BsonElement("profileImage")]
        public string? profileImage { get; set; }

        [BsonElement("createdAt")]
        public DateTime? createdAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime? updatedAt { get; set; }
    }
}