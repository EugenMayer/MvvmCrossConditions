using System;
using Cirrious.MvvmCross.Touch.Views;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.ViewModels;
using System.Threading;
using Cirrious.CrossCore.Platform;

namespace MvvmCross.Conditions.Touch
{
    // TODO: we could instead of manually syncing the code from MvxTouchViewDispatcher use decoration
    // we cannot simply derive from MvxTouchViewDispatcher since ShowViewModel is not virtual
    public class ConditionalTouchDispatcher : MvxTouchUIThreadDispatcher, IMvxViewDispatcher
    {
        private readonly IMvxTouchViewPresenter _presenter;

        public ConditionalTouchDispatcher(IMvxTouchViewPresenter presenter) : base()
        {
            _presenter = presenter;
        }

        public bool ShowViewModel(Cirrious.MvvmCross.ViewModels.MvxViewModelRequest request)
        {
            Action action = () => {
                MvxTrace.TaggedTrace("TouchNavigation", "Navigate requested");
                _presenter.Show(request);
            };
            return RequestMainThreadAction(action);
        }


        public bool ChangePresentation(MvxPresentationHint hint)
        {
            return RequestMainThreadAction(() => _presenter.ChangePresentation(hint));
        }
    }
}

