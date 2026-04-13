using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MemberApi.Models.Entities
{
    [BsonIgnoreExtraElements]
    public class AuthCode
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("code")]
        public string Code { get; set; } = string.Empty;

        [BsonElement("name")]
        public string? Name { get; set; }

        [BsonElement("desc")]
        public string? Desc { get; set; }

        [BsonElement("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
    }
}
