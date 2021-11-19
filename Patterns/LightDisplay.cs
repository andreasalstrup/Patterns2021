using System;
using System.Collections.Generic;

namespace Patterns
{
    class LightDisplay : IObserver
    {
        private Stack<ICommand> _stack = new Stack<ICommand>();
        private ILightFactory lightFactory ;

        public class DefaultLightFactory : ILightFactory
        {
            public ILight CreateLight() => new Light();
        }

        public ILightFactory LightFactory { get => lightFactory; set =>  lightFactory = new WattReportingLightFactory(value, this); }
        class AddLightCommandImpl : ICommand
        {
            public AddLightCommandImpl(LightDisplay ld)
            {
                Ld = ld;
            }

            public LightDisplay Ld { get; }

            public void Execute()
            {
                Ld.Lights.Add(Ld.LightFactory.CreateLight());
            }
        }
        // *************************************************************************************************************
        class UndoCommand : IUndoableCommand
        {
            private LightDisplay _Ld;
            public UndoCommand(LightDisplay lightDisplay)
            {
                _Ld = lightDisplay;
            }
            public void Execute()
            {
                throw new NotImplementedException();
            }

            public void Undo()
            {
                throw new NotImplementedException();
            }
        }

        class RedoCommand : IRedoableCommand
        {
            private LightDisplay _ld;

            public RedoCommand(LightDisplay lightDisplay)
            {
                _ld = lightDisplay;
            }
            public void Execute()
            {
                throw new NotImplementedException();
            }

            public void Redo()
            {
                throw new NotImplementedException();
            }
        }
        // *************************************************************************************************************
        class SwitchCommandImpl : ICommand
        {
            public SwitchCommandImpl(LightDisplay ld, int lightIndex)
            {
                Ld = ld;
                LightIndex = lightIndex;
            }

            public LightDisplay Ld { get; }
            public int LightIndex { get; }

            public void Execute()
            {
                if (LightIndex >= 0 && LightIndex < Ld.Lights.Count)
                    Ld.Lights[LightIndex].On = !Ld.Lights[LightIndex].On;
            }
        }
        public ICommand AddLightCommand => new AddLightCommandImpl(this);
        public ICommand StopCommand => new RelayCommand(() => Running = false);
        public ICommand SwitchCommand(int light) => new SwitchCommandImpl(this, light);

        public LightDisplay(IInputHandler ih)
        {
            Ih = ih;
            LightFactory = new DefaultLightFactory();
        }
        List<ILight> Lights { get; set; } = new List<ILight>();
        public bool Running { get; private set; } = true;
        public IInputHandler Ih { get; }

        public void Run()
        {
            while (Running)
            {
                Draw();
                //Ih.GetCommand(this).Execute();
                var cmd = Ih.GetCommand(this);
                _stack.Push(cmd);
                cmd.Execute();
            }
        }

        private void Draw()
        {
            Console.CursorLeft = 3;
            Console.CursorTop = 3;

            foreach (ILight light in Lights)
            {
                light.Draw();
                Console.CursorLeft += 3;
            }
        }
        double accWattage = 0;
        public void Update(double wh)
        {
            var top = Console.CursorTop;
            var left = Console.CursorLeft;
            Console.CursorTop = 20;
            Console.CursorLeft = 10;
            accWattage += wh;
            Console.WriteLine(accWattage);
            Console.CursorLeft = left;
            Console.CursorTop = top;
        }
    }
}
