using System.Threading;
using Cysharp.Threading.Tasks;
using LoadSequence.Runtime.Interface;

namespace LoadSequence.Runtime
{
    public abstract class BaseSequenceStep : ILoadingStep
    {
        protected int LoadingWeight;
        protected IGameSystems GameSystems;

        public LoadStepType LoadStepType { get; private set; }

        protected BaseSequenceStep(IGameSystems gameSystems)
        {
            GameSystems = gameSystems;
        }

        public virtual void Init(LoadStepType loadStepType)
        {
            LoadStepType = loadStepType;
        }

        public virtual float GetWeight()
        {
            return LoadingWeight;
        }
        
        public async UniTask Start(IPerformanceCalculator performanceCalculator,CancellationToken cancellationToken)
        {
            performanceCalculator.StartProfile(metricPointName:GetType().Name);
            await StartInternal();
            performanceCalculator.StopProfile(metricPointName:GetType().Name);
        }
        
        protected abstract UniTask StartInternal();
    }
}