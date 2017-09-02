using System.Collections.Generic;
using System.Linq;
using SpamREST.Models;
using SpamREST.ServiceDefinitions;
using SpamREST.Services;

namespace SpamREST.Tests.Mocks
{
  public class SpamRESTRepositoryMock : SpamRESTRepositoryInMemory, ISpamRESTRepository
  {
  }
}