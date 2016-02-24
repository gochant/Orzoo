using System;
using System.Linq;
using System.Reflection;

namespace Orzoo.Core.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// 调用泛型方法（支持单个类型）
        /// </summary>
        /// <param name="obj">调用对象</param>
        /// <param name="methodName">方法名</param>
        /// <param name="t">类型</param>
        /// <param name="parameters">方法参数</param>
        /// <returns>方法的返回值</returns>
        public static object CallGenericMethod(this object obj, string methodName, Type t, object[] parameters = null)
        {
            var method = obj.GetType().GetMethods().FirstOrDefault(d => d.Name == methodName && d.IsGenericMethod);
            var generic = method?.MakeGenericMethod(t);
            return generic?.Invoke(obj, parameters);
        }

        /// <summary>
        /// 获取对象的描述名称
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetTypeDisplayName(this object value)
        {
            var type = value.GetType();
            return type.GetDisplayName();
        }

        /// <summary>
        /// 获取某个对象的类型名称
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetTypeFullName(this object value)
        {
            return value.GetType().FullName;
        }

        /// <summary>
        /// 根据属性名获取值（支持层级和数组项）
        /// </summary>
        /// <param name="srcobj">对象</param>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public static T GetPropertyValue<T>(this object srcobj, string propertyName)
        {
            var obj = srcobj.GetPropertyValue(propertyName);
            return obj == null ? default(T) : (T)obj;
        }

        /// <summary>
        /// 根据属性名获取值（支持层级和数组项）
        /// </summary>
        /// <param name="srcobj">对象</param>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        /// <see cref="http://stackoverflow.com/questions/1196991/get-property-value-from-string-using-reflection-in-c-sharp"/>
        public static object GetPropertyValue(this object srcobj, string propertyName)
        {
            if (srcobj == null)
                return null;

            object obj = srcobj;

            // Split property name to parts (propertyName could be hierarchical, like obj.subobj.subobj.property
            string[] propertyNameParts = propertyName.Split('.');

            foreach (string propertyNamePart in propertyNameParts)
            {
                if (obj == null) return null;

                // propertyNamePart could contain reference to specific 
                // element (by index) inside a collection
                if (!propertyNamePart.Contains("["))
                {
                    PropertyInfo pi = obj.GetType().GetProperty(propertyNamePart);
                    if (pi == null) return null;
                    obj = pi.GetValue(obj, null);
                }
                else
                {   // propertyNamePart is areference to specific element 
                    // (by index) inside a collection
                    // like AggregatedCollection[123]
                    //   get collection name and element index
                    int indexStart = propertyNamePart.IndexOf("[") + 1;
                    string collectionPropertyName = propertyNamePart.Substring(0, indexStart - 1);
                    int collectionElementIndex = Int32.Parse(propertyNamePart.Substring(indexStart, propertyNamePart.Length - indexStart - 1));
                    //   get collection object
                    PropertyInfo pi = obj.GetType().GetProperty(collectionPropertyName);
                    if (pi == null) return null;
                    object unknownCollection = pi.GetValue(obj, null);
                    //   try to process the collection as array
                    if (unknownCollection.GetType().IsArray)
                    {
                        object[] collectionAsArray = unknownCollection as Array[];
                        obj = collectionAsArray[collectionElementIndex];
                    }
                    else
                    {
                        //   try to process the collection as IList
                        System.Collections.IList collectionAsList = unknownCollection as System.Collections.IList;
                        if (collectionAsList != null)
                        {
                            obj = collectionAsList[collectionElementIndex];
                        }
                        else
                        {
                            // ??? Unsupported collection type
                        }
                    }
                }
            }

            return obj;
        }

        /// <summary>
        /// 去除对象中的所有null值（有bug）
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static object ToNoNullObject(this object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.PropertyType == typeof(string) &&
                                   p.CanRead &&
                                   p.CanWrite || p.PropertyType == typeof(DateTime?)
                                   || p.PropertyType == typeof(int?)
                                   || p.PropertyType == typeof(double?)
                                   || p.PropertyType == typeof(decimal?)
                             select p;

            foreach (var property in properties)
            {
                var value = property.GetValue(obj, null);
                if (value == null)
                {
                    property.SetValue(obj, string.Empty, null);
                }
            }

            return obj;
        }

    }
}