using System.Linq;
using System.Reflection.Metadata;
using FluentAssertions;
using Moq;
using TinyAggregate.UnitTests.Aggregates.Payment;
using TinyAggregate.UnitTests.Aggregates.Payment.Event;
using TinyAggregate.UnitTests.Aggregates.Transport;
using TinyAggregate.UnitTests.Aggregates.Transport.Event;
using Xunit;

namespace TinyAggregate.UnitTests
{
    public class AggregateShould
    {
        private const decimal Amount = 100.00m;
        private const string Currency = "USD";

        [Fact]
        public void Use_The_Visitor_When_Applying_Events()
        {
            var visitor = new Mock<IVehicleVisitor>();
            var sut = new Vehicle(visitor.Object);

            sut.StartTheEngine();

            visitor.Verify(v => v.Visit(It.IsAny<EngineStarted>()), Times.Once);
        }

        [Fact]
        public void Have_Uncommited_Events_After_Applying_Them()
        {
            var sut = new Payment();
            sut.TakePayment(Amount, Currency);
            ((IAggregate<IPaymentVisitor>)sut).UncommitedEvents.Count().Should().NotBe(0);
        }

        [Fact]
        public void Have_No_Uncommited_Events_After_Clearing_Them()
        {
            var sut = new Payment();
            sut.TakePayment(Amount, Currency);

            ((IAggregate<IPaymentVisitor>)sut).ClearUncommitedEvents();

            ((IAggregate<IPaymentVisitor>)sut).UncommitedEvents.Count().Should().Be(0);
        }

        [Fact]
        public void Have_A_LoadedAt_Version_Equal_To_Zero_When_Creating_A_New_Aggregate()
        {
            IAggregate<IPaymentVisitor> sut = new Payment();

            sut.LoadedAtVersion.Should().Be(0);
        }

        [Fact]
        public void Have_A_LoadedAt_Version_Equal_To_Zero_After_Creating_A_New_Event()
        {
            var sut = new Payment();

            sut.TakePayment(100.00m, "USD");

            ((IAggregate<IPaymentVisitor>)sut).LoadedAtVersion.Should().Be(0);
        }

        [Fact]
        public void Apply_Domain_Events_To_Itself()
        {
            var sut = new Payment();

            sut.TakePayment(Amount, Currency);

            sut.Amount.Should().Be(Amount);
            sut.Currency.Should().Be(Currency);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        public void Set_The_LoadedAt_Property_After_Replaying_Events(int loadedAtVersion)
        {
            var domainEvents = new[]
            {
                new PaymentTaken { Amount = Amount, Currency = Currency } 
            };

            IAggregate<IPaymentVisitor> sut = new Payment();
            sut.Replay(loadedAtVersion, domainEvents);

            sut.LoadedAtVersion.Should().Be(loadedAtVersion);
        }

        [Fact]
        public void Replay_Domain_Events_To_The_Aggregate()
        {
            var domainEvents = new[]
            {
                new PaymentTaken { Amount = Amount, Currency = Currency }
            };

            var sut = new Payment();
            ((IAggregate<IPaymentVisitor>)sut).Replay(1, domainEvents);

            sut.Amount.Should().Be(Amount);
            sut.Currency.Should().Be(Currency);
        }

        [Fact]
        public void Contain_Zero_Uncommited_Events_After_Replaying_Domain_Events()
        {
            var domainEvents = new[]
            {
                new PaymentTaken { Amount = Amount, Currency = Currency }
            };

            IAggregate<IPaymentVisitor> sut = new Payment();
            sut.Replay(1, domainEvents);

            sut.UncommitedEvents.Count().Should().Be(0);
        }
    }
}