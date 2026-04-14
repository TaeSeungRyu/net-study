using System.ComponentModel.DataAnnotations;

namespace MemberApi.Config
{
    public class MongoDbSettings
    {
        [Required]
        public string ConnectionString { get; set; } = "";

        [Required]
        public string DatabaseName { get; set; } = "";
    }
}
