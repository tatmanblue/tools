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
#endregion

namespace BigWoo.Common
{
    /// <summary>
    /// This class provides validation of a DateTime type.  
    /// 
    /// Please note: Even though this class can be considered
    /// business logic it cannot be in the business logic layer as the data 
    /// access layer requires it.
    /// </summary>
    public static class DateTimeValidation
    {
        #region private data
        private static bool _isInitialized = false;
        private static DateTime _minDefaultValidDate;
        private static DateTime _maxDefaultValidDate;
        #endregion

        #region public readonly members
        // a format string like                                     2009-01-01 14:01:01.0001

        /// <summary>
        /// specifies that the DateTime will be compared down to the millisecond.  Default when
        /// no comparisionMethod is specified
        /// </summary>
        public readonly static string DefaultDateComparison = "{0:yyyy-MM-dd HH:mm:ss.FFFF}";
        /// <summary>
        /// the DateTime will be compared down to the second
        /// </summary>
        public readonly static string ResolutionSecondsDateComparison = "{0:yyyy-MM-dd HH:mm:ss}";
        /// <summary>
        /// the DateTime will be compared down to the minute
        /// </summary>
        public readonly static string ResolutionMinutesDateComparison = "{0:yyyy-MM-dd HH:mm}";
        /// <summary>
        /// Date only comparision
        /// </summary>
        public readonly static string DateOnlyComparison = "{0:yyyy-MM-dd}";
        /// <summary>
        /// time only comparision
        /// </summary>
        public readonly static string TimeOnlyComparison = "{0:HH:mm:ss}";
        #endregion


        #region properties
        /// <summary>
        /// Read only property
        /// returns the legal minimin default value for a DateTime
        /// </summary>
        public static DateTime MinDefaultValidDate
        {
            get { return DateTimeValidation.GetMinDefaultValidDate(); }
        }

        /// <summary>
        /// Read only property
        /// returns the legal maximum default value for a DateTime
        /// </summary>
        public static DateTime MaxDefaultValidDate
        {
            get { return DateTimeValidation.GetMaxDefaultValidDate(); }
        }

        /// <summary>
        /// returns Today at 12:00:00 AM
        /// </summary>
        public static DateTime StartOfToday
        {
            get { return DateTime.Now.Date; }
        }

        /// <summary>
        /// return Today at 11:59:59 PM
        /// </summary>
        public static DateTime EndOfToday
        {
            // date gets us today at 12:00:00 am, adding 1 day and substracting 1
            // millisecond gets us to today at 11:59:59 PM
            get { return DateTime.Now.Date.AddDays(1).AddMilliseconds(-1); }
        }
        #endregion

        #region private methods
        /// <summary>
        /// property helper method for returning the max default legal value
        /// </summary>
        /// <returns>DateTime</returns>
        private static DateTime GetMaxDefaultValidDate()
        {
            DateTimeValidation.Initialize();

            return DateTimeValidation._maxDefaultValidDate;
        }

        /// <summary>
        /// propertye helper method for returning the min default legal value
        /// </summary>
        /// <returns>DateTime</returns>
        private static DateTime GetMinDefaultValidDate()
        {
            DateTimeValidation.Initialize();

            return DateTimeValidation._minDefaultValidDate;
        }

        /// <summary>
        /// Ensures the date values have been properly initialized
        /// </summary>
        private static void Initialize()
        {
            if (true == DateTimeValidation._isInitialized)
                return;

            string minDefaultDate = ConfigurationManagerUtility.GetAppSettingString("MinDefaultValidDate", "1/1/1970");
            string maxDefaultDate = ConfigurationManagerUtility.GetAppSettingString("MaxDefaultValidDate", "12/31/2021");

            _maxDefaultValidDate = DateTime.Parse(maxDefaultDate);
            _minDefaultValidDate = DateTime.Parse(minDefaultDate);

            DateTimeValidation._isInitialized = true;

        }
        #endregion

        #region static ctor
        /// <summary>
        /// 
        /// </summary>
        static DateTimeValidation()
        {
            DateTimeValidation._isInitialized = false;
        }
        #endregion

        /// <summary>
        /// Determines if a DateTime is valid using the default values/rules 
        /// </summary>
        /// <param name="date">DateTime</param>
        /// <returns>bool, true if the date is within the ranges</returns>
        public static bool IsDateValidDefault(DateTime date)
        {
            bool isDateValid = false;

            DateTimeValidation.Initialize();

            if ((DateTimeValidation.MaxDefaultValidDate >= date) && (DateTimeValidation.MinDefaultValidDate <= date))
                isDateValid = true;

            return isDateValid;
        }

        /// <summary>
        /// Checks to see if a date is a number of days into the future without considering the
        /// time portion of the DateTime field.   
        /// 
        /// In other words if date = 8/8/08 and Now = 8/7/08 and daysExpected = 1 then date the 
        /// test is true
        /// 
        /// If date = 8/8/08 and Now = 8/7/08 and daysExpected = 2 then date the 
        /// test is false
        /// </summary>
        /// <param name="date">DateTime, date to check</param>
        /// <param name="daysExpected">int, min number of days date should be in the future</param>
        /// <returns>true if the input date is daysExpected into the future</returns>
        public static bool IsDateDaysInFuture(DateTime date, int daysExpected)
        {
            if (0 >= daysExpected)
            {
                Diagnostics.Instance.Assert(false, "daysExpected is not a positive number");
                return false;
            }

            DateTime nowDate = DateTime.Now.Date;

            DateTime inputDate = date.Date;

            TimeSpan dateDiff = new TimeSpan(inputDate.Ticks - nowDate.Ticks);

            if (dateDiff.Days >= daysExpected)
                return true;

            // return false because 
            return false;
        }

        /// <summary>
        /// Date comparision helper method.  Performs comparision down to the millisecond level.
        /// 
        /// Expected use is to compare dates retrieved from the database.
        /// There is a problem in that two dates read from the database that might visually appear to be
        /// same do not evaulate to equal when used like date1 == date2.   This has to do with
        /// some anomly withing the database or .NET framework not the starfish code.  
        /// </summary>
        /// <param name="date1">DateTime</param>
        /// <param name="date2">DateTime</param>
        /// <returns>bool, true if the dates and the times down the milliseconds are the same</returns>
        public static bool AreDatesEqual(DateTime date1, DateTime date2)
        {
            // as long as we get full date and full time down to
            // the millisecond (which always correct) then
            // the format of the strings doesnt matter for comparisions as 
            return DateTimeValidation.AreDatesEqual(date1, date2, DateTimeValidation.DefaultDateComparison);
        }

        /// <summary>
        /// Date comparision helper method.  Performs comparision down to the millisecond level.
        /// 
        /// Expected use is to compare dates retrieved from the database.
        /// There is a problem in that two dates read from the database that might visually appear to be
        /// same do not evaulate to equal when used like date1 == date2.   This has to do with
        /// some anomly withing the database or .NET framework not the starfish code.  
        /// </summary>
        /// <param name="date1">DateTime</param>
        /// <param name="date2">DateTime</param>
        /// <param name="comparisonMethod">string, C# formatter format used to build string for how the dates are compared</param>
        /// <returns>bool, true if the dates and the times down the milliseconds are the same</returns>
        public static bool AreDatesEqual(DateTime date1, DateTime date2, string comparisonMethod)
        {
            // as long as we get full date and full time down to
            // the millisecond (which always correct) then
            // the format of the strings doesnt matter for comparisions as 
            string date1Str = string.Format(comparisonMethod, date1);
            string date2Str = string.Format(comparisonMethod, date2);

            if (0 == string.Compare(date1Str, date2Str))
                return true;

            return false;
        }

        /// <summary>
        /// Determines if the first date is less than the second date.  Performs comparision down to the millisecond level.
        /// 
        /// Expected use is to compare dates retrieved from the database.
        /// There is a problem in that two dates read from the database that might visually appear to be
        /// same do not evaulate to equal when used like date1 == date2.   This has to do with
        /// some anomly withing the database or .NET framework not the starfish code.  
        /// </summary>
        /// <param name="date1">DateTime</param>
        /// <param name="date2">DateTime</param>
        /// <returns>bool, true if the first date dates less than the second date</returns>
        public static bool IsDateBeforeDate(DateTime date1, DateTime date2)
        {
            // as long as we get full date and full time down to
            // the millisecond (which always correct) then
            // the format of the strings doesnt matter for comparisions as 
            return DateTimeValidation.IsDateBeforeDate(date1, date2, DateTimeValidation.DefaultDateComparison);
        }

        /// <summary>
        /// Determines if the first date is less than the second date. details of comparison based on comparison method format string passed in.
        /// 
        /// Expected use is to compare dates retrieved from the database.
        /// There is a problem in that two dates read from the database that might visually appear to be
        /// same do not evaulate to equal when used like date1 == date2.   This has to do with
        /// some anomly withing the database or .NET framework not the starfish code.  
        /// </summary>
        /// <param name="date1">DateTime</param>
        /// <param name="date2">DateTime</param>
        /// <param name="comparisonMethod">string, C# formatter format used to build string for how the dates are compared</param>
        /// <returns>bool, true if the first date dates less than the second date</returns>
        public static bool IsDateBeforeDate(DateTime date1, DateTime date2, string comparisonMethod)
        {
            // as long as we get full date and full time down to
            // the millisecond (which always correct) then
            // the format of the strings doesnt matter for comparisions as 
            string date1Str = string.Format(comparisonMethod, date1);
            string date2Str = string.Format(comparisonMethod, date2);

            if (-1 == string.Compare(date1Str, date2Str))
                return true;

            return false;
        }
    }
}
