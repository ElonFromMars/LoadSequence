namespace LoadSequence.Runtime.Interface
{
    public interface IFactory<in TParam, TValue>
    {
        public bool Create(TParam value1, out TValue value2);
    }
}