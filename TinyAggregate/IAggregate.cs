using System.Collections.Generic;

namespace TinyAggregate
{
    public interface IAggregate<TVisitor>
    {
        IEnumerable<IAcceptVisitors<TVisitor>> UncommitedEvents { get; }

        int CurrentVersion { get; }

        int LoadedAtVersion { get; }

        string StreamId { get; }

        void ClearUncommitedEvents();

        void Replay(int version, IEnumerable<IAcceptVisitors<TVisitor>> events);
    }
}