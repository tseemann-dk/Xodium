![Xodium Logo](https://github.com/tseemann-dk/Xodium/blob/master/images/Xodium_96.png "Xodium Logo")
# Xodium
A library of cross-platform functionality for solutions running on top of .NET. Xodium provides basic ingredients for many common applications ... similar to the sodium in your everyday household table salt.

Xodium is a collection of reusable constructs and components commonly used in .NET applications that require running across multiple devices, platforms and operating systems.
The primary goal of the library is to abstract away similarities across various platforms and at the same time provide implementations of these abstractions for the most popular platforms.

The library contains components organized into the following categories:

## Xodium.Core
Common base types and general extensions for the .NET BCL.

## Xodium.Injection
An abstraction of dependency injection that enables inversion of control independently of the choice of IoC container. 
Includes support for Microsoft Unity and Microsoft.Extensions.DependencyInjection. Others can easily be added by implementing a few simple bridge interfaces.

## Xodium.Mvvm
Yet another MVVM framework. Like every other MVVM library or framework, this library contains base classes for basic MVVM elements like view models, commands and more. But instead of dictating one way of applying MVVM to your code, this library aims at providing you with a generic toolset of components and abstractions that work together with your favorite choice of MVVM framework. Beside the basic building blocks of MVVM, the library also contains a navigation system for navigating between view models independently of the underlying UI framework and infrastructure. Includes specific support for Xamarin Forms and ReactiveUI.

## Xodium.Platform
...

## Xodium.Productivity
...

## Xodium.Redux
...
