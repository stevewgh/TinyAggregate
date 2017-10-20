using TinyAggregate.UnitTests.Payment.Event;

namespace TinyAggregate.UnitTests.Payment
{
    public class PaymentAggregate : Aggregate<IPaymentVisitor>, IPaymentVisitor
    {
        internal decimal Amount { get; set; }

        internal string Currency { get; set; }

        public void TakePayment(decimal amount, string currency)
        {
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