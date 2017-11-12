using TinyAggregate.UnitTests.Aggregates.Payment.Event;

namespace TinyAggregate.UnitTests.Aggregates.Payment
{
    public class Payment : Aggregate<IPaymentVisitor>, IPaymentVisitor
    {
        internal decimal Amount { get; set; }

        internal string Currency { get; set; }

        public void TakePayment(decimal amount, string currency)
        {
            var domainEvent = new PaymentTaken { Amount = amount, Currency = currency };
            ApplyEvent(domainEvent);
        }

        void IPaymentVisitor.Accept(PaymentTaken paymentTaken)
        {
            Amount = paymentTaken.Amount;
            Currency = paymentTaken.Currency;
        }
    }
}