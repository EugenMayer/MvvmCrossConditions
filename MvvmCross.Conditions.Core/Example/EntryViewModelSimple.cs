using System;
using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;
using Cirrious.CrossCore.Platform;

namespace MvvmCross.Conditions.Core.Example
{
    public class EntryModelSimple : MvxViewModel
    {
        public ICommand GoToEvent {
            get {
                return new MvxCommand(() => {
                    ShowViewModel<SecondViewModel>(); // we should land on ThirdViewModel here
                });
            }
        }
       
    }
}

