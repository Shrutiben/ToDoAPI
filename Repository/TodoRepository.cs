
using Dapper;
using TodoApi.Data;
using TodoApi.Data.TodoApi.Data;
using TodoApi.IRepository;
using TodoApi.Models;

namespace TodoApi.Repository
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoDbContext _dbContext;
        private readonly DapperContext _dapper;
        public TodoRepository(TodoDbContext dbContext, DapperContext dapper)
        {
            _dbContext = dbContext;
            _dapper = dapper;
        }

        public async Task<IEnumerable<Todo>> GetAllAsync()
        {
            var query = "SELECT * FROM Todos"; //linq
            using var conn =  _dapper.CreateConnection();
            return await conn.QueryAsync<Todo>(query);

        }
        public async Task<Todo?> GetByIdAsync(int id)
        {
            return await _dbContext.Todos.FindAsync(id);
        }

        public async Task<Todo> CreateAsync(Todo todo)
        {
            _dbContext.Todos.Add(todo);
            await _dbContext.SaveChangesAsync();
            return todo;
        }

        public async Task<bool> UpdateAsync(int id, Todo updated)
        {
            var query = "UPDATE Todos SET Title = @Title, IsCompleted = @IsCompleted WHERE Id = @Id";
            using var conn = _dapper.CreateConnection();
            var result = await conn.ExecuteAsync(query, new { updated.Title, updated.IsCompleted, Id = id });
            return result > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var todo = await _dbContext.Todos.FindAsync(id);
            if (todo == null) return false;
            _dbContext.Todos.Remove(todo);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
