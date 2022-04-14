using System;
using eAgenda.ConsoleApp.Enums;

namespace eAgenda.ConsoleApp.Utils
{
    internal static class Messenger
    {
        public static void Send(string message, MessageLevel messageLevel, bool writeLine)
        {
            switch (messageLevel)
            {
                case MessageLevel.Informacao:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;

                case MessageLevel.Sucesso:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;

                case MessageLevel.Erro:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }

            Console.WriteLine();
            if (writeLine)
                Console.WriteLine(message);
            else
                Console.Write(message);
            Console.WriteLine();

            Console.ResetColor();
        }

        public static void SendCustom(string message, ConsoleColor color, bool writeLine)
        {
            Console.ForegroundColor = color;

            if (writeLine)
                Console.WriteLine(message);
            else
                Console.Write(message);

            Console.ResetColor();
        }
    }
}