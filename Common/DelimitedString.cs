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
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text; 
#endregion

namespace BigWoo.Common
{
    #region DelimitedString class implementation
    /// <summary>
    /// This class contains functionality for dealing with string that is delimited by
    /// some type of charactor (default is pipe symbol | )
    /// </summary>
    public class DelimitedString : IEnumerable<string>
    {
        #region interface handlers
        #region IEnumerable<string> Members

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<string> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
        #endregion

        #region object overrides
        /// <summary>
        /// 
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return _rawString;
        }
        #endregion

        #region private data
        private const char DEFAULT_DELIMITER = '|';

        private readonly char _delimiter;
        private readonly int _numOfFields;

        private string _rawString = string.Empty;
        private string[] _stringParts = null;
        private bool _isDirty = false;
        #endregion

        #region properties
        /// <summary>
        /// Indicates delimiter being used
        /// </summary>
        public char Delimiter { get { return _delimiter; } }

        /// <summary>
        /// Indicates the number of fields expected in the string
        /// </summary>
        public int NumberOfFields { get { return _numOfFields; } }

        /// <summary>
        /// The string assembled with all the parts, if you set this value you reset
        /// all of the "parts"
        /// </summary>
        public string RawString
        {
            get { GetRawStringFromArray(); return _rawString; }
            set { _rawString = value; SetArrayFromRawString(); _isDirty = true; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">int</param>
        /// <returns>string</returns>
        public string this[int index]
        {
            get { return GetField(index); }
            set { SetField(index, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDirty
        {
            get { return _isDirty; }
            set { _isDirty = value; }
        }
        #endregion

        #region private methods
        /// <summary>
        /// 
        /// </summary>
        private void InitializeArray()
        {
            for (int count = 0; count < _numOfFields; count++)
            {
                _stringParts[count] = string.Empty;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void GetRawStringFromArray()
        {
            StringBuilder builder = new StringBuilder();

            for (int count = 0; count < _numOfFields; count++)
            {
                if (0 < count)
                    builder.AppendFormat(" {0} ", _delimiter);

                builder.Append(_stringParts[count]);
            }

            _rawString = builder.ToString();
        }

        /// <summary>
        /// validates the key.  used on load to validate that it was called correctly.
        /// </summary>
        private void SetArrayFromRawString()
        {
            string[] stringParts = _rawString.Split(_delimiter);

            // If there is more members in the array than we expect then there is an initialization problem
            if (_numOfFields < stringParts.Length)
                throw new DelimitedStringException("DelimitedString stringParts length did not match expected _numOfFields");

            // assign our stuff over
            for (int count = 0; count < _numOfFields; count++)
            {
                // if count > stringParts.Length is true that means
                // we probably got RawString from the constructor and it's
                // smaller than expected result, we set the remaining fields
                // to empty string
                if (count >= stringParts.Length)
                {
                    _stringParts[count] = string.Empty;
                }
                else
                {
                    _stringParts[count] = stringParts[count].Trim();
                }
            }

        }
        #endregion

        #region ctors
        /// <summary>
        /// uses default delimiter of pipe, setting the number of fields expected to be found in the string
        /// </summary>
        public DelimitedString(int numberOfFields)
        {
            _numOfFields = numberOfFields;
            _delimiter = DEFAULT_DELIMITER;
            _stringParts = new string[_numOfFields];
            InitializeArray();
            GetRawStringFromArray();
        }

        /// <summary>
        /// uses default delimiter of pipe, setting the number of fields expected to be found in the string and 
        /// setting the input to a starting value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="numberOfFields"></param>
        public DelimitedString(int numberOfFields, string value)
            : this(numberOfFields)
        {
            _rawString = value;
            SetArrayFromRawString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delimiter"></param>
        /// <param name="numberOfFields"></param>
        public DelimitedString(char delimiter, int numberOfFields)
            : this(numberOfFields)
        {
            _delimiter = delimiter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delimiter"></param>
        /// <param name="value"></param>
        /// <param name="numberOfFields"></param>
        public DelimitedString(char delimiter, int numberOfFields, string value)
            : this(delimiter, numberOfFields)
        {
            _rawString = value;
            SetArrayFromRawString();
        }
        #endregion

        #region public methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetField(int index)
        {
            return _stringParts[index];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void SetField(int index, string value)
        {
            _stringParts[index] = value;
            _isDirty = true;
        }
        #endregion

    }
    #endregion

    #region DelimitedStringException implementation
    /// <summary>
    /// This exception is thrown by DelimitedString when it encounters an error
    /// </summary>
    public class DelimitedStringException : Exception
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="msg">string</param>
        public DelimitedStringException(string msg) : base(msg) { }
    }
    #endregion
}
