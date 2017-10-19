using System.Collections.Generic;

namespace Aggregate
{
    public interface IAggregate<TVisitor> where TVisitor : class
    {
        IEnumerable<IAcceptVisitors<TVisitor>> UncommitedEvents { get; }

        int CurrentVersion { get; }

        int LoadedAtVersion { get; }

        string StreamId { get; }

        void ClearUncommitedEvents();

        void Replay(int version, IEnumerable<IAcceptVisitors<TVisitor>> events);
    }
}