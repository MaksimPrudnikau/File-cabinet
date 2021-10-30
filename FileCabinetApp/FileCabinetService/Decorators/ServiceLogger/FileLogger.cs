using System;
using System.IO;
using System.Text;

namespace FileCabinetApp
{
    public class FileLogger
    {
        private readonly TextWriter _outputFile;
        
        public FileLogger(string path)
        {
            _outputFile = new StreamWriter(File.OpenWrite(path)) {AutoFlush = true};
        }
        
        private static string GetCurrentTime()
        {
            return $"{DateTime.Now:g}";
        }

        private static string GetActionMessage(string methodName)
        {
            return $"{GetCurrentTime()} - Calling {methodName}()";
        }

        private static string GetParametersMessage<T>(T record, string functionName)
        where T: FileCabinetRecord
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
        
        private static string GetMethodWithParameterMessage<T>(string methodName, T parameter)
        {
            return $"{GetCurrentTime()} - Calling {methodName}() with parameter '{parameter}'";
        }

        private static string GetMethodWithReturnValueMessage(string functionName, long returnValue)
        {
            var message = new StringBuilder();
            message.Append($"{GetCurrentTime()} - ");
            message.Append($"{functionName}() returned '{returnValue}'");

            return message.ToString();
        }
        
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
                
                _outputFile.WriteLine(GetParametersMessage(parameter as FileCabinetRecord, method.Method.Name));
            }
            else
            {
                _outputFile.WriteLine(GetMethodWithParameterMessage(method.Method.Name, parameter));
            }

            var ticks = TicksMeter.GetElapsedTicks(method, parameter, out var methodsOut);
            _outputFile.WriteLine(GetMethodWithReturnValueMessage(method.Method.Name, ticks));
            return methodsOut;
        }

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
        
        public void LogMethod(Action method)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }
            
            _outputFile.WriteLine(GetActionMessage(method.Method.Name));
            var ticks = TicksMeter.GetElapsedTicks(method);
            _outputFile.WriteLine(GetMethodWithReturnValueMessage(method.Method.Name, ticks));
        }
    }
}