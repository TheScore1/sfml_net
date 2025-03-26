// https://youtu.be/NL1zhckb5hc
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Formats.Asn1;

namespace SFMLproj
{
    public abstract class GameLoop
    {
        public const int TargetFps = 60;
        public const float TimeUntilUpdate = 1f / TargetFps; // Время между каждым тиками

        public RenderWindow Window
        {
            get;
            protected set;
        }

        public GameTime GameTime
        {
            get;
            protected set;
        }

        public Color WindowClearColor
        {
            get;
            protected set;
        }

        protected GameLoop(uint windowWidth, uint windowHeight, string windowTitle, Color windowClearColor)
        {
            this.WindowClearColor = windowClearColor;
            // this.Window = new RenderWindow(new VideoMode(windowWidth, windowHeight), windowTitle);
            // Сделал так, что нельзя изменить размер системного окна
            this.Window = new RenderWindow(new VideoMode(windowWidth, windowHeight), windowTitle, Styles.Close | Styles.Titlebar);
            this.GameTime = new GameTime();
            Window.Closed += WindowClosed;
        }

        public void Run()
        {
            LoadContent();
            Initialize();

            float totalTimeBeforeUpdate = 0f;
            float previousTimeElapsed = 0f;
            float deltaTime = 0f;
            float totalTimeElapsed = 0f;

            Clock clock = new Clock();
            
            while (Window.IsOpen)
            {
                Window.DispatchEvents();

                totalTimeElapsed = clock.ElapsedTime.AsSeconds();
                deltaTime = totalTimeElapsed - previousTimeElapsed;
                previousTimeElapsed = totalTimeElapsed;

                totalTimeBeforeUpdate += deltaTime;

                if (totalTimeBeforeUpdate >= TimeUntilUpdate)
                {
                    GameTime.Update(totalTimeBeforeUpdate, clock.ElapsedTime.AsSeconds()); // clock.ElapsedTime.AsSeconds() т.к. уже сколько-то прошло с totalTimeElapsed
                    totalTimeBeforeUpdate = 0f;

                    Update(GameTime);

                    Window.Clear(WindowClearColor); // перед тем как рендерить новый кадр. Обязательно.

                    Draw(GameTime);
                    Window.Display();
                }
            }
        }

        private void WindowClosed(object? sender, EventArgs e)
        {
            Window.Close();
        }

        public abstract void LoadContent();
        public abstract void Initialize();
        public abstract void Update(GameTime gameTime); // тик
        public abstract void Draw(GameTime gameTime);
    }
}
