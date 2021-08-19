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
using System.Collections;
using System.Collections.Specialized;
#endregion

namespace BigWoo.Common
{
    #region ApplicationCommandLine Class Implementation
    /// <summary>
    /// The purpose of the ApplicationCommandLine class is to allow application
    /// programmers the ability to avoid having to parse the command line,
    /// handling all of the complexities of the commandline, and allow them
    /// to focus on the data given in the command line.
    /// 
    /// The ApplicationCommandLine class maps program defined data (instances of 
    /// CommandLineArgument) to actual command line input (thats the Main(String[] args) 
    /// input).  
    /// 
    /// For the purpose of this class, there are two types inputs recieved on the
    /// command line.   Arguments that are deliniated with a delimter such as -, and
    /// arguments that are not deliniated.
    /// 
    /// Arguments deliniated with a delimter are called cmdArgs and
    /// those that are not will be called fileArgs
    /// 
    /// One thing to note, not all undelinated arguments are fileArgs.  Some of them could be
    /// data for a cmdArg.  In such case they are called dataArgs.  The content for dataArgs are
    /// stored with cmdArg class instances if the cmdArg is an instance of DataCommandLineArgument
    /// 
    /// Example:  myprog -c -f notes.txt notes2.txt
    /// 
    /// -c is a cmdArg
    /// -f is a cmdArg with a dataArg
    /// notes.txt is the dataArg for -f
    /// notes2.txt is a fileArg
    /// 
    /// of course the above example assumes that -f is defined as a dataArg
    /// </summary>
    public class ApplicationCommandLine : IEnumerable, IComparer
    {
        #region private data
        /// <summary>
        /// identifies valid syntax for denoting a command line argument
        /// </summary>
        private string[] _validArgPrefixes = new string[] { "-", "/" };

        /// <summary>
        /// 
        /// </summary>
        private Hashtable _cmdArgs = new Hashtable();
        private CommandListArgumentList _cmdArgs2 = new CommandListArgumentList();
        private FixedPositionArgumentList _fixedCmdArgs = new FixedPositionArgumentList();
        /// <summary>
        /// 
        /// </summary>
        private StringCollection _fileArgs = new StringCollection();
        #endregion

        #region properties
        /// <summary>
        /// 
        /// </summary>
        public int ArgCount
        {
            get { return _cmdArgs.Count; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string[] ArgumentPrefixes
        {
            get { return _validArgPrefixes; }
            set { _validArgPrefixes = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public StringCollection FileArgs
        {
            get { return _fileArgs; }
            set { _fileArgs = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string[] HelpLines
        {
            get { return GetSortedHelpLines(); }
        }
        #endregion

        #region private methods
        private string[] GetSortedHelpLines()
        {
            int count = 0;
            int argLen = 0;
            int max = ArgCount;
            string[] helpLines = new string[max];
            string formatStr = string.Empty;

            // find the longest argument
            foreach (CommandLineArgument cmd in this)
            {
                argLen = (cmd.Argument.Length > argLen ? cmd.Argument.Length : argLen);
            }

            // one extra padding
            argLen++;

            // using that length to make a format string
            formatStr = string.Format("{0}0,{2}{1}  - {0}1{1}", "{", "}", argLen);

            // for each of the argments, make a string that will be shown
            // on the console using the prebuilt format string
            foreach (CommandLineArgument cmd in this)
            {
                System.Diagnostics.Debug.Assert(count < max, "array size doesnt match args");

                helpLines[count] = string.Format(formatStr, cmd.Argument, cmd.HelpText);
                count++;
            }

            // sort the lines
            Array.Sort(helpLines, this as IComparer);

            return helpLines;
        }

        /// <summary>
        /// search through the known acceptable argument prefixes and see if the 
        /// string input begins with one of them.
        /// 
        /// This search is case sensitive 
        /// </summary>
        /// <param name="arg"></param>
        /// <returns>true if input begins with one of the items in _validArgPrefixes</returns>
        private bool IsStartingWithArgPrefix(string arg)
        {
            bool ret = false;

            foreach (string prefix in _validArgPrefixes)
            {
                string possiblePrefix = arg.Substring(0, prefix.Length);

                if (0 == prefix.CompareTo(possiblePrefix))
                {
                    ret = true;
                    break;
                }
            }

            return ret;
        }

        #endregion

        #region public methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return _cmdArgs2.GetEnumerator();
        }


        #region IComparer Members
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(object x, object y)
        {
            string left = (string)x;
            string right = (string)y;

            left = left.Trim();
            right = right.Trim();

            return string.Compare(left, right);
        }

        #endregion

        /// <summary>
        /// Running through the array of commandline arguements
        /// compares them with _cmdArgs array and sets them
        /// to selected when found, and passes in data to them
        /// when the data is found
        /// </summary>
        /// <param name="args">string array, typically the string[] received from Main</param>
        public void ParseCmdLineToArgs(string[] args)
        {
            // if we didnt get anything we aint running
            if (0 == args.Length)
                throw new CommandlineException("no arguments received to parse");

            int count = 0;

            // allow for some arguments to be fixed in place.  we
            // "insist" these be at the begining of the list
            for (; count < _fixedCmdArgs.Count; count++)
            {
                if (count > args.Length)
                    break;

                DataCommandLineArgument nextArgument = _fixedCmdArgs[count];

                // the data is the argument itself not the next one in the list
                // it is assumed that fixed arguments do not have switchs (aka - or --)
                // or variable data after it.  Rather they are a fixed in position as
                // well as content of 1 word
                nextArgument.Data = args[count];
            }

            // go through each argument looking for ones that
            // start with something in _validArgPrefixes array
            // if is then find that in _cmdArgs
            for (; count < args.Length; count++)
            {
                // if the argument start with an argument prefix
                if (true == IsStartingWithArgPrefix(args[count]))
                {
                    // test to see if the argument is in our _cmdArgs hashtable

                    // to allow for arguments to have more than a single char
                    // eg:   -ag is a valid argument now
                    string argValue = args[count].Substring(1);

                    if (true == _cmdArgs.ContainsKey(argValue))
                    {
                        // it is, so get the object and process it
                        CommandLineArgument argument = (CommandLineArgument)_cmdArgs[argValue];

                        // set it to selected
                        argument.Selected = true;

                        // and if its a data argument
                        // get the data that should go with it
                        if (argument is DataCommandLineArgument)
                        {
                            count++;

                            // need to make sure there is some data to get
                            if (count >= args.Length)
                                throw new CommandlineException("expected additional parameters");

                            // and need to make sure the next in the array is not another
                            // argument
                            if (true == IsStartingWithArgPrefix(args[count]))
                                throw new CommandlineException("parameters not formatted as expected");

                            // we have data!
                            (argument as DataCommandLineArgument).Data = args[count];
                        }

                    }
                    else
                    {
                        // we have an invalid argument
                        throw new CommandlineException("invalid cmdArg received");
                    }
                }
                else
                {
                    // it doesnot start with an argument prefix so
                    // we assume this is a file name
                    _fileArgs.Add(args[count]);
                }

            }
        }

        /// <summary>
        /// just adds an argument our internal lists of arguments
        /// </summary>
        /// <param name="arg">CommandLineArgument, or one of its derived types</param>
        public void AddArg(CommandLineArgument arg)
        {
            // the old list
            _cmdArgs.Add(arg.Argument, arg);

            // all args go into this list
            _cmdArgs2.Add(arg);

            // now if the input is a DataCommandLineArgument and it
            // is indicated to be a fixed position argument then 
            // add it to the fixed position list as well
            if (arg is DataCommandLineArgument)
            {
                DataCommandLineArgument dataArg = arg as DataCommandLineArgument;
                if (true == dataArg.IsFixedPosition)
                    _fixedCmdArgs.Add(dataArg);
            }
        } 
        #endregion
    }
    #endregion

}
