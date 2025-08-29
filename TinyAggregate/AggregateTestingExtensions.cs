namespace TinyAggregate
{
    public static class AggregateTestingExtensions
    {
        public static IAggregate<TVisitor> ToAggregate<TVisitor>(this IAggregate<TVisitor> aggregate) where TVisitor : class
        {
            return aggregate;
        }
    }
}