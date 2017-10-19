using System.Collections.Generic;

namespace Aggregate
{
    public abstract class Aggregate<TVisitor> : IAggregate<TVisitor> where TVisitor : class
    {
        private readonly List<IAcceptVisitors<TVisitor>> uncommitedEvents = new List<IAcceptVisitors<TVisitor>>();
        private readonly string streamId;
        private int loadedAtVersion;

        protected int CurrentVersion;

        protected Aggregate(string streamId)
        {
            this.streamId = streamId;
        }

        int IAggregate<TVisitor>.CurrentVersion => CurrentVersion;

        int IAggregate<TVisitor>.LoadedAtVersion => loadedAtVersion;

        IEnumerable<IAcceptVisitors<TVisitor>> IAggregate<TVisitor>.UncommitedEvents => uncommitedEvents;

        string IAggregate<TVisitor>.StreamId => streamId;

        void IAggregate<TVisitor>.Replay(int version, IEnumerable<IAcceptVisitors<TVisitor>> events)
        {
            foreach (var domainEvent in events)
            {
                ApplyEvent(domainEvent);
            }
            loadedAtVersion = version;
            uncommitedEvents.Clear();
        }

        protected virtual void ApplyEvent(IAcceptVisitors<TVisitor> domainEvent)
        {
            CurrentVersion++;
            uncommitedEvents.Add(domainEvent);
        }

        void IAggregate<TVisitor>.ClearUncommitedEvents()
        {
            uncommitedEvents.Clear();
        }

    }
}