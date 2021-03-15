using System;
using FluentAssertions;
using jumpfs.CommandLineParsing;
using jumpfs.Commands;
using jumpfs.Extensions;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class CommandParserTests
    {
        private static void Noop(ParseResults p, ApplicationContext context)
        {
        }

        [Test]
        public void ParsingCommandSucceeds()
        {
            var command = new CommandDescriptor(Noop, "test");

            var parser = new CommandLineParser(command);
            var results = parser.Parse("test".Tokenise());
            results.IsSuccess.Should().BeTrue("because we recognised the command");
            results.CommandDescriptor.Name.Should().Be("test");
        }

        [Test]
        public void ParsingUnrecognisedCommandFails()
        {
            var parser = new CommandLineParser();
            var results = parser.Parse(Array.Empty<string>());
            results.IsSuccess.Should().BeFalse("because there are no commands to parse");
        }

        [Test]
        public void ParsingRecognisesArguments()
        {
            var command = new CommandDescriptor(Noop, "test")
                .WithArguments(ArgumentDescriptor.Create<string>("foo")
                );

            var parser = new CommandLineParser(command);
            var results = parser.Parse("test -foo xyz".Tokenise());
            results.IsSuccess.Should().BeTrue("because we recognised the command");
            results.CommandDescriptor.Name.Should().Be("test");
            results.ValueOf<string>("foo").Should().Be("xyz");
        }

        [Test]
        public void MissingCommandProvidesListInErrorOutput()
        {
            var command = new CommandDescriptor(Noop, "testcommand")
                .WithArguments(ArgumentDescriptor.Create<string>("foo")
                );

            var parser = new CommandLineParser(command);
            var results = parser.Parse(Array.Empty<string>());
            results.IsSuccess.Should().BeFalse();
            results.Message.Should().Contain("testcommand");
        }

        [Test]
        public void UnrecognisedArgumentFails()
        {
            var command = new CommandDescriptor(Noop, "test")
                .WithArguments(ArgumentDescriptor.Create<string>("foo"));

            var parser = new CommandLineParser(command);
            var results = parser.Parse("test -fo xyz".Tokenise());
            results.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void MissingValueFails()
        {
            var command = new CommandDescriptor(Noop, "test")
                .WithArguments(ArgumentDescriptor.Create<string>("foo"));

            var parser = new CommandLineParser(command);
            var results = parser.Parse("test -foo ".Tokenise());
            results.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void OptionalValueReturnsEmptyString()
        {
            var command = new CommandDescriptor(Noop, "test")
                .WithArguments(
                    ArgumentDescriptor.Create<string>("foo")
                        .AllowEmpty());

            var parser = new CommandLineParser(command);
            var results = parser.Parse("test -foo ".Tokenise());
            results.IsSuccess.Should().BeTrue();
            results.ValueOf<string>("foo").Should().Be(string.Empty);
        }

        [Test]
        public void OptionalIntReturns0()
        {
            var command = new CommandDescriptor(Noop, "test")
                .WithArguments(
                    ArgumentDescriptor.Create<int>("foo"));

            var parser = new CommandLineParser(command);
            var results = parser.Parse("test".Tokenise());
            results.IsSuccess.Should().BeTrue();
            results.ValueOf<int>("foo").Should().Be(0);
        }


        [Test]
        public void IntParsesCorrectly()
        {
            var command = new CommandDescriptor(Noop, "test")
                .WithArguments(
                    ArgumentDescriptor.Create<int>("foo"));

            var parser = new CommandLineParser(command);
            var results = parser.Parse("test -foo 123".Tokenise());
            results.IsSuccess.Should().BeTrue();
            results.ValueOf<int>("foo").Should().Be(123);
        }

        [Test]
        public void SwitchWorksAsExpected()
        {
            var command = new CommandDescriptor(Noop, "test")
                .WithArguments(
                    ArgumentDescriptor
                        .CreateSwitch("foo")
                );

            var parser = new CommandLineParser(command);

            var results = parser.Parse("test".Tokenise());
            results.IsSuccess.Should().BeTrue();
            results.ValueOf<bool>("foo").Should().Be(false);

            var results2 = parser.Parse("test -foo".Tokenise());
            results2.IsSuccess.Should().BeTrue();
            results2.ValueOf<bool>("foo").Should().Be(true);
        }


        [Test]
        public void MissingParameterFails()
        {
            var command = new CommandDescriptor(Noop, "test")
                .WithArguments(
                    ArgumentDescriptor.Create<string>("foo")
                        .Mandatory()
                );

            var parser = new CommandLineParser(command);
            var results = parser.Parse("test".Tokenise());
            results.IsSuccess.Should().BeFalse();
        }

        [Test]
        public void OptionalValuesDontElideSuccessiveTokens()
        {
            var command = new CommandDescriptor(Noop, "test")
                .WithArguments(
                    ArgumentDescriptor.Create<string>("foo")
                        .AllowEmpty(),
                    ArgumentDescriptor.Create<string>("bar")
                        .AllowEmpty()
                );

            var parser = new CommandLineParser(command);
            var results = parser.Parse("test -foo -bar xyz".Tokenise());
            results.IsSuccess.Should().BeTrue();
            results.ValueOf<string>("foo").Should().Be(string.Empty);
            results.ValueOf<string>("bar").Should().Be("xyz");
        }
    }
}
