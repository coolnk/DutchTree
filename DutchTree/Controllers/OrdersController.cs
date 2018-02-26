using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DutchTree.Data;
using DutchTree.Data.Entities;
using DutchTree.ViewModel;
using Microsoft.Extensions.Logging;

namespace DutchTree.Controllers
{
    [Produces("application/json")]
    [Route("api/[Controller]")]
    public class OrdersController : Controller
    {
        private readonly IDutchRepository _repository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public OrdersController(IDutchRepository repository, ILogger<OrdersController> logger, IMapper mapper)
        {
           _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        // GET: api/Orders
        [HttpGet]
        public IActionResult GetOrders(bool includeItems = false)
        {
            try
            {
                var result = _repository.GetAllOrders(includeItems);
                return Ok(_mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(result));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get orders: {ex}");
                return BadRequest("Failed to get orders");
            }
        }

        // GET: api/Orders/5
        [HttpGet("{id:int}")]
        public IActionResult GetOrder([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var order = _repository.GetOrderById(id);
                // optional Where( m => m.Id == id).FirstOrDefault();

                if (order == null)
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<Order, OrderViewModel>(order));

            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get order:{ex}");
                return BadRequest("Failed to get orders");
            }
            
        }

        //// PUT: api/Orders/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutOrder([FromRoute] int id, [FromBody] Order order)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != order.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(order).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!OrderExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Orders
        [HttpPost]
        public IActionResult PostOrder([FromBody] OrderViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {

                    var newOrder = _mapper.Map<OrderViewModel, Order>(model);
               
                    //min means they did not specify anything

                if (newOrder.OrderDate == DateTime.MinValue)
                {
                    newOrder.OrderDate = DateTime.Now;
                }


                    _repository.AddEntity(newOrder);
                    if (_repository.SaveAll())
                    {
                        //var vm = new OrderViewModel()
                        //{
                        //    OrderNumber = newOrder.OrderNumber,
                        //    OrderDate = newOrder.OrderDate,
                        //    OrderId = newOrder.Id
                        //};

                        //created is 201 you could pas the order but below 
                        return CreatedAtAction("GetOrder", new { id = newOrder.Id }, _mapper.Map<Order,OrderViewModel>(newOrder));
                        //return Created($"/api/orders/{model.Id}", model);

                    }
                    _logger.LogError("Failed to save a new order, something wrong happened");
                    return null;
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save a new order: {ex}");
                throw;
            }


        }

        //// DELETE: api/Orders/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteOrder([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var order = await _context.Orders.SingleOrDefaultAsync(m => m.Id == id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Orders.Remove(order);
        //    await _context.SaveChangesAsync();

        //    return Ok(order);
        //}

        //private bool OrderExists(int id)
        //{
        //    return _context.Orders.Any(e => e.Id == id);
        //}
    }
}