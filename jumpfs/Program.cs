using jumpfs.Commands;

namespace jumpfs
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var context = JumpFs.CliContext();
            JumpFs.ExecuteWithContext(args, context);
        }
    }
}