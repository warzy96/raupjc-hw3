using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Zadatak_1.Exceptions;

namespace Zadatak_1.Interfaces
{
    public class TodoSqlRepository : ITodoRepository
    {
        private readonly TodoDbContext _context;
        public TodoSqlRepository(TodoDbContext context)
        {
            _context = context;
        }


        public Task<List<TodoItemLabel>> GetLabels()
        {
            return _context.TodoItemLabels.ToListAsync();
        }
        public TodoItem Get(Guid todoId, Guid userId)
        {
            var item = _context.TodoItems.Find(todoId);

            if (item == null) return null;

            if(!item.UserId.Equals(userId))
                throw new TodoAccessDeniedException
                    ("You do not have permission to access this item!");

            return item;
        }

        public void Add(TodoItem todoItem)
        {
            if(_context.TodoItems.Find(todoItem.Id) != null) 
                throw new DuplicateTodoItemException
                    ($"Duplicate id: {todoItem.Id}");
            _context.TodoItems.Add(todoItem);
            _context.SaveChanges();
        }

        public bool Remove(Guid todoId, Guid userId)
        {
            var item = _context.TodoItems.Find(todoId);

            if (item == null) return false;

            if (!item.UserId.Equals(userId))
                throw new TodoAccessDeniedException
                    ("You do not have permission to remove this item!");
            
            _context.TodoItems.Remove(item);
            _context.SaveChanges();
            return true;
        }

        public void Update(TodoItem todoItem, Guid userId)
        {
            var item = _context.TodoItems.Find(todoItem.Id);

            if (item == null) return;

            if (item.UserId.Equals(userId))
                Remove(todoItem.Id, userId);
            else throw new TodoAccessDeniedException
                ("You do not have permission to update this item");
            Add(todoItem);
        }

        public bool MarkAsCompleted(Guid todoId, Guid userId)
        {
            var item = _context.TodoItems.Find(todoId);

            if (item == null) return false;

            if (!item.UserId.Equals(userId))
                throw new TodoAccessDeniedException
                    ("You do not have permission to make changes to this item");
            var success = item.MarkAsCompleted();
            _context.SaveChanges();
            return success;
        }

        public async Task<List<TodoItem>> GetAll(Guid userId)
        {
            return await _context.TodoItems.Where(t => t.UserId.Equals(userId))
                .OrderByDescending(t => t.DateCreated).ToListAsync();
        }

        public async Task<List<TodoItem>> GetActive(Guid userId)
        {
            return await _context.TodoItems.Where(t => !t.DateCompleted.HasValue 
                                            && t.UserId == userId)
                                            .ToListAsync();
        }

        public async Task<List<TodoItem>> GetCompleted(Guid userId)
        {
            return await _context.TodoItems.Where(t => t.IsCompleted 
                                            && t.UserId.Equals(userId))
                                            .ToListAsync();
        }

        public List<TodoItem> GetFiltered(Func<TodoItem, bool> filterFunction, Guid userId)
        {
            return _context.TodoItems.Where(filterFunction)
                .Where(t => t.UserId.Equals(userId)).ToList();
        }
    }
}
