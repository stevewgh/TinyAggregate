using System;
using TinyAggregate.UnitTests.Aggregates.Transport.Event;

namespace TinyAggregate.UnitTests.Aggregates.Transport
{
    public class Vehicle : Aggregate<IVehicleVisitor>
    {
        private readonly IVehicleVisitor visitor;

        public Vehicle(IVehicleVisitor visitor)
        {
            this.visitor = visitor ?? throw new ArgumentNullException(nameof(visitor));
        }

        public void StartTheEngine()
        {
            ApplyEvent(new EngineStarted());
        }

        protected override IVehicleVisitor GetVisitor()
        {
            return visitor;
        }
    }
}