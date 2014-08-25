# Purpose
This is an library project for MvvmCross which enables you to implement a condition on your ViewModel, whether the instantiation of the View should be allowed or not.
This implementation is needed because there is no way in iOS/Android/WP to stop a View creation, after the ViewController/Similar has been instantiated. This is one of the flaws of 
MvvmCross, after calling ShowViewModel you cannot go back, you only can try to handle issues, hide the view and then redirect to a new one - this is ugly due to screen flickering and so on.

MvvmCross.Conditions enables you to Precondition() on your ViewModel and if that method returns false, the View instantiation does never happen. It also enable you to .e.g handle the error case in Precondition yourself, like redirecting, or let the caller decide what to do
if the targeted viewModel cannot be loaded.
## Examples
See the examples in .Core and in .Touch.Example to understand the workflow and implementation in general

# How to use and implement (developers)
It might look complicated, but its actually very easy. The first setup ( replacing dispatcher/presenter) is a first hurdle, but then every viewmodel is just on method - thats it.

## Core: Do this in your core project (platform independend) (once)
- Add MvvmCross.Conditions.Core as a project reference
- IMvxConditionalDispatcher this needs to be implemented by your platformDispatcher, for Touch use the ConditionalTouchDispatcher which is already implemented
Hint:The ViewModel is reused after the preconditions is checked, it does not get instantated twice ( when the view gets loaded ) - this is very useful.

## Core: Do this for every ViewModel you like (for every ViewModel)
- Implement the interface IConditionalViewModel and if wishing ( see notes ) derive from MvxConditionalViewModel ( optional )
- implement bool Precondition(bool shouldHandleError); and return true if the view should be loaded. This method is called BEFORE the View is instantiated. Returning false will cancel the view instantiation ( rather let it never happen) 
- Thats it :) the Precondition and have any logic and since Init() is already happen, you have access to your whole Model and see, if everything is just fine

## Touch: Do this in your ios project (once)

Please see the ExampleSetup.cs to see how you enable your app to use the Dispatcher and Presenter described below
- Add MvvmCross.Conditions.Touch as a project reference
- ConditionalTouchPresenter this needs to be used to actually have any use of the conditions
- ConditionalTouchDispatcher example implementation for the dispatcher, can be used out of the box and should be working for your cases in general ( no need to reimplement it)

## Droid: Do this in your Android project (once)
(same as Touch, see there)
- Add MvvmCross.Conditions.Driod as a project reference
- ConditionalDroidPresenter this needs to be used to actually have any use of the conditions
- ConditionalDroidDispatcher example implementation for the dispatcher, can be used out of the box and should be working for your cases in general ( no need to reimplement it)

# TODO
- Use nuget for all the project references for MvvmCross / CrossCore
- Describe / finish implementation of the Preloader for ViewModel reusage in general (optional for Conditions)
