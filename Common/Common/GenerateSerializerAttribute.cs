using System;

namespace Common
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class GenerateSerializerAttribute : Attribute
    {
    }
}
