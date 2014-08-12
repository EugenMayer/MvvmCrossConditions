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
            // TODO: use an as cast with an null check instead?
            if (ImplementsInterface(request.ViewModelType, typeof(IConditionalViewModel))) {
                // check condition here
                var loader = Mvx.Resolve<IMvxViewModelLoader>() as IMvxViewModelPreloader;
                var viewModel = loader.LoadViewModel(request, null) as IConditionalViewModel;
                if (viewModel.Precondition() == false) {
                    // EXCEPTION HERE
                }
                else {
                    var view = this.CreateViewControllerFor(request);
                    // reuse the viewModel
                    view.ViewModel = viewModel;
                    Show(view);
                }
            }
            else {
                // no conditions, classiv way
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

        public bool ImplementsInterface(Type type, Type ifaceType)
        {
            Type[] intf = type.GetInterfaces();
            for (int i = 0; i < intf.Length; i++) {
                if (intf[i] == ifaceType) {
                    return true;
                }
            }
            return false;
        }
    }
}

