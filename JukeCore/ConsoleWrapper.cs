using System;
using System.Text;

namespace JukeCore
{
    public class ConsoleWrapper : IConsole, IFunctionKeyEvents
    {
        public event EventHandler<ConsoleKey> OnFunctionKeyPressed;

        public void WriteLine(string value)
        {
            Console.WriteLine(value);
        }

        private bool IsFunctionKey(ConsoleKey key)
        {
            return key == ConsoleKey.F1 ||
                   key == ConsoleKey.F2 ||
                   key == ConsoleKey.F3 ||
                   key == ConsoleKey.F4 ||
                   key == ConsoleKey.F5 ||
                   key == ConsoleKey.F6 ||
                   key == ConsoleKey.F7 ||
                   key == ConsoleKey.F8 ||
                   key == ConsoleKey.F9 ||
                   key == ConsoleKey.F10 ||
                   key == ConsoleKey.F11 ||
                   key == ConsoleKey.F12 ||
                   key == ConsoleKey.F13 ||
                   key == ConsoleKey.F14 ||
                   key == ConsoleKey.F15 ||
                   key == ConsoleKey.F16 ||
                   key == ConsoleKey.F17 ||
                   key == ConsoleKey.F18 ||
                   key == ConsoleKey.F19 ||
                   key == ConsoleKey.F20 ||
                   key == ConsoleKey.F21 ||
                   key == ConsoleKey.F22 ||
                   key == ConsoleKey.F23 ||
                   key == ConsoleKey.F24;
        }

        public string ReadLine()
        {
            var readLine = new StringBuilder();
            bool lineComplete = false;
            do
            {
                var key = Console.ReadKey(true);

                if (IsFunctionKey(key.Key))
                {
                    OnFunctionKeyPressed?.Invoke(this, key.Key);
                    continue;
                }

                if (key.Key == ConsoleKey.Enter)
                {
                    lineComplete = true;
                }

                Console.Write(key.KeyChar);
                readLine.Append(key.KeyChar);

            } 
            while (!lineComplete);

            return readLine.ToString().TrimEnd('\r', '\n');
        }
    }
}