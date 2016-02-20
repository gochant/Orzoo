using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Microsoft.CSharp;

namespace Orzoo.Core.Utility
{
    public class RuntimeHelper
    {
        #region Methods

        #region Public Methods

        public static MethodInfo CreateFunction(string sourcecode, Type genericType = null)
        {
            var provider = new CSharpCodeProvider();
            var parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            parameters.GenerateExecutable = false;

            // 将当前域的程序集添加到编译器中
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    string location = assembly.Location;
                    if (!string.IsNullOrEmpty(location))
                    {
                        parameters.ReferencedAssemblies.Add(location);
                    }
                }
                catch (NotSupportedException)
                {
                    // this happens for dynamic assemblies, so just ignore it. 
                }
            }

            var results = provider.CompileAssemblyFromSource(parameters, sourcecode);

            var binaryFunction = results.CompiledAssembly.GetType("UserFunctions.BinaryFunction");

            var method = binaryFunction.GetMethod("Function");
            if (method.IsGenericMethod)
            {
                method = method.MakeGenericMethod(genericType);
            }
            return method;

            //public delegate int IncreaseFunc<T>(T data);
            //var functionMethodInfo = CreateIncreaseFunction(properties[j].Increase, typeof(T));
            //var increaseFunction = (IncreaseFunc<T>)Delegate.CreateDelegate(typeof(IncreaseFunc<T>), functionMethodInfo);
            //increase = increaseFunction(data);
        }

        public static T CreateFunction<T>(string sourcecode, Type genericType = null)
        {
            var methodInfo = CreateFunction(sourcecode, genericType);
            var func = (T) (object) Delegate.CreateDelegate(typeof (T), methodInfo);
            return func;
        }

        public static string Md5(string text)
        {
            byte[] encoded = new UTF8Encoding().GetBytes(text);
            byte[] hash = ((HashAlgorithm) CryptoConfig.CreateFromName("MD5")).ComputeHash(encoded);

            // string representation (similar to UNIX format)
            string result = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();

            return result;
        }

        /// <summary>
        /// 获取当前应用程序中某个类型的所有实现（或子级）
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>类型列表</returns>
        public static IEnumerable<Type> GetCurrentDomainImplementTypes(Type type)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(t => type.IsAssignableFrom(t) && type != t);

            return types;
        }

        public static object CallGenericMethod(object obj, string methodName, Type type, object[] parameters = null)
        {
            var method = obj.GetType().GetMethod(methodName);
            var generic = method.MakeGenericMethod(type);
            return generic.Invoke(obj, parameters);
        }

        #endregion

        #endregion
    }
}