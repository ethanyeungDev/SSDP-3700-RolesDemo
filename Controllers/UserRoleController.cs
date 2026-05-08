using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RolesDemo.Repositories;
using RolesDemo.ViewModels;

namespace RolesDemo.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserRoleController : Controller
    {
        private readonly UserRepository _userRepo;
        private readonly UserRoleRepository _userRoleRepo;
        private readonly MyRegisteredUserRepository _myUserRepo;
        private readonly RoleRepository _roleRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRoleController"/> class.
        /// </summary>
        /// <param name="userRepo">The user repository.</param>
        /// <param name="userRoleRepo">The user-role repository.</param>
        /// <param name="myUserRepo">The registered user repository.</param>
        public UserRoleController(
            UserRepository userRepo,
            UserRoleRepository userRoleRepo,
            MyRegisteredUserRepository myUserRepo,
            RoleRepository roleRepo)
        {
            _userRepo = userRepo;
            _userRoleRepo = userRoleRepo;
            _myUserRepo = myUserRepo;
            _roleRepo = roleRepo;
        }

        /// <summary>
        /// Displays the list of all registered users.
        /// </summary>
        /// <returns>The Index view with a list of <see cref="UserVM"/>.</returns>
        public IActionResult Index()
        {
            List<UserVM> users = _userRepo.GetAllUsers();
            return View(users);
        }

        /// <summary>
        /// Displays the role assignment form.
        /// </summary>
        /// <param name="email">Optional preselected user email.</param>
        /// <returns>The Create view.</returns>
        [HttpGet]
        public IActionResult Create(string? email = null)
        {
            PopulateDropdowns(email, null);
            return View(new UserRoleVM
            {
                Email = email ?? string.Empty
            });
        }

        /// <summary>
        /// Handles role assignment submission.
        /// </summary>
        /// <param name="userRoleVM">The role assignment model.</param>
        /// <returns>Detail view on success; Create view on failure.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserRoleVM userRoleVM)
        {
            if (!ModelState.IsValid)
            {
                PopulateDropdowns(userRoleVM.Email, userRoleVM.Role);
                return View(userRoleVM);
            }

            bool success = await _userRoleRepo.AddUserRoleAsync(userRoleVM.Email, userRoleVM.Role);
            if (!success)
            {
                ModelState.AddModelError("", "Unable to assign role to user.");
                PopulateDropdowns(userRoleVM.Email, userRoleVM.Role);
                return View(userRoleVM);
            }

            return await RenderDetailViewAsync(userRoleVM.Email, "Role assigned to user.");
        }

        /// <summary>
        /// Displays all roles assigned to the specified user.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <returns>The Detail view with the user's roles.</returns>
        public async Task<IActionResult> Detail(string email)
        {
            return await RenderDetailViewAsync(email);
        }

        /// <summary>
        /// Removes a role assignment from the specified user and reloads the detail page.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <param name="roleName">The role to remove.</param>
        /// <returns>The Detail view with status message.</returns>
        [HttpGet]
        public async Task<IActionResult> DeleteUserRole(string email, string roleName)
        {
            bool success = await _userRoleRepo.RemoveUserRoleAsync(email, roleName);
            return success
                ? await RenderDetailViewAsync(email, "Role removed from user.")
                : await RenderDetailViewAsync(email, null, "Failed to remove role from user.");
        }

        private void PopulateDropdowns(string? selectedEmail, string? selectedRole)
        {
            var users = _userRepo.GetAllUsers()
                .Select(u => u.Email)
                .Where(e => !string.IsNullOrWhiteSpace(e))
                .ToList();

            var roles = _roleRepo.GetAllRoles()
                .Select(r => r.RoleName)
                .Where(r => !string.IsNullOrWhiteSpace(r))
                .ToList();

            ViewBag.Users = new SelectList(users, selectedEmail);
            ViewBag.Roles = new SelectList(roles, selectedRole);
        }

        private async Task<IActionResult> RenderDetailViewAsync(
            string email,
            string? confirmationMessage = null,
            string? errorMessage = null)
        {
            var (firstName, lastName) = _myUserRepo.GetNameByEmail(email);
            string fullName = $"{firstName} {lastName}".Trim();
            ViewBag.UserName = string.IsNullOrWhiteSpace(fullName) ? email : fullName;
            ViewBag.Email = email;
            ViewBag.ConfirmationMessage = confirmationMessage;
            ViewBag.ErrorMessage = errorMessage;

            IList<string> roles = await _userRoleRepo.GetUserRolesAsync(email);
            var roleViewModels = roles.Select(role => new RoleVM
            {
                RoleName = role
            }).ToList();

            return View("Detail", roleViewModels);
        }
    }
}
