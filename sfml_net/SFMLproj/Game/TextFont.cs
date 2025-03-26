using SFML.System;
using SFML.Graphics;

namespace SFMLproj
{
    public static class TextFont
    {
        public const string ConsoleFontPath = "./fonts/arial.ttf";

        public static Font font;

        public static void LoadContent()
        {
            font = new Font(ConsoleFontPath);
        }
    }
}
