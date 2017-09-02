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
    public class SpamsController : Controller {
        private readonly ISpamRESTRepository repository;

        public SpamsController(ISpamRESTRepository repository) {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get() {
            return Ok(await Task.FromResult(repository.Spams.Take(10)));
        }

        [HttpGet("{endPointUri}")]
        public async Task<IActionResult> Get(string endPointUri) {
            return Ok(
                await Task.FromResult(
                    repository.Spams
                        .Where(s => s.EndPointUri.Equals(endPointUri))
                        .Single()));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Spam spam) {
            await Task.FromResult(repository.Add(spam));
            return Created($"/api/spams/{spam.EndPointUri}", spam);
        }

        [HttpPut("{endPointUri}")]
        public async Task<IActionResult> Put(string endPointUri, [FromBody]Spam spam) {
            var existing = repository.Spams
                .SingleOrDefault(s => s.EndPointUri.Equals(endPointUri));
            if(existing == null){
                return await Post(spam);
            } else {
                repository.Update(spam);
                return Ok(spam);
            }
        }

        [HttpDelete("{endPointUri}")]
        public async Task<IActionResult> Delete(string endPointUri) {
            var spam = repository.Spams
                .Single(s => s.EndPointUri.Equals(endPointUri));
            await Task.FromResult(repository.Delete(spam));
            return NoContent();
        }
    }
}
