using System;
using System.Linq;
using Aggregate.UnitTests.Widget;
using Aggregate.UnitTests.Widget.Event;
using FluentAssertions;
using Xunit;

namespace Aggregate.UnitTests
{
    public class AggregateShould
    {
        private const string StreamId = "123";

        [Fact]
        public void Have_A_LoadedAt_Version_Equal_To_Zero_When_Creating_A_New_Aggregate()
        {
            IAggregate sut = new WidgetAggregate(StreamId);

            sut.LoadedAtVersion.Should().Be(0);
        }

        [Fact]
        public void Have_A_Current_Version_Equal_To_Zero_When_Creating_A_New_Aggregate()
        {
            IAggregate sut = new WidgetAggregate(StreamId);

            sut.CurrentVersion.Should().Be(0);
        }

        [Fact]
        public void Have_A_Current_Version_Equal_To_One_When_Creating_A_New_Event()
        {
            var sut = new WidgetAggregate(StreamId);

            sut.DoSomethingThatCreatesADomainEvent(Guid.NewGuid());

            ((IAggregate)sut).CurrentVersion.Should().Be(1);
            ((IAggregate) sut).UncommitedEvents.Count().Should().Be(1);
        }

        [Fact]
        public void Have_A_LoadedAt_Version_Equal_To_Zero_After_Creating_A_New_Event()
        {
            var sut = new WidgetAggregate(StreamId);

            sut.DoSomethingThatCreatesADomainEvent(Guid.NewGuid());

            ((IAggregate)sut).LoadedAtVersion.Should().Be(0);
        }

        [Fact]
        public void Apply_Domain_Events_To_Itself()
        {
            var sut = new WidgetAggregate(StreamId);

            var newGuid = Guid.NewGuid();
            sut.DoSomethingThatCreatesADomainEvent(newGuid);

            sut.LatestDomainProperty.Should().Be(newGuid.ToString());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        public void Set_The_LoadedAt_Property_After_Replaying_Events(int loadedAtVersion)
        {
            var newGuid = Guid.NewGuid();
            var domainEvents = new[]
            {
                new WidgetDomainEvent { DomainProperty = newGuid.ToString() }
            };

            IAggregate sut = new WidgetAggregate(StreamId);
            sut.Replay(loadedAtVersion, domainEvents);

            sut.LoadedAtVersion.Should().Be(loadedAtVersion);
        }

        [Fact]
        public void Replay_Domain_Events_To_The_Aggregate()
        {
            var newGuid = Guid.NewGuid();
            var domainEvents = new[]
            {
                new WidgetDomainEvent { DomainProperty = newGuid.ToString() }
            };

            var sut = new WidgetAggregate(StreamId);
            ((IAggregate)sut).Replay(1, domainEvents);

            sut.LatestDomainProperty.Should().Be(newGuid.ToString());
        }

        [Fact]
        public void Contain_Zero_Uncommited_Events_After_Replaying_Domain_Events()
        {
            var newGuid = Guid.NewGuid();
            var domainEvents = new[]
            {
                new WidgetDomainEvent { DomainProperty = newGuid.ToString() }
            };

            IAggregate sut = new WidgetAggregate(StreamId);
            sut.Replay(1, domainEvents);

            sut.UncommitedEvents.Count().Should().Be(0);
        }
    }
}