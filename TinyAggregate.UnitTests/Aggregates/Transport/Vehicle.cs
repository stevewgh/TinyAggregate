using System;
using TinyAggregate.UnitTests.Aggregates.Transport.Event;

namespace TinyAggregate.UnitTests.Aggregates.Transport
{
    public class Vehicle : Aggregate<IVehicleVisitor>
    {
        public Vehicle(IVehicleVisitor visitor)
        {
            this.Visitor = visitor ?? throw new ArgumentNullException(nameof(visitor));
        }

        public void StartTheEngine()
        {
            ApplyEvent(new EngineStarted());
        }

        /// <summary>
        /// Added to assist unit tests
        /// </summary>
        public void ApplyEventForUnitTests(IAcceptVisitors<IVehicleVisitor> domainEvent)
        {
            ApplyEvent(domainEvent);
        }

        protected override IVehicleVisitor Visitor { get; }
    }
}