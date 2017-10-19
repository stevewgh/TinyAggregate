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

Example use (Aggregate and Visitor as separate implementations)
```c#
    public class PaymentTaken : IAcceptVisitors<PaymentVisitor>
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public void Accept(PaymentVisitor visitor)
        {
            visitor.Accept(this);
        }
    }
    
    public class PaymentVisitor
    {
        private readonly PaymentAggregate aggregate;

        public PaymentVisitor(PaymentAggregate aggregate)
        {
            this.aggregate = aggregate;
        }

        public void Accept(PaymentTaken paymentTaken)
        {
            aggregate.Amount = paymentTaken.Amount;
            aggregate.Currency = paymentTaken.Currency;
        }
    }
    
    public class PaymentAggregate : Aggregate<PaymentVisitor>
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }

        public PaymentAggregate(string streamId) : base(streamId) { }

        protected override void ApplyEvent(IAcceptVisitors<PaymentVisitor> domainEvent)
        {
            base.ApplyEvent(domainEvent);

            var visitor = new PaymentVisitor(this);
            domainEvent.Accept(visitor);
        }
    }
```
