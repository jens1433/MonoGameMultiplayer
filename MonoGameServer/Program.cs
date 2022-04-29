using Microsoft.Xna.Framework;
using MonoGameServer.Game;
using MonoGameServer.Packets;
using System;

namespace MonoGameServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var server = new GameServer(1111);
            server.Start();

            var running = true;

            while (running)
            {
                ConsoleKey key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.Escape:
                        running = false;
                        break;
                }
                
            }

            server.Stop();
        }
    }
}
