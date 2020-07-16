// onotseike@hotmail.comPaula Aliu
using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;

namespace WebRTC.iOS.Extensions
{
    internal static class DictionaryExtensions
    {
        public static NSDictionary<NSString, NSString> ToNative(this IDictionary<string, string> self)
        {
            var keys = self.Keys.Select(k => k.ToNative()).ToArray();
            var values = self.Values.Select(v => v.ToNative()).ToArray();
            return new NSDictionary<NSString, NSString>(keys, values);
        }

        public static NSString ToNative(this string self)
        {
            return new NSString(self);
        }
    }
}
