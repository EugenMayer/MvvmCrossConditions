using System;
using Cirrious.MvvmCross.ViewModels;

namespace MvvmCross.Conditions.Core.Example
{
    public class SecondViewModel : MvxConditionalViewModel, IConditionalViewModel
    {
        public bool Precondition(bool shouldHandleError)
        {
            if (true /* some interesint code here, you have dataContext and state, use it */) {
                return true; // this will allow the view to appear
            }
            else { // error case
                if (shouldHandleError) {
                    ShowViewModel<ThirdViewModel>();// redirect to a different viewmodel
                    return false; // the calle of this ViewModel will not get informed now, he does not want to handle this case
                }
                else {
                    return false; // this will throw an ViewModelNotAllowedException and allows the caller of ShowViewModel to react/handle this
                }
            }
        }
       
    }
}

