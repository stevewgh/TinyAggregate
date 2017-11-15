using System;
using System.Collections.Generic;

namespace TinyAggregate
{
    public abstract class Aggregate<TVisitor> : IAggregate<TVisitor> where TVisitor : class
    {
        private readonly List<IAcceptVisitors<TVisitor>> uncommitedEvents = new List<IAcceptVisitors<TVisitor>>();
        private int loadedAtVersion;

        int IAggregate<TVisitor>.LoadedAtVersion => loadedAtVersion;

        IEnumerable<IAcceptVisitors<TVisitor>> IAggregate<TVisitor>.UncommitedEvents => uncommitedEvents;

        protected virtual TVisitor Visitor => this as TVisitor ?? throw new InvalidOperationException($"{nameof(Visitor)} was about to return null. If your aggregate doesn't implement the TVisitor inerface then you must override {nameof(Visitor)} and return the TVisitor instance manually.");

        void IAggregate<TVisitor>.Replay(int loadedVersion, IEnumerable<IAcceptVisitors<TVisitor>> events)
        {
            var visitor = Visitor;
            foreach (var domainEvent in events)
            {
                ApplyEvent(domainEvent, visitor);
            }
            loadedAtVersion = loadedVersion;
        }

        protected void ApplyEvent(IAcceptVisitors<TVisitor> domainEvent)
        {
            uncommitedEvents.Add(domainEvent);
            ApplyEvent(domainEvent, Visitor);
        }

        void IAggregate<TVisitor>.ClearUncommitedEvents()
        {
            uncommitedEvents.Clear();
        }

        private void ApplyEvent(IAcceptVisitors<TVisitor> domainEvent, TVisitor visitor)
        {
            if (domainEvent == null) throw new ArgumentNullException(nameof(domainEvent));
            if (visitor == null) throw new ArgumentNullException(nameof(visitor));
            domainEvent.Accept(visitor);
        }
    }
}