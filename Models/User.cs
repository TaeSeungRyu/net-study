using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MemberApi.Models
{
    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("username")]
        public string Username { get; set; } = "";

        [BsonElement("password")]
        public string Password { get; set; } = "";

        [BsonElement("name")]
        public string? Name { get; set; } = "";

        [BsonElement("accessDate")]
        public string? AccessDate { get; set; } = "";

        [BsonElement("role")]
        public List<object>? Role { get; set; }

        [BsonElement("email")]
        public string? Email { get; set; }

        [BsonElement("phone")]
        public string? Phone { get; set; }

        [BsonElement("profileImage")]
        public string? ProfileImage { get; set; }

        [BsonElement("authList")]
        public List<string>? AuthList { get; set; }

        [BsonElement("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
    }
}