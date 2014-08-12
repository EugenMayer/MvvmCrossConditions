using System;
using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;
using Cirrious.CrossCore.Platform;

namespace MvvmCross.Conditions.Core.Example
{
    public class EntryViewModelComplex : MvxConditionalViewModel // here we dont need conditions, so just implementing MvxConditionalViewModel optionally to be able to call ShowViewModel with a paramter to be able to handle erros, see GoToEventAndHandle
    {
        public ICommand GoToEventAndHandle {
            get {
                return new MvxCommand(() => {
                    try {
                        ShowViewModel<SecondViewModel>(false);
                    }
                    catch (Exception ex) {
                        MvxTrace.Error("Could not load SecondViewmodel");
                    }
                });
            }
        }

        public ICommand GoToEvent {
            get {
                return new MvxCommand(() => {
                    ShowViewModel<SecondViewModel>(); // we should land on ThirdViewModel here
                });
            }
        }
       
    }
}

