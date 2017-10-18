namespace Aggregate
{

    public interface IAcceptVisitors
    {
        void Accept(object visitor);
    }

    public interface IAcceptVisitors<in TVisitor> : IAcceptVisitors where TVisitor : class
    {
        void Accept(TVisitor visitor);
    }
}