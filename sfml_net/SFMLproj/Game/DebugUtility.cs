// https://youtu.be/NL1zhckb5hc
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SFMLproj
{
    public static class DebugUtility
    {
        public static void DrawPerfomanceData(GameLoop gameLoop, Color fontColor)
        {
            Font consoleFont = TextFont.font;
            if (consoleFont == null) return;

            string totalTimeElapsedStr = gameLoop.GameTime.TotalTimeElapsed.ToString("0.000");
            string deltaTime = gameLoop.GameTime.deltaTime.ToString("0.00000");
            float fps = 1f / gameLoop.GameTime.deltaTime;
            string fpsStr = fps.ToString("0.00");


            Text text = new Text("totalTimeElapsed: " + totalTimeElapsedStr, consoleFont, 20);
            text.Position = new Vector2f(4f, 208f);
            text.FillColor = fontColor;
            text.OutlineColor = Color.Black;
            text.OutlineThickness = 1;

            Text textB = new Text("deltaTime: " + deltaTime, consoleFont, 20);
            textB.Position = new Vector2f(4f, 228f);
            textB.FillColor = fontColor;
            textB.OutlineColor = Color.Black;
            textB.OutlineThickness = 1;

            Text textC = new Text("fps: " + fpsStr, consoleFont, 20);
            textC.Position = new Vector2f(4f, 248f);
            textC.FillColor = fontColor;
            textC.OutlineColor = Color.Black;
            textC.OutlineThickness = 1;

            gameLoop.Window.Draw(text);
            gameLoop.Window.Draw(textB);
            gameLoop.Window.Draw(textC);
        }
    }
}
