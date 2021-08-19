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
    /// static class containing utility functions for loading assemblies and
    /// loading types from an assembly and other reflection related tasks associated
    /// with assemblies
    /// </summary>
    public static class AssemblyUtility
    {
        /// <summary>
        /// Attempts to load assembly by the file name, if that fails, attempts to load it
        /// from the appdomain root path
        /// </summary>
        /// <param name="assemblyFileName">string</param>
        /// <returns>Assembly</returns>
        public static Assembly LoadFile(string assemblyFileName)
        {
            Assembly loadedFile = null;

            try
            {
                loadedFile = Assembly.LoadFrom(assemblyFileName);
            }
            catch (System.IO.FileNotFoundException)
            {
                string assemblyInBasePath = string.Format("{0}{1}", AppDomain.CurrentDomain.BaseDirectory, assemblyFileName);
                loadedFile = Assembly.LoadFile(assemblyInBasePath);
            }

            return loadedFile;
        }

        /// <summary>
        /// Attempts to laod a type from the assembly, if that fails, building a namespace type name and attempting to load the
        /// generated name.  it assume the type name is not namespace qualified and assumes the assembly name can be used as the namespace.
        /// </summary>
        /// <param name="assembly">Assembly</param>
        /// <param name="typeName">string</param>
        /// <returns>Type</returns>
        public static Type GetType(Assembly assembly, string typeName)
        {
            Type loadedType = null;

            loadedType = assembly.GetType(typeName);

            // if laodedType is null, assume that typeName is not a fully namespaced qualified type
            // so we will string building our own type name and try loading that
            if (null == loadedType)
            {
                string builtTypeName = string.Format("{0}.{1}", assembly.GetName().Name, typeName);
                loadedType = assembly.GetType(builtTypeName, true);
            }

            return loadedType;
        }

        /// <summary>
        /// Loads a Type from an assembly (combination of calling AssemblyUtility.LoadFile and
        /// AssemblyUtility.GetType).  Be prepared to handle standard .NET exceptions
        /// </summary>
        /// <param name="assemblyFilePathName">string, path name + file name of assembly expected to contain the type</param>
        /// <param name="typeName">string, name of the type to load</param>
        /// <returns>Type</returns>
        public static Type GetTypeFromFile(string assemblyFilePathName, string typeName)
        {
            Assembly assembly = AssemblyUtility.LoadFile(assemblyFilePathName);
            return AssemblyUtility.GetType(assembly, typeName);
        }

        /// <summary>
        /// Loads a type from an assembly.  If the assemblyFileName is empty or not provide will load it
        /// from the app domain
        /// </summary>
        /// <param name="assemblyFileName">string, can be empty if you want to load whats already in the app domain</param>
        /// <param name="className">string, full namespace class name</param>
        /// <param name="typeType">type instance of BaseClass or specific class type</param>
        /// <exception cref="TypeLoadException">TypeLoadException if the type cannot be loaded</exception>
        /// <returns>Type</returns>
        public static Type GetTypeSmartLoad(string assemblyFileName, string className, Type typeType)
        {
            Assembly logicAssembly = null;
            Type customLogType = null;

            try
            {
                // start by loading our assembly information, if the source does
                // not specify an assembly assume its in the startfish business logic assembly
                // otherwise load it from the specified assembly
                if (true == string.IsNullOrEmpty(assemblyFileName))
                {
                    logicAssembly = Assembly.GetAssembly(typeType);
                }
                else
                {
                    logicAssembly = AssemblyUtility.LoadFile(assemblyFileName);

                }

                // this could be a runtime error
                if (null == logicAssembly)
                {
                    Diagnostics.Instance.Assert(null != logicAssembly);
                    throw new TypeLoadException(string.Format("'{0}' could not be loaded", assemblyFileName));
                }

                // get the business object type information
                customLogType = logicAssembly.GetType(className, true, true);
            }
            catch (Exception ex)
            {
                string msg = string.Format("Failed to load '{0} from assembly '{1}'.  Check configuration and install.  If this exception occurred in a Message workflow, an error contract will be returned to client.", className, assemblyFileName);
                Diagnostics.Instance.AssertableLoggedMessage(msg);

                throw ex;
            }

            return customLogType;
        }

        /// <summary>
        /// Creates an instance of Type calling the default constructor.  It is the responsibility
        /// of the caller of this method to make sure the constructor was called.
        /// </summary>
        /// <param name="objectType">Type</param>
        /// <returns>object, exact instance type = input type, null if a default constructor could not be found</returns>
        public static object InvokeDefaultCtor(Type objectType)
        {
            // excecute ctor
            // now we can get the default, parameterless constructor
            ConstructorInfo defaultCtor = objectType.GetConstructor(new Type[] { });

            // a default public constructor was not found, lets go crazy and see what else
            // can bring up in place
            if (null == defaultCtor)
                defaultCtor = objectType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { }, null);

            if (null == defaultCtor)
            {
                string msg = string.Format("Attempted to instantiate object type {0} with a default CTOR when none is present.", objectType.Name);
                Diagnostics.Instance.AssertableLoggedMessage(msg);
                return null;
            }

            // create the business object using the constructor we found
            return (defaultCtor.Invoke(null));
        }

        /// <summary>
        /// Queries the type's interfaces to see if one matches the input
        /// </summary>
        /// <param name="interfaceType">Type, the type of the Interface you are looking for</param>
        /// <param name="implementingType">Type, the type expected to implement the inferface</param>
        /// <returns>true if implementingType implements interfaceType</returns>
        public static bool HasInterface(Type interfaceType, Type implementingType)
        {
            Type[] interfaces = implementingType.GetInterfaces();

            // evaluate the implemented types
            foreach (Type type in interfaces)
            {
                // if they are the same, we got a match
                if (type == interfaceType) return true;
            }

            return false;
        }


        /// <summary>
        /// For the given type, checks to see if the attribute has been applied to it
        /// </summary>
        /// <param name="attributeType">Type</param>
        /// <param name="implementingType">Type</param>
        /// <returns>true if the attribute has been applied to it</returns>
        public static bool HasAttribute(Type attributeType, Type implementingType)
        {
            // get an array of tributes that match the input type
            object[] attributes = implementingType.GetCustomAttributes(attributeType, false);

            if (0 < attributes.Length)
                return true;

            return false;
        }
    }
}
