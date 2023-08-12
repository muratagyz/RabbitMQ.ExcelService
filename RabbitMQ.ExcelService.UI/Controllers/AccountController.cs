using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace RabbitMQ.ExcelService.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser>? _signInManager;

        public AccountController(SignInManager<IdentityUser>? signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string Email, string Password)
        {
            var hasUser = await _userManager.FindByEmailAsync(Email);

            if (hasUser == null)
            {
                return RedirectToAction("Index");
            }

            var signInResult = await _signInManager.PasswordSignInAsync(hasUser, Password, true, false);

            if (signInResult.Succeeded is false)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
