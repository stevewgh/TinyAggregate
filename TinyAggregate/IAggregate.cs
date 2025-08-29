using System.Collections.Generic;

namespace TinyAggregate
{
    public interface IAggregate<TEventUnion> where TEventUnion : IEventUnion
    {
        IEnumerable<TEventUnion> UncommitedEvents { get; }

        int LoadedAtVersion { get; }

        void ClearUncommitedEvents();

        void Replay(int loadedVersion, IEnumerable<TEventUnion> events);
    }
}