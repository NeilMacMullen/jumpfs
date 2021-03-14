using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using jumpfs.Extensions;

namespace jumpfs.CommandLineParsing
{
    /// <summary>
    ///     Parses an array of strings passed on the command line
    /// </summary>
    /// <remarks>
    ///     No action is taken by the parser - it's up to the caller to use
    ///     the returned ParseResult
    /// </remarks>
    public class CommandLineParser
    {
        private const string CommandPrefix = "-";
        private readonly CommandDescriptor[] _commands;

        public CommandLineParser(params CommandDescriptor[] args) => _commands = args.ToArray();

        public string ConstructHelp()
        {
            return
                @"Missing/unrecognised command.
Use of of the following:
" + string.Join(Environment.NewLine, _commands.Select(c => $"  {c.Name} - {c.HelpText}"))
  + @"
";
        }

        public ParseResults Parse(string[] suppliedArguments)
        {
            if (!suppliedArguments.Any())
                return ParseResults.Error(CommandDescriptor.Empty, ConstructHelp());

            if (!_commands.TryGetSingle(c => c.Name == suppliedArguments[0], out var requestedCommand))
                return ParseResults.Error(CommandDescriptor.Empty, ConstructHelp());

            bool IsValue(int i) => i < suppliedArguments.Length && !suppliedArguments[i].StartsWith(CommandPrefix);

            var assignedArguments = new Dictionary<string, object>();


            for (var i = 1; i < suppliedArguments.Length; i++)
            {
                var token = suppliedArguments[i];
                if (!token.StartsWith(CommandPrefix)) continue;
                var p = token.Substring(CommandPrefix.Length);
                if (!requestedCommand.TryArgument(p, out var arg))
                {
                    return ParseResults.Error(requestedCommand,
                        ConstructHelpForCommand(requestedCommand,
                            $"unrecognised argument '{p}'"
                        ));
                }

                //move on to the next token
                i++;

                if (IsValue(i))
                {
                    if (!arg.TryConvert(suppliedArguments[i], out var v))
                        return ParseResults.Error(requestedCommand, $"Parameter '{arg.Name}' invalid value");
                    assignedArguments[arg.Name] = v;
                }
                else
                {
                    if (arg.CanBeEmpty)
                        assignedArguments[arg.Name] = arg.DefaultValueWhenFlagPresent();
                    else
                        return ParseResults.Error(requestedCommand, $"Parameter '{arg.Name}' missing value");
                }
            }

            var missingParameters = requestedCommand
                .Arguments
                .Where(p => p.IsMandatory)
                .Where(req => !assignedArguments.ContainsKey(req.Name))
                .ToArray();
            if (missingParameters.Any())
                return ParseResults.Error(requestedCommand, ConstructHelpForCommand(requestedCommand,
                    $"missing arguments: {string.Join(" ", missingParameters.Select(p => p.Name))}"
                ));

            return ParseResults.Success(requestedCommand, assignedArguments);
        }

        public static string ConstructHelpForCommand(CommandDescriptor cmd, string additional)
        {
            var sb = new StringBuilder();
            if (additional.Length != 0)
                sb.AppendLine(additional);
            foreach (var a in cmd.Arguments)
            {
                sb.AppendLine($"  {CommandPrefix}{a.Name} {a.Type} {a.HelpText}");
            }

            return sb.ToString();
        }
    }
}