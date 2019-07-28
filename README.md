![Xodium Logo](https://github.com/tseemann-dk/Xodium/blob/master/images/Xodium_96.png "Xodium Logo")
# Xodium
A library of cross-platform functionality for solutions running on top of .NET. 

Xodium is a basic ingredient for many common application types ... just like the sodium in your household table salt.

Xodium provides a collection of reusable constructs and components designed for .NET applications that need to run across multiple devices, platforms and operating systems. One of the primary goals of the library is to handle differences between the various platforms via rich abstractions and platform-specific implementations of these abstractions for the most popular platforms.

The library contains components organized into the following categories:

## Xodium.Core
Common base types and interfaces as well as general extensions for the .NET BCL.

## Xodium.Injection
An abstraction of dependency injection that enables inversion of control in your applications without taking a direct dependency on the chosen IoC container. Includes support for Microsoft Unity and Microsoft DependencyInjection. Others can easily be added by implementing a few simple bridge interfaces.

## Xodium.Mvvm
Yet another MVVM library. Like every other MVVM library or framework, this library contains base classes for basic MVVM elements like view models, commands and more. In an unopinionated approach to how to apply MVVM to your code, this library aims at providing you with a generic toolset of components and abstractions that work together with your favorite choice of MVVM library. 

Beside the basic building blocks of MVVM, the library also contains a navigation system for navigating between view models independently of the underlying UI framework and infrastructure. Includes specific support for Xamarin Forms and ReactiveUI.

## Xodium.Platform
Features that are common amongst many platforms, but vary in API implementation, are made accessible to your application via platform-independent abstractions defined in the Xodium core service interfaces. The Xodium.Platform.* assemblies contain platform-specific implementations of these interfaces, designed to be injected into your application to allow you to code against one simplified yet rich API across all platforms.

## Xodium.Productivity
The productivity category of Xodium enables unified access to various personal and work related features like calendars, e-mails, notes, contacts, documents and more. The library provides implementations of these features for Microsft Office 365, Apple iCloud and Google Services. In addition, the library contains a content management API focused on managing the internal structure of hierarchical documents by enabling manipulation of the document structure in an immutable fashion.

## Xodium.Redux
The fundamental idea of improved state management via unidirectional data flow proposed by architectural patterns like CQRS has gained a lot of popularity in the recent years when maintaining state in complex application user interfaces. In particular, the pattern has been used with great success in frameworks like [Flux](https://facebook.github.io/flux/) and [Redux](https://redux.js.org/) in the JavaScript/React movement within web development. These frameworks have let to the creation of several similar libraries in the .NET community, each offering its own take on a Redux-like API for .NET. 

Xodium provides a generic abstraction of the basic concept of actions, stores, reducers, middleware and dispatchers introduced by Redux. This abstraction enables unidirectional data flow in any application by supporting different Redux implementations without direct knowledge of the Redux library in use. Xodium.Redux is an implementation of this abstraction for [Redux.NET](https://github.com/GuillaumeSalles/redux.NET) by Guillaume Salles.
