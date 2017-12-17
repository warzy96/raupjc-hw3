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
            CombinedModel combined = new CombinedModel
            {
                IndexViewModel = new IndexViewModel(await _repository.GetActive(new Guid(currentUser.Id)))
            };
            return View(combined);
        }

        public async Task<IActionResult> Completed()
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            CombinedModel combined = new CombinedModel
            {
                CompletedViewModel = new CompletedViewModel(await _repository.GetCompleted(new Guid(currentUser.Id)))
            };
            return View(combined);
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

        public async Task<IActionResult> Add()
        {
            var model = new AddTodoViewModel(await _repository.GetLabels());
            return View(model);
        }

        
        [HttpPost]
        public async Task<IActionResult> Add(AddTodoViewModel item)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);
                var storedLabels = await _repository.GetLabels();
                var todoItem = new TodoItem(item.Text, new Guid(currentUser.Id))
                {
                    DateDue = item.DateDue
                };
                foreach (var asociatedLabel in  item.SelectedLabels)
                {
                    var label = storedLabels.Find(t => t.Value.Equals(asociatedLabel));
                    if (label == null) continue;
                    todoItem.Labels.Add(label);
                }
                _repository.Add(todoItem);
                return RedirectToAction("Index");
            }
            return View(item);
        }

        public IActionResult Labels()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Labels(LabelsViewModel label)
        {
            if (ModelState.IsValid)
            {
                var labels = await _repository.GetLabels();
                var addLabel = new TodoItemLabel(label.Value)
                {
                    Id = Guid.NewGuid(),
                    LabelTodoItems = label.LabelTodoItems
                };
                if (labels.Any(dbLabels => dbLabels.Value.Equals(addLabel.Value)))
                {
                    ModelState.AddModelError("ErrorMessage", "Label with the same name already exists!");
                    return View(label);
                }

                _repository.AddLabel(addLabel);
                return RedirectToAction("Add");
            }
            return View(label);
        }
    }
}
