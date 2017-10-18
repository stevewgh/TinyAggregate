using System.Collections.Generic;

namespace Aggregate
{
    public abstract class Aggregate : IAggregate
    {
        private readonly List<object> uncommitedEvents = new List<object>();
        private int currentVersion;
        private int loadedAtVersion;
        private readonly string streamId;

        protected Aggregate(string streamId)
        {
            this.streamId = streamId;
        }

        int IAggregate.CurrentVersion => currentVersion;

        int IAggregate.LoadedAtVersion => loadedAtVersion;

        IEnumerable<object> IAggregate.UncommitedEvents => uncommitedEvents;

        string IAggregate.StreamId => streamId;

        void IAggregate.Replay(IEnumerable<object> events)
        {
            foreach (var domainEvent in events)
            {
                ApplyEvent(domainEvent);
            }
            loadedAtVersion = uncommitedEvents.Count;
            uncommitedEvents.Clear();
        }

        protected virtual void ApplyEvent(object domainEvent)
        {
            currentVersion++;
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