using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdService.Models;

namespace AdService.Helpers
{
    public class EnumHelper
    {
        public static Status EnumParse(string value, Status defaultStatus)
        {
            if (!Enum.TryParse(value, out Status adStatus))
            {
                return defaultStatus;
            }
            return adStatus;
        }
    }
}