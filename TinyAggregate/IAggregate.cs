using System.Collections.Generic;

namespace TinyAggregate
{
    public interface IAggregate<TVisitor>
    {
        IEnumerable<IAcceptVisitors<TVisitor>> UncommitedEvents { get; }

        int LoadedAtVersion { get; }

        void ClearUncommitedEvents();

        void Replay(int loadedVersion, IEnumerable<IAcceptVisitors<TVisitor>> events);
    }
}