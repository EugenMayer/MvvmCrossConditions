using System;
using Cirrious.MvvmCross.ViewModels;

namespace MvvmCross.Conditions.Core
{
    public interface IMvxConditionalDispatcher
    {
        bool ShowViewModel(MvxViewModelRequest request, bool viewModelShouldHandleError = true);
    }
}

