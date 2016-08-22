using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace MyConsole
{
    class Program
    {
        const string _commandNamespace = "MyConsole.Commands";
        static Dictionary<string, Dictionary<string, IEnumerable<ParameterInfo>>> _commandLibraries;

        static void Main(string[] args)
        {
            Console.Title = typeof(Program).Name;

            _commandLibraries = new Dictionary<string, Dictionary<string, IEnumerable<ParameterInfo>>>();
						var p = typeof(Commands.Factorization).GetTypeInfo().Assembly.GetTypes();

            //reflection to load the classes in the commands namespace
            var q = from t in p
                    where t.GetTypeInfo().IsClass && t.GetTypeInfo().Namespace == _commandNamespace
                    && !t.Name.ToUpper().Contains("<>")
                    select t;

            var commandClasses = q.ToList();

            //add commands as method names using reflection
            foreach (var commandClass in commandClasses)
            {
                var methods = commandClass.GetMethods(BindingFlags.Static | BindingFlags.Public);
                var methodDictionary = new Dictionary<string, IEnumerable<ParameterInfo>>();
                foreach (var method in methods)
                {
                    string commandName = method.Name;
                    methodDictionary.Add(commandName, method.GetParameters());
                }

                _commandLibraries.Add(commandClass.Name, methodDictionary);
            }

            Run();
        }

        static void Run()
        {

            while (true)
            {
                var consoleInput = ReadFromConsole();
                if (string.IsNullOrWhiteSpace(consoleInput)) continue;

                try
                {
                    var command = new ConsoleCommand(consoleInput);
                    string result = Execute(command); 
                    WriteToConsole(result);
                }
                catch (Exception ex)
                {
                    WriteToConsole(ex.Message);
                }
            }
        }


        static string Execute(ConsoleCommand command)
        {
            string invalidOperation = "Command does not exist";

            //check command exists
            if (!_commandLibraries.ContainsKey(command.LibraryClassName))
            {
                return invalidOperation;
            }

            var methodDictionary = _commandLibraries[command.LibraryClassName];
            if (!methodDictionary.ContainsKey(command.Name))
            {
                return invalidOperation;
            }

            //Validate paramters are correct
            var methodParameterValueList = new List<object>();
            IEnumerable<ParameterInfo> paramInfoList = methodDictionary[command.Name].ToList();

            var requiredParams = paramInfoList.Where(p => p.IsOptional == false);
            var optionalParams = paramInfoList.Where(p => p.IsOptional == true);
            int requiredCount = requiredParams.Count();
            int optionalCount = optionalParams.Count();
            int providedCount = command.Arguments.Count();

            if (requiredCount > providedCount)
            {
                return string.Format(
                    "Missing required arguments. {0} required, {1} optional, {2} provided",
                    requiredCount, optionalCount, providedCount);
            }
            //TODO get required parameter names/info

            if (paramInfoList.Count() > 0)
            {
                //returns null if the parameter is required. TODO modify this to a dictionary of parameter names and values
                foreach (var param in paramInfoList)
                {
                    methodParameterValueList.Add(param.DefaultValue);
                }
            }

            //parse provided parameters/////////////////////////////////////////////////////
            for (int i = 0; i < command.Arguments.Count(); i++)
            {
                var methodParam = paramInfoList.ElementAt(i);
                var typeRequired = methodParam.ParameterType;
                object value = null;

                try
                {
                    value = CoerceArgument(typeRequired, command.Arguments.ElementAt(i));
                    methodParameterValueList.RemoveAt(i);
                    methodParameterValueList.Insert(i, value);
                }

                catch (ArgumentException)
                {
                    string argumentName = methodParam.Name;
                    string argumentTypeName = typeRequired.Name;
                    string message = string.Format(
                        "The value passes for argument '{0}' cannot be parsed to type '{1}'",
                        argumentName, argumentTypeName);
                    throw new ArgumentException(message);
                }
                //////////////////////////////////////////////////////////////////////////////
            }

            //invoke the method using reflection
            Assembly currentAssembly = typeof(Program).GetTypeInfo().Assembly;

            Type commandLibraryClass = currentAssembly.GetType(_commandNamespace + "." + command.LibraryClassName);

            object[] inputArgs = null;
            if (methodParameterValueList.Count > 0)
            {
                inputArgs = methodParameterValueList.ToArray();
            }

            var typeInfo = commandLibraryClass;

            try
            {
							  var method = typeInfo.GetTypeInfo().GetMethod(command.Name, BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.Public);

                var result = method.Invoke(null, inputArgs);
                return result.ToString();

            }

            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }


        public static void WriteToConsole(string message = "")
        {
            if (message.Length > 0)
            {
                Console.WriteLine(message);
            }
        }
        const string _readPrompt = "console> ";
        public static string ReadFromConsole(string promptMessage = "")
        {
            // Show a prompt, and get input:
            Console.Write(_readPrompt + promptMessage);
            return Console.ReadLine();
        }

        static object CoerceArgument(Type requiredType, string inputValue)
        {
            var requiredTypeCode = Type.GetTypeCode(requiredType);
            string exceptionMessage =
                string.Format("Cannnot coerce the input argument {0} to required type {1}",
                inputValue, requiredType.Name);

            object result = null;
            switch (requiredTypeCode)
            {
                case TypeCode.String:
                    result = inputValue;
                    break;
                case TypeCode.Int16:
                    short number16;
                    if (Int16.TryParse(inputValue, out number16))
                    {
                        result = number16;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }
                    break;
                case TypeCode.Int32:
                    int number32;
                    if (Int32.TryParse(inputValue, out number32))
                    {
                        result = number32;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }
                    break;
                case TypeCode.Int64:
                    long number64;
                    if (Int64.TryParse(inputValue, out number64))
                    {
                        result = number64;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }
                    break;
                case TypeCode.Boolean:
                    bool trueFalse;
                    if (bool.TryParse(inputValue, out trueFalse))
                    {
                        result = trueFalse;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }
                    break;
                case TypeCode.Byte:
                    byte byteValue;
                    if (byte.TryParse(inputValue, out byteValue))
                    {
                        result = byteValue;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }
                    break;
                case TypeCode.Char:
                    char charValue;
                    if (char.TryParse(inputValue, out charValue))
                    {
                        result = charValue;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }
                    break;
                case TypeCode.DateTime:
                    DateTime dateValue;
                    if (DateTime.TryParse(inputValue, out dateValue))
                    {
                        result = dateValue;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }
                    break;
                case TypeCode.Decimal:
                    Decimal decimalValue;
                    if (Decimal.TryParse(inputValue, out decimalValue))
                    {
                        result = decimalValue;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }
                    break;
                case TypeCode.Double:
                    Double doubleValue;
                    if (Double.TryParse(inputValue, out doubleValue))
                    {
                        result = doubleValue;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }
                    break;
                case TypeCode.Single:
                    Single singleValue;
                    if (Single.TryParse(inputValue, out singleValue))
                    {
                        result = singleValue;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }
                    break;
                case TypeCode.UInt16:
                    UInt16 uInt16Value;
                    if (UInt16.TryParse(inputValue, out uInt16Value))
                    {
                        result = uInt16Value;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }
                    break;
                case TypeCode.UInt32:
                    UInt32 uInt32Value;
                    if (UInt32.TryParse(inputValue, out uInt32Value))
                    {
                        result = uInt32Value;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }
                    break;
                case TypeCode.UInt64:
                    UInt64 uInt64Value;
                    if (UInt64.TryParse(inputValue, out uInt64Value))
                    {
                        result = uInt64Value;
                    }
                    else
                    {
                        throw new ArgumentException(exceptionMessage);
                    }
                    break;
                default:
                    throw new ArgumentException(exceptionMessage);
            }
            return result;
        }
    }

    
}
