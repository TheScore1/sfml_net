using SFML.Graphics;
using SFML.System;

namespace SFMLproj
{
    public class CombinationsText
    {
        private Random rand;

        private const float RiseSpeed = 20f;
        private const float FadeSpeed = 100f;

        private Text text;
        private Vector2f position;
        private float alpha;

        public bool IsVisible { get; private set; }

        public CombinationsText(string combination, Vector2f position)
        {
            text = new Text(combination, TextFont.font, 20);
            text.FillColor = Color.White;
            text.OutlineColor = Color.Black;
            text.OutlineThickness = 1f;

            this.position = position;
            alpha = 255f;

            IsVisible = true;

            rand = new Random();
            var outlineColor = GenerateRandomOutlineColor();
            text.OutlineColor = outlineColor;
            text.OutlineThickness = 1.0f;
        }

        public void Update(float deltaTime)
        {
            // Поднятие текста вверх
            position.Y -= RiseSpeed * deltaTime;

            // Затухание текста
            alpha -= FadeSpeed * deltaTime;
            if (alpha <= 0f)
            {
                alpha = 0f;
                IsVisible = false;
            }

            // Обновление цвета текста с учетом альфа-канала
            Color color = text.FillColor;
            color.A = (byte)alpha;
            text.FillColor = color;
            color = text.OutlineColor;
            color.A = (byte)alpha;
            text.OutlineColor = color;

            // Установка позиции текста
            text.Position = position;
        }

        public void Draw(RenderTarget target)
        {
            target.Draw(text);
        }

        private Color GenerateRandomOutlineColor()
        {
            byte r = (byte)rand.Next(256); // Случайное значение для красного цвета (0-255)
            byte g = (byte)rand.Next(256); // Случайное значение для зеленого цвета (0-255)
            byte b = (byte)rand.Next(256); // Случайное значение для синего цвета (0-255)
            byte a = (byte)rand.Next(256); // Случайное значение для альфа-канала (0-255)

            return new Color(r, g, b, a);
        }
    }
}
