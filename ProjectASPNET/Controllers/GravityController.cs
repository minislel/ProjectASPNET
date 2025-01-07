using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectASPNET.Models;
using X.PagedList.Extensions;

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
                            join country in _context.Countries
                            on address.CountryId equals country.CountryId

                            let orderCount = _context.CustOrders
                                .Count(o => o.CustomerId == customer.CustomerId) // Zliczanie zamówień
                            select new CustomerViewModel
                            {
                                CustomerId = customer.CustomerId,
                                FirstName = customer.FirstName,
                                LastName = customer.LastName,
                                Email = customer.Email,
                                CountryName = country.CountryName,
                                orderCount = orderCount
                            };


            var model = _context.Customers.OrderBy(x => x.CustomerId).ToPagedList(page, pagesize);
            var list = customers.ToPagedList(page, pagesize);

            return View(list);
        }
        public async Task<IActionResult> Orders(int customerId = 1, int page = 1, int pagesize = 10)
        {
            var orders = from order in _context.CustOrders
                         join customer in _context.Customers
                         on order.CustomerId equals customer.CustomerId
                         join address in _context.Addresses
                         on order.DestAddressId equals address.AddressId
                         join country in _context.Countries
                         on address.CountryId equals country.CountryId
                         where order.CustomerId == customerId
                         select new CustOrder
                         {
                             OrderId = order.OrderId,
                             OrderDate = order.OrderDate,
                             CustomerId = order.CustomerId

                         };

            var model = _context.CustOrders.OrderBy(x => x.OrderId).ToPagedList(page, pagesize);
            var list = orders.ToPagedList(page, pagesize);

            return View(list);
        }
        [HttpGet]
        public async Task<IActionResult> OrderDetails(int orderId)
        {
            var details = (from order in _context.CustOrders
                          join customer in _context.Customers
                          on order.CustomerId equals customer.CustomerId
                          join address in _context.Addresses
                          on order.DestAddressId equals address.AddressId
                          join Country in _context.Countries
                          on address.CountryId equals Country.CountryId
                          where order.OrderId == orderId
                          select new OrderDetailsViewModel
                          {
                              OrderId = order.OrderId,
                              OrderDate = order.OrderDate,
                              CustomerFirstName = customer.FirstName,
                              CustomerLastName = customer.LastName,
                              ShippingMethod = order.ShippingMethod.MethodName,
                              DestAddress = address,
                              BookWithPriceAndTitle = new Dictionary<int?, List<string>>(),
                              CustomerId = customer.CustomerId,
                              CountryName = Country.CountryName
                          }).FirstOrDefault();
            var lines = await (from orderLine in _context.OrderLines
                               join book in _context.Books
                               on orderLine.BookId equals book.BookId
                               where orderLine.OrderId == orderId
                               select new
                               {
                                   orderLine.BookId,
                                   book.Title,
                                   orderLine.Price
                               }).ToListAsync();
            details.StatusId =  _context.OrderHistories
    .Where(x => x.OrderId == orderId)
    .OrderByDescending(x => x.StatusDate)
    .Select(x => x.StatusId)
    .First();
            details.statuslist = await (from statuses in _context.OrderStatuses
                                    orderby statuses.StatusId
                                    select new status
                                    {
                                       StatusId = statuses.StatusId,
                                       StatusValue = statuses.StatusValue
                                    }
                                    ).ToListAsync();
            foreach (var line in lines)
            {
                details.BookWithPriceAndTitle.Add(line.BookId, new List<string>{ line.Title, line.Price.ToString() });
            }

            return View(details);
        }
        [HttpPost]
        public async Task<IActionResult> AddOrderHistory(OrderHistory model)
        { 
            var newHistoryId = (from history in _context.OrderHistories
                                select history.HistoryId).Max();
                                
            var newHistory = new OrderHistory
            {
                HistoryId = newHistoryId + 1,
                OrderId = model.OrderId,
                StatusId = model.StatusId,
                StatusDate = DateTime.Now
            };
            _context.OrderHistories.Add(newHistory);
            await _context.SaveChangesAsync();
            return RedirectToAction("OrderDetails", new { orderId = model.OrderId });
        }
    }

}
