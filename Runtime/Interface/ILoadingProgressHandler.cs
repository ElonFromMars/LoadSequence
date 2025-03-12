namespace LoadSequence.Runtime.Interface
{
    public interface ILoadingProgressHandler
    {
        public void SetTotalProgressWeight(float totalProgressValue);

        public void SetCurrentProgressWeight(float currentProgressValue);
    }
}