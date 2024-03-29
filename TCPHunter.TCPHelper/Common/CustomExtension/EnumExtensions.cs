﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TCPHunter.TCPHelper.Common.CustomExtension
{
    internal static class EnumExtensions
    {
        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
            where TAttribute : Attribute
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())?
                            .First()?
                            .GetCustomAttribute<TAttribute>();
        }
    }
}
