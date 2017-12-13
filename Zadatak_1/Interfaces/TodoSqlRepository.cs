using System;
using System.Collections.Generic;
using System.Linq;
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
        }

        public bool Remove(Guid todoId, Guid userId)
        {
            var item = _context.TodoItems.Find(todoId);

            if (item == null) return false;

            if (!item.UserId.Equals(userId))
                throw new TodoAccessDeniedException
                    ("You do not have permission to remove this item!");
            
            _context.TodoItems.Remove(item);
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

            if (item.UserId.Equals(userId))
                return item.MarkAsCompleted();

            throw new TodoAccessDeniedException
                ("You do not have permission to make changes to this item");
        }

        public List<TodoItem> GetAll(Guid userId)
        {
            return _context.TodoItems.Where(t => t.UserId.Equals(userId))
                .OrderByDescending(t => t.DateCreated).ToList();
        }

        public List<TodoItem> GetActive(Guid userId)
        {
            return _context.TodoItems.Where(t => !t.IsCompleted 
                                            && t.UserId.Equals(userId))
                                            .ToList();
        }

        public List<TodoItem> GetCompleted(Guid userId)
        {
            return _context.TodoItems.Where(t => t.IsCompleted 
                                            && t.UserId.Equals(userId))
                                            .ToList();
        }

        public List<TodoItem> GetFiltered(Func<TodoItem, bool> filterFunction, Guid userId)
        {
            return _context.TodoItems.Where(filterFunction)
                .Where(t => t.UserId.Equals(userId)).ToList();
        }
    }
}
