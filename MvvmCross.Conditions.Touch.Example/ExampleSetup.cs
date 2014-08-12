using System;
using Cirrious.MvvmCross.Touch.Platform;
using Cirrious.MvvmCross.ViewModels;
using MvvmCross.Conditions.Core;
using MonoTouch.UIKit;

namespace MvvmCross.Conditions.Touch.Example
{
    public class ExampleSetup : MvxTouchSetup
    {
        public ExampleSetup(MvxApplicationDelegate applicationDelegate, UIWindow window) : base(applicationDelegate, window)
        {
        }

        protected override IMvxViewModelLoader CreateViewModelLoader()
        {
            return new PreloaderViewModelLoader();
        }
    }
}

