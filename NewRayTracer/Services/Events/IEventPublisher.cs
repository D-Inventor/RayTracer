using System.Threading.Tasks;

namespace NewRayTracer.Services.Events
{
    public interface IEventPublisher<TEvent>
    {
        Task Publish(TEvent @event);
    }
}