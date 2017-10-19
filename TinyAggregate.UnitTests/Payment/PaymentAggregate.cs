using TinyAggregate.UnitTests.Payment.Event;

namespace TinyAggregate.UnitTests.Payment
{
    public class PaymentAggregate : Aggregate<IPaymentVisitor>, IPaymentVisitor
    {
        internal decimal Amount { get; set; }

        internal string Currency { get; set; }

        public PaymentAggregate(string streamId) : base(streamId) { }

        protected override void ApplyEvent(IAcceptVisitors<IPaymentVisitor> domainEvent)
        {
            base.ApplyEvent(domainEvent);
            domainEvent.Accept(this);
        }

        public void TakePayment(decimal amount, string currency)
        {
            //  TODO: validation and taking payment
            var domainEvent = new PaymentTaken { Amount = amount, Currency = currency };
            ApplyEvent(domainEvent);
        }

        public void Accept(PaymentTaken paymentTaken)
        {
            Amount = paymentTaken.Amount;
            Currency = paymentTaken.Currency;
        }
    }
}