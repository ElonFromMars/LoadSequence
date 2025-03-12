namespace LoadSequence.Runtime.Interface
{
    public interface IPerformanceCalculator
    {
        public void StartProfile(string metricPointName);
        public void StopProfile(string metricPointName);

        public void SendPerformanceData();
    }
}