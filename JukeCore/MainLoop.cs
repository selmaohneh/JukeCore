using System;
using System.Threading.Tasks;

namespace JukeCore
{
    public class MainLoop
    {
        private readonly IConsole _console;
        private readonly IIdProcessor _processor;

        public MainLoop(IConsole console, IIdProcessor processor)
        {
            _console = console;
            _processor = processor;
        }

        public async Task Run(string jukeCoreMediaPath)
        {
            _processor.Process("boot", jukeCoreMediaPath);

            while (true)
            {
                try
                {
                    _console.WriteLine("Waiting for ID ...");
                    var id = _console.ReadLine();

                    _processor.Process(id, jukeCoreMediaPath);
                    await Task.Delay(50);
                }
                catch (Exception e)
                {
                    _console.WriteLine(e.Message);
                    await Task.Delay(50);
                }
            }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}