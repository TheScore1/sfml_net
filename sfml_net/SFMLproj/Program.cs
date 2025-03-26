using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SFMLproj
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ClickerGame clickerGame = new ClickerGame();
            clickerGame.Run();
        }
    }
}