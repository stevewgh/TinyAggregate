namespace TinyAggregate.UnitTests.Aggregates.Payment.Event;

public class PaymentTaken : IEventUnion
{
    public decimal Amount { get; set; }
    public string Currency { get; set; }
}