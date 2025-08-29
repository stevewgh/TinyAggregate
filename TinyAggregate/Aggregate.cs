using System;
using System.Collections.Generic;

namespace TinyAggregate;

public abstract class Aggregate<TEventUnion> : IAggregate<TEventUnion> where TEventUnion : IEventUnion
{
    private readonly List<TEventUnion> _uncommitedEvents = [];
    private int _loadedAtVersion;

    int IAggregate<TEventUnion>.LoadedAtVersion => _loadedAtVersion;

    IEnumerable<TEventUnion> IAggregate<TEventUnion>.UncommitedEvents => _uncommitedEvents;

    void IAggregate<TEventUnion>.Replay(int loadedVersion, IEnumerable<TEventUnion> events)
    {
        foreach (var domainEvent in events)
        {
            RouteEvent(domainEvent);
        }
        _loadedAtVersion = loadedVersion;
    }

    protected void ApplyEvent(TEventUnion domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent, nameof(domainEvent));
        _uncommitedEvents.Add(domainEvent);
        RouteEvent(domainEvent);
    }

    void IAggregate<TEventUnion>.ClearUncommitedEvents()
    {
        _uncommitedEvents.Clear();
    }
    
    protected abstract void RouteEvent(TEventUnion domainEvent);
}