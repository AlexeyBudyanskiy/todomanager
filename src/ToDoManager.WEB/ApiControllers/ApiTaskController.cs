using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ToDoManager.WEB.DataAccess.Interfaces;

namespace ToDoManager.WEB.ApiControllers
{
    [Route("api")]
    public class ApiTaskController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApiTaskController(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        [HttpGet("tasks")]
        public ActionResult Get()
        {
            if (CheckAvaliablity() == false)
            {
                return BadRequest();
            }

            var tasks = _unitOfWork.Assignments.GetAll().ToList();
            return Json(tasks);
        }

        [HttpGet("tasks/{id}")]
        public JsonResult Get(int id)
        {
            var task = _unitOfWork.Assignments.Get(id);
            return Json(task);
        }

        private bool CheckAvaliablity()
        {
            var rand = new Random();
            var number = rand.Next(1, 10);
            var result = number%2 == 0;

            return result;
        }
    }
}
