using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaEPluginCore
{
    public abstract class ReflectedMemberAttribute : Attribute
    {
        /// <summary>
        /// Name of the member to access.  If null, the tagged field's name.
        /// </summary>
        public string Name { get; set; } = null;

        /// <summary>
        /// Declaring type of the member to access.  If null, inferred from the instance argument type.
        /// </summary>
        public Type Type { get; set; } = null;

        /// <summary>
        /// Assembly qualified name of <see cref="Type"/>
        /// </summary>
        public string TypeName
        {
            get => Type?.AssemblyQualifiedName;
            set => Type = value == null ? null : Type.GetType(value, true);
        }
    }

    /// <summary>
    /// Indicates that this field should contain a delegate capable of retrieving the value of a field.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// [ReflectedGetterAttribute(Name="_instanceField")]
    /// private static Func<Example, int> _instanceGetter;
    /// 
    /// [ReflectedGetterAttribute(Name="_staticField", Type=typeof(Example))]
    /// private static Func<int> _staticGetter;
    /// 
    /// private class Example {
    ///     private int _instanceField;
    ///     private static int _staticField;
    /// }
    /// ]]>
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Field)]
    public class ReflectedGetterAttribute : ReflectedMemberAttribute
    {
    }

    /// <summary>
    /// Indicates that this field should contain a delegate capable of invoking an instance method.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// [ReflectedMethodAttribute]
    /// private static Func<Example, int, float, string> ExampleInstance;
    /// 
    /// private class Example {
    ///     private int ExampleInstance(int a, float b) {
    ///         return a + ", " + b;
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Field)]
    public class ReflectedMethodAttribute : ReflectedMemberAttribute
    {
        /// <summary>
        /// When set the parameters types for the method are assumed to be this.
        /// </summary>
        public Type[] OverrideTypes { get; set; }

        /// <summary>
        /// Assembly qualified names of <see cref="OverrideTypes"/>
        /// </summary>
        public string[] OverrideTypeNames
        {
            get => OverrideTypes.Select(x => x.AssemblyQualifiedName).ToArray();
            set => OverrideTypes = value?.Select(x => x == null ? null : Type.GetType(x, true)).ToArray();
        }

        public abstract class ReflectedMemberAttribute : Attribute
        {
            /// <summary>
            /// Name of the member to access.  If null, the tagged field's name.
            /// </summary>
            public string Name { get; set; } = null;

            /// <summary>
            /// Declaring type of the member to access.  If null, inferred from the instance argument type.
            /// </summary>
            public Type Type { get; set; } = null;

            /// <summary>
            /// Assembly qualified name of <see cref="Type"/>
            /// </summary>
            public string TypeName
            {
                get => Type?.AssemblyQualifiedName;
                set => Type = value == null ? null : Type.GetType(value, true);
            }
        }
    }
}
