using System.Reflection;

namespace Universe.SqlServer.SystemViews.Tests
{
    internal class PropertyAccessor
    {
        public static Func<TClass, TProperty> CreatePropertyGetter<TClass,TProperty>(PropertyInfo propertyInfo)
        {
            var getMethod = propertyInfo.GetGetMethod();
            return (Func<TClass, TProperty>)Delegate.CreateDelegate(typeof(Func<TClass, TProperty>), getMethod);
        }
    }
}
