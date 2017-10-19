using System.Collections.Generic;

namespace TinyAggregate
{
    public abstract class Aggregate<TVisitor> : IAggregate<TVisitor>
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
            if (this is TVisitor) {
                domainEvent.Accept((TVisitor)(object)this);
            }
        }

        void IAggregate<TVisitor>.ClearUncommitedEvents()
        {
            uncommitedEvents.Clear();
        }

    }
}