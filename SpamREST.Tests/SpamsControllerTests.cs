using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using SpamREST.Controllers;
using SpamREST.ServiceDefinitions;
using SpamREST.Models;
using Xunit;
using SpamREST.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SpamREST.Tests
{
    public class SpamsControllerTests
    {
        private static IEnumerable<Spam> SpamsList(int count){
            for(int i = 0; i < count; i++){
                yield return new Spam(){
                    EndPointUri = $"https://www.spamREST.com/spam-example{i}",
                    ReporterId = $"spamrest{i}",
                    ReporteeId = $"spammer{i}",
                    Content = $"Spam Content {i}",
                    Created = DateTime.UtcNow,
                };
            }
        }

        [Fact]
        public async Task Get_ReturnsSpams_FromRepository(){
            var repoMock = new Mock<ISpamRESTRepository>();
            repoMock.Setup(r => r.Spams)
                .Returns(SpamsList(2).AsQueryable());

            var controller = new SpamsController(repoMock.Object);
            var response = Assert.IsType<OkObjectResult>(await controller.Get());
            var actual = Assert.IsType<EnumerableQuery<Spam>>(response.Value);
            Assert.Equal(actual.Count(), 2);
        }

        [Fact]
        public async Task Get_ReturnsSpam_ById(){
            var repoMock = new Mock<ISpamRESTRepository>();
            repoMock.Setup(r => r.Spams)
                .Returns(SpamsList(2).AsQueryable());

            var controller = new SpamsController(repoMock.Object);
            var response = Assert.IsType<OkObjectResult>(await controller.Get("https://www.spamREST.com/spam-example1"));
            var actual = Assert.IsType<Spam>(response.Value);
            Assert.Equal(actual.Content, "Spam Content 1");
        }

        [Fact]
        public async Task Post_AddsNewSpam(){
            ISpamRESTRepository repo = new SpamRESTRepositoryInMemory();
            var controller = new SpamsController(repo);
            var response = Assert.IsType<OkObjectResult>(await controller.Get());
            var actual = Assert.IsType<EnumerableQuery<Spam>>(response.Value);
            Assert.Equal(0, actual.Count());

            await controller.Post(SpamsList(1).First());

            response = Assert.IsType<OkObjectResult>(await controller.Get());
            actual = Assert.IsType<EnumerableQuery<Spam>>(response.Value);
            Assert.Equal(1, actual.Count());
        }

        [Fact]
        public async Task Put_UpsertsSpam(){
            ISpamRESTRepository repo = new SpamRESTRepositoryInMemory();
            var controller = new SpamsController(repo);
            var response = Assert.IsType<OkObjectResult>(await controller.Get());
            var actual = Assert.IsType<EnumerableQuery<Spam>>(response.Value);
            Assert.Equal(0, actual.Count());

            var spamToCreateViaPUT = SpamsList(1).Single();
            await controller.Put(spamToCreateViaPUT.EndPointUri, spamToCreateViaPUT);
            response = Assert.IsType<OkObjectResult>(await controller.Get());
            actual = Assert.IsType<EnumerableQuery<Spam>>(response.Value);

            Assert.Equal("Spam Content 0", actual.Single().Content);

            var spamToUpdateViaPUT = SpamsList(1).Single();
            spamToUpdateViaPUT.Content = "Spam Modified Content 0";
            await controller.Put(spamToUpdateViaPUT.EndPointUri, spamToUpdateViaPUT);

            response = Assert.IsType<OkObjectResult>(await controller.Get());
            actual = Assert.IsType<EnumerableQuery<Spam>>(response.Value);

            Assert.Equal("Spam Modified Content 0", actual.Single().Content);
        }

        [Fact]
        public async Task Delete_DeletesSpamById(){
            ISpamRESTRepository repo = new SpamRESTRepositoryInMemory();
            repo.Add(SpamsList(1).Single());

            var controller = new SpamsController(repo);
            var actual = Assert.IsType<NoContentResult>(await controller.Delete("https://www.spamREST.com/spam-example0"));
            Assert.False(repo.Spams.Any());
        }
    }
}
