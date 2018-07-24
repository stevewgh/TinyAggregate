namespace TinyAggregate.Analyzer.Test.Resources
{
    public interface IVisitor
    {
        void Visit();
    }

    public abstract class AggregateBase : Aggregate<IVisitor>
    {

    }

    public class AggregateWithoutVisitorImplementation : AggregateBase, IVisitor
    {
        public void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}