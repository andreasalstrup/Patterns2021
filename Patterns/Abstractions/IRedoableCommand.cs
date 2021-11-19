namespace Patterns
{
    public interface IRedoableCommand : ICommand
    {
        void Redo();
    }
}