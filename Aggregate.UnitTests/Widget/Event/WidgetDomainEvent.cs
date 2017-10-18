namespace Aggregate.UnitTests.Widget.Event
{
    public class WidgetDomainEvent : AcceptVisitorsBase<IWidgetVisitor>
    {
        public string DomainProperty { get; set; }

        public override void Accept(IWidgetVisitor visitor)
        {
            visitor.Apply(this);
        }
    }
}