using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using LoadSequence.Runtime.Interface;

namespace LoadSequence.Runtime
{
    public class LoadingService
    {
        private CancellationTokenSource _cancellationTokenSource;
        private ILoadingStepFactory _loadingStepFactory;
        private IErrorHandler _errorHandler;
        private LoadSequencePlayer _loadSequencePlayer;
        
        public event Action OnCompleted;
        
        public void SetupFactory(ILoadingStepFactory loadingStepFactory)
        {
            _loadingStepFactory = loadingStepFactory;
        }
        
        public async UniTask PlayLoadingSequence(ILoadingQueueContainer loadingQueueContainer)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            LoadSequence loadSequence = loadingQueueContainer.GetLoadingQueue();
            _errorHandler = loadSequence.ErrorHandler;
            Queue<ILoadingStep> loadStepByTypes = CreateAndSetupLoadingStep(loadSequence);
            _loadSequencePlayer = new LoadSequencePlayer(loadStepByTypes,loadSequence.PerformanceCalculator);
            await Play();
        }

        public async UniTask RestartLastLoadingQueue()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            await _loadSequencePlayer.Play(_cancellationTokenSource.Token);
        }

        public void CancelLoadOperation()
        {
            _cancellationTokenSource.Cancel();
        }

        private async UniTask Play()
        {
            try
            {
                await UniTask.SwitchToMainThread();
                await _loadSequencePlayer.Play(_cancellationTokenSource.Token);
            }
            catch (Exception e)
            {
                _errorHandler?.SendError(e);
                CancelLoadOperation();
            }
            finally
            {
                await UniTask.SwitchToMainThread();
                OnCompleted?.Invoke();
                _loadSequencePlayer.Dispose();
                _cancellationTokenSource.Dispose();
                _loadSequencePlayer = null;
            }
        }
        
        private Queue<ILoadingStep> CreateAndSetupLoadingStep(LoadSequence loadSequence)
        {
            float totalWeightSum = 0;
            ILoadingProgressHandler progressHandler = loadSequence.LoadingProgressHandler;
            Queue<ILoadingStep> loadStepByTypes = new Queue<ILoadingStep>();
            foreach (StepInfo stepInfoIt in loadSequence.Steps)
            {
                if (_loadingStepFactory.Create(stepInfoIt, out ILoadingStep loadingStep))
                {
                    loadingStep.Init(stepInfoIt.LoadStepType);
                    totalWeightSum += loadingStep.GetWeight();
                    loadStepByTypes.Enqueue(loadingStep);
                    SetupPerformanceHandler(loadingStep, loadSequence.PerformanceCalculator);
                }
            }
            progressHandler?.SetTotalProgressWeight(totalWeightSum);
            
            void SetupPerformanceHandler(ILoadingStep loadingStep,IPerformanceCalculator performanceCalculator)
            {
                if (performanceCalculator != null)
                {
                    if (loadingStep is IPerformanceHandlerReporter reporter)
                    {
                        reporter.PerformanceHandlerReporter(loadSequence.PerformanceCalculator);
                    }
                }
            }
            
            return loadStepByTypes;
        }
    }
}