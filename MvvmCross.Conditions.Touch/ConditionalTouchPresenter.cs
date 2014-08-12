using System;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using MonoTouch.UIKit;
using Cirrious.CrossCore.Exceptions;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.CrossCore;
using MvvmCross.Conditions.Core;

namespace MvvmCross.Conditions.Touch
{
    public class ConditionalTouchPresenter : MvxTouchViewPresenter
    {
        public ConditionalTouchPresenter(UIApplicationDelegate applicationDelegate, UIWindow window) : base(applicationDelegate, window)
        {
        }

        public override void Show(MvxViewModelRequest request)
        {
            var loader = Mvx.Resolve<IMvxViewModelLoader>() as IMvxViewModelPreloader;
            if (request.ViewModelType is IConditionalViewModel) {
                // check condition here
                var viewModel = loader.LoadViewModel(request, null) as IConditionalViewModel;
                if (viewModel.Precondition() == false) {
                    // EXCEPTION HERE
                }
                else {
                    // save the viewModel as preloaded so it can be reused later in the instantiation of the view
                    loader.AddPreloadedViewModel(request.ViewModelType.ToString(), viewModel as IMvxViewModel);
                    var view = this.CreateViewControllerFor(request);
                    Show(view);
                    // we do this for now to minimize sideeffects of preloaded viewmodel - those preloaded viewmodels
                    // are therefor only 
                    loader.RemovePreloadedViewModel(request.ViewModelType.ToString());
                }
            }
            else {
                var view = this.CreateViewControllerFor(request);
                Show(view);
            }
        }

        public override void Show(IMvxTouchView view)
        {
            var viewController = view as UIViewController;
            if (viewController == null)
                throw new MvxException("Passed in IMvxTouchView is not a UIViewController");

            if (MasterNavigationController == null)
                ShowFirstView(viewController);
            else
                MasterNavigationController.PushViewController(viewController, true /*animated*/);
        }
    }
}

