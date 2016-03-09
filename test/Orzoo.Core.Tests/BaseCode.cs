using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Orzoo.Core.Tests
{
    public enum TestEnum
    {
        [Display(Name = "display")]
        [Description("description")]
        A,
        B
    }

    public class A
    {
        public A()
        {

        }
    }

    public class B : A
    {
        public B(string name)
        {

        }
    }

    [DisplayName("Test Class")]
    public class TestClass
    {
        public const string DefaultStringValue = "A string";
        public const int DefaultIntValue = 1;

        [Display(Name = "String Value")]
        public string StringValue { get; set; }
        public int IntValue { get; set; }

        public T CreateClass<T>(bool creatIt = true) where T : class, new()
        {
            return creatIt ? new T() : null;
        }

        public static TestClass CreateTestInstance()
        {
            return new TestClass()
            {
                StringValue = DefaultStringValue,
                IntValue = DefaultIntValue
            };
        }

        public static List<TestClass> CreateTestInstanceList()
        {
            return new List<TestClass>
            {
                CreateTestInstance(),
                CreateTestInstance()
            };
        }

    }
}