using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using NerdDinner.Web.Models;
using NerdDinner.Web.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace NerdDinner.Web.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class RsvpController : Controller
    {
        private readonly INerdDinnerRepository _repository;

        private readonly UserManager<ApplicationUser> _userManager;

        public RsvpController(INerdDinnerRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRsvpAsync(int dinnerId)
        {
            var dinner = await _repository.GetDinnerAsync(dinnerId);
            if (dinner == null)
            {
                return View("Error");
            }
            
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            var rsvp = await _repository.CreateRsvpAsync(dinner, user.UserName);
            return new JsonResult(rsvp);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRsvpAsync(int dinnerId)  
        {
            var dinner = await _repository.GetDinnerAsync(dinnerId);
            if (dinner == null)
            {
                return View("Error");
            }

            var user = HttpContext.User;

            await _repository.DeleteRsvpAsync(dinner, user.Identity.Name);
            return View("Error");
        }
    }
}