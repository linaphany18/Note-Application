using Dapper;
using NoteApi.Dto;
using System.Security.Claims;
using System.Text;

namespace NoteApi.Services
{
    public class NoteRepository
    {
        private readonly DbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NoteRepository(DbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }
        private int GetUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("UserId not found in token");

            return int.Parse(userIdClaim.Value);
        }
        public async Task<NoteListResponse<Note[]?>> GetNoteList(NoteQueryParam query)
        {
            var userId = GetUserId();

            var sql = new StringBuilder("SELECT * FROM dbo.Notes WHERE UserId = @UserId");
            var totalSql = new StringBuilder("SELECT COUNT(*) FROM dbo.Notes WHERE UserId = @UserId");

            var parameters = new DynamicParameters();
            parameters.Add("UserId", userId);

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                sql.Append(" AND (Title LIKE @Search OR Content LIKE @Search)");
                totalSql.Append(" AND (Title LIKE @Search OR Content LIKE @Search)");
                parameters.Add("Search", $"%{query.Search}%");
            }

            if (query.StartedAt.HasValue)
            {
                sql.Append(" AND CreatedAt >= @StartedAt");
                totalSql.Append(" AND CreatedAt >= @StartedAt");
                parameters.Add("StartedAt", query.StartedAt.Value);
            }

            if (query.EndedAt.HasValue)
            {
                sql.Append(" AND CreatedAt <= @EndedAt");
                totalSql.Append(" AND CreatedAt <= @EndedAt");
                parameters.Add("EndedAt", query.EndedAt.Value);
            }

            string orderBy = query.OrderBy ?? "CreatedAt";
            if (orderBy.StartsWith("-"))
                sql.Append($" ORDER BY {orderBy.Substring(1)} DESC");
            else
                sql.Append($" ORDER BY {orderBy} ASC");

            int offset = query.Offset ?? 0;
            int limit = query.Limit ?? 10;
            sql.Append(" OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY");
            parameters.Add("Offset", offset);
            parameters.Add("Limit", limit);

            using var connection = _dbContext.CreateConnection();

            var notes = await connection.QueryAsync<Note>(sql.ToString(), parameters);
            var totalCount = await connection.ExecuteScalarAsync<int>(totalSql.ToString(), parameters);

            return new NoteListResponse<Note[]?>
            {
                Data = notes.ToArray(),
                Total = totalCount,
                Limit = limit,
                Offset = offset
            };
        }

        public async Task<Note?> GetNoteById(int id)
        {
            var userId = GetUserId();

            var query = "SELECT * FROM dbo.Notes WHERE Id = @Id AND UserId = @UserId";
            using var connection = _dbContext.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Note>(query, new { Id = id, UserId = userId });
        }

        public async Task<Note?> CreateNote(Note note)
        {
            var userId = GetUserId();

            var query = @"
            INSERT INTO dbo.Notes (UserId, Title, Content, CreatedAt, UpdatedAt)
            VALUES (@UserId, @Title, @Content, @CreatedAt, @UpdatedAt);
            SELECT CAST(SCOPE_IDENTITY() AS int);";

            using var connection = _dbContext.CreateConnection();

            note.Id = await connection.QuerySingleAsync<int>(query, new
            {
                UserId = userId,
                Title = note.Title,
                Content = note.Content,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

            return note;
        }

        public async Task<Note?> UpdateNote(int id, Note note)
        {
            var userId = GetUserId();

            var query = @"
            UPDATE dbo.Notes
            SET Title = @Title, Content = @Content, UpdatedAt = @UpdatedAt
            WHERE Id = @Id AND UserId = @UserId";

            using var connection = _dbContext.CreateConnection();

            var res = await connection.ExecuteAsync(query, new
            {
                Title = note.Title,
                Content = note.Content,
                UpdatedAt = DateTime.UtcNow,
                Id = id,
                UserId = userId
            });

            if (res == 0)
                return null;

            note.Id = id;
            note.UserId = userId;

            return note;
        }

        public async Task<bool> DeleteNote(int id)
        {
            var userId = GetUserId();

            var query = "DELETE FROM dbo.Notes WHERE Id = @Id AND UserId = @UserId";
            using var connection = _dbContext.CreateConnection();
            var res = await connection.ExecuteAsync(query, new { Id = id, UserId = userId });
            return res > 0;
        }
    }
}