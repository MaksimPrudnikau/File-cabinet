using System;
using System.Diagnostics;

namespace FileCabinetApp.FileCabinetService.Decorators.Meter
{
    public static class TicksMeter
    {
        private static readonly Stopwatch Stopwatch = new();
        
        /// <summary>
        /// Get the method's execution time in elapsed ticks
        /// </summary>
        /// <param name="method">Delegate to the method to invoke</param>
        /// <param name="parameter">Method's parameter</param>
        /// <param name="methodsOut">Method's return value</param>
        /// <typeparam name="TIn">Input parameter type</typeparam>
        /// <typeparam name="TOut">Method's return value type</typeparam>
        /// <returns>Method's execution time in elapsed ticks</returns>
        /// <exception cref="ArgumentNullException">The delegate is null</exception>
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
        
        /// <summary>
        /// Get the method's execution time in elapsed ticks
        /// </summary>
        /// <param name="method">Delegate to the method to invoke</param>
        /// <param name="methodsOut">Method's return value</param>
        /// <typeparam name="T">Method's return value type</typeparam>
        /// <returns>Method's execution time in elapsed ticks</returns>
        /// <exception cref="ArgumentNullException">The delegate is null</exception>
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
        
        /// <summary>
        /// Get the method's execution time in elapsed ticks
        /// </summary>
        /// <param name="method">Delegate to the method to invoke</param>
        /// <param name="parameter">Method's parameter</param>
        /// <typeparam name="T">Method's return value type</typeparam>
        /// <returns>Method's execution time in elapsed ticks</returns>
        /// <exception cref="ArgumentNullException">The delegate is null</exception>
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
        
        /// <summary>
        /// Get the method's execution time in elapsed ticks
        /// </summary>
        /// <param name="method">Delegate to the method to invoke</param>
        /// <returns>Method's execution time in elapsed ticks</returns>
        /// <exception cref="ArgumentNullException">The delegate is null</exception>
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
        
        /// <summary>
        /// Get the method's execution time in elapsed ticks
        /// </summary>
        /// <param name="method">Delegate to the method to invoke</param>
        /// <param name="first">The first method's parameter</param>
        /// <param name="second">The second method's parameter</param>
        /// <param name="methodsOut">The method's return value</param>
        /// <typeparam name="T1In">The first method's parameter type</typeparam>
        /// <typeparam name="T2In">The second method's parameter type</typeparam>
        /// <typeparam name="TOut">The method's return value type</typeparam>
        /// <returns>Method's execution time in elapsed ticks</returns>
        /// <exception cref="ArgumentNullException">The delegate is null</exception>
        public static long GetElapsedTicks<T1In, T2In, TOut>(Func<T1In, T2In, TOut> method, T1In first, T2In second, out TOut methodsOut)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }
            
            Stopwatch.Start();
            methodsOut = method.Invoke(first, second);
            Stopwatch.Stop();
            var ticks = Stopwatch.ElapsedTicks;
            Stopwatch.Reset();
            
            return ticks;
        }
    }
}