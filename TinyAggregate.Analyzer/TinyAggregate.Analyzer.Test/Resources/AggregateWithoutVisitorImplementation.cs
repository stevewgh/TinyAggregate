namespace TinyAggregate.Analyzer.Test.Resources
{
    public interface IVisitor
    {
        void Visit();
    }

    public class AggregateWithoutVisitorImplementation : Aggregate<IVisitor>
    {
    }
}