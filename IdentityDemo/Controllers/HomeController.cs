using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IdentityDemo.Models;
using Microsoft.AspNetCore.Identity;

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
                        ID = Guid.NewGuid( ).ToString( ),
                        UserName = model.UserName
                    };

                    var result = await _userManager.CreateAsync( user, model.Password );
                }
                return View( "Success" );
            }
            return View( );
        }
    }
}
