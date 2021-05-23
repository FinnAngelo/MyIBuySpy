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
    // TODO: replace View with proper EFCore select so it can run on sqlite
    [Route("api/[controller]")]
    [ApiController]
    public class ViewAlsoPurchasedsController : ControllerBase
    {
        private readonly CommerceDbContext _context;
        
        public ViewAlsoPurchasedsController(CommerceDbContext context)
        {
            _context = context;
        }

        // GET: api/ViewAlsoPurchaseds
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ViewAlsoPurchased>>> GetViewAlsoPurchaseds()
        {
            return await _context.ViewAlsoPurchaseds.ToListAsync();
        }

        // GET: api/ViewAlsoPurchaseds/5
        [HttpGet("{orderId}")]
        public async Task<ActionResult<IEnumerable<ViewAlsoPurchased>>> GetViewAlsoPurchaseds(int orderId)
        {
            var viewAlsoPurchaseds = await _context.ViewAlsoPurchaseds
                .Where(vap => vap.OrderId == orderId)
                .ToListAsync();

            if (viewAlsoPurchaseds == null)
            {
                return NotFound();
            }

            return viewAlsoPurchaseds;
        }
    }
}
