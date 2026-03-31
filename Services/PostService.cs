using Npgsql;
using MemberApi.Models;
using MemberApi.Config;
using Microsoft.Extensions.Options;

namespace MemberApi.Services
{
    public class PostService    
    {

        private readonly string _connectionString;

        // 생성자에서 IOptions를 통해 설정을 주입받습니다.
        public PostService(IOptions<PostgresSettings> postgresOptions)
        {
            var settings = postgresOptions.Value;
            // 연결 문자열을 한 번만 조립하여 보관합니다.
            _connectionString = $"Host={settings.Host};Port={settings.Port};Database={settings.Database};Username={settings.Username};Password={settings.Password}";
        }

        // 이제 도우미 메서드가 매우 간결해집니다.
        private NpgsqlConnection CreateConnection() 
        {
            return new NpgsqlConnection(_connectionString);
        }
        public async Task<List<Post>> GetAllAsync()
        {
            var result = new List<Post>();
            // using 문을 사용하여 예외가 발생해도 연결이 확실히 닫히도록 합니다.
            using var conn = CreateConnection();
            await conn.OpenAsync();

            var query = "SELECT id, title, content FROM posts";
            using var cmd = new NpgsqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.Add(new Post
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Content = reader.GetString(2)
                });
            }
            return result;
        }

        public async Task<Post?> GetByIdAsync(int id)
        {
            using var conn = CreateConnection();
            await conn.OpenAsync();

            var query = "SELECT id, title, content FROM posts WHERE id = @id";
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("id", id);

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Post
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Content = reader.GetString(2)
                };
            }
            return null;
        }

        public async Task<int> CreateAsync(Post post)
        {
            using var conn = CreateConnection();
            await conn.OpenAsync();

            // 단일 쿼리는 트랜잭션 없이 RETURNING id를 사용하는 것이 효율적입니다.
            var query = "INSERT INTO posts (title, content) VALUES (@title, @content) RETURNING id";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("title", post.Title);
            cmd.Parameters.AddWithValue("content", post.Content);

            var result = await cmd.ExecuteScalarAsync();
            return result != null ? (int)result : throw new Exception("Insert failed");
        }

        public async Task<bool> UpdateAsync(Post post)
        {
            using var conn = CreateConnection();
            await conn.OpenAsync();

            var query = "UPDATE posts SET title = @title, content = @content WHERE id = @id";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("id", post.Id);
            cmd.Parameters.AddWithValue("title", post.Title);
            cmd.Parameters.AddWithValue("content", post.Content);

            var affected = await cmd.ExecuteNonQueryAsync();
            return affected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var conn = CreateConnection();
            await conn.OpenAsync();

            var query = "DELETE FROM posts WHERE id = @id";
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("id", id);

            var affected = await cmd.ExecuteNonQueryAsync();
            return affected > 0;
        }
    }
}