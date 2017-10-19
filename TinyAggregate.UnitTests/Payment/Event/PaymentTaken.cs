namespace TinyAggregate.UnitTests.Payment.Event
{
    public class PaymentTaken : IAcceptVisitors<IPaymentVisitor>
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public void Accept(IPaymentVisitor visitor)
        {
            visitor.Accept(this);
        }
    }
}