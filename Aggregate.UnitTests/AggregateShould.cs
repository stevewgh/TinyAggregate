using System.Linq;
using Aggregate.UnitTests.Widget;
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

            sut.CreateIntent();

            ((IAggregate)sut).CurrentVersion.Should().Be(1);
            ((IAggregate) sut).UncommitedEvents.Count().Should().Be(1);
        }

        [Fact]
        public void Have_A_LoadedAt_Version_Equal_To_Zero_After_Creating_A_New_Event()
        {
            var sut = new WidgetAggregate(StreamId);

            sut.CreateIntent();

            ((IAggregate)sut).LoadedAtVersion.Should().Be(0);
        }

    }
}
