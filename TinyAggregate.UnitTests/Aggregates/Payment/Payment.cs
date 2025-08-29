using System.Threading.Tasks;
using TinyAggregate.UnitTests.Aggregates.Payment.Event;

namespace TinyAggregate.UnitTests.Aggregates.Payment;

public class Payment : Aggregate<PaymentTaken>
{
    internal decimal Amount { get; set; }

    internal string Currency { get; set; }

    public void TakePayment(decimal amount, string currency)
    {
        var domainEvent = new PaymentTaken { Amount = amount, Currency = currency };
        ApplyEvent(domainEvent);
    }

    private void Apply(PaymentTaken paymentTaken)
    {
        Amount = paymentTaken.Amount;
        Currency = paymentTaken.Currency;
    }

    protected override void RouteEvent(PaymentTaken domainEvent)
    {
        Apply(domainEvent);
    }
}