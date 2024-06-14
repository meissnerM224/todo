using System.Collections.Generic;

namespace Todo.Models.ViewModels
{
    public class TodoViewModel
    {
        public required List<TodoItem> TodoList { get; set; }
        public TodoItem? Todo { get; set; }
    }
}