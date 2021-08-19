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
    #region CommandLineArgument class implementation
    /// <summary>
    /// This class represents a command line parameter (argument) that requires an option to
    /// set.  For example /? is a commandline argument that is valid for this class
    /// 
    /// Used in conjuction with a ApplicationCommandLine instance
    /// </summary>
    public class CommandLineArgument
    {
        #region private data
        /// <summary>
        /// this is the argument identifer without the - or / 
        /// it can be anything, such as ? o overwrite, as long as its a single
        /// "word" (since spaces delimit command line arguments)
        /// </summary>
        private string _argument = "";
        /// <summary>
        /// this is text that will be printed when help is needed
        /// </summary>
        private string _helpText = "";
        /// <summary>
        /// indicates if this argument was chosen (found in the command line)
        /// </summary>
        private bool _selected = false;
        /// <summary>
        /// an id as assigned by user, its expected to be unique but there is no enforcement 
        /// of that rule in the command line classes.  It comes in really handy for 
        /// handling word style arguments like -table
        /// </summary>
        private object _id = -1;
        #endregion

        #region private/protected methods
        /// <summary>
        /// sets the value for the _selected memeber
        /// made as a function so that derived classes 
        /// dont have to reimplement the Selected property
        /// but instead reimplement this function as needed
        /// </summary>
        /// <param name="state">bool, selected state</param>
        protected virtual void SetSelected(bool state)
        {
            _selected = state;
        } 
        #endregion

        #region ctor/init/cleanup
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="arg">string, argument identifier such as ?</param>
        /// <param name="help">string, text to help the user understand the meaninging of the argument</param>
        public CommandLineArgument(string arg, string help) : this(arg, help, null) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="arg">string, argument identifier such as ?</param>
        /// <param name="help">string, text to help the user understand the meaninging of the argument</param>
        /// <param name="id"></param>
        public CommandLineArgument(string arg, string help, object id)
        {
            _argument = arg;
            _helpText = help;
            _id = id;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="arg">string, argument identifier such as ?</param>
        /// <param name="help">string, text to help the user understand the meaninging of the argument</param>
        /// <param name="state">bool, selected state</param>
        public CommandLineArgument(string arg, string help, bool state) : this(arg, help, state, null) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="arg">string, argument identifier such as ?</param>
        /// <param name="help">string, text to help the user understand the meaninging of the argument</param>
        /// <param name="state">bool, selected state</param>
        /// <param name="id">type user defined, intended to give user ability to progamatically idenity this instance, 
        /// suggested use is user defined enums</param>
        public CommandLineArgument(string arg, string help, bool state, object id)
        {
            _argument = arg;
            _helpText = help;
            _selected = state;
            _id = id;
        } 
        #endregion

        #region public methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return _helpText;
        } 
        #endregion

        #region properties
        /// <summary>
        /// Property:  the argument identifer without the - or / 
        /// it can be anything, such as ? o overwrite, as long as its a single
        /// "word" (since spaces delimit command line arguments)
        /// </summary>
        public string Argument
        {
            get { return _argument; }
            set { _argument = value; }
        }

        /// <summary>
        /// Property: text that will be printed when help is needed
        /// </summary>
        public string HelpText
        {
            get { return _helpText; }
            set { _helpText = value; }
        }

        /// <summary>
        /// Property: indicates if this argument was chosen (found in the command line)
        /// </summary>
        public bool Selected
        {
            get { return _selected; }
            set { SetSelected(value); }
        }

        /// <summary>
        /// an id as assigned by class consumer. its expected to be unique but there is no enforcement 
        /// of that rule in the command line classes.  It comes in really handy for 
        /// handling word style arguments like -table
        /// </summary>
        public object Id
        {
            get { return _id; }
            set { _id = value; }
        }
        #endregion
    }
    #endregion

    #region SwitchableCommandLineArgument class implementation
    /// <summary>
    /// Switchable arguments are arguments that are exclusive of on another--for example
    /// lighton and lightoff would be switchable because light cannot be both on and off
    /// at the same time
    /// </summary>
    public class SwitchableCommandLineArgument : CommandLineArgument
    {
        /// <summary>
        /// This is the counter point to the argument 
        /// digging into this can get recurvise and nasty
        /// </summary>
        SwitchableCommandLineArgument _switchArg = null;

        /// <summary>
        /// should only be called from SetSelected
        /// it is communicated from a related SwitchableCommandLineArgument change
        /// in state
        /// </summary>
        /// <param name="state">bool, selected state</param>
        protected void StateChangeTogglesRelativesState(bool state)
        {
            base.SetSelected(state);
        }

        /// <summary>
        /// Overides base class so that the related SwitchableCommandLineArgument member _switchArg
        /// selected state can be toggled to the opposite of this instance state will be 
        /// </summary>
        /// <param name="state">bool, selected state</param>
        protected override void SetSelected(bool state)
        {
            base.SetSelected(state);
            if (null != _switchArg)
            {
                // dont want to call _switchArg.SetState(!state) because that would causes
                // an ending recursion problem
                _switchArg.StateChangeTogglesRelativesState(!state);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="arg">string (parent), argument identifier such as ?</param>
        /// <param name="help">string (parent), text to help the user understand the meaninging of the argument</param>
        /// <param name="switchArg">Another argument that is exclusively tied to this argument</param>
        public SwitchableCommandLineArgument(string arg, string help, SwitchableCommandLineArgument switchArg)
            :
           base(arg, help)
        {
            _switchArg = switchArg;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="help"></param>
        public SwitchableCommandLineArgument(string arg, string help) : base(arg, help) { }

        /// <summary>
        /// 
        /// </summary>
        public SwitchableCommandLineArgument SwitchArg
        {
            get { return _switchArg; }
            set { _switchArg = value; }
        }
    }
    #endregion

    #region DataCommandLineArgument implementation
    /// <summary>
    /// This class is for command line arguments that receive input after them
    /// such as -f myfile.txt 
    /// the argument is f and the data is myfile.txt
    /// all data is kept as text, users must decide how to translate it
    /// </summary>
    public class DataCommandLineArgument : CommandLineArgument
    {
        /// <summary>
        /// identifies if data was found
        /// </summary>
        private bool _hasData = false;
        /// <summary>
        /// the data its self
        /// </summary>
        private string _data = "";
        /// <summary>
        /// When true indicates that the argument is expected at a specific position
        /// in the argument chain.  fixed position arguments are expected to come at the front
        /// of the chain and are processed in the order they are added to array
        /// </summary>
        private bool _isFixedPosition = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="help"></param>
        public DataCommandLineArgument(string arg, string help) : base(arg, help, null) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="help"></param>
        /// <param name="id"></param>
        public DataCommandLineArgument(string arg, string help, object id)
            : base(arg, help, id) {}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">default data for the argument</param>
        /// <param name="arg"></param>
        /// <param name="help"></param>
        public DataCommandLineArgument(string data, string arg, string help)
            : base(arg, help, null)
        {
            _hasData = true;
            _data = data;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Data
        {
            get { return _data; }
            set
            { 
                _data = value;
                if (false == string.IsNullOrEmpty(value))
                    _hasData = true;
                else
                    _hasData = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool HasData
        {
            get { return _hasData; }
            set
            {
                _hasData = value;
                if (false == _hasData)
                    _data = "";
            }
        }

        /// <summary>
        /// True indicates this argument is expected at a certain position
        /// in the argument list.  The position is assumed to be the position
        /// in this class instance in the array (aka order it was added)
        /// </summary>
        public bool IsFixedPosition
        {
            get { return _isFixedPosition; }
            set { _isFixedPosition = value; }
        }

    }
    #endregion

    #region CommandListArgumentList implementation
    /// <summary>
    /// </summary>
    public class CommandListArgumentList : List<CommandLineArgument> { }
    #endregion

    #region FixedPositionArgumentList implementation
    /// <summary>
    /// </summary>
    public class FixedPositionArgumentList : List<DataCommandLineArgument> { }
    #endregion
}
