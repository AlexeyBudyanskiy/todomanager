using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using ToDoManager.WEB.DataAccess.Entities;
using ToDoManager.WEB.DataAccess.Enums;
using ToDoManager.WEB.DataAccess.Interfaces;

namespace ToDoManager.WEB.Controllers
{
    public class TaskController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TaskController(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var tasks = _unitOfWork.Assignments.GetAll().ToList();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("10.23.22.103/todolist/api/cards");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // New code:
                var response = client.GetAsync("");
                if (response.IsSuccessStatusCode)
                {
                    var contentString = await response.Content.ReadAsStringAsync();
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Card>>(contentString);
                }
            }

            return View(tasks);
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
