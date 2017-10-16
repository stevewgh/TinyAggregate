using System.Collections.Generic;

namespace Aggregate
{
    public abstract class Aggregate<TAcceptVisitor> : IAggregate
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
                ApplyEvent((TAcceptVisitor) domainEvent);
            }
            loadedAtVersion = uncommitedEvents.Count;
            uncommitedEvents.Clear();
        }

        protected virtual void ApplyEvent(TAcceptVisitor domainEvent)
        {
            currentVersion++;
            uncommitedEvents.Add(domainEvent);
        }

        void IAggregate.ClearUncommitedEvents()
        {
            uncommitedEvents.Clear();
        }

    }
}