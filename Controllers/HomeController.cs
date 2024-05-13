using MCSExam.DataSource;
using MCSExam.Models;
using MCSTask.Models;
using MCSTask.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;

namespace MCSTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _unitOfWork._repository.GetAll();
            if (!result.Status)
                return NotFound($"Something went wrong: {result.Message}");
            var emps = result.Data;
            return View(emps);
        }

        //Get XValue to filter out employees.
        [HttpGet]
        public IActionResult FilterOutEmployees()
        {
            var vm = new FilteringValueViewModel();
            return View(vm);
        }

        //View for filtered list of employees.
        [HttpPost]
        public async Task<IActionResult> Index(FilteringValueViewModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(FilterOutEmployees));
            var result = await _unitOfWork._repository.FilterOutEmployees(model.XValue);
            if (!result.Status)
                return NotFound(result.Message);
            return View(result.Data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
