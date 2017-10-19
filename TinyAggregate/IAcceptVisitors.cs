namespace TinyAggregate
{
    public interface IAcceptVisitors<in TVisitor>
    {
        void Accept(TVisitor visitor);
    }
}