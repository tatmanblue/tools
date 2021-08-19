/*
 ******************************************************************************
 This file is part of BigWoo.NET.

    BigWoo.NET is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    BigWoo.NET is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with BigWoo.NET; if not, write to the Free Software
    Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

    architected and written by 
    matt raffel 
    email: matt {dot} raffel at gmail {dot} com
    website: quartersforhomies.com
             mattraffel.com

       copyright (c) 2010 by matt raffel unless noted otherwise

 ******************************************************************************
*/
#region using statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;
#endregion

namespace BigWoo.Common
{
    /// <summary>
    /// This class is a static class providing a set of methods for handling 
    /// conversions of simple data types to another
    /// </summary>
    public static class TypeConversionUtility
    {
        /// <summary>
        /// 
        /// </summary>
        private static Regex _isNumber = new Regex(@"^\d+$");

        /// <summary>
        /// Determines if a string contains completely numeric characters
        /// </summary>
        /// <param name="stringInput">string</param>
        /// <returns></returns>
        public static bool IsInteger(string stringInput)
        {
            Match regExMatch = _isNumber.Match(stringInput);
            return regExMatch.Success;
        }

        /// <summary>
        /// returns the input string, unless input string is null, in which case empty string
        /// is returned
        /// </summary>
        /// <param name="stringInput">string, possibly null string reference</param>
        /// <returns>string</returns>
        public static string StringOrEmptyString(string stringInput)
        {
            if (false == string.IsNullOrEmpty(stringInput))
                return stringInput;

            return string.Empty;
        }

        /// <summary>
        /// Attempts to convert string to int, all errors result in the default value returned.
        /// If the input is nonnumeric, default is also returned
        /// </summary>
        /// <param name="stringInput">string</param>
        /// <param name="returnDefault">int</param>
        /// <returns>int, returnDefault on error</returns>
        public static int StringToInt(string stringInput, int returnDefault)
        {
            int ret = returnDefault;

            try
            {
                // only attempt to convert if string is a valid string
                // can still get a 0 instead of returnDefault if the input
                // string is non numeric
                if (false == string.IsNullOrEmpty(stringInput))
                    // so we check to make sure we have numeric values
                    if (true == TypeConversionUtility.IsInteger(stringInput))
                        ret = Convert.ToInt32(stringInput);
            }
            catch (FormatException) { }
            catch (OverflowException) { }

            return ret;
        }

        /// <summary>
        /// Attempts to convert a string to a bool.  Input expects either the
        /// word true or false (case insensitive).  returns default of it fails
        /// to find a suitable conversion.
        /// </summary>
        /// <param name="stringInput">string, either the workd true or false</param>
        /// <param name="returnDefault">bool, boolean value to return if the input string is neither true or false</param>
        /// <returns>bool</returns>
        public static bool StringToBool(string stringInput, bool returnDefault)
        {
            bool ret = returnDefault;

            try
            {
                if (0 == string.Compare(stringInput, "TRUE", true))
                    ret = true;
                else if (0 == string.Compare(stringInput, "FALSE", true))
                    ret = false;

            }
            catch (FormatException) { }
            catch (OverflowException) { }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">enumeration type</typeparam>
        /// <param name="enumString">value to be converted</param>
        /// <param name="defaultValue">default</param>
        /// <returns></returns>
        public static T EnumParse<T>(string enumString, T defaultValue)
        {
            T returnEnum = defaultValue;

            try
            {
                returnEnum = (T)Enum.Parse(typeof(T), enumString);
            }
            catch { }

            return returnEnum;
        }
    }
}
