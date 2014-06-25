using System;

namespace Aero
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (AeroGame game = new AeroGame())
            {
                game.Run();
            }
        }
    }
}

