using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using LoadSequence.Runtime.Interface;

namespace LoadSequence.Runtime
{
    public class LoadSequencePlayer : IDisposable
    {
        private readonly IPerformanceCalculator _performanceCalculator;
        private readonly Queue<ILoadingStep> _loadingStepsType;
        private readonly List<UniTask> _parallelStartedLoadingStep = new List<UniTask>();
        
        public LoadSequencePlayer(Queue<ILoadingStep> loadingStepsType, IPerformanceCalculator performanceCalculator)
        {
            _loadingStepsType = loadingStepsType;
            _performanceCalculator = performanceCalculator;
        }
        
        public async UniTask Play(CancellationToken cancellationToken)
        {
            await PlayInternal(cancellationToken);
        }

        private async Task PlayInternal(CancellationToken cancellationToken)
        {
            while (_loadingStepsType.Count > 0)
            {
                ILoadingStep loadingStep = _loadingStepsType.Peek();
                if (loadingStep.LoadStepType == LoadStepType.Sequential)
                {
                    await UniTask.WhenAll(_parallelStartedLoadingStep);
                    _parallelStartedLoadingStep.Clear();
                    await loadingStep.Start(_performanceCalculator,cancellationToken);
                }
                else if (loadingStep.LoadStepType == LoadStepType.Parallel)
                {
                    UniTask task = loadingStep.Start(_performanceCalculator,cancellationToken);
                    _parallelStartedLoadingStep.Add(task);
                }
                _loadingStepsType.Dequeue();
            }

            _performanceCalculator.SendPerformanceData();
        }

        public void Dispose()
        {
            _loadingStepsType.Clear();
            _parallelStartedLoadingStep.Clear();
        }
    }
}