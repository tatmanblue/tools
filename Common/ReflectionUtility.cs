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
    /// Utility class for providing reflection based functionality 
    /// </summary>
    public static class ReflectionUtility
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TClassInstanceType"></typeparam>
        /// <returns></returns>
        public static TClassInstanceType InvokeDefaultCtor<TClassInstanceType>()
        {
            object theThing = null;
            Type theThingType = typeof(TClassInstanceType);
            ConstructorInfo ctorInfo = theThingType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { }, null);

            theThing = ctorInfo.Invoke(null);

            return (TClassInstanceType)theThing;
        }

        /// <summary>
        /// Use to set the value of a value type private member 
        /// </summary>
        /// <typeparam name="TClassInstanceType">Class type</typeparam>
        /// <typeparam name="TDesiredValueType">type of the value setting</typeparam>
        /// <param name="classInstance"></param>
        /// <param name="variableName">string</param>
        /// <param name="desiredValue"></param>
        public static void SetPrivateMemberValueType<TClassInstanceType, TDesiredValueType>(ref TClassInstanceType classInstance, string variableName, TDesiredValueType desiredValue)
        {
            Type classType = classInstance.GetType();
            FieldInfo fieldInfo = classType.GetField(variableName, BindingFlags.NonPublic | BindingFlags.Instance);

            if (null != fieldInfo)
            {
                object ob = classInstance;
                fieldInfo.SetValue(ob, desiredValue);
                classInstance = (TClassInstanceType)ob;
            }
        }

        /// <summary>
        /// Use to set the value of a value type private member 
        /// </summary>
        /// <typeparam name="TClassInstanceType">Class type</typeparam>
        /// <param name="classInstance"></param>
        /// <param name="variableName">string</param>
        public static object GetPrivateMemberValueType<TClassInstanceType>(ref TClassInstanceType classInstance, string variableName)
        {
            object retVal = null;
            Type classType = classInstance.GetType();
            FieldInfo fieldInfo = classType.GetField(variableName, BindingFlags.NonPublic | BindingFlags.Instance);

            if (null != fieldInfo)
            {
                object ob = classInstance;
                retVal = fieldInfo.GetValue(ob);
            }

            return retVal;
        }

        /// <summary>
        /// Use to set the value of a value type property that is not public
        /// </summary>
        /// <typeparam name="TClassInstanceType">Class type</typeparam>
        /// <typeparam name="TDesiredValueType">type of the value setting</typeparam>
        /// <param name="classInstance"></param>
        /// <param name="variableName">string</param>
        /// <param name="desiredValue"></param>
        public static void SetPrivatePropertyValueType<TClassInstanceType, TDesiredValueType>(ref TClassInstanceType classInstance, string variableName, TDesiredValueType desiredValue)
        {
            Type classType = classInstance.GetType();
            PropertyInfo propertyInfo = classType.GetProperty(variableName, BindingFlags.NonPublic | BindingFlags.Instance);

            if (null != propertyInfo)
            {
                propertyInfo.SetValue(classInstance, desiredValue, null);
            }
        }

        /// <summary>
        /// Use to set the value of a value type property that is public
        /// </summary>
        /// <typeparam name="TClassInstanceType">Class type</typeparam>
        /// <typeparam name="TDesiredValueType">type of the value setting</typeparam>
        /// <param name="classInstance"></param>
        /// <param name="propertyName">string</param>
        /// <param name="desiredValue"></param>
        public static void SetPropertyValueType<TClassInstanceType, TDesiredValueType>(ref TClassInstanceType classInstance, string propertyName, TDesiredValueType desiredValue)
        {
            Type classType = classInstance.GetType();
            PropertyInfo propertyInfo = classType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

            if (null != propertyInfo)
            {
                propertyInfo.SetValue(classInstance, desiredValue, null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="InvalidProgramException">InvalidProgramException</exception>
        /// <typeparam name="TClassInstanceType"></typeparam>
        /// <param name="classType"></param>
        /// <param name="methodName"></param>
        /// <param name="methodParams"></param>
        /// <returns></returns>
        public static TClassInstanceType InvokeInternalStaticMethod<TClassInstanceType>(string methodName, Type classType, object[] methodParams) 
        {
            MethodInfo loadMethod = classType.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.IgnoreCase);

            if (null != loadMethod)
            {
                TClassInstanceType classInstance = (TClassInstanceType)loadMethod.Invoke(null, methodParams);
                return classInstance;
            }

            throw new InvalidProgramException(string.Format("InvokeInternalStaticMethod failed to invoke {0}", methodName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="InvalidProgramException">InvalidProgramException</exception>
        /// <typeparam name="TReturnType"></typeparam>
        /// <param name="classInstance"></param>
        /// <param name="methodName"></param>
        /// <param name="methodParams"></param>
        /// <returns>MethodInfo</returns>
        public static TReturnType InvokeInternalMethod<TReturnType>(string methodName, object classInstance, object[] methodParams)
        {
            Type classType = classInstance.GetType();
            MethodInfo expectedMethod = classType.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.IgnoreCase | BindingFlags.Instance);

            if (null != expectedMethod)
            {
                TReturnType something = (TReturnType)expectedMethod.Invoke(classInstance, methodParams);
                return something;
            }

            throw new InvalidProgramException(string.Format("InvokeInternalMethod failed to invoke {0}", methodName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="InvalidProgramException">InvalidProgramException</exception>
        /// <param name="classInstance"></param>
        /// <param name="methodName"></param>
        /// <param name="methodParams"></param>
        /// <returns>MethodInfo</returns>
        public static MethodInfo InvokeInternalMethod(string methodName, object classInstance, object[] methodParams)
        {
            Type classType = classInstance.GetType();
            MethodInfo expectedMethod = classType.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.IgnoreCase | BindingFlags.Instance);

            if (null != expectedMethod)
            {
                expectedMethod.Invoke(classInstance, methodParams);
                return expectedMethod;
            }

            throw new InvalidProgramException(string.Format("InvokeInternalMethod failed to invoke {0}", methodName));
        }
    }
}
