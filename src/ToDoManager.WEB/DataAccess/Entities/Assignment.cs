using System.ComponentModel.DataAnnotations;
using ToDoManager.WEB.DataAccess.Enums;

namespace ToDoManager.WEB.DataAccess.Entities
{
    public class Assignment : BaseType
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Text { get; set; }

        public AssignmentStatus Status { get; set; }
    }
}
