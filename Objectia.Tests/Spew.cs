using System;
using Newtonsoft.Json;

namespace Objectia.Tests
{
    public class Spew
    {
        public static void Dump(object obj)
        {
            Console.WriteLine(JsonConvert.SerializeObject(obj));
        }
    }
}