using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace Orzoo.AspNet.Mvc
{
    internal class PropertyHelper
    {
        #region Static Fields and Constants

        private static ConcurrentDictionary<Type, PropertyHelper[]> _reflectionCache =
            new ConcurrentDictionary<Type, PropertyHelper[]>();

        private static readonly MethodInfo _callPropertyGetterOpenGenericMethod =
            typeof (PropertyHelper).GetMethod("CallPropertyGetter", BindingFlags.NonPublic | BindingFlags.Static);

        private static readonly MethodInfo _callPropertyGetterByReferenceOpenGenericMethod =
            typeof (PropertyHelper).GetMethod("CallPropertyGetterByReference",
                BindingFlags.NonPublic | BindingFlags.Static);

        private static readonly MethodInfo _callPropertySetterOpenGenericMethod =
            typeof (PropertyHelper).GetMethod("CallPropertySetter", BindingFlags.NonPublic | BindingFlags.Static);

        #endregion

        #region Fields

        private Func<object, object> _valueGetter;

        #endregion

        #region Constructors

        public PropertyHelper(PropertyInfo property)
        {
            Contract.Assert(property != null);

            Name = property.Name;
            _valueGetter = MakeFastPropertyGetter(property);
        }

        #endregion

        #region Properties, Indexers

        public virtual string Name { get; protected set; }

        #endregion

        #region Methods

        #region Private Methods

        private static PropertyHelper CreateInstance(PropertyInfo property)
        {
            return new PropertyHelper(property);
        }

        private static object CallPropertyGetter<TDeclaringType, TValue>(Func<TDeclaringType, TValue> getter,
            object @this)
        {
            return getter((TDeclaringType) @this);
        }

        private static object CallPropertyGetterByReference<TDeclaringType, TValue>(
            ByRefFunc<TDeclaringType, TValue> getter, object @this)
        {
            TDeclaringType unboxed = (TDeclaringType) @this;
            return getter(ref unboxed);
        }

        private static void CallPropertySetter<TDeclaringType, TValue>(Action<TDeclaringType, TValue> setter,
            object @this, object value)
        {
            setter((TDeclaringType) @this, (TValue) value);
        }

        #endregion

        #region Protected Methods

        protected static PropertyHelper[] GetProperties(object instance,
            Func<PropertyInfo, PropertyHelper> createPropertyHelper,
            ConcurrentDictionary<Type, PropertyHelper[]> cache)
        {
            PropertyHelper[] helpers;

            Type type = instance.GetType();

            if (!cache.TryGetValue(type, out helpers))
            {
                IEnumerable<PropertyInfo> properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(prop => prop.GetIndexParameters().Length == 0 &&
                                   prop.GetMethod != null);

                var newHelpers = new List<PropertyHelper>();

                foreach (PropertyInfo property in properties)
                {
                    PropertyHelper propertyHelper = createPropertyHelper(property);

                    newHelpers.Add(propertyHelper);
                }

                helpers = newHelpers.ToArray();
                cache.TryAdd(type, helpers);
            }

            return helpers;
        }

        #endregion

        #region Public Methods

        public static Action<TDeclaringType, object> MakeFastPropertySetter<TDeclaringType>(PropertyInfo propertyInfo)
            where TDeclaringType : class
        {
            MethodInfo setMethod = propertyInfo.GetSetMethod();

            Type typeInput = propertyInfo.ReflectedType;
            Type typeValue = setMethod.GetParameters()[0].ParameterType;

            Delegate callPropertySetterDelegate;

            var propertySetterAsAction =
                setMethod.CreateDelegate(typeof (Action<,>).MakeGenericType(typeInput, typeValue));
            var callPropertySetterClosedGenericMethod = _callPropertySetterOpenGenericMethod.MakeGenericMethod(
                typeInput, typeValue);
            callPropertySetterDelegate = Delegate.CreateDelegate(typeof (Action<TDeclaringType, object>),
                propertySetterAsAction, callPropertySetterClosedGenericMethod);

            return (Action<TDeclaringType, object>) callPropertySetterDelegate;
        }

        public object GetValue(object instance)
        {
            return _valueGetter(instance);
        }

        public static PropertyHelper[] GetProperties(object instance)
        {
            return GetProperties(instance, CreateInstance, _reflectionCache);
        }

        public static Func<object, object> MakeFastPropertyGetter(PropertyInfo propertyInfo)
        {
            MethodInfo getMethod = propertyInfo.GetGetMethod();
            Type typeInput = getMethod.ReflectedType;
            Type typeOutput = getMethod.ReturnType;

            Delegate callPropertyGetterDelegate;
            if (typeInput.IsValueType)
            {
                Delegate propertyGetterAsFunc =
                    getMethod.CreateDelegate(typeof (ByRefFunc<,>).MakeGenericType(typeInput, typeOutput));
                MethodInfo callPropertyGetterClosedGenericMethod =
                    _callPropertyGetterByReferenceOpenGenericMethod.MakeGenericMethod(typeInput, typeOutput);
                callPropertyGetterDelegate = Delegate.CreateDelegate(typeof (Func<object, object>), propertyGetterAsFunc,
                    callPropertyGetterClosedGenericMethod);
            }
            else
            {
                Delegate propertyGetterAsFunc =
                    getMethod.CreateDelegate(typeof (Func<,>).MakeGenericType(typeInput, typeOutput));
                MethodInfo callPropertyGetterClosedGenericMethod =
                    _callPropertyGetterOpenGenericMethod.MakeGenericMethod(typeInput, typeOutput);
                callPropertyGetterDelegate = Delegate.CreateDelegate(typeof (Func<object, object>), propertyGetterAsFunc,
                    callPropertyGetterClosedGenericMethod);
            }

            return (Func<object, object>) callPropertyGetterDelegate;
        }

        #endregion

        #endregion

        #region Nested type: ByRefFunc

        private delegate TValue ByRefFunc<TDeclaringType, TValue>(ref TDeclaringType arg);

        #endregion
    }
}