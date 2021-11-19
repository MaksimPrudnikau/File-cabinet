using System;
using System.IO;
using System.Text;
using FileCabinetApp.FileCabinetService.Decorators.Meter;

namespace FileCabinetApp.FileCabinetService.Decorators.Logger
{
    public class FileSystemLogger : IDisposable
    {
        private readonly TextWriter _outputFile;
        private bool _disposed;
        
        public FileSystemLogger(string path)
        {
            _outputFile = new StreamWriter(File.OpenWrite(path)) {AutoFlush = true};
        }
        
        /// <summary>
        /// Create message with current time
        /// </summary>
        /// <example>04/10/2008 06:30</example>
        /// <returns><see cref="string"/> object</returns>
        private static string GetCurrentTime()
        {
            return $"{DateTime.Now:g}";
        }

        /// <summary>
        /// Create message to method without parameters and return value
        /// </summary>
        /// <param name="methodName">Name of the method</param>
        /// <example>04/10/2008 06:30 - Calling Create()</example>
        /// <returns><see cref="string"/> object</returns>
        private static string GetActionMessage(string methodName)
        {
            return $"{GetCurrentTime()} - Calling {methodName}()";
        }

        /// <summary>
        /// Create message where record is a parameter
        /// </summary>
        /// <param name="record">The record to print</param>
        /// <param name="functionName">Method's name</param>
        /// <example>04/10/2008 06:30 - Calling Create() with Firstname = 'Ivan', LastName = 'Ivanov'... and etc.</example>
        /// <returns><see cref="string"/> object</returns>
        private static string GetMethodWithRecordMessage(FileCabinetRecord record, string functionName)
        {
            var message = new StringBuilder();
            message.Append($"{GetActionMessage(functionName)} with FirstName = '{record.FirstName}', ");
            message.Append($"LastName = '{record.LastName}', ");
            message.Append($"DateOfBirth = '{record.DateOfBirth}', ");
            message.Append($"JobExperience = '{record.JobExperience}', ");
            message.Append($"Salary = '{record.Salary}', ");
            message.Append($"Rank = '{record.Rank}'");

            return message.ToString();
        }
        
        /// <summary>
        /// Create method with a parameter
        /// </summary>
        /// <param name="methodName">Method's name</param>
        /// <param name="parameter">Method's parameter</param>
        /// <typeparam name="T">The object with implemented <see cref="object.ToString()"/></typeparam>
        /// <example>04/10/2008 06:30 - Calling Delete() with parameter 'where id='1''</example>
        /// <returns><see cref="string"/> object</returns>
        private static string GetMethodWithParameterMessage<T>(string methodName, T parameter)
        {
            return $"{GetCurrentTime()} - Calling {methodName}() with parameter '{parameter}'";
        }


        /// <summary>
        /// Create message with elapsed ticks
        /// </summary>
        /// <param name="functionName">Method's name</param>
        /// <param name="returnValue">The ticks means time to execute method</param>
        /// <typeparam name="T">The object with implemented <see cref="object.ToString()"/></typeparam>
        /// <example>04/10/2008 06:30 - Delete() returned '5472384'</example>
        /// <returns><see cref="string"/> message</returns>
        private static string GetMethodWithReturnValueMessage<T>(string functionName, T returnValue)
        {
            var message = new StringBuilder();
            message.Append($"{GetCurrentTime()} - ");
            message.Append($"{functionName}() returned '{returnValue}'");

            return message.ToString();
        }

        /// <summary>
        /// Invoke method and log it into source file
        /// </summary>
        /// <param name="method">Delegate to the method to invoke</param>
        /// <param name="parameter">The method's parameter</param>
        /// <typeparam name="TIn">Input parameter type</typeparam>
        /// <typeparam name="TOut">Output parameter type</typeparam>
        /// <returns>Source method return value</returns>
        /// <exception cref="ArgumentNullException">The method's delegate is null or in the case method's parameter
        /// is <see cref="FileCabinetRecord"/>, the record is null</exception>
        public TOut LogMethod<TIn, TOut>(Func<TIn, TOut> method, TIn parameter)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            if (parameter is FileCabinetRecord)
            {
                if (parameter is null)
                {
                    throw new ArgumentNullException(nameof(parameter));   
                }
                
                _outputFile.WriteLine(GetMethodWithRecordMessage(parameter as FileCabinetRecord, method.Method.Name));
            }
            else
            {
                _outputFile.WriteLine(GetMethodWithParameterMessage(method.Method.Name, parameter));
            }

            var ticks = TicksMeter.GetElapsedTicks(method, parameter, out var methodsOut);
            _outputFile.WriteLine(GetMethodWithReturnValueMessage(method.Method.Name, ticks));
            return methodsOut;
        }
        
        /// <summary>
        /// Invoke method and log it into source file
        /// </summary>
        /// <param name="method">Delegate to the method to invoke</param>
        /// <typeparam name="T">Input parameter type</typeparam>
        /// <returns>Source method return value</returns>
        /// <exception cref="ArgumentNullException">The method's delegate is null</exception>
        public T LogMethod<T>(Func<T> method)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }
            
            _outputFile.WriteLine(GetActionMessage(method.Method.Name));
            var ticks = TicksMeter.GetElapsedTicks(method, out var methodsOut);
            _outputFile.WriteLine(GetMethodWithReturnValueMessage(method.Method.Name, ticks));
            
            return methodsOut;
        }
        
        /// <summary>
        /// Invoke method and log it into source file
        /// </summary>
        /// <param name="method">Delegate to the method to invoke</param>
        /// <param name="parameter">Method's parameter</param>
        /// <typeparam name="T">Input parameter type</typeparam>
        /// <exception cref="ArgumentNullException">The method's delegate is null</exception>
        public void LogMethod<T>(Action<T> method, T parameter)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }
            
            _outputFile.WriteLine(GetMethodWithParameterMessage(method.Method.Name, parameter));
            var ticks = TicksMeter.GetElapsedTicks(method, parameter);
            _outputFile.WriteLine(GetMethodWithReturnValueMessage(method.Method.Name, ticks));
        }
        
        /// <summary>
        /// Invoke method and log it into source file
        /// </summary>
        /// <param name="method">Delegate to the method to invoke</param>
        /// <exception cref="ArgumentNullException">The method's delegate is null</exception>
        public void LogMethod(Action method)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }
            
            _outputFile.WriteLine(GetActionMessage(method.Method.Name));
            method.Invoke();
        }
        
        /// <summary>
        /// Invoke method and log it into source file
        /// </summary>
        /// <param name="method">Delegate to the method to invoke</param>
        /// <param name="first">First input parameter</param>
        /// <param name="second">Second input parameter</param>
        /// <typeparam name="T1">Method's first parameter type</typeparam>
        /// <typeparam name="T2">Method's second parameter type</typeparam>
        /// <typeparam name="T">Method's return value</typeparam>
        /// <returns>Method's return value</returns>
        /// <exception cref="ArgumentNullException">The method's delegate is null</exception>
        public T LogMethod<T1, T2, T>(Func<T1, T2, T> method, T1 first, T2 second)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }
            
            _outputFile.WriteLine(GetActionMessage(method.Method.Name));
            var returnValue = method.Invoke(first, second);
            _outputFile.WriteLine(GetMethodWithReturnValueMessage(method.Method.Name, returnValue));
            
            return returnValue;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(!_disposed)
            { 
                if(disposing)
                {
                    _outputFile.Dispose();
                }
                
                _disposed = true;
            }
        }
    }
}