using Microsoft.AspNetCore.Mvc;
using RolesDemo.Repositories;
using RolesDemo.ViewModels;

namespace RolesDemo.Controllers
{
    public class RoleController : Controller
    {
        private readonly ILogger<RoleController> _logger;
        private readonly RoleRepository _roleRepo;

        public RoleController(ILogger<RoleController> logger,
                              RoleRepository roleRepo)
        {
            _logger = logger;
            _roleRepo = roleRepo;
        }

        public IActionResult Index()
        {
            List<RoleVM> roleVM = _roleRepo.GetAllRoles();
            return View(roleVM);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(RoleVM roleVM)
        {
            if (ModelState.IsValid)
            {
                bool isSuccess = _roleRepo.CreateRole(roleVM.RoleName);

                if (isSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Role creation failed." +
                                             " The role may already exist.");
                }
            }
            return View(roleVM);
        }
    }
}
