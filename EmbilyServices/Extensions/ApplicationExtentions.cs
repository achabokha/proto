using Embily.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmbilyServices
{
    public static class ApplicationExtentions
    {
        public static string GenApplicationNumber(this Application application, ApplicationUser user)
        {
            if (user.Applications == null || user.Applications?.Count == 0)
            {
                return user.UserNumber.ToString() + "-0001";
            }
            else
            {
                var apps = user.Applications.OrderByDescending(a => a.ApplicationNumber).ToList();
                var nextNumber = Convert.ToUInt64(apps[0].ApplicationNumber.Replace("-", "")) + 1;
                var num = nextNumber.ToString().Insert(10, "-");
                return num;
            }
        }
    }
}
