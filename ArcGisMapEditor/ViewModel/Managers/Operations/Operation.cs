namespace ArcGisMapEditor.ViewModel.Managers.Operations
{
    public abstract class Operation
    {
        public OperationType OperationType { get; protected set; }

        public virtual void Finish()
        {
        }
    }
}