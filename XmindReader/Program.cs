using System;
using System.Linq;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace XmindReader
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var command = new RootCommand 
            { 
                GetFlattingCommand(),
                GetMakingWBSCommand(),
            };

#if DEBUG
            var debugArgs = new string[] 
            { 
                "XmindReader",
                "flat",
                "../../../test.xmind",
                "../../../../TestResults/test.csv",
            };

            var ret = await command.InvokeAsync(debugArgs);
#else
            var ret = await command.InvokeAsync(args);
#endif

            Console.WriteLine("fin.");

            return ret;
        }

        private static Command GetFlattingCommand()
        {
            var command = new Command("flat")
            {
                new Argument<string>("inputFilepath", "Xmind filepath."),
                new Argument<string>("outputFilepath", "CSV filepath"),
            };

            command.Handler =
                CommandHandler.Create<string, string>(HandleFlatting);

            return command;
        }

        private static void HandleFlatting(
            string inputFilepath, 
            string outputFilepath)
        {
            var treeRepository = new Infrastructure.XmindRepository(inputFilepath);
            var saveRepository = new Infrastructure.WindowsCsvRepository(outputFilepath);
            var interactor = new Usecases.FlattingTreeService();

            interactor.Handle(treeRepository, saveRepository);
        }

        private static Command GetMakingWBSCommand()
        {
            var command = new Command("wbs")
            {
                new Argument<string>("inputFilepath", "Xmind filepath."),
                new Argument<string>("outputFilepath", "CSV filepath"),
            };

            command.Handler =
                CommandHandler.Create<string, string>(HandleMakingWbs);

            return command;
        }

        private static void HandleMakingWbs(
            string inputFilepath, 
            string outputFilepath)
        {
            var treeRepository = new Infrastructure.XmindRepository(inputFilepath);
            var saveRepository = new Infrastructure.WindowsCsvRepository(outputFilepath);
            var interactor = new Usecases.MakeWBSService();

            interactor.Handle(treeRepository, saveRepository);
        }
    }
}
