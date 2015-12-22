﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;

using MVCBase.Enums;

namespace MVCBase
{
    public class CommonExpressions
    {
        public static string AlphaNumericString(string normalString)
        {
            return Regex.Replace(normalString, @"[^0-9a-zA-Z\*\+\(\)]+", "");
        }

        public static string ComposeCommodityCode(string code, int commodityTypeID)
        {
            code = MVCBase.CommonExpressions.AlphaNumericString(code);

            if (commodityTypeID != (int)GlobalEnums.CommodityTypeID.Vehicles && code.Length >= 9)
                return code.Substring(0, 5) + "-" + code.Substring(5, 3) + "-" + code.Substring(8, code.Length - 8);
            else
                return code;
        }

    }
}
