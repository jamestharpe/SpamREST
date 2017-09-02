using System.Linq;
using SpamREST.Models;

namespace SpamREST.ServiceDefinitions
{
  public interface ISpamRESTRepository {
    IQueryable<Spam> Spams { get; }
    ISpamRESTRepository Add(params Spam[] spams);
    ISpamRESTRepository Update(params Spam[] spams);
    ISpamRESTRepository Delete(params Spam[] spams);
  }
}