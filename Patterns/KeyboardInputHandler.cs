using System;
using LibPhilips;

namespace Patterns
{
    class KeyboardInputHandler : IInputHandler
    {
        public ICommand GetCommand(LightDisplay ld)
        {
            Console.CursorLeft = 5;
            Console.CursorTop = 10;
            Console.Write("".PadRight(50));
            Console.CursorLeft = 5;
            Console.CursorTop = 10;
            var actions = Console.ReadLine().Split(' ');
            var mc = new MultiCommand();
            foreach (string action in actions)
            {
                switch (action)
                {
                    case "quit": mc.Add(ld.StopCommand);break;
                    case "a": mc.Add(ld.AddLightCommand);break;
                    case "red": mc.Add(new RedLightCommand(ld));break;
                    case "white": mc.Add(new DefaultLightCommand(ld));break;
                    case "philips": mc.Add(new GLightFactoryCommand<PhilipsAdapter>(ld));break;
                    case "osram": mc.Add(new GLightFactoryCommand<OsramAdapter>(ld)); break;
                    case "undo": mc.Add( ld); break;
                    case "redo": mc.Add( new RedoCommand(ld)); break;
                    case string s when int.TryParse(s, out var n): mc.Add(ld.SwitchCommand(n)); break;
                }
            }
            return mc;
        }
    }
}
