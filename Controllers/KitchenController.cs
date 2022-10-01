using Kitchen.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kitchen.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KitchenController : ControllerBase
    {
       

        private readonly ILogger<KitchenController> _logger;

        public KitchenController(ILogger<KitchenController> logger)
        {
            _logger = logger;
        }
        //freastra la care putem sa ne adresam
        [HttpPost("Order")]
        public void Order([FromBody] Order order)
        {
            
        }
        
    }
}