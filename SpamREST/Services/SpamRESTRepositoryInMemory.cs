using System.Collections.Generic;
using System.Linq;
using SpamREST.Models;
using SpamREST.ServiceDefinitions;

namespace SpamREST.Services
{
  public class SpamRESTRepositoryInMemory : ISpamRESTRepository
  {
    private List<Spam> storage = new List<Spam>();

    private Spam ByEndPointUri(string endPointUri) =>
      storage
        .Where(s => 
          s.EndPointUri.Equals(endPointUri))
        .Single();

    IQueryable<Spam> ISpamRESTRepository.Spams => 
      storage.AsQueryable();

    ISpamRESTRepository ISpamRESTRepository.Add(Spam spam)
    {
      storage.Add(spam);
      return this;
    }

    ISpamRESTRepository ISpamRESTRepository.Delete(Spam spam)
    {
      storage
        .Remove(ByEndPointUri(spam.EndPointUri));
      return this;
    }

    ISpamRESTRepository ISpamRESTRepository.Update(Spam spam)
    {
      var index = storage.IndexOf(ByEndPointUri(spam.EndPointUri));
      storage.RemoveAt(index);
      return this;
    }
  }
}