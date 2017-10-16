using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aggregate.UnitTests.Widget
{
    public class WidgetRepository : IAggregateEventRepository
    {
        private readonly List<IAcceptVisitors<IWidgetVisitor>> eventSource;

        public WidgetRepository(List<IAcceptVisitors<IWidgetVisitor>> eventSource)
        {
            this.eventSource = eventSource;
        }

        public Task<IAggregate> Load(string streamId)
        {
            IAggregate aggregate = new WidgetAggregate(streamId);
            aggregate.Replay(eventSource);
            return Task.FromResult(aggregate);
        }

        public Task Save(IAggregate aggregate)
        {
            if (aggregate.LoadedAtVersion != eventSource.Count)
            {
                throw new Exception("The event source has changed since loading.");
            }

            eventSource.AddRange(aggregate.UncommitedEvents.OfType<IAcceptVisitors<IWidgetVisitor>>());

            return Task.CompletedTask;
        }
    }
}