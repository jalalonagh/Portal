using ApplicationService.UserManager;
using DataTransferObjects.InputModels.UserManager.Users;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class Account : Controller
    {
        private IUserAppServices _userAppServices;

        public Account(IUserAppServices userAppServices)
        {
            _userAppServices = userAppServices;
        }

        public async Task<IActionResult> Index(CancellationToken cancellation)
        {

            return Ok();
        }

        [HttpGet]
        public IActionResult Login(CancellationToken cancellation)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(CancellationToken cancellation, LoginSwaggerDTO dto)
        {
            var result = await _userAppServices.LoginSwaggerAsync(cancellation, dto).ConfigureAwait(true);

            return Redirect("/swagger/index.html");
        }
    }
}
