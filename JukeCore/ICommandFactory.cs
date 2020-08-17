namespace JukeCore
{
    public interface ICommandFactory
    {
        ICommand Create(string id, string jukeCorePath);
    }
}