namespace Aggregate
{
    public abstract class AcceptVisitorsBase<TVisitor> : IAcceptVisitors<TVisitor> where TVisitor : class
    {
        public void Accept(object visitor)
        {
            Accept((TVisitor)visitor);
        }

        public abstract void Accept(TVisitor visitor);
    }
}