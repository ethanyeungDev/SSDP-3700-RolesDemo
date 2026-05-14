using Microsoft.AspNetCore.Mvc;
using RolesDemo.Repositories;
using RolesDemo.ViewModels;

namespace RolesDemo.Controllers
{
    public class RoleController : Controller
    {
        private readonly ILogger<RoleController> _logger;
        private readonly RoleRepository _roleRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="roleRepo">The role repository.</param>
        public RoleController(ILogger<RoleController> logger,
                              RoleRepository roleRepo)
        {
            _logger = logger;
            _roleRepo = roleRepo;
        }

        /// <summary>
        /// Displays the list of all roles.
        /// </summary>
        /// <returns>The Index view with a list of <see cref="RoleVM"/>.</returns>
        public IActionResult Index()
        {
            List<RoleVM> roleVM = _roleRepo.GetAllRoles();
            return View(roleVM);
        }

        /// <summary>
        /// Displays the role creation form.
        /// </summary>
        /// <returns>The Create view.</returns>
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Handles the role creation form submission.
        /// </summary>
        /// <param name="roleVM">The role view model containing the role name.</param>
        /// <returns>Redirects to Index on success; otherwise redisplays the Create view with an error.</returns>
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

        /// <summary>
        /// Displays the role deletion confirmation page.
        /// </summary>
        /// <param name="id">The ID of the role to delete.</param>
        /// <returns>The Delete confirmation view, or NotFound if the role does not exist.</returns>
        [HttpGet]
        public ActionResult Delete(string id)
        {
            var role = _roleRepo.GetRoleById(id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        /// <summary>
        /// Handles the role deletion form submission.
        /// </summary>
        /// <param name="id">The ID of the role to delete.</param>
        /// <returns>Redirects to Index on success; redisplays the Delete view with an error if the role is assigned to users.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (_roleRepo.IsRoleAssigned(id))
            {
                var role = _roleRepo.GetRoleById(id);
                ModelState.AddModelError("", "This role is currently assigned to users and cannot be deleted.");
                return View(role);
            }

            bool deleted = _roleRepo.DeleteRole(id);
            if (!deleted)
            {
                var role = _roleRepo.GetRoleById(id);
                ModelState.AddModelError("", "Role deletion failed. Please try again.");
                return View(role);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
