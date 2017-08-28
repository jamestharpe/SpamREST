using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpamREST.Models;

namespace SpamREST.Controllers
{
    [Route("api/[controller]")]
    public class SpamsController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<Spam> Get()
        {
            return new Spam[] { 
                new Spam(){
                    Content = "Spam 1",
                    ReporteeId = "Spammer123",
                    ReporterId = "GoodCitizen456",
                    EndPointUri = "http://localhost/api/spammer123/nigerian-prince",
                    Created = DateTime.UtcNow
                }, 
                new Spam(){
                    Content = "Spam 2",
                    ReporteeId = "Spammer123",
                    ReporterId = "GoodCitizen456",
                    EndPointUri = "http://localhost/api/spammer123/nigerian-princess",
                    Created = DateTime.UtcNow
                },
            };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
