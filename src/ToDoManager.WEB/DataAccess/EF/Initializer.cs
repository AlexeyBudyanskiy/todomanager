using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using ToDoManager.WEB.DataAccess.Entities;
using ToDoManager.WEB.DataAccess.Enums;

namespace ToDoManager.WEB.DataAccess.EF
{
    public static class Initializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<ManagerContext>();

            if (!context.Assignments.Any())
            {
                context.Assignments.AddRange(
                    new Assignment
                    {
                        Name = "Make Gamestore",
                        Text = "To make game store with the ability to buy games.",
                        Status = AssignmentStatus.Planned
                    },
                    new Assignment
                    {
                        Name = "English",
                        Text = "Go to the English",
                        Status = AssignmentStatus.Done
                    },
                    new Assignment
                    {
                        Name = "Drink Cofee",
                        Text = "To drink some cofee",
                        Status = AssignmentStatus.InProgress
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
