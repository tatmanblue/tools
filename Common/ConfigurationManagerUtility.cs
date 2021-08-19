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
using System.Configuration;
#endregion

namespace BigWoo.Common
{
    /// <summary>
    /// This static class provides methods that make accessing the configuration manager simplier
    /// </summary>
    public static class ConfigurationManagerUtility
    {
        /// <summary>
        /// Reads a string from the application config file, appsettings section, returning the value in the fil
        /// unless there is an exception in which case it returns the default.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="retDefault"></param>
        /// <returns></returns>
        public static string GetAppSettingString(string key, string retDefault)
        {
            string ret = retDefault;

            try
            {
                ret = ConfigurationManager.AppSettings[key];

                if (true == string.IsNullOrEmpty(ret))
                    ret = retDefault;
            }
            catch (ConfigurationErrorsException) { }

            return ret;
        }

        /// <summary>
        /// Reads a string from the application config file, appsettings section, returning the value as an integer
        /// unless there is an exception in which case it returns the default.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="retDefault"></param>
        /// <returns></returns>
        public static int GetAppSettingInt(string key, int retDefault)
        {
            int ret = retDefault;

            try
            {
                ret = TypeConversionUtility.StringToInt(ConfigurationManager.AppSettings[key], retDefault);
            }
            catch (ConfigurationErrorsException) { }


            return ret;
        }

        /// <summary>
        /// Reads a string from the application config file, appsettings section, returning the value as an integer
        /// unless there is an exception in which case it returns the default.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="retDefault"></param>
        /// <returns></returns>
        public static bool GetAppSettingBool(string key, bool retDefault)
        {
            bool ret = retDefault;

            try
            {
                ret = TypeConversionUtility.StringToBool(ConfigurationManager.AppSettings[key], retDefault);
            }
            catch (ConfigurationErrorsException) { }


            return ret;

        }
    }
}
