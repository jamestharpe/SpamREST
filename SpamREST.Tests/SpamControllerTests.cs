using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using SpamREST.Controllers;
using SpamREST.ServiceDefinitions;
using SpamREST.Models;
using Xunit;

namespace SpamREST.Tests
{
    public class SpamControllerTests
    {
        [Fact]
        public void Get_ReturnsSpamsFromRepository()
        {
            var repoMock = new Mock<ISpamRESTRepository>();
            repoMock.Setup(r => r.Spams)
                .Returns(
                    new List<Spam>(){
                        new Spam(){
                            EndPointUri = "https://www.spamREST.com/spam-example1",
                            ReporterId = "spamrest",
                            ReporteeId = "spammer",
                            Content = "Spam 1 Content",
                            Created = DateTime.UtcNow,
                        },
                        new Spam(){
                            EndPointUri = "https://www.spamREST.com/spam-example2",
                            ReporterId = "spamrest",
                            ReporteeId = "spammer",
                            Content = "Spam 2 Content",
                            Created = DateTime.UtcNow,
                        }
                    }.AsQueryable()
                );

            var controller = new SpamsController(repoMock.Object);
            var actual = controller.Get();
            Assert.Equal(actual.Count(), 2);
        }

        [Fact]
        public void Get_ReturnsSpamById(){
            var repoMock = new Mock<ISpamRESTRepository>();
            repoMock.Setup(r => r.Spams)
                .Returns(
                    new List<Spam>(){
                        new Spam(){
                            EndPointUri = "https://www.spamREST.com/spam-example1",
                            ReporterId = "spamrest",
                            ReporteeId = "spammer",
                            Content = "Spam 1 Content",
                            Created = DateTime.UtcNow,
                        },
                        new Spam(){
                            EndPointUri = "https://www.spamREST.com/spam-example2",
                            ReporterId = "spamrest",
                            ReporteeId = "spammer",
                            Content = "Spam 2 Content",
                            Created = DateTime.UtcNow,
                        }
                    }.AsQueryable()
                );

            var controller = new SpamsController(repoMock.Object);
            var actual = controller.Get("https://www.spamREST.com/spam-example2");
            Assert.Equal(actual.Content, "Spam 2 Content");
        }
    }
}
