namespace Aggregate.UnitTests.Widget.Event
{
    public class WidgetDomainEvent : IAcceptVisitors<IWidgetVisitor>
    {
        public string DomainProperty { get; set; }

        public void Accept(IWidgetVisitor visitor)
        {
            visitor.Apply(this);
        }
    }
}