using System.Collections.Generic;

namespace TinyAggregate
{
    public abstract class Aggregate<TVisitor> : IAggregate<TVisitor>
    {
        private readonly List<IAcceptVisitors<TVisitor>> uncommitedEvents = new List<IAcceptVisitors<TVisitor>>();
        private int loadedAtVersion;

        protected int CurrentVersion;

        int IAggregate<TVisitor>.CurrentVersion => CurrentVersion;

        int IAggregate<TVisitor>.LoadedAtVersion => loadedAtVersion;

        IEnumerable<IAcceptVisitors<TVisitor>> IAggregate<TVisitor>.UncommitedEvents => uncommitedEvents;

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