using Npgsql;
using MemberApi.Models;

namespace MemberApi.Services
{
    public class PostService
    {
        private readonly NpgsqlConnection _connection;

        public PostService(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<List<Post>> GetAllAsync()
        {
            var result = new List<Post>();

            await _connection.OpenAsync();

            var query = "SELECT id, title, content FROM posts";

            using var cmd = new NpgsqlCommand(query, _connection);
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

            await _connection.CloseAsync();
            return result;
        }

        public async Task<Post?> GetByIdAsync(int id)
        {
            await _connection.OpenAsync();

            var query = "SELECT id, title, content FROM posts WHERE id = @id";

            using var cmd = new NpgsqlCommand(query, _connection);
            cmd.Parameters.AddWithValue("id", id);

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var post = new Post
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Content = reader.GetString(2)
                };

                await _connection.CloseAsync();
                return post;
            }

            await _connection.CloseAsync();
            return null;
        }

        public async Task<int> CreateAsync(Post post)
        {
            await _connection.OpenAsync();

            var query = @"
                INSERT INTO posts (title, content)
                VALUES (@title, @content)
                RETURNING id;
            ";

            using var cmd = new NpgsqlCommand(query, _connection);
            cmd.Parameters.AddWithValue("title", post.Title);
            cmd.Parameters.AddWithValue("content", post.Content);

            var result = await cmd.ExecuteScalarAsync();

            if (result == null)
            {
                throw new Exception("Insert failed, no ID returned.");
            }

            var id = (int)result;

            await _connection.CloseAsync();
            return id;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _connection.OpenAsync();

            var query = "DELETE FROM posts WHERE id = @id";

            using var cmd = new NpgsqlCommand(query, _connection);
            cmd.Parameters.AddWithValue("id", id);

            var affected = await cmd.ExecuteNonQueryAsync();

            await _connection.CloseAsync();
            return affected > 0;
        }
    }
}