# TinyAggregate
A Tiny Domain Driven Design (DDD) Aggregate

Aggregate designed to simplify development when using an [event sourcing pattern](https://martinfowler.com/eaaDev/EventSourcing.html).

* Visitor pattern to apply Domain Events to the Aggregate
* Consistent event handling (applying them and replaying when loading an aggregate)
* Uncommited events allow unit testing of the aggregate by asserting the events produced by the aggregate
* Version numbers allows easy concurrency checks when integrating with Event stores

Get it from NuGet
```
Install-Package TinyAggregate
```

## Getting started

1. Define an interface that acts as the Visitor between the Events and Aggregate. Create a method for all domain events that your aggregate handles.
```c#
    interface IVehicleVisitor
    {
        void Visit(EngineStarted engineStarted);
    }
```

2. Create your domain events, implement the IAcceptVisitors interface and supply the visitor interface you created (`IVehicleVisitor` in this case). as the generic parameter. Provide the implementation and call the visitor with the object instance (see example).
```c#
    class EngineStarted : IAcceptVisitors<IVehicleVisitor>
    {
        public void Accept(IVehicleVisitor visitor) {
            visitor.Visit(this);
        }
    }
```

3. Create your aggregate, either inherit from the Aggregate class and supply the visitor interface you created (`IVehicleVisitor` in this case) as the generic parameter. Provide domain specific operations (StartTheEngine in this case) and any domain events generated should be passed to the ApplyEvent method.
```c#
    class Vehicle : Aggregate<IVehicleVisitor>
    {
        public void StartTheEngine() {
            ApplyEvent(new EngineStarted());
        }
    }
```

## Replaying events, e.g. loading the aggregate:
```c#
    var events = GetEventsFromStore();
    var car = new Vehicle();
    ((IAggregate<IVehicleVisitor>)car).Replay(events.Count, events});
```

## Testing (using FluentAssertions):
```c#
    var car = new Vehicle();
    car.StartTheEngine();
    ((IAggregate<IVehicleVisitor>)car).UncommitedEvents.OfType<EngineStarted>().Count().Should().Be(1);
```
