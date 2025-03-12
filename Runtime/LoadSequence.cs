using System.Collections.Generic;
using LoadSequence.Runtime.Interface;

namespace LoadSequence.Runtime
{
    public class LoadSequence
    {
        private IErrorHandler _errorHandler;
        private IPerformanceCalculator _performanceCalculator;
        private ILoadingProgressHandler _loadingProgressHandler;
        
        public IErrorHandler ErrorHandler => _errorHandler;
        public IPerformanceCalculator PerformanceCalculator => _performanceCalculator;
        public ILoadingProgressHandler LoadingProgressHandler => _loadingProgressHandler;
        
        internal List<StepInfo> Steps = new List<StepInfo>();
        
        public LoadSequence Parallel<TStep>() where TStep : ILoadingStep
        {
            Steps.Add(new StepInfo()
            {
                StepType = typeof(TStep),
                LoadStepType = LoadStepType.Parallel
            });
            return this;
        }
        
        public LoadSequence Sequential<TStep>(bool condition) where TStep : ILoadingStep
        {
            if (!condition)
            {
                return this;
            }
            return Sequential<TStep>();
        }
        
        public LoadSequence Sequential<TStep>() where TStep : ILoadingStep
        {
            Steps.Add(new StepInfo()
            {
                StepType = typeof(TStep),
                LoadStepType = LoadStepType.Sequential
            });
            return this;
        }

        public LoadSequence RegisterPerformanceHandler(IPerformanceCalculator performanceHandler)
        {
            _performanceCalculator = performanceHandler;
            return this;
        }

        public LoadSequence RegisterErrorHandler(IErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;
            return this;
        }
        
        public LoadSequence RegisterErrorHandler(ILoadingProgressHandler progressHandler)
        {
            _loadingProgressHandler = progressHandler;
            return this;
        } 

        public void End()
        {
            
        }
    }
}