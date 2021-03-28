using System.Threading.Tasks;

namespace NewRayTracer.Services.Events
{
    public interface IEventPublisher<TEvent>
    {
        Task PublishAsync(TEvent @event);
    }
}