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

4. Testing that it works is really easy. Using xUnit we could do something like this (notice the cast to the base interface to expose the UncommitedEvents collection):
```c#
[Fact]
public void Vehicle_Should_Start_When_StartTheEngine_Is_Called() 
{
    // arrange
    var car = new Vehicle();
    // act
    car.StartTheEngine();
    // assert
    ((IAggregate<IVehicleVisitor>)car).UncommitedEvents.OfType<EngineStarted>().Count().Should().Be(1);
}
```

### Doing something with the events
So now you've got an aggregate and events and it's all tested and working great, what now? Well this is where your domain will dictate what should happen. Our vehicle aggregate supports starting the engine, but nothing tells us the running state, this is where the visitor interface comes into play again. 

By implementing the visitor interface on the aggregate we can be notified of when the event should be applied. In our `Vehicle` aggregate we have explicitly implemented the `IVehicleVisitor` to keep the public interface of the aggregate clean.

```c#
    class Vehicle : Aggregate<IVehicleVisitor>, IVehicleVisitor
    {
        public bool EngineIsRunning { get; private set; }

        public void StartTheEngine() {
            ApplyEvent(new EngineStarted());
        }

        void IVehicleVisitor.Visit(EngineStarted engineStarted) {
            EngineIsRunning = true;
        }
    }
```

## Replaying events, e.g. loading the aggregate:
```c#
    var events = GetEventsFromStore();
    var car = new Vehicle();
    ((IAggregate<IVehicleVisitor>)car).Replay(events.Count, events});
```
