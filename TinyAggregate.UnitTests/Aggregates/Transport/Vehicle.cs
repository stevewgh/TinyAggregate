using System;
using TinyAggregate.UnitTests.Aggregates.Transport.Event;

namespace TinyAggregate.UnitTests.Aggregates.Transport;

public class Vehicle : Aggregate<IEventUnion>
{
    public void StartTheEngine()
    {
        ApplyEvent(new EngineStarted());
    }

    /// <summary>
    /// Added to assist unit tests
    /// </summary>
    public void ApplyEventForUnitTests(IEventUnion domainEvent)
    {
        ApplyEvent(domainEvent);
    }

    protected override void RouteEvent(IEventUnion domainEvent)
    {
        ApplyEvent(domainEvent);
    }
}