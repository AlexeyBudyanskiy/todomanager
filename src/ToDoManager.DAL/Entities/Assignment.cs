
using ToDoManager.DAL.Enums;

namespace ToDoManager.DAL.Entities
{
    public class Assignment : BaseType
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public TaskStatus Status { get; set; }
    }
}
