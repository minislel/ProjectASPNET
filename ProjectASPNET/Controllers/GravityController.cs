using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectASPNET.Models;
using X.PagedList;

namespace ProjectASPNET.Controllers
{
	public class GravityController : Controller
	{
		private readonly GravityContext _context;		
		public GravityController(GravityContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			return View();
		}
		public async Task<IActionResult> Clients()
		{
			return View(await _context.Customers.ToListAsync());
		}
	}
}
