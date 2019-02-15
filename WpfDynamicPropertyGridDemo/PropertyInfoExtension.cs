using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfDynamicPropertyGridDemo
{
    public static class PropertyInfoExtension
    {
        public static T GetCustomAttribute<T>(this System.Reflection.PropertyInfo propery, bool inherit) where T : Attribute
        {
            object[] objs = propery.GetCustomAttributes(typeof(T), inherit);
            if (objs == null || objs.Length == 0)
                return null;
            return objs[0] as T;
        }

        public static void Map<TSource>(this IEnumerable<TSource> sources, Action<TSource> func)
        {
            foreach(TSource source in sources)
            {
                func(source);
            }
        }
    }
        
}
