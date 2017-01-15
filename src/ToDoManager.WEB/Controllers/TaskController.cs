using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ToDoManager.WEB.DataAccess.Entities;
using ToDoManager.WEB.DataAccess.Enums;
using ToDoManager.WEB.DataAccess.Interfaces;
using ToDoManager.WEB.Infrastructure;

namespace ToDoManager.WEB.Controllers
{
    public class TaskController : Controller
    {
        private const string AssignmentsUrl = "http://managerreturner.azurewebsites.net/api/values";

        private readonly IUnitOfWork _unitOfWork;
        

        //public TaskController(IUnitOfWork uow)
        //{
        //    _unitOfWork = uow;
        //}

        public TaskController()
        {
            
        }

        //[HttpGet]
        //public IActionResult Index()
        //{
        //    //var tasks = _unitOfWork.Assignments.GetAll().ToList();
        //    var tasks = GetTasks();
        //    tasks.AddRange(MakeRequest());
            
        //    return View(tasks);
        //}

        [HttpGet]
        public IActionResult Index()
        {
            var tasks = RequestBuilder.GetAssignmentsPolly(AssignmentsUrl);
            tasks.AddRange(GetTasks());

            return View(tasks);
        }

        private List<Assignment> MakeRequest()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var responseMsg = client.GetAsync("http://managerreturner.azurewebsites.net/api/values").Result;
            if (responseMsg.IsSuccessStatusCode)
            {
                var res = JsonConvert.DeserializeObject<List<Assignment>>(responseMsg.Content.ReadAsStringAsync().Result);
                return res;
            }
            else
            {
                return MakeRequest();
            }
        }

        private List<Assignment> GetTasks()
        {
            var assignments = new List<Assignment>
            {
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
            };
            return assignments;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                assignment.Status = AssignmentStatus.Planned;
                _unitOfWork.Assignments.Create(assignment);
                _unitOfWork.Save();

                return RedirectToAction("Index");
            }

            return View(assignment);
        }

        [HttpGet]
        public IActionResult Edit(int taskId)
        {
            var task = _unitOfWork.Assignments.Get(taskId);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        [HttpPost]public IActionResult Edit(Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                var updatingAssignment = _unitOfWork.Assignments.Get(assignment.Id);
                MapAssignment(assignment, updatingAssignment);

                _unitOfWork.Assignments.Update(updatingAssignment);
                _unitOfWork.Save();

                return RedirectToAction("Index");
            }

            return View(assignment);
        }

        [HttpGet]
        public IActionResult ChangeStatus(int taskId, AssignmentStatus status)
        {
            var task = _unitOfWork.Assignments.Get(taskId);
            task.Status = status;
            _unitOfWork.Assignments.Update(task);
            _unitOfWork.Save();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Remove(int taskId)
        {
            _unitOfWork.Assignments.Delete(taskId);
            _unitOfWork.Save();

            return RedirectToAction("Index");
        }

        private static void MapAssignment(Assignment assignment, Assignment updatingAssignment)
        {
            updatingAssignment.Status = assignment.Status;
            updatingAssignment.Name = assignment.Name;
            updatingAssignment.Text = assignment.Text;
        }
    }

    
}
