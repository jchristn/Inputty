<img src="https://github.com/jchristn/Inputty/raw/main/Assets/icon.png" width="100" height="100">

# Inputty

[![NuGet Version](https://img.shields.io/nuget/v/Inputty.svg?style=flat)](https://www.nuget.org/packages/Inputty/) [![NuGet](https://img.shields.io/nuget/dt/Inputty.svg)](https://www.nuget.org/packages/Inputty) 

Inputty is a simple library that helps simplify the process of getting input from the console.  Inputty allows you to define the prompt, specify allowable values, and specify boundary conditions.

## New in v1.0.x

- Initial release

## Help or feedback

First things first - do you need help or have feedback?  File an issue here or start a discussion!

## It's Super Easy
```csharp
using GetSomeInput;

string answer1 = Inputty.GetString("What's your name?", null, false);    
// question, default answer, allow null

int answer2    = Inputty.GetInteger("What's your age?", 42, true, true); 
// question, default answer, positive only, allow zero
```
Yep, it's that simple.

## Supported Types

Inputty supports a decent range of return types:

- ```String```
- ```List<String>```
- ```Int```
- ```Boolean```
- ```Decimal```
- ```Double```
- ```Dictionary```
- ```NameValueCollection```
- ```DateTime```
- ```DateTime?```
- ```Guid```
