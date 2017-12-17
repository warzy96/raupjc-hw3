using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Zadatak_1;
using Zadatak_1.Interfaces;
using Zadatak_2.Data;
using Zadatak_2.Models;
using Zadatak_2.Pages;

namespace Zadatak_2.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ITodoRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public TodoController(ITodoRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            IndexViewModel model = new IndexViewModel(await _repository.GetActive(new Guid(currentUser.Id)));
            return View(model);
        }

        public async Task<IActionResult> Completed()
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            CompletedViewModel model = new CompletedViewModel(await _repository.GetCompleted(new Guid(currentUser.Id)));
            return View(model);
        }

        public async Task<IActionResult> RemoveFromCompleted(TodoViewModel item)
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            TodoItem todoItem = new TodoItem(item.Text, new Guid(currentUser.Id))
            {
                Id = item.Id,
                DateDue = item.DateDue,
                DateCreated = item.DateCreated,
                DateCompleted = null
            };
            _repository.Update(todoItem, new Guid(currentUser.Id));
            return RedirectToAction("Completed");
        }

        public async Task<IActionResult> MarkAsCompleted(Guid id)
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            _repository.MarkAsCompleted(id, new Guid(currentUser.Id));
            return RedirectToAction("Index");
        }

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult Labels()
        {
            return View();
        }


        //TODO: Check if label is in database, use apropriate actions, add a drop down view in addView for user to chose created labels
        [HttpPost]
        public async Task<IActionResult> Labels(LabelsViewModel label)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);
                var labels = await  _repository.GetLabels(new Guid(currentUser.Id));
                TodoItemLabel addLabel = new TodoItemLabel(label.Value);
                if(labels.Contains(addLabel))
                return RedirectToAction("Add");
            }
            return View(label);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddTodoViewModel item)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);
                var todoItem = new TodoItem(item.Text, new Guid(currentUser.Id))
                                   { DateDue = item.DateDue};
                _repository.Add(todoItem);
                return RedirectToAction("Index");
            }
            return View(item);
        }
    }
}
