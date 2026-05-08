using Microsoft.AspNetCore.Mvc;
using RolesDemo.Repositories;
using RolesDemo.ViewModels;

namespace RolesDemo.Controllers
{
    public class UserRoleController : Controller
    {
        private readonly UserRepository _userRepo;
        private readonly UserRoleRepository _userRoleRepo;
        private readonly MyRegisteredUserRepository _myUserRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRoleController"/> class.
        /// </summary>
        /// <param name="userRepo">The user repository.</param>
        /// <param name="userRoleRepo">The user-role repository.</param>
        /// <param name="myUserRepo">The registered user repository.</param>
        public UserRoleController(
            UserRepository userRepo,
            UserRoleRepository userRoleRepo,
            MyRegisteredUserRepository myUserRepo)
        {
            _userRepo = userRepo;
            _userRoleRepo = userRoleRepo;
            _myUserRepo = myUserRepo;
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
        /// Displays the roles assigned to the specified user, showing the user's first and last name.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <returns>The Detail view with the user's roles.</returns>
        public async Task<IActionResult> Detail(string email)
        {
            var (firstName, lastName) = _myUserRepo.GetNameByEmail(email);
            ViewBag.FirstName = firstName;
            ViewBag.LastName = lastName;
            ViewBag.Email = email;

            IList<string> roles = await _userRoleRepo.GetUserRolesAsync(email);
            return View(roles);
        }

        /// <summary>
        /// Removes a role from the specified user and reloads the Detail view with a confirmation message.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <param name="roleName">The name of the role to remove.</param>
        /// <returns>The Detail view with an updated role list and a status message.</returns>
        [HttpPost]
        public async Task<IActionResult> DeleteUserRole(string email, string roleName)
        {
            bool success = await _userRoleRepo.RemoveUserRoleAsync(email, roleName);

            if (success)
            {
                ViewBag.ConfirmationMessage = "Role removed from user.";
            }
            else
            {
                ViewBag.ErrorMessage = "Failed to remove role from user.";
            }

            var (firstName, lastName) = _myUserRepo.GetNameByEmail(email);
            ViewBag.FirstName = firstName;
            ViewBag.LastName = lastName;
            ViewBag.Email = email;

            IList<string> roles = await _userRoleRepo.GetUserRolesAsync(email);
            return View("Detail", roles);
        }
    }
}
