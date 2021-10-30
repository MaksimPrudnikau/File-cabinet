using System;
using System.Diagnostics;

namespace FileCabinetApp
{
    public static class TicksMeter
    {
        private static readonly Stopwatch Stopwatch = new();
        
        public static long GetElapsedTicks<TIn, TOut>(Func<TIn, TOut> method, TIn parameter, out TOut methodsOut)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }
            
            Stopwatch.Start();
            methodsOut = method.Invoke(parameter);
            Stopwatch.Stop();
            var ticks = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            
            return ticks;
        }
        
        public static long GetElapsedTicks<T>(Func<T> method, out T methodsOut)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }
            
            Stopwatch.Start();
            methodsOut = method.Invoke();
            Stopwatch.Stop();
            var ticks = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            
            return ticks;
        }
        
        public static long GetElapsedTicks<T>(Action<T> method, T parameter)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }
            
            Stopwatch.Start();
            method.Invoke(parameter);
            Stopwatch.Stop();
            var ticks = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            
            return ticks;
        }
        
        public static long GetElapsedTicks(Action method)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }
            
            Stopwatch.Start();
            method.Invoke();
            Stopwatch.Stop();
            var ticks = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            return ticks;
        }
    }
}