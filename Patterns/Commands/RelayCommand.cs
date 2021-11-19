using System;

namespace Patterns
{
    public class RelayCommand : ICommand
    {
        private readonly Action _lambda;
        public RelayCommand(Action lambda ) => _lambda = lambda;
        public void Execute() => _lambda();
    }
}