using EronNew.Data;
using EronNew.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EronNew.Controllers
{
    public class OrderInfo
    {
        public int productId { get; set; }
        public string ownerId { get; set; }
        public long postId { get; set; }
    }

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IDomainModel _domainModel { get; set; }
        private readonly ExtendedUserManager<ExtendedIdentityUser> _userManager;
        public OrderController(
            IDomainModel domainModel, 
            ExtendedUserManager<ExtendedIdentityUser> userManager)
        {
            _userManager = userManager;
            _domainModel = domainModel;
        }

        [HttpPost]
        public string Post([FromForm] OrderInfo value)
        {
            var user = _userManager.GetUserAsync(User).Result;
            var result = _domainModel.InsertOrder(value.productId, user.Id, null).Result;
            return result.ToString();
        }

        // POST api/<OrderController>
        [HttpPost("{id}")]
        public string Post(long id, [FromForm] OrderInfo value)
        {
            var user = _userManager.GetUserAsync(User).Result;
            var result = _domainModel.InsertOrder(value.productId, user.Id, id).Result;
            return result.ToString();
        }

        // PUT api/<OrderController>/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            var user = _userManager.GetUserAsync(User).Result;
            var result = _domainModel.CancelOrder(id, user.Id).Result;
            return result.ToString();
        }
    }
}
