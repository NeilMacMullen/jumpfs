using System;
using System.Linq;
using jumpfs.Commands;
using jumpfs.Extensions;

namespace jumpfs.CommandLineParsing
{
    /// <summary>
    ///     A command that can be supported by the command line parser
    /// </summary>
    public readonly struct CommandDescriptor
    {
        /// <summary>
        ///     The action to be taken when this command is run
        /// </summary>
        public readonly Action<ParseResults, ApplicationContext> Action;

        /// <summary>
        ///     Name of the command (string used to invoke it)
        /// </summary>
        public readonly string Name;

        /// <summary>
        ///     List of Argument Descriptors for this command
        /// </summary>
        public readonly ArgumentDescriptor[] Arguments;

        public CommandDescriptor(Action<ParseResults, ApplicationContext> action, string name,
            ArgumentDescriptor[] arguments,
            string helpText)
        {
            Action = action;
            //ensure the name is always lower-cased
            Name = name.ToLowerInvariant();
            Arguments = arguments.ToArray();
            HelpText = helpText;
        }

        public readonly string HelpText;

        public CommandDescriptor(Action<ParseResults, ApplicationContext> action, string name) : this(action, name,
            Array.Empty<ArgumentDescriptor>(), string.Empty)
        {
        }

        public CommandDescriptor WithArguments(params ArgumentDescriptor[] args) =>
            new(Action, Name, args, HelpText);

        public CommandDescriptor WithHelpText(string helpText) =>
            new(Action, Name, Arguments, helpText);

        public static readonly CommandDescriptor Empty = new((_, _) => { }, string.Empty);

        public bool TryArgument(string name, out ArgumentDescriptor arg)
        {
            return Arguments.TryGetSingle(a => a.Name == name, out arg);
        }
    }
}
