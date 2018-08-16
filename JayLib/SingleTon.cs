using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JayLib
{
    public static class SingleTon<T> where T : class, new()
    {
        private static T _Instance = default(T);

        public static T GetInstance()
        {
            if ((object)SingleTon<T>._Instance == null)
                Interlocked.CompareExchange<T>(ref SingleTon<T>._Instance, Activator.CreateInstance<T>(), default(T));
            return SingleTon<T>._Instance;
        }
    }
}
