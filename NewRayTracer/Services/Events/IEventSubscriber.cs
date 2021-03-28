using System.Threading.Tasks;

namespace NewRayTracer.Services.Events
{
    public interface IEventSubscriber<in TEvent>
    {
        Task Publish(TEvent @event);
    }
}
