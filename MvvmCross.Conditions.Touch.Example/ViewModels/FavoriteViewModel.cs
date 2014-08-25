using System;
using System.Windows.Input;
using System.Diagnostics;
using Cirrious.MvvmCross.Localization;
using ReserviX.Core.Services;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using MvvmCross.Conditions.Core;
using MvvmCross.Conditions;
using ReserviX.Core.Utils;

namespace ReserviX.Core.ViewModels
{
    public class FavoritesViewModel : MvxConditionalViewModel, IDisposable,IConditionalViewModel
    {
        protected readonly IMvxMessenger _messageService;
        protected bool _timeoutException = false;
        protected bool _modelLoaded = true;

	protected IFavoriteService _favoritesService;
	private List<Event> _favoritesAsEvents;

        public Favorites(IFavoriteService favoriteService)
        {
            _favoritesService = favoriteService;
        }
        {
            _localizationService = Mvx.Resolve<ILocalizationService>();
            _messageService = Mvx.Resolve<IMvxMessenger>();
        }
	// this is were we would load external ressources or whatever and in cases
	// something goes wrong, we use the internal flags to tell the procindition function that it should fail
  	// since our data-model is corrupt or whatever
	public void Init()
        {
            try {
  		_favoritesAsEvents = _favoritesService.LoadFavoritesAsEvents();
            }
            catch (WebException ex) {
                _timeoutException = true;
                return;
            }
            catch (TimeoutException ex) {
                _timeoutException = true;
                return;
            }
            catch (Exception ex) {
                _modelLoaded = false;
                return;
            }

            if (_genres.Count() <= 0) {
                _modelLoaded = false;
            }
        }
 
	// This is were the magic happens. If this evaluates to false, then the view/activity is never instantiated
        public virtual bool Precondition(bool shouldHandleError)
        {
            bool errors = false;
            // you can have anything in here like user is not logged in, date is older then whatever
            if (_timeoutException) {
                HandleTimeoutError();
  		errors = true;
            }

            if (_modelLoaded == false) {
                HandleModelError();
		errors = true;
            }
            
	    if(errors && shouldHandleError) {  // we should handle the error, in this case we can do a redirect or do nothing, or display an error
		ShowViewModel<HomeViewModel>(); // optional, if we dont redirect the view would just not open and we would just growl a messsage created above
                return false;
 	    }

            return true;
        }
	
	protected virtual void HandleTimeoutError()
        {
            _messageService.Publish(new NotificationMessage(this) { 
                Notification = new Notification { 
                    Body = "Server nicht erreichbar" 
                }
            });
        }

        protected virtual void HandleModelError()
        {
            _messageService.Publish(new NotificationMessage(this) { 
                Notification = new Notification { 
                    Body = "Es ist ein Fehler aufgetreten" 
                }
            });
        }


	public List<Event> Favorites {
            get {
                return _favoritesAsEvents;
            }
            set {
                _favoritesAsEvents = value;
                RaisePropertyChanged(() => Favorites);
            }
        }
}
