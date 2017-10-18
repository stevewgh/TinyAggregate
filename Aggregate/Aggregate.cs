using System.Collections.Generic;

namespace Aggregate
{
    public abstract class Aggregate : IAggregate
    {
        private readonly List<object> uncommitedEvents = new List<object>();
        private readonly string streamId;
        private int loadedAtVersion;

        protected int CurrentVersion;

        protected Aggregate(string streamId)
        {
            this.streamId = streamId;
        }

        int IAggregate.CurrentVersion => CurrentVersion;

        int IAggregate.LoadedAtVersion => loadedAtVersion;

        IEnumerable<object> IAggregate.UncommitedEvents => uncommitedEvents;

        string IAggregate.StreamId => streamId;

        void IAggregate.Replay(int version, IEnumerable<object> events)
        {
            foreach (var domainEvent in events)
            {
                ApplyEvent(domainEvent);
            }
            loadedAtVersion = version;
            uncommitedEvents.Clear();
        }

        protected virtual void ApplyEvent(object domainEvent)
        {
            CurrentVersion++;
            uncommitedEvents.Add(domainEvent);
        }

        void IAggregate.ClearUncommitedEvents()
        {
            uncommitedEvents.Clear();
        }

    }

    public abstract class Aggregate<TVisitor> : Aggregate where TVisitor : class 
    {
        protected Aggregate(string streamId) : base(streamId) { }

        protected sealed override void ApplyEvent(object domainEvent)
        {
            var domainEventThatAcceptsVisitors = domainEvent as IAcceptVisitors<TVisitor>;
            ApplyEvent(domainEventThatAcceptsVisitors);
        }

        protected virtual void ApplyEvent(IAcceptVisitors<TVisitor> domainEvent)
        {
            base.ApplyEvent(domainEvent);
        }
    }
}