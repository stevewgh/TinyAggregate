using System;
using System.Linq;
using FluentAssertions;
using TinyAggregate.UnitTests.Payment;
using TinyAggregate.UnitTests.Payment.Event;
using Xunit;

namespace TinyAggregate.UnitTests
{
    public class TinyAggregateShould
    {
        private const decimal Amount = 100.00m;
        private const string Currency = "USD";

        [Fact]
        public void Have_A_LoadedAt_Version_Equal_To_Zero_When_Creating_A_New_Aggregate()
        {
            IAggregate<IPaymentVisitor> sut = new PaymentAggregate();

            sut.LoadedAtVersion.Should().Be(0);
        }

        [Fact]
        public void Have_A_Current_Version_Equal_To_Zero_When_Creating_A_New_Aggregate()
        {
            IAggregate<IPaymentVisitor> sut = new PaymentAggregate();

            sut.CurrentVersion.Should().Be(0);
        }

        [Fact]
        public void Have_A_Current_Version_Equal_To_One_When_Creating_A_New_Event()
        {
            var sut = new PaymentAggregate();

            sut.TakePayment(100.00m, "USD");

            ((IAggregate<IPaymentVisitor>)sut).CurrentVersion.Should().Be(1);
            ((IAggregate<IPaymentVisitor>) sut).UncommitedEvents.Count().Should().Be(1);
        }

        [Fact]
        public void Have_A_LoadedAt_Version_Equal_To_Zero_After_Creating_A_New_Event()
        {
            var sut = new PaymentAggregate();

            sut.TakePayment(100.00m, "USD");

            ((IAggregate<IPaymentVisitor>)sut).LoadedAtVersion.Should().Be(0);
        }

        [Fact]
        public void Apply_Domain_Events_To_Itself()
        {
            var sut = new PaymentAggregate();

            sut.TakePayment(Amount, Currency);

            sut.Amount.Should().Be(Amount);
            sut.Currency.Should().Be(Currency);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        public void Set_The_LoadedAt_Property_After_Replaying_Events(int loadedAtVersion)
        {
            var newGuid = Guid.NewGuid();
            var domainEvents = new[]
            {
                new PaymentTaken { Amount = Amount, Currency = Currency } 
            };

            IAggregate<IPaymentVisitor> sut = new PaymentAggregate();
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

            var sut = new PaymentAggregate();
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

            IAggregate<IPaymentVisitor> sut = new PaymentAggregate();
            sut.Replay(1, domainEvents);

            sut.UncommitedEvents.Count().Should().Be(0);
        }
    }
}