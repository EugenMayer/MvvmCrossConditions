using System;
using Cirrious.CrossCore.Exceptions;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.CrossCore;
using MvvmCross.Conditions.Core;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.CrossCore.Droid.Platform;
using Android.App;
using Android.OS;

namespace MvvmCross.Conditions.Droid
{
    public class ViewNotAllowedException : Exception
    {

    }

    public class ConditionalDroidPresenter : MvxAndroidViewPresenter
    {
        Activity _currentAcitvity;

        public Activity CurrentActivity {
            get {
                return _currentAcitvity;
            }
            set {
                _currentAcitvity = value;
            }
        }

        public ConditionalDroidPresenter() : base()
        {
            _currentAcitvity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
        }

        public override void Show(MvxViewModelRequest request)
        {
            Show(request, true);
        }

        // thats an OVERLOAD!, just in case somebody reads way to fast ;)
        public void Show(MvxViewModelRequest request, bool viewModelShouldHandleError = true)
        {
            // TODO: use an as cast with an null check instead?
            if (ImplementsInterface(request.ViewModelType, typeof(IConditionalViewModel))) {
                // check condition here
                var loader = Mvx.Resolve<IMvxViewModelLoader>();
                var viewModel = loader.LoadViewModel(request, null) as IConditionalViewModel;
                if (viewModel.Precondition(viewModelShouldHandleError) == false) {
                    if (viewModelShouldHandleError) {
                        // in this case the ViewModel already handled the error - if it did nothing ( no redirect ) basically just nothing happens
                        return;
                    }
                    else {
                        // The calle asked us ot inform him if the viewmodel does not allow instantiation to handle the error himself, so throw an Exception
                        throw new ViewNotAllowedException();
                    }
                }
                else {
                    // reuse the viewModel
                    var intent = CreateIntentForRequest(request);

                    // This little thingy is used to "inject the viewmodel" into the activity to avoid reinstantiation
                    // see MvxAndroidViewsContainer.TryGetEmbeddedViewModel
                    var key = Mvx.Resolve<IMvxChildViewModelCache>().Cache(viewModel);
                    Bundle extras = intent.Extras;
                    extras.PutInt("MvxSubViewModelKey", key); // MvxSubViewModelKey is a special static key.. we cant use MvxAndroidViewsContainer.SubViewModelKey since its static 
                    intent.PutExtras(extras);
                    Show(intent);
                }
            }
            else {
                // no conditions, classiv way
                var intent = CreateIntentForRequest(request);
                Show(intent);
            }
        }

        public bool ImplementsInterface(Type type, Type ifaceType)
        {
            Type[] intf = type.GetInterfaces();
            for (int i = 0; i < intf.Length; i++) {
                if (intf[i] == ifaceType) {
                    return true;
                }
            }
            return false;
        }
    }
}

