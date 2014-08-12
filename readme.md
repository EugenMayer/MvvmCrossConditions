# Purpose
This is an library project for MvvmCross which enables you to implement a condition on your ViewModel, whether the instantiation of the View should be allowed or not.
This implementation is needed because there is no way in iOS/Android/WP to stop a View creation, after the ViewController/Similar has been instantiated. This is one of the flaws of 
MvvmCross, after calling ShowViewModel you cannot go back, you only can try to handle issues, hide the view and then redirect to a new one - this is ugly due to screen flickering and so on.

MvvmCross.Conditions enables you to Precondition() on your ViewModel and if that method returns false, the View instantiation does never happen. It also enable you to .e.g handle the error case in Precondition yourself, like redirecting, or let the caller decide what to do
if the targeted viewModel cannot be loaded.
## Examples
See the examples in .Core and in .Touch.Example to understand the workflow and implementation in general

## Core

- IConditionalViewModel this interface needs to be implemented on every ViewModel which wants to have this kind of condition
- IMvxConditionalDispatcher this needs to be implemented by your platformDispatcher, for Touch use the ConditionalTouchDispatcher which is already implemented

## Touch

Please see the ExampleSetup.cs to see how you enable your app to use the Dispatcher and Presenter described below
- ConditionalTouchPresenter this needs to be used to actually have any use of the conditions
- ConditionalTouchDispatcher example implementation for the dispatcher, can be used out of the box and should be working for your cases in general ( no need to reimplement it)

## TODO
- Use nuget for all the project references for MvvmCross / CrossCore
- Describe / finish implementation of the Preloader for ViewModel reusage in general (optional for Conditions)
