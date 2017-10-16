using System.Collections.Generic;

namespace Aggregate
{
    public interface IAggregate
    {
        IEnumerable<object> UncommitedEvents { get; }

        int CurrentVersion { get; }

        int LoadedAtVersion { get; }

        string StreamId { get; }

        void ClearUncommitedEvents();

        void Replay(IEnumerable<object> events);
    }
}