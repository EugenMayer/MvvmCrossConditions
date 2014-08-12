using System;
using Cirrious.MvvmCross.Touch.Platform;
using Cirrious.MvvmCross.ViewModels;
using MvvmCross.Conditions.Core;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using Cirrious.MvvmCross.Views;

namespace MvvmCross.Conditions.Touch.Example
{
    public class ExampleSetup : MvxTouchSetup
    {
        protected MvxApplicationDelegate _applicationDelegate;
        protected UIWindow _window;

        public ExampleSetup(MvxApplicationDelegate applicationDelegate, UIWindow window) : base(applicationDelegate, window)
        {
            _window = window;
            _applicationDelegate = applicationDelegate;
        }

        // not needed anymore for the spefici case of "conditional views"
        /*
        protected override IMvxViewModelLoader CreateViewModelLoader()
        {
            return new PreloaderViewModelLoader();
        }*/

        protected override IMvxTouchViewPresenter CreatePresenter()
        {
            return new ConditionalTouchPresenter(_applicationDelegate, _window);
        }

        protected override IMvxViewDispatcher CreateViewDispatcher()
        {
            return new ConditionalTouchDispatcher(Presenter);
        }
    }
}

