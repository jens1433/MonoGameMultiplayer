using System;
using System.Diagnostics;

namespace BaseProject
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
#if DEBUG
            //var p = Process.Start("C:\\Users\\jensg\\Desktop\\MonoGameServer\\bin\\Debug\\netcoreapp3.1\\MonoGameServer.exe");
#else
            var p = Process.Start("C:\\Users\\jensg\\Desktop\\MonoGameServer\\bin\\Release\\netcoreapp3.1\\MonoGameServer.exe");
#endif

            using (var game = new Game1())
                game.Run();

            //p.Kill();
        }
    }
}
