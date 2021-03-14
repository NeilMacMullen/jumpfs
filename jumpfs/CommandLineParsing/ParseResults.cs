using System;
using System.Collections.Generic;
using jumpfs.Commands;

namespace jumpfs.CommandLineParsing
{
    public class ParseResults
    {
        private Dictionary<string, object> _argumentValues = new();

        public CommandDescriptor CommandDescriptor { get; private set; }

        public string Message { get; private set; } = string.Empty;
        public bool IsSuccess { get; private set; }

        public static ParseResults Error(CommandDescriptor commandDescriptor, string error) =>
            new()
            {
                Message = error,
                CommandDescriptor = commandDescriptor
            };

        public static ParseResults Success(CommandDescriptor commandDescriptor, Dictionary<string, object> vals) =>
            new()
            {
                CommandDescriptor = commandDescriptor,
                IsSuccess = true,
                _argumentValues = vals,
            };

        public T ValueOf<T>(string argName)
        {
            if (!CommandDescriptor.TryArgument(argName, out var arg))
                throw new ArgumentException($"attempt to retrieve invalid argument {argName}");

            return _argumentValues.TryGetValue(arg.Name, out var v)
                ? (T) v
                : (T) arg.DefaultValue();
        }

        public void Execute(ApplicationContext context)
        {
            CommandDescriptor.Action(this, context);
        }
    }
}