using System;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.ViewModels;
using System.Threading;
using Cirrious.CrossCore.Platform;
using MvvmCross.Conditions.Core;
using Cirrious.CrossCore.Core;

namespace MvvmCross.Conditions.Droid
{
    // TODO: we could instead of manually syncing the code from MvxTouchViewDispatcher use decoration
    // we cannot simply derive from MvxTouchViewDispatcher since ShowViewModel is not virtual
    public class ConditionalDroidDispatcher :  MvxAndroidViewDispatcher, IMvxViewDispatcher, IMvxConditionalDispatcher
    {
        private readonly IMvxAndroidViewPresenter _presenter;

        public ConditionalDroidDispatcher(IMvxAndroidViewPresenter presenter) : base(presenter)
        {
            _presenter = presenter;
        }

        // thats an OVERLOAD!, just in case somebody reads way to fast ;)
        public bool ShowViewModel(MvxViewModelRequest request, bool viewModelShouldHandleError = true)
        {
            Action action = () => {
                MvxTrace.TaggedTrace("DroidNavigation", "Navigate requested");
                _presenter.Show(request);
            };
            return RequestMainThreadAction(action);
        }

        public bool ShowViewModel(MvxViewModelRequest request)
        {
            return ShowViewModel(request, true);
        }

        public bool ChangePresentation(MvxPresentationHint hint)
        {
            return RequestMainThreadAction(() => _presenter.ChangePresentation(hint));
        }
    }
}

