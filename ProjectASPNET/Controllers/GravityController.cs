using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectASPNET.Models;
using X.PagedList.Extensions;
using X.PagedList.Mvc.Core;

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
		public async Task<IActionResult> Clients(int page = 1, int pagesize = 10)
		{
			var customers = from customer in _context.Customers
							join customerAddress in _context.CustomerAddresses
							on customer.CustomerId equals customerAddress.CustomerId
							join address in _context.Addresses 
							on customerAddress.AddressId equals address.AddressId
							join Country in _context.Countries
							on address.CountryId equals Country.CountryId
							orderby customer.CustomerId
							select new
							{
								customer.CustomerId,
								customer.FirstName,
								customer.LastName,
								customer.Email,
								Country.CountryName
							};
			var model = _context.Customers.OrderBy(x => x.CustomerId).ToPagedList(page, pagesize);
			var list = customers.ToPagedList(page, pagesize);
			
			return View(list);
		}
	}

}
