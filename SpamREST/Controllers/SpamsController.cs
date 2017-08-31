using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpamREST.ServiceDefinitions;
using SpamREST.Models;

namespace SpamREST.Controllers
{
    [Route("api/[controller]")]
    public class SpamsController : Controller
    {
        private readonly ISpamRESTRepository repository;

        public SpamsController(ISpamRESTRepository repository)
        {
            this.repository = repository;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<Spam> Get()
        {
            return repository.Spams.Take(10);
        }

        // GET api/values/5
        [HttpGet("{endPointUri}")]
        public Spam Get(string endPointUri)
        {
            return repository.Spams
                .Where(s => s.EndPointUri.Equals(endPointUri))
                .Single();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Spam spam)
        {
            repository.Add(spam);
        }

        // PUT api/values/5
        [HttpPut("{endPointUri}")]
        public void Put(string endPointUri, [FromBody]Spam spam)
        {
            var existing = repository.Spams
                .SingleOrDefault(s => s.EndPointUri.Equals(endPointUri));
            if(existing == null){
                Post(spam);
            } else {
                repository.Update(spam);
            }
        }

        // DELETE api/values/5
        [HttpDelete("{endPointUri}")]
        public void Delete(string endPointUri)
        {
            var spam = repository.Spams
                .Single(s => s.EndPointUri.Equals(endPointUri));
            repository.Delete(spam);
        }
    }
}
