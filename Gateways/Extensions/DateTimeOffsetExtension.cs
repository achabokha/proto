using System;
using System.Collections.Generic;
using System.Text;

namespace Embily.Gateways
{
    public static class DateTimeOffsetExtension
    {
        // references 
        /*
        var time1 = DateTimeOffset.Now.ToString("o");
        var time2 = DateTimeOffset.Now.ToString("O");
        var time3 = DateTimeOffset.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffffzzz");
        var time4 = DateTimeOffset.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffzzz");
        var time5 = DateTimeOffset.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffK");
        */

        public static string ToString3fzzz(this DateTimeOffset date)
        {
            //return date.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffK")
            return date.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffzzz");
        }

        public static string ToString3fK(this DateTimeOffset date)
        {
            return date.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffK");
        }
    }
}
