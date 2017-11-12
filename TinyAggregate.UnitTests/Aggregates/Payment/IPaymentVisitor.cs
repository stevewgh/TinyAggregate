using TinyAggregate.UnitTests.Aggregates.Payment.Event;

namespace TinyAggregate.UnitTests.Aggregates.Payment
{
    public interface IPaymentVisitor
    {
        void Accept(PaymentTaken paymentTaken);
    }
}
