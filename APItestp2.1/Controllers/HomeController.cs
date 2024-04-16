using APItestp2._1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace APItestp2._1.Controllers
{
    public class HomeController : Controller
    {

        

        public IActionResult Index()
        {
            return View();
        }
       
    }
}
