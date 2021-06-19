using EU4_PCP.Models;
using System;

namespace EU4_PCP.Converters
{
    public static class StringToEnum
    {
        public static Type ToEnum(this string enumName)
        {
            return enumName switch
            {
                "AutoLoad" => typeof(AutoLoad),
                "ProvinceNames" => typeof(ProvinceNames),
                _ => throw new NotImplementedException()
            };
        }
    }
}
