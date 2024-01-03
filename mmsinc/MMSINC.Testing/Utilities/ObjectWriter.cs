using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MMSINC.Testing.Utilities
{
    public static class ObjectWriter
    {
        /// <summary>
        /// Writes all properties and their values for an object to the console.
        /// Useful for comparing multiple objects without having to go back and 
        /// forth through the debugger window. AKA: Ross will probably be the only
        /// person who ever uses this.
        /// </summary>
        public static void WriteAllProperties(object obj, string message = null)
        {
            Console.WriteLine("");
            Console.WriteLine("WriteAllProperties: {0}", message);
            if (obj == null)
            {
                Console.WriteLine("Object is null");
                return;
            }

            var props =
                obj.GetType()
                   .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                                  BindingFlags.FlattenHierarchy).OrderBy(x => x.Name);

            foreach (var p in props)
            {
                object value = null;
                try
                {
                    value = p.GetValue(obj, new object[] { });
                }
                catch (Exception ex)
                {
                    value = "EXCEPTION: " + ex.Message;
                }

                if (value == null)
                {
                    value = "[null]";
                }
                else if (value == string.Empty)
                {
                    value = "[String.Empty]";
                }

                Console.WriteLine("[{0}] : {1}", p.Name, value);
            }
        }
    }
}
