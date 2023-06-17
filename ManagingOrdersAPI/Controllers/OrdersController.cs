using OrderManagement.Data;
using OrderManagement.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace OrderManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly DatabaseContext dbcontext;
        public OrdersController(DatabaseContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

 
        [HttpPost]
        public async Task<IActionResult> AddOrder(Order order)
        {
            if(order.OrderId<1)
            {
                return BadRequest("Enter valid id");
            }
            
            var orderCon = new Order()
            {
                OrderId = order.OrderId,
                CustomerName= order.CustomerName,
                CustomerAddress= order.CustomerAddress,
                OrderedItems = order.OrderedItems,
                TotalPrice=order.TotalPrice,
                Status=order.Status.ToLower(),
            };

            if(await dbcontext.Orders.FindAsync(order.OrderId)==null)
            {
                await dbcontext.Orders.AddAsync(orderCon);
                await dbcontext.SaveChangesAsync();
                return Ok(orderCon);
            }
            else { return BadRequest("Id already exist"); }
           
        }



        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetOrderDetails([FromRoute] int id)
        {
            var orderInDatabase = await dbcontext.Orders.Where(e=>e.OrderId==id).Include(e=>e.OrderedItems).ToListAsync();

            if (orderInDatabase != null && orderInDatabase.Any() )
            {
                return Ok(orderInDatabase);
            }
            return NotFound();
        }



        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {   
            if(dbcontext.Orders!=null)
            {
                return Ok(await dbcontext.Orders.Include(e=> e.OrderedItems).ToListAsync());
            }
            return NotFound();
        }



        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateOrderStatus([FromRoute] int id, string status)
        {
            status = status.ToLower();
            var orderInDatabase = await dbcontext.Orders.FindAsync(id);
            if (orderInDatabase != null)
            { 
                if (status == "pending" || status == "shipped" || status == "delivered")
                {
                    orderInDatabase.OrderId = orderInDatabase.OrderId;
                    orderInDatabase.CustomerName = orderInDatabase.CustomerName;
                    orderInDatabase.OrderedItems = orderInDatabase.OrderedItems;
                    orderInDatabase.TotalPrice = orderInDatabase.TotalPrice;
                    orderInDatabase.Status = status.ToLower();

                    await dbcontext.SaveChangesAsync();
                    return Ok(orderInDatabase.Status);
                }
                else
                {
                    status = "Not specified";
                }                
            }
            return NotFound();
        }



        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> CancelOrder([FromRoute] int id)
        {
            var orderInDatabase = await dbcontext.Orders.FindAsync(id);
            if (orderInDatabase != null)
            {
                dbcontext.Remove(orderInDatabase);
                await dbcontext.SaveChangesAsync();
                return Ok(200);
            }
            return NotFound("No order with this id");
        }


        [HttpGet("searchCustomer")]
        public async Task<IActionResult> SearchOrdersByCustomer(string customerName)
        {
            IQueryable<Order> query = dbcontext.Orders;
            if(!string.IsNullOrEmpty(customerName))
            {
                query = query.Where(e => e.CustomerName.ToLower().Contains(customerName.ToLower())).Include(e=>e.OrderedItems);
                return Ok(await query.ToListAsync());
            }
            return NotFound("No Orders by this customer");

        }


        [HttpGet("searchStatus")]
        public async Task<IActionResult> SearchOrdersByStatus(string status)
        {
            status = status.ToLower();
            if (status != "pending" && status != "shipped" && status != "delivered")
            {
                return BadRequest("Enter valid status");
            }
            if (!string.IsNullOrEmpty(status))
            {
                IQueryable<Order> query = dbcontext.Orders.Where(e => e.Status.ToLower().Contains(status)).Include(e => e.OrderedItems);
                return Ok(await query.ToListAsync());
            }
            return NotFound("No Orders in this status");

        }


    }
    
}

