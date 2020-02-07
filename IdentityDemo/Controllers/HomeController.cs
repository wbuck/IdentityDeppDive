using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IdentityDemo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace IdentityDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<PluralSightUser> _userManager;

        public HomeController( UserManager<PluralSightUser> userManager,
                               ILogger<HomeController> logger )
        {
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult Index( )
        {
            return View( );
        }

        public IActionResult Privacy( )
        {
            return View( );
        }

        [Authorize]
        public IActionResult About( ) => View( );


        [ResponseCache( Duration = 0, Location = ResponseCacheLocation.None, NoStore = true )]
        public IActionResult Error( )
        {
            return View( new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier } );
        }

        [HttpGet]
        public IActionResult Register( )
        {
            return View( );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register( RegisterModel model )
        {
            if ( ModelState.IsValid )
            {
                var user = await _userManager.FindByNameAsync( model.UserName );

                if ( user is null )
                {
                    user = new PluralSightUser
                    {
                        Id = Guid.NewGuid( ).ToString( ),
                        UserName = model.UserName                        
                    };

                    var _ = await _userManager.CreateAsync( user, model.Password );
                }
                return View( "Success" );
            }
            return View( );
        }

        [HttpGet]
        public IActionResult Login( ) => View( );

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login( LoginModel model )
        {
            if ( ModelState.IsValid )
            {
                var user = await _userManager.FindByNameAsync( model.UserName );

                if ( user != null &&
                     await _userManager.CheckPasswordAsync( user, model.Password ) )
                {
                    var identity = new ClaimsIdentity( "Cookies" );
                    identity.AddClaim( new Claim( ClaimTypes.NameIdentifier, user.Id ) );
                    identity.AddClaim( new Claim( ClaimTypes.Name, user.UserName ) );

                    await HttpContext.SignInAsync( "Cookies", new ClaimsPrincipal( identity ) );
                    return RedirectToAction( "Index" );
                }
                ModelState.AddModelError( "", "Invalid username or password" );
            }
            return View( );
        }
    }
}
