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
    public class ViewOrderDetailsController : ControllerBase
    {
        private readonly CommerceDbContext _context;
        
        public ViewOrderDetailsController(CommerceDbContext context)
        {
            _context = context;
        }

        private IQueryable<VewOrderDetail> GetViewOrderDetailsQuery()
        {
            var result =
                from od in _context.OrderDetails
                join p in _context.Products on od.ProductId equals p.ProductId
                select new VewOrderDetail()
                {
                    ProductId = p.ProductId,
                    ModelNumber = p.ModelNumber,
                    ModelName = p.ModelName,
                    Quantity = od.Quantity,
                    UnitCost = od.UnitCost,
                    OrderId = od.OrderId
                };
            return result;
        }

        // GET: api/ViewOrderDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VewOrderDetail>>> GetVewOrderDetails()
        {
            var orderDetails = await GetViewOrderDetailsQuery()
                .ToListAsync();

            if (orderDetails == null || !orderDetails.Any())
            {
                return NotFound();
            }

            return orderDetails;
        }

        // GET: api/ViewOrderDetails/5
        [HttpGet("{orderId}")]
        public async Task<ActionResult<VewOrderDetail>> GetVewOrderDetail(int orderId)
        {
            var orderDetails = await GetViewOrderDetailsQuery()
                .Where(vod => vod.OrderId == orderId).FirstOrDefaultAsync();

            if (orderDetails == null)
            {
                return NotFound();
            }

            return orderDetails;
        }
    }
}
