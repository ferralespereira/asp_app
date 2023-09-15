using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace asp_app.Controllers;

public class EmployeeController : Controller
{
    public IActionResult Index()
    {
        ViewData["Message"] = "Hello sasd";
        return View();
    }
    public IActionResult Welcome(string name, int numTimes = 1)
    {
        ViewData["Message"] = "Hello " + name;
        ViewData["NumTimes"] = numTimes;
        return View();
    }
}