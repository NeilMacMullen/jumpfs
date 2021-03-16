using System;

namespace jumpfs.CommandLineParsing
{
    [Flags]
    public enum ArgumentFlags
    {
        None,
        AllowNoValue = (1 << 1),
        Mandatory = (1 << 2)
    }

    /// <summary>
    ///     Describes an argument that can be passed to a particular command
    /// </summary>
    public readonly struct ArgumentDescriptor
    {
        public readonly string Name;
        public readonly Type Type;
        public readonly string HelpText;

        private ArgumentDescriptor(string name, Type type, string helpText, ArgumentFlags flags)
        {
            Name = name;
            Type = type;
            HelpText = helpText;
            Flags = flags;
        }

        public readonly ArgumentFlags Flags;
        public bool IsMandatory => Flags.HasFlag(ArgumentFlags.Mandatory);
        public bool CanBeEmpty => Flags.HasFlag(ArgumentFlags.AllowNoValue);

        /// <summary>
        ///     Creates a string argument that doesn't necessarily need to be supplied
        /// </summary>
        public static ArgumentDescriptor Create<T>(string name) =>
            new(name, typeof(T), string.Empty, ArgumentFlags.None);

        public ArgumentDescriptor WithHelpText(string helpText) =>
            new(Name, Type, helpText, Flags);


        public ArgumentDescriptor WithFlags(ArgumentFlags flags) =>
            new(Name, Type, HelpText, flags);

        public ArgumentDescriptor Mandatory() =>
            WithFlags(Flags | ArgumentFlags.Mandatory);

        public ArgumentDescriptor AllowEmpty() => WithFlags(Flags | ArgumentFlags.AllowNoValue);

        public object DefaultValue()
        {
            if (Type == typeof(string)) return string.Empty;
            if (Type == typeof(int)) return 0;
            if (Type == typeof(bool)) return false;
            throw new NotImplementedException($"Unable to provide default value for type {Type.Name}");
        }

        public object DefaultValueWhenFlagPresent() => Type == typeof(bool) ? true : DefaultValue();

        public static ArgumentDescriptor CreateSwitch(string name) => Create<bool>(name).AllowEmpty();

        public bool TryConvert(string valStr, out object o)
        {
            if (Type == typeof(string))
            {
                o = valStr;
                return true;
            }

            if (Type == typeof(int))
            {
                if (int.TryParse(valStr, out var i))
                {
                    o = i;
                    return true;
                }
            }

            if (Type == typeof(bool))
            {
                if (bool.TryParse(valStr, out var b))
                {
                    o = b;
                    return true;
                }
            }

            o = null;
            return false;
        }
    }
}
