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
using System.Diagnostics;
using System.Configuration;
#endregion

namespace BigWoo.Common
{
    /// <summary>
    /// This configuration file section initializes and controls the starfish diagnostic system.
    /// The starfish diagnostic system is intended to give the developer a toolset for documenting
    /// runtime monitoring.
    /// </summary>
    [DebuggerStepThrough]
    public class DiagnosticsConfigSection : ConfigurationSection
    {
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("logWriters")]
        public LogWriterElementCollection LogWriters
        {
            get { return ((LogWriterElementCollection)(base["logWriters"])); }
        }

        /// <summary>
        /// when true, diagnostics called with the Assert functions will assert
        /// Default setting is false
        /// </summary>
        [ConfigurationProperty("allowAsserts", DefaultValue = false, IsKey = false, IsRequired = false)]
        public bool AllowAsserts
        {
            get { return (bool)(base["allowAsserts"]); }
            set { base["allowAsserts"] = value; }
        }

        /// <summary>
        /// when true (default), permits logging (logging is implemented by implementing the IDiagnosticsLogWriter
        /// interface and declaring the class in logWriters section)
        /// </summary>
        [ConfigurationProperty("allowLogging", DefaultValue = true, IsKey = false, IsRequired = false)]
        public bool AllowLogging
        {
            get { return (bool)(base["allowLogging"]); }
            set { base["allowLogging"] = value; }
        }

        /// <summary>
        /// When true (default) all messages are sent via the trace system (System.Diagnostics.Trace)
        /// </summary>
        [ConfigurationProperty("traceAllMessages", DefaultValue = true, IsKey = false, IsRequired = false)]
        public bool TraceAllMessages
        {
            get { return (bool)(base["traceAllMessages"]); }
            set { base["traceAllMessages"] = value; }
        }

    }

    #region LogWriterElementCollection class implementation
    /// <summary>
    /// used to read in the list of assemblies which will be loaded for data contracts used to
    /// process messges found in the message queue
    /// </summary>
    [ConfigurationCollection(typeof(LogWriterElement))]
    [DebuggerStepThrough]
    public class LogWriterElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// for creating new elements
        /// </summary>
        /// <returns>AssemblyElement</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new LogWriterElement();
        }

        /// <summary>
        /// searchs the collection for a given AssemblyElement based on its key
        /// </summary>
        /// <param name="element">AssemblyElement</param>
        /// <returns>AssemblyElement</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((LogWriterElement)(element)).LogWriterClassName;
        }

        /// <summary>
        /// Indexor into the array
        /// </summary>
        /// <param name="idx">int</param>
        /// <returns>LogWriterElement</returns>
        public LogWriterElement this[int idx]
        {
            get
            {
                return (LogWriterElement)BaseGet(idx);
            }
        }
    }
    #endregion

    #region LogWriterElement configuration file element definition
    /// <summary>
    /// defines a IDiagnosticsLogWriter implementation that will be loaded by the starfish diagnostics system
    /// </summary>
    [DebuggerStepThrough]
    public class LogWriterElement : ConfigurationElement
    {

        /// <summary>
        /// the assembly file name, required
        /// </summary>
        [ConfigurationProperty("assembly", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Assembly
        {
            [DebuggerStepThrough]
            get
            {
                return ((string)(base["assembly"]));
            }
            [DebuggerStepThrough]
            set
            {
                base["assembly"] = value;
            }
        }

        /// <summary>
        /// class name of the IDiagnosticsLogWriter implementation to load, full class name including namespace
        /// </summary>
        [ConfigurationProperty("logWriterClassName", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string LogWriterClassName
        {
            [DebuggerStepThrough]
            get
            {
                return ((string)(base["logWriterClassName"]));
            }
            [DebuggerStepThrough]
            set
            {
                base["logWriterClassName"] = value;
            }
        }

        /// <summary>
        /// indicates the writer should not be loaded in the SystemDiagnostic constructor.  
        /// Elements that are delay loaded will be initialized by calling SystemDiagnostic.Instance.DelayLoadInitialization().
        /// It is up to the consumer of SystemDiagnostic engine to make sure they 
        /// actually call the delay load initialization routines
        /// </summary>
        [ConfigurationProperty("delayLoad", DefaultValue = false, IsKey = false, IsRequired = false)]
        public bool DelayLoad
        {
            [DebuggerStepThrough]
            get
            {
                return ((bool)(base["delayLoad"]));
            }
            [DebuggerStepThrough]
            set
            {
                base["delayLoad"] = value;
            }
        }
    }
    #endregion

}
