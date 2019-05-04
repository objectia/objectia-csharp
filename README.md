# objectia-csharp
[![Build Status](https://travis-ci.org/objectia/objectia-csharp.svg?branch=master)](https://travis-ci.org/objectia/objectia-csharp)

C#/.NET client for [Objectia API](https://objectia.com)

 
## Documentation

See the [C# API docs](https://docs.objectia.com/guide/csharp.html).


## Installation

With .NET CLI 

```powershell
dotnet add package Objectia --version 1.0.0
```    

With Package Manager

```powershell
PM> Install-Package Objectia -Version 1.0.0
```    


## Usage

The library needs to be configured with your account's API key. Get your own API key by signing up for a free [Objectia account](https://objectia.com).

```csharp
using System;
using Objectia;
using Objectia.Api;
using Objectia.Exceptions;

class Program 
{
    static void Main(string[] args)
    {
        var apiKey = Environment.GetEnvironmentVariable("OBJECTIA_APIKEY");
        try 
        {
            ObjectiaClient.Init(apiKey);
            var location = await Api.GeoLocation.Get("8.8.8.8"); 
            Console.WriteLine("Country code: " + location.CountryCode);
        } catch (ResponseException e) {
            Console.WriteLine("Failed to get location");
        }
    }
}
```


## License and Trademarks

Copyright (c) 2018-19 UAB Salesfly.

Licensed under the [MIT license](https://en.wikipedia.org/wiki/MIT_License). 

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

Objectia is a registered trademark of [UAB Salesfly](https://www.salesfly.com). 