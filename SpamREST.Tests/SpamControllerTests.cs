using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using SpamREST.Controllers;
using SpamREST.ServiceDefinitions;
using SpamREST.Models;
using Xunit;
using SpamREST.Services;

namespace SpamREST.Tests
{
    public class SpamControllerTests
    {
        [Fact]
        public void Get_ReturnsSpamsFromRepository(){
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

        [Fact]
        public void Post_AddsNewSpam(){
            ISpamRESTRepository repo = new SpamRESTRepositoryInMemory();
            var controller = new SpamsController(repo);

            Assert.Equal(0, controller.Get().Count());

            controller.Post(new Spam(){
                EndPointUri = "https://www.spamREST.com/spam-example1",
                ReporterId = "spamrest",
                ReporteeId = "spammer",
                Content = "Spam 1 Content",
                Created = DateTime.UtcNow,
            });

            var spams = controller.Get().Count();
            Assert.Equal(1, spams);
        }

        [Fact]
        public void Put_UpsertsSpam(){
            ISpamRESTRepository repo = new SpamRESTRepositoryInMemory();
            var controller = new SpamsController(repo);

            Assert.Equal(0, controller.Get().Count());

            controller.Put("https://www.spamREST.com/spam-example1", new Spam(){
                EndPointUri = "https://www.spamREST.com/spam-example1",
                ReporterId = "spamrest",
                ReporteeId = "spammer",
                Content = "Spam 1 Content",
                Created = DateTime.UtcNow,
            });

            var spams = controller.Get();
            Assert.Equal(1, spams.Count());

            var spam = spams.First();
            Assert.Equal("Spam 1 Content", spam.Content);

            controller.Put("https://www.spamREST.com/spam-example1", new Spam(){
                EndPointUri = "https://www.spamREST.com/spam-example1",
                ReporterId = "spamrest",
                ReporteeId = "spammer",
                Content = "Spam 1 Modified Content",
                Created = DateTime.UtcNow,
            });

            spams = controller.Get();
            Assert.Equal(1, spams.Count());

            spam = spams.First();
            Assert.Equal("Spam 1 Modified Content", spam.Content);
        }

        [Fact]
        public void Delete_DeletesSpamById(){
            ISpamRESTRepository repo = new SpamRESTRepositoryInMemory();
            repo.Add(new Spam(){
                EndPointUri = "https://www.spamREST.com/spam-example1",
                ReporterId = "spamrest",
                ReporteeId = "spammer",
                Content = "Spam 1 Content",
                Created = DateTime.UtcNow,
            });

            var controller = new SpamsController(repo);
            controller.Delete("https://www.spamREST.com/spam-example1");
            var spam = repo.Spams
                .Where(s => s.EndPointUri.Equals("https://www.spamREST.com/spam-example1"))
                .SingleOrDefault();
            Assert.Null(spam);
        }
    }
}
