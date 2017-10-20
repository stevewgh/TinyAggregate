# Aggregate
Tiny Domain Driven Design (DDD) Aggregate

Aggregate designed to simplify development when using an [event sourcing pattern](https://martinfowler.com/eaaDev/EventSourcing.html).

* Visitor pattern to apply Domain Events to the Aggregate
* Consistent event handling (initial and replay)
* Uncommited events allow unit testing of the aggregate by asserting the events produced by the aggregate
* Version numbers allows easy concurrency checks when integrating with Event stores

NuGet
```
Install-Package TinyAggregate
```

Example
```c#
    public class PaymentAggregate : TinyAggregate.Aggregate<IPaymentVisitor>, IPaymentVisitor
    {
        private decimal Amount { get; set; }

        private string Currency { get; set; }

        public void TakePayment(decimal amount, string currency)
        {
            var domainEvent = new PaymentTaken { Amount = amount, Currency = currency };
            ApplyEvent(domainEvent);
        }
        
	// Implmentation of the IPaymentVisitor interface
        public void Accept(PaymentTaken paymentTaken)
        {
            Amount = paymentTaken.Amount;
            Currency = paymentTaken.Currency;
        }
    }

    // Event which is created by the TakePayment method
    public class PaymentTaken : IAcceptVisitors<IPaymentVisitor>
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public void Accept(IPaymentVisitor visitor)
        {
            visitor.Accept(this);
        }
    }

    public interface IPaymentVisitor
    {
        void Accept(PaymentTaken paymentTaken);
    }

```

Replaying events, e.g. loading the aggregate:
```c#
    IAggregate<IPaymentVisitor> aggregate = new PaymentAggregate("stream-id-123");
    aggregate.Replay(1, new []{ new PaymentTaken{ Amount = 100.00m, Currency  = "USD" } });
```

Testing (using FluentAssertions):
```c#
    var aggregate = new PaymentAggregate("stream-id-123");

    aggregate.TakePayment(100.00m, "USD");

    ((IAggregate<IPaymentVisitor>) aggregate).UncommitedEvents.OfType<PaymentTaken>().Count().Should().Be(1);
```
