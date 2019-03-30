using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OpenDevBlog.Web.Controllers
{
    public class ArticlesController : Controller
    {
        [HttpGet]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(object model)
        {
            return this.Redirect("Success");
        }

        [HttpGet]
        public IActionResult Success()
        {
            return this.View();
        }
    }
}