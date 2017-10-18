using System;
using Aggregate.UnitTests.Widget.Event;

namespace Aggregate.UnitTests.Widget
{
    public class WidgetAggregate : Aggregate<IWidgetVisitor>, IWidgetVisitor
    {
        public string LatestDomainProperty { get; private set; }

        public WidgetAggregate(string streamId) : base(streamId) { }

        public void DoSomethingThatCreatesADomainEvent(Guid guid)
        {
            ApplyEvent(new WidgetDomainEvent { DomainProperty = guid.ToString() });
        }

        protected override void ApplyEvent(IAcceptVisitors<IWidgetVisitor> paymentIntent)
        {
            base.ApplyEvent(paymentIntent);
            paymentIntent.Accept(this);
        }

        void IWidgetVisitor.Apply(WidgetDomainEvent domainEvent)
        {
            LatestDomainProperty = domainEvent.DomainProperty;
        }
    }
}