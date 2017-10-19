namespace Aggregate
{

    public interface IAcceptVisitors<in TVisitor> where TVisitor : class
    {
        void Accept(TVisitor visitor);
    }
}