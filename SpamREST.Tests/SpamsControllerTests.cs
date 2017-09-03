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
using SpamREST.Tests.Mocks;

namespace SpamREST.Tests
{
    public class SpamsControllerTests : IDisposable
    {
        #region Plumbing
        private readonly SpamsController sut;
        private readonly ISpamRESTRepository repo;

        private static IEnumerable<Spam> SpamsList(int count){
            for(int i = 0; i < count; i++){
                yield return new Spam(){
                    EndPointUri = $"https://localhost/spam-example{i}",
                    ReporterId = $"spamrest{i}",
                    ReporteeId = $"spammer{i}",
                    Content = $"Spam Content {i}",
                    Created = DateTime.UtcNow,
                };
            }
        }

        public SpamsControllerTests(){
            sut = new SpamsController(
                repo = new SpamRESTRepositoryMock());
        }

        public void Dispose(){
            sut.Dispose();
        }
        #endregion Plumbing

        [Fact]
        public async Task Get_ReturnsSpams_FromRepository(){
            repo.Add(SpamsList(2).ToArray());
            var response = Assert.IsType<OkObjectResult>(await sut.Get());
            var actual = Assert.IsType<EnumerableQuery<Spam>>(response.Value);
            Assert.Equal(actual.Count(), 2);
        }

        [Fact]
        public async Task Get_ReturnsSpam_ById(){
            repo.Add(SpamsList(2).ToArray());
            var response = Assert.IsType<OkObjectResult>(await sut.Get("https://localhost/spam-example1"));
            var actual = Assert.IsType<Spam>(response.Value);
            Assert.Equal(actual.Content, "Spam Content 1");
        }

        [Fact]
        public async Task Post_AddsNewSpam(){
            await sut.Post(SpamsList(1).First());
            var response = Assert.IsType<OkObjectResult>(await sut.Get());
            var actual = Assert.IsType<EnumerableQuery<Spam>>(response.Value);
            Assert.Equal(1, actual.Count());
        }

        [Fact]
        public async Task Put_UpsertsSpam(){
            var spamToCreateViaPUT = SpamsList(1).Single();
            await sut.Put(spamToCreateViaPUT.EndPointUri, spamToCreateViaPUT);
            var response = Assert.IsType<OkObjectResult>(await sut.Get());
            var actual = Assert.IsType<EnumerableQuery<Spam>>(response.Value);
            Assert.Equal("Spam Content 0", actual.Single().Content);

            var spamToUpdateViaPUT = SpamsList(1).Single();
            spamToUpdateViaPUT.Content = "Spam Modified Content 0";
            await sut.Put(spamToUpdateViaPUT.EndPointUri, spamToUpdateViaPUT);
            response = Assert.IsType<OkObjectResult>(await sut.Get());
            actual = Assert.IsType<EnumerableQuery<Spam>>(response.Value);
            Assert.Equal("Spam Modified Content 0", actual.Single().Content);
        }

        [Fact]
        public async Task Delete_DeletesSpamById(){
            repo.Add(SpamsList(1).Single());
            var actual = Assert.IsType<NoContentResult>(await sut.Delete("https://localhost/spam-example0"));
            Assert.False(repo.Spams.Any());
        }
    }
}
