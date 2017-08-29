using System.Linq;
using SpamREST.Models;

namespace SpamREST.ServiceDefinitions
{
  public interface ISpamRESTRepository
  {
    IQueryable<Spam> Spams { get; }
    ISpamRESTRepository Add(Spam spam);
    ISpamRESTRepository Update(Spam spam);
    ISpamRESTRepository Delete(Spam spam);
  }
}