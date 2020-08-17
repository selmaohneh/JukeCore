namespace JukeCore
{
    public class IdProcessor : IIdProcessor
    {
        private readonly ICommandFactory _commandFactory;
        private readonly IConsole _console;

        public IdProcessor(ICommandFactory commandFactory, IConsole console)
        {
            _commandFactory = commandFactory;
            _console = console;
        }

        public void Process(string id, string jukeCoreMediaPath)
        {
            _console.WriteLine($"Processing ID {id}");
            var command = _commandFactory.Create(id, jukeCoreMediaPath);
            command.Execute();
        }
    }
}