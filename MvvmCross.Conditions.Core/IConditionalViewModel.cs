using System;
using Cirrious.MvvmCross.ViewModels;

namespace  MvvmCross.Conditions
{
    public interface IConditionalViewModel : IMvxViewModel
    {
        bool Precondition(bool shouldHandleError);
    }
}

