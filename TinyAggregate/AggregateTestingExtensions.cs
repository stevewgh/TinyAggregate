namespace TinyAggregate
{
    public static class AggregateTestingExtensions
    {
        public static IAggregate<TEventUnion> ToAggregate<TEventUnion>(this IAggregate<TEventUnion> aggregate) where TEventUnion : IEventUnion
        {
            return aggregate;
        }
    }
}