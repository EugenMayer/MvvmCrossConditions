using System;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using System.Collections.Generic;

namespace MvvmCross.Conditions.Core
{
    public interface IMvxViewModelPreloader : IMvxViewModelLoader
    {
        void AddPreloadedViewModel(string viewModelType, IMvxViewModel viewModel);

        void RemovePreloadedViewModel(string viewModelType);

        IMvxViewModel GetPreloadedViewModel(string viewModelType);
    }
    // TODO: since most of the code is "stolen" from MvxViewModelLoader, since we cannot dirive/override
    // we could rather decorate instead of code dublication
    public class PreloaderViewModelLoader
        : IMvxViewModelPreloader
    {

        private Dictionary<string, IMvxViewModel> _preloadedViewModels;

        public PreloaderViewModelLoader()
        {
            _preloadedViewModels = new Dictionary<string, IMvxViewModel>();
        }


        public IMvxViewModel GetPreloadedViewModel(string viewModelType)
        {
            if (_preloadedViewModels.ContainsKey(viewModelType)) {
                return _preloadedViewModels[viewModelType];
            }
            // else
            return null;
        }

        public void AddPreloadedViewModel(string viewModelType, IMvxViewModel viewModel)
        {
            if (_preloadedViewModels.ContainsKey(viewModelType)) { // replace
                // TODO: destroy old instance??
                _preloadedViewModels[viewModelType] = viewModel;
            }
            else {
                _preloadedViewModels.Add(viewModelType, viewModel);
            }
        }

        public void RemovePreloadedViewModel(string viewModelType)
        {
            if (_preloadedViewModels.ContainsKey(viewModelType)) {
                // TODO: destroy old instance??
                _preloadedViewModels.Remove(viewModelType);
            }
        }

        private IMvxViewModelLocatorCollection _locatorCollection;

        protected IMvxViewModelLocatorCollection LocatorCollection {
            get {
                _locatorCollection = _locatorCollection ?? Mvx.Resolve<IMvxViewModelLocatorCollection>();
                return _locatorCollection;
            }
        }

        public IMvxViewModel LoadViewModel(MvxViewModelRequest request, IMvxBundle savedState)
        {
            var preloadedViewModel = GetPreloadedViewModel(request.ViewModelType.ToString());
            if (preloadedViewModel != null) {
                // TODO check if the viewmodel support the PreloadWakeup interace and trigger it
                return preloadedViewModel;
            }
            // else default loader
            if (request.ViewModelType == typeof(MvxNullViewModel)) {
                return new MvxNullViewModel();
            }

            var viewModelLocator = FindViewModelLocator(request);

            return LoadViewModel(request, savedState, viewModelLocator);
        }

        private IMvxViewModel LoadViewModel(MvxViewModelRequest request, IMvxBundle savedState, IMvxViewModelLocator viewModelLocator)
        {
            IMvxViewModel viewModel = null;
            var parameterValues = new MvxBundle(request.ParameterValues);
            if (!viewModelLocator.TryLoad(request.ViewModelType, parameterValues, savedState, out viewModel)) {
                throw new MvxException("Failed to construct and initialize ViewModel for type {0} from locator {1} - check MvxTrace for more information", request.ViewModelType, viewModelLocator.GetType().Name);
            }

            viewModel.RequestedBy = request.RequestedBy;
            return viewModel;
        }

        private IMvxViewModelLocator FindViewModelLocator(MvxViewModelRequest request)
        {
            var viewModelLocator = LocatorCollection.FindViewModelLocator(request);

            if (viewModelLocator == null) {
                throw new MvxException("Sorry - somehow there's no viewmodel locator registered for {0}", request.ViewModelType);
            }

            return viewModelLocator;
        }
    }
}

