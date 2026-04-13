using MongoDB.Bson.Serialization.Attributes;

namespace MemberApi.Models.Entities
{
    /// <summary>
    /// MongoDB $lookup 결과 매핑용. User + 권한(AuthCode) 리스트.
    /// </summary>
    [BsonIgnoreExtraElements]
    public class UserWithRoles : User
    {
        [BsonElement("roles")]
        public List<AuthCode>? Roles { get; set; }
    }
}
