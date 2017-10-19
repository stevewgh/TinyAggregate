# Aggregate
Domain Driven Design (DDD) Aggregate

Aggregate designed to simplify development when using an [event sourcing pattern](https://martinfowler.com/eaaDev/EventSourcing.html).

* Actor pattern to apply Domain Events to the Aggregate
* Consistent event handling (first time and replaying)
* Uncommited events allow unit testing of the aggregate
* Version numbers allows easy concurrency checks when integrating with Event stores

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
