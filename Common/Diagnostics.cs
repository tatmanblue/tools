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
using System.Diagnostics;
using System.Configuration; 
#endregion

namespace BigWoo.Common
{

    #region IDiagnosticsLogWriter definition
    /// <summary>
    /// Implement this interface to provide logging for your system.  It is your implementation
    /// that determines how logging is implemented and what is logged.   It is the Diagnostics 
    /// system (class instance) that will determine when logging is made.  
    /// </summary>
    public interface IDiagnosticsLogWriter
    {
        /// <summary>
        /// Interface initialization
        /// </summary>
        void Initialize();

        /// <summary>
        /// Log a string message
        /// </summary>
        /// <param name="msg">string</param>
        void LogMessage(string msg);

        /// <summary>
        /// Log an exception message
        /// </summary>
        /// <param name="ex">System.Exception</param>
        void LogException(Exception ex);

        /// <summary>
        /// shutdown, cleanup
        /// </summary>
        void Deinitialize();
    }

    /// <summary>
    /// 
    /// </summary>
    public class DiagnosticsLogWriterList : List<IDiagnosticsLogWriter> { }
    #endregion

    #region Diagnostics implementation
    /// <summary>
    /// This class is used by all components as a means of providing debugging
    /// and support to the application on errors.   It wraps asserts (allowing for
    /// asserts to dynamically turned on/off), 
    /// 
    /// The actual recording of the information is managed by implementations of 
    /// IDiagnosticsLogWriter
    /// </summary>
    public class Diagnostics
    {
        #region delegate definitions
        private delegate void LogMessageEvent(string msg);
        private delegate void LogExceptionEvent(Exception ex);
        #endregion

        #region private data
        private static Diagnostics _instance = null;
        private DiagnosticsLogWriterList _logWriters = new DiagnosticsLogWriterList();
        private bool _allowAsserts = false;
        private bool _allowLogging = true;
        private bool _traceAllMessages = true;
        private bool _debugAllMessages = true;

        private event LogMessageEvent OnLogMessage;
        private event LogExceptionEvent OnLogException;
        #endregion

        #region properties
        /// <summary>
        /// Instance property
        /// </summary>
        public static Diagnostics Instance
        {
            get
            {
                if (null == _instance)
                {
                    // if _instance is null, it most likely means that 
                    // the instance property was called during the construction
                    // of this class instance (aka in Diagnostics constructor code path)
                    // that is a problem.   Trace log it 
                    System.Diagnostics.Trace.WriteLine("Diagnostics _instance was null in the get property of Instance");
                }

                return _instance;
            }
        }
        #endregion

        #region private/protected methods
        /// <summary>
        /// So that our code is simple, we make each IDiagnosticsLogWriter subscribe to our
        /// OnLogMessage event on construction.  Now all we have to do is trigger the event, and each IDiagnosticsLogWriter
        /// instance will get notification to log a message
        /// </summary>
        /// <param name="msg">string</param>
        protected void FireMessageLogging(string msg)
        {
            if (false == _allowLogging)
                return;

            if (null != OnLogMessage)
                OnLogMessage(msg);
        }

        /// <summary>
        /// So that our code is simple, we make each IDiagnosticsLogWriter subscribe to our
        /// OnLogException event.  Now all we have to do is trigger the event, and each IDiagnosticsLogWriter
        /// instance will get notification to log a exception
        /// </summary>
        /// <param name="ex">System.Exception</param>
        protected void FireExceptionLogging(Exception ex)
        {
            if (false == _allowLogging)
                return;

            if (null != OnLogException)
                OnLogException(ex);
        }


        /// <summary>
        /// Reading the app config file information, spin up our log writers as well as set up system diagnostic 
        /// "permissions" and available functions
        /// </summary>
        protected void InitializeFromConfigFile()
        {
            try
            {
                string sectionName = "diagnostics";
                DiagnosticsConfigSection configSection = (DiagnosticsConfigSection)ConfigurationManager.GetSection(sectionName);

                if (null != configSection)
                {
                    _allowAsserts = configSection.AllowAsserts;
                    _allowLogging = configSection.AllowLogging;
                    _traceAllMessages = configSection.TraceAllMessages;

                    _logWriters.Clear();

                    foreach (LogWriterElement configElement in configSection.LogWriters)
                    {
                        // the expectation is InitializeFromConfigFile is called from the constructor
                        // so we dont want to initialize writers that have to be loaded later
                        if (true == configElement.DelayLoad)
                            continue;

                        CreateLoggerInstance(configElement.Assembly, configElement.LogWriterClassName);
                    }
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// allows derived classes to initialize accordingly
        /// </summary>
        protected void Initialize() { }      
        #endregion

        #region ctors
        /// <summary>
        /// let the static ctor intialize an instance of the class
        /// </summary>
        static Diagnostics()
        {
            _instance = new Diagnostics();
        }

        /// <summary>
        /// ctor
        /// </summary>
        private Diagnostics()
        {
            InitializeFromConfigFile();
        }
        #endregion

        #region public methods  
        /// <summary>
        /// Initializes a IDiagnosticsLogWriter implementation using the input assembly and class name.
        /// The IDiagnosticsLogWriter instance is "forced" to subscribe to our events so that it
        /// can properly receive logging requests
        /// </summary>
        /// <param name="assembly">string, of the assembly containing className</param>
        /// <param name="className">class name of IDiagnosticsLogWriter implementation, full name including namespace</param>
        public void CreateLoggerInstance(string assembly, string className)
        {
            try
            {
                IDiagnosticsLogWriter instance = null;
                Type interfaceType = typeof(IDiagnosticsLogWriter);

                Type logWriterType = AssemblyUtility.GetTypeFromFile(assembly, className);

                // only going to try spinning up the instance of the logWriterType if it actually
                // implemented IDiagnosticsLogWriter
                if (false == AssemblyUtility.HasInterface(interfaceType, logWriterType))
                {
                    string msg = string.Format("{0} does not implement {1}", logWriterType.Name, interfaceType.Name);

                    // try an assert, just in case its allowable as it will help identify a problem at runtime
                    Assert(false, msg);

                    // the only means we have of reliably noting that the config file contains information attempting
                    // to install a log writer that does not implement the IDiagnosticsLogWriter is via
                    // System.Diagnostics.Trace.    
                    System.Diagnostics.Trace.WriteLine(msg);
                    return;
                }

                // create the log writer instance
                instance = AssemblyUtility.InvokeDefaultCtor(logWriterType) as IDiagnosticsLogWriter;

                if (null == instance)
                {
                    string msg = string.Format("{0} did not construct", logWriterType.Name);

                    // try an assert, just in case its allowable as it will help identify a problem at runtime
                    Assert(false, msg);

                    // the only means we have of reliably noting that the config file contains information attempting
                    // to install a log writer that does not implement the IDiagnosticsLogWriter is via
                    // System.Diagnostics.Trace.    
                    System.Diagnostics.Trace.WriteLine(msg);
                    return;
                }

                // allow the instance to do their initialization, which may be nothing but
                // don't want to deny them that opportunity
                instance.Initialize();

                // and now subscribe to our notifications
                OnLogException += instance.LogException;
                OnLogMessage += instance.LogMessage;

                _logWriters.Add(instance);
            }
            catch (Exception ex)
            {
                string msg = string.Format("{0} exception.  {1}", ex.GetType().Name, ex.Message);
                System.Diagnostics.Trace.WriteLine(msg);
            }
        }

        /// <summary>
        /// when the boolean condition evaluates as false (same as System.Diagnotistics.Debug.Assert)
        /// the assert is displayed if the config file settings indicate asserts are permitted
        /// </summary>
        /// <param name="condition">boolean, when false Assert fires</param>
        [DebuggerStepThrough]
        public void Assert(bool condition)
        {
            Assert(condition, string.Empty);
        }

        /// <summary>
        /// when the boolean condition evaluates as false (same as System.Diagnotistics.Debug.Assert)
        /// the assert is displayed if the config file settings indicate asserts are permitted
        /// </summary>
        /// <param name="condition">boolean, when false Assert fires</param>
        /// <param name="message">string</param>
        [DebuggerStepThrough]
        public void Assert(bool condition, string message)
        {
            // since we are controlling when asserts actually appear we need check that asserts are permitted
            // and the condition is false
            if ((true == _allowAsserts) &&
                (false == condition))
            {
                // Attention developers....!!!!
                // tricky part is now figuring out where in the stack trace the actual assertable "problem"
                // was encountered.  Well, its pretty straight forward, just look back in the call stack 
                // one method.   To help, we print the method and source file name out in the 
                // debugger output window
                System.Diagnostics.Debug.Assert(condition, message);
            }
        }

        /// <summary>
        /// Logs a message and if asserts are permitted triggers an assert.  It is possible that exception
        /// is not logged if logging is turned off (not advisable).  
        /// </summary>
        /// <param name="msg">string</param>
        [DebuggerStepThrough]
        public void AssertableLoggedMessage(string msg)
        {
            FireMessageLogging(msg);

            if (true == _traceAllMessages)
            {
                TraceWrite(msg);
            }

            if (true == _allowAsserts)
            {
                // Attention developers....!!!!
                // tricky part is now figuring out where in the stack trace the actual assertable "problem"
                // was encountered.  Well, its pretty straight forward, just look back in the call stack 
                // one method.   To help, we print the method and source file name out in the 
                // debugger output window
                System.Diagnostics.Debug.Assert(false, msg);
            }

        }

        /// <summary>
        /// If the condition is truue, logs a message and if asserts are permitted triggers an assert.  It is possible that exception
        /// is not logged if logging is turned off (not advisable).  
        /// </summary>
        /// <param name="condition">bool, true to trigger asserts and logs (assuming these features are turned on)</param>
        /// <param name="msg">string</param>
        [DebuggerStepThrough]
        public void AssertableLoggedMessageIf(bool condition, string msg)
        {
            if (false == condition)
                return;

            FireMessageLogging(msg);

            if (true == _traceAllMessages)
            {
                TraceWrite(msg);
            }

            if (true == _allowAsserts)
            {
                // Attention developers....!!!!
                // tricky part is now figuring out where in the stack trace the actual assertable "problem"
                // was encountered.  Well, its pretty straight forward, just look back in the call stack 
                // one method.   To help, we print the method and source file name out in the 
                // debugger output window
                System.Diagnostics.Debug.Assert(false, msg);
            }

        }

        /// <summary>
        /// Logs an exception  and if asserts are permitted triggers an assert.  It is possible that exception
        /// is not logged if logging is turned off (not advisable).  
        /// </summary>
        /// <param name="ex">Exception</param>
        [DebuggerStepThrough]
        public void AssertableLoggedMessage(Exception ex)
        {
            // log the exception....
            FireExceptionLogging(ex);

            if (true == _traceAllMessages)
            {
                TraceWrite(ex.Message);
            }


            if (true == _allowAsserts)
            {
                // Attention developers....!!!!
                // tricky part is now figuring out where in the stack trace the actual assertable "problem"
                // was encountered.  Well, its pretty straight forward, just look back in the call stack 
                // one method.   To help, we print the method and source file name out in the 
                // debugger output window
                System.Diagnostics.Debug.Assert(false, ex.Message);
            }
        }

        /// <summary>
        /// Logs a message, if logging is enabled (strongly encouraged to keep logging enabled)
        /// </summary>
        /// <param name="msg">string</param>
        //[DebuggerStepThrough]
        public void LogMessage(string msg)
        {
            FireMessageLogging(msg);

            if (true == _traceAllMessages)
            {
                TraceWrite(msg);
            }

        }


        /// <summary>
        /// Logs a message, if logging is enabled (strongly encouraged to keep logging enabled) and
        /// the condition is true
        /// </summary>
        /// <param name="condition">bool, condition to be true for the message to be written</param>
        /// <param name="msg">string</param>
        [DebuggerStepThrough]
        public void LogMessageIf(bool condition, string msg)
        {
            if (false == condition)
                return;

            LogMessage(msg);
        }

        /// <summary>
        /// Logs an exception, if logging is enabled (strongly encouraged to keep logging enabled)
        /// </summary>
        /// <param name="ex">Exception</param>
        [DebuggerStepThrough]
        public void LogMessage(Exception ex)
        {
            FireExceptionLogging(ex);

            if (true == _traceAllMessages)
            {
                TraceWrite(ex.Message);
            }

        }

        /// <summary>
        /// A simple wrapper for the Trace.WriteLine, allowing for it to be turned off via
        /// our diagnostics config section.  This method does not fire off the loggers.  Only
        /// output is to the trace "window"
        /// </summary>
        /// <param name="msg">string</param>
        [DebuggerStepThrough]
        public void TraceWrite(string msg)
        {
            if (true == _traceAllMessages)
            {
                System.Diagnostics.Trace.WriteLine(msg);
            }
        }

        /// <summary>
        /// A simple wrapper for the Trace.WriteLine, allowing for it to be turned off via
        /// our diagnostics config section.  This method does not fire off the loggers.  Only
        /// output is to the trace "window"
        /// </summary>
        /// <param name="condition">condition to trigger the trace writing</param>
        /// <param name="msg">string</param>
        [DebuggerStepThrough]
        public void TraceWriteIf(bool condition, string msg)
        {
            if (false == condition)
                return;

            TraceWrite(msg);
        }

        /// <summary>
        /// A simple wrapper for the Debug.Writeline, allowing for it be turned off via
        /// our diagnostics config section.   This moethod does not fire off the loggers. Only
        /// output is the to the trace "window"
        /// </summary>
        /// <param name="msg">string</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void DebugWrite(string msg)
        {
            if (true == _debugAllMessages)
            {
                System.Diagnostics.Debug.WriteLine(msg);
            }
        }
        #endregion
    }
    #endregion

}
