using System;
using System.Collections.Generic;

namespace Xodium.Extensions
{
    public static class ExceptionExtensions
    {
        public static string GetMessageList(this Exception self, string delimiter = null) => 
            string.Join(delimiter ?? Environment.NewLine, self.GetMessages());

        public static IEnumerable<string> GetMessages(this Exception self)
        {
            for (var e = self; e != null; e = e.InnerException)
            {
                yield return e.Message;
            }
        }
    }
}
