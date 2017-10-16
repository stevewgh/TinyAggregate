using System.Threading.Tasks;

namespace Aggregate
{
    public interface IAggregateEventRepository
    {
        Task<IAggregate> Load(string streamId);

        Task Save(IAggregate aggregateEventStream);
    }
}