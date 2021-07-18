using System.Threading.Tasks;
namespace ExchangeRates.Processor.Services
{
    public interface IEventNotificationService
    {
        Task Notify(object changedRates);
    }

    public class EventNotificationService : IEventNotificationService
    {
        public Task Notify(object changedRates)
        {
            throw new System.NotImplementedException();
        }
    }
}