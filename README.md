# Aggregate
Domain Driven Design (DDD) Aggregate

Aggregate designed to simplify development when using an [event sourcing pattern](https://martinfowler.com/eaaDev/EventSourcing.html).

* Visitor pattern to apply Domain Events to the Aggregate
* Consistent event handling (initial and replay)
* Uncommited events allow unit testing of the aggregate by asserting the events produced by the aggregate
* Version numbers allows easy concurrency checks when integrating with Event stores

Example use (Aggregate and Visitor in a single class)

```c#
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

    public class PaymentAggregate : Aggregate<IPaymentVisitor>, IPaymentVisitor
    {
        private decimal Amount { get; set; }

        private string Currency { get; set; }

        public PaymentAggregate(string streamId) : base(streamId) { }

        protected override void ApplyEvent(IAcceptVisitors<IPaymentVisitor> domainEvent)
        {
            base.ApplyEvent(domainEvent);
            domainEvent.Accept(this);
        }

        public void Accept(PaymentTaken paymentTaken)
        {
            Amount = paymentTaken.Amount;
            Currency = paymentTaken.Currency;
        }
    }
```

Using the example PaymentAggregate aggregate shown above, we can replay events like this:
```c#
    IAggregate<IPaymentVisitor> aggregate = new PaymentAggregate("stream-id-123");
    aggregate.Replay(1, new []{ new PaymentTaken{ Amount = 100.00m, Currency  = "USD" } });
```
