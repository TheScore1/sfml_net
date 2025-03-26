// https://youtu.be/NL1zhckb5hc
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SFMLproj
{
    public static class Score
    {
        public static Font scoreFont = TextFont.font;
        public static uint Clicks = 0;
        public static uint Money;
        public static float MoneyFactor = 1;
        public static bool moneyFactorActive;
        public static float TotalTimeBeforeEventEnd;

        public static void AddMoneyByFactor(float n)
        {
            Money += Convert.ToUInt32(n * MoneyFactor);
        }

        public static void DrawData(GameLoop gameLoop, Color color)
        {
            Text money = new Text("Money: " + Money, scoreFont, 40);
            money.OutlineColor = Color.Black;
            money.OutlineThickness = 4;
            gameLoop.Window.Draw(money);
            /*Text clicks = new Text("clicks: " + Clicks, scoreFont, 16);
            clicks.Position = new Vector2f(4, 4);*/
            /*clicks.Position = new Vector2f(4, 24);*/
            /*gameLoop.Window.Draw(clicks);*/
        }

        public static void DrawMoneyFactorChanged(GameLoop gameLoop)
        {
            Text text = new Text($"Money Factor Changed! It's x{MoneyFactor} for {TotalTimeBeforeEventEnd.ToString("0.0")}s", TextFont.font, 40);
            text.Position = new Vector2f(1120 / 2, 20);
            text.Position = new Vector2f((1120 / 2 - text.GetLocalBounds().Width / 2), 30);
            text.FillColor = Color.White;
            text.OutlineColor = Color.Black;
            text.OutlineThickness = 3;
            gameLoop.Window.Draw(text);
        }
        public static void DrawTimeFactorChanged(GameLoop gameLoop)
        {
            Text text = new Text($"Money Factor Changed! It's x{MoneyFactor} for {TotalTimeBeforeEventEnd.ToString("0.0")}s", TextFont.font, 40);
            text.Position = new Vector2f(1120 / 2, 20);
            text.Position = new Vector2f((1120 / 2 - text.GetLocalBounds().Width / 2), 30);
            text.FillColor = Color.White;
            text.OutlineColor = Color.Black;
            text.OutlineThickness = 3;
            gameLoop.Window.Draw(text);
        }
    }
}
