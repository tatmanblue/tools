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
	/// StringHelper contains a "bunch" of methods that help manipulate
	/// strings.  The methods kinda extend the string class, but since
	/// it's sealed, we cannot inherit from it
	/// </summary>
	public class StringHelper
	{
        private StringHelper() {}

        /// <summary>
        /// Given a sentence, this method reverses the order of the words
        /// in the sentence.  For example, "I work for money" will be returned
        /// as "money for work I".
        /// </summary>
        /// <param name="start">string</param>
        /// <returns>string</returns>
        static public string ReverseSentence(string start)
        {
            System.Text.StringBuilder ret = new System.Text.StringBuilder();
            
            // reverse the input and split it into words by spaces
            // if the input was "I work for money" the result from ReverseString
            // would be "yenom rof krow I".  These get split into individual words.
            string[] words = ReverseString(start).Split(new char[] {' '});

            // for each word found, reverse it and add it to the string            
            foreach(string word in words)
            {
                ret.Append(ReverseString(word));
                ret.Append(" ");
            }

            // the work is done, return the results
            return ret.ToString().TrimEnd();
        }

        /// <summary>
        /// Reverses input.  For example "work" is returned as "krow".  
        /// This method does not parse the input into words.  
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>string</returns>
        static public string ReverseString(string str)
        {
            int end = str.Length - 1;
            int start = 0;

            // turn the string into a chararray so that we
            // can work with each char individually
            char[] array = str.ToCharArray();


            // working from start to end and
            // from end to start, until they
            // meet, swap chars 
            while (start < end)
            {                
                char s = array[start];
                char e = array[end];

                array[start] = e;
                array[end] = s;

                start ++;
                end --;
            }

            // return the results
            return new string(array);
        }

        /// <summary>
        /// Cheezy hack function to make the first letter of string lowercase.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string LowerCaseFirstChar(string input)
        {
            char[] text = input.ToCharArray();
            text[0] = char.ToLower(text[0]);

            return new string(text);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string HtmlEncode(string text)
        {
            char[] chars = text.ToCharArray();

            StringBuilder result = new StringBuilder(text.Length + (int)(text.Length * 0.1));

            foreach (char c in chars)
            {
                int value = Convert.ToInt32(c);
                if (value > 127)
                    result.AppendFormat("&#{0};", value);
                else
                    result.Append(c);
            }

            return result.ToString();
        }
    }
}
