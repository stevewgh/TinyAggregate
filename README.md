# TinyAggregate
A Tiny Domain Driven Design (DDD) Aggregate base class and interfaces which are designed to simplify development when using an [event sourcing pattern](https://martinfowler.com/eaaDev/EventSourcing.html).

### Highlights
* No magic! Events are applied to the aggregate using the Visitor pattern
* Auto wiring of event handling by the `Aggregate<TVisitor>` class
* Consistent event handling (applying and replaying events)
* Uncommited events allow unit testing of the aggregate by asserting the events produced by the aggregate
* Version numbers allows easy concurrency checks when integrating with Event stores

## Getting started
```
Install-Package TinyAggregate
```

1. Define an interface that acts as the Visitor between the Events and Aggregate. Create a method for all domain events that your aggregate handles.
```c#
    interface IVehicleVisitor
    {
        void Visit(EngineStarted engineStarted);
    }
```

2. Create your domain events, implement the `IAcceptVisitors<TVisitor>` interface and supply the visitor interface you created  as the generic parameter. Provide the implementation and call the visitor with the object instance (see example).
```c#
    class EngineStarted : IAcceptVisitors<IVehicleVisitor>
    {
        public void Accept(IVehicleVisitor visitor) {
            visitor.Visit(this);
        }
    }
```

3. Create your aggregate, inherit from the `Aggregate<TVisitor>` class and supply the visitor interface you created as the generic parameter. Provide your domain specific operations and any domain events generated should be passed to the `ApplyEvent()` method.
```c#
    class Vehicle : Aggregate<IVehicleVisitor>
    {
        public void StartTheEngine() {
            ApplyEvent(new EngineStarted());
        }
    }
```

4. Testing that it works is really easy. Using xUnit we could do something like this (notice the cast to the base interface to expose the `UncommitedEvents` collection):
```c#
[Fact]
public void Vehicle_Should_Have_Started_When_StartTheEngine_Is_Called() 
{
    // arrange
    var car = new Vehicle();
    // act
    car.StartTheEngine();
    // assert
    ((IAggregate<IVehicleVisitor>)car).UncommitedEvents.OfType<EngineStarted>().Count().Should().Be(1);
}
```
A simpler way of accessing the IAggregate interface is to use the ToAggregate() extension method. Using the 
extension method the assert would look like this:
```c#
    // assert
    car.ToAggregate().UncommitedEvents.OfType<EngineStarted>().Count().Should().Be(1);
```

### Doing something with the events
So now you've got an aggregate, it's tested and working great, but what now? Well this is where your domain will dictate what should happen. Our vehicle aggregate supports starting the engine, but nothing tells us if the engine is running, this is where the visitor interface comes into play again. 

It's important to keep the domain action (`StartTheEngine()`) separate to the application of the events which the action creates. This is so that the events can be applied and replayed without the initial domain action being called again. The way we do that in TinyAggregate is by calling the `ApplyEvent()` method.

#### Free event handling wireup
A nice side effect of implementing the visitor interface on the aggregate is that we are notified when the event should be applied. This wiring up is done in the `Aggregate` class so we don't need to concern ourselves with it. One thing to note, in our `Vehicle` class we have explicitly implemented the `IVehicleVisitor` interface to keep the public interface of the `Vehicle` class clean.

```c#
    class Vehicle : Aggregate<IVehicleVisitor>, IVehicleVisitor
    {
        public bool EngineIsRunning { get; private set; }

        public void StartTheEngine() {
            ApplyEvent(new EngineStarted());
        }

        //  this method will be called when Apply() or Replay() are called with an EngineStarted event
        void IVehicleVisitor.Visit(EngineStarted engineStarted) {
            EngineIsRunning = true;
        }
    }
```

### Putting it all together:
```c#
    static void Main(string[] args)
    {
        var car = new Vehicle();

        Console.WriteLine($"Engine running: {car.EngineIsRunning}");

        car.StartTheEngine();

        Console.WriteLine($"Engine running: {car.EngineIsRunning}");

        Console.ReadLine();
    }
```

### Saving events:
How you choose to save the events is up to you, but getting them is really easy.
```c#
    var car = new Vehicle();
    car.StartTheEngine();
    // save the events somewhere    
    var events = car.ToAggregate().UncommitedEvents;
```

### Replaying events, e.g. loading the aggregate:
Retrieve the events from memory or storage, then call the `Replay` method. You are responsible for telling the aggregate which is the current version, the reason for this is in case the event store has allowed a snapshot to be taken and therefore the number of events being replayed and version number will no longer match.
```c#
    var events = GetEventsFromStore();
    var car = new Vehicle();
    car.ToAggregate().Replay(events.Count, events);
```

### Using an injected visitor
If you prefer to keep the visitor and aggregate completely separate then you need to override the Visitor property and return your own visitor instance. However, as the handling of events and storage of any state will be done in different classes, you will need a mechanism of updating one from the other.

```c#
    class Vehicle : Aggregate<IVehicleVisitor>
    {
        private readonly IVehicleVisitor visitor;

        protected override IVehicleVisitor Visitor { get; }

		//	set is now internal to allow the injected visitor to set it
        public bool EngineIsRunning { get; internal set; }

        public Vehicle(IVehicleVisitor visitor) {
            this.visitor = visitor;
        }

        public void StartTheEngine() {
            ApplyEvent(new EngineStarted());
        }
    }
```