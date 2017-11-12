namespace TinyAggregate.UnitTests.Aggregates.Transport.Event
{
    public class EngineStarted : IAcceptVisitors<IVehicleVisitor>
    {
        public void Accept(IVehicleVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
