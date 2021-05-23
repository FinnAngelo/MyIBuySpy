using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinnAngelo.MyIBuySpy.AngUI.Areas.Commerce.Data;
using FinnAngelo.MyIBuySpy.AngUI.Areas.Commerce.Models;

namespace FinnAngelo.MyIBuySpy.AngUI.Areas.Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewCartController : ControllerBase
    {
        private readonly CommerceDbContext _context;
        
        public ViewCartController(CommerceDbContext context)
        {
            _context = context;
        }

        private IQueryable<ViewCart> GetViewCartsQuery()
        {
            var result =
                from p in _context.Products
                join sc in _context.ShoppingCarts
                    on p.ProductId equals sc.ProductId
                orderby p.ModelName, p.ModelNumber
                select new ViewCart()
                {
                    ProductId = p.ProductId,
                    ModelNumber = p.ModelNumber,
                    ModelName = p.ModelName,
                    UnitCost = p.UnitCost,
                    Quantity = sc.Quantity,
                    CartId = sc.CartId
                };
            return result;
        }

        // GET: api/ViewOrderDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ViewCart>>> GetViewCarts()
        {
            var viewCarts = await GetViewCartsQuery()
                .ToListAsync();

            if (viewCarts == null || !viewCarts.Any())
            {
                return NotFound();
            }

            return viewCarts;
        }

        // GET: api/ViewOrderDetails/5
        [HttpGet("{cartId}")]
        public async Task<ActionResult<IEnumerable<ViewCart>>> GetViewCarts(string cartId)
        {
            var viewCart = await GetViewCartsQuery()
                .Where(vod => vod.CartId == cartId).ToListAsync();

            if (viewCart == null || !viewCart.Any())
            {
                return NotFound();
            }

            return viewCart;
        }
    }
}
