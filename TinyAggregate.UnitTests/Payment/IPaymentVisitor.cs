using TinyAggregate.UnitTests.Payment.Event;

namespace TinyAggregate.UnitTests.Payment
{
    public interface IPaymentVisitor
    {
        void Accept(PaymentTaken paymentTaken);
    }
}
