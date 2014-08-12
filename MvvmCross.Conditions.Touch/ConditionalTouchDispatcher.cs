using System;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.ViewModels;
using System.Threading;
using Cirrious.CrossCore.Platform;
using MvvmCross.Conditions.Core;

namespace MvvmCross.Conditions.Touch
{
    // TODO: we could instead of manually syncing the code from MvxTouchViewDispatcher use decoration
    // we cannot simply derive from MvxTouchViewDispatcher since ShowViewModel is not virtual
    public class ConditionalTouchDispatcher : MvxTouchUIThreadDispatcher, IMvxViewDispatcher, IMvxConditionalDispatcher
    {
        private readonly IMvxTouchViewPresenter _presenter;

        public ConditionalTouchDispatcher(IMvxTouchViewPresenter presenter) : base()
        {
            _presenter = presenter;
        }

        // thats an OVERLOAD!, just in case somebody reads way to fast ;)
        public bool ShowViewModel(MvxViewModelRequest request, bool viewModelShouldHandleError = true)
        {
            Action action = () => {
                MvxTrace.TaggedTrace("TouchNavigation", "Navigate requested");
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

