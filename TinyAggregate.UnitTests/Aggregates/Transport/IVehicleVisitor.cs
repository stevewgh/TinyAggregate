using TinyAggregate.UnitTests.Aggregates.Transport.Event;

namespace TinyAggregate.UnitTests.Aggregates.Transport
{
    public interface IVehicleVisitor
    {
        void Visit(EngineStarted engineStarted);
    }
}