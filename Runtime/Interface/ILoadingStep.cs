using System.Threading;
using Cysharp.Threading.Tasks;

namespace LoadSequence.Runtime.Interface
{
    public interface ILoadingStep
    {
        public LoadStepType LoadStepType { get; }
        
        public void Init(LoadStepType loadStepType);
        public float GetWeight();
        public UniTask Start(IPerformanceCalculator performanceCalculator,CancellationToken cancellationToken);
    }
}