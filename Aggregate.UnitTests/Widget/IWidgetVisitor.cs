using Aggregate.UnitTests.Widget.Event;

namespace Aggregate.UnitTests.Widget
{
    public interface IWidgetVisitor
    {
        void Apply(WidgetDomainEvent domainEvent);
    }
}
