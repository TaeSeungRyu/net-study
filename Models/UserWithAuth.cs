using MongoDB.Bson.Serialization.Attributes;

namespace MemberApi.Models;


[BsonIgnoreExtraElements]
public class UserWithAuth : User
{
    public List<Auth>? roles { get; set; }
}