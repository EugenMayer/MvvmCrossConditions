using System;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.CrossCore.Platform;
using System.Collections.Generic;

namespace MvvmCross.Conditions.Core
{
    /*
     * Use this class as a base class for your view Models if you want to handle the instantiation errors yourself
     * using the exceptions. You can use the viewModelshouldHandleError to decide whether you do want or not
     * 
     * This class is optional, see above
     */
    public class MvxConditionalViewModel : MvxViewModel
    {
        protected bool ShowViewModel<TViewModel>(bool viewModelshouldHandleError)
            where TViewModel : IMvxViewModel
        {
            return ShowViewModelConditionalImpl<TViewModel>(viewModelshouldHandleError, new MvxViewModelRequest(typeof(TViewModel), null, null, null));
        }

        protected bool ShowViewModel<TViewModel>(bool viewModelshouldHandleError, IDictionary<string, string> parameterValues)
            where TViewModel : IMvxViewModel
        {
            return ShowViewModelConditionalImpl<TViewModel>(viewModelshouldHandleError, new MvxViewModelRequest(typeof(TViewModel), new MvxBundle(parameterValues), null, null));
        }

        protected bool ShowViewModelConditionalImpl<TViewModel>(bool viewModelshouldHandleError, Type viewModelType, IMvxBundle parameterBundle, IMvxBundle presentationBundle, MvxRequestedBy requestedBy)
        {
            MvxTrace.Trace("Showing ViewModel {0}", viewModelType.Name);
            var test = this;
            var viewDispatcher = ViewDispatcher as IMvxConditionalDispatcher;
            // TODO: maybe check for null here, means, there is no IMvxConditionalDispatcher compatible dispatcher, but it is required for this calle
            if (viewDispatcher != null)
                return viewDispatcher.ShowViewModel(new MvxViewModelRequest(typeof(TViewModel), parameterBundle, presentationBundle, requestedBy), viewModelshouldHandleError);

            return false;
        }
    }
}

