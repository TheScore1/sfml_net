using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SFMLproj
{
    public class GameTime
    {
        private float DeltaTime = 0f;
        private float TimeScale = 1f;

        public float timeScale
        {
            get { return TimeScale; }
            set { TimeScale = value; }
        }

        public float deltaTime
        {
            get { return DeltaTime * TimeScale; }
            set { DeltaTime = value; }
        }

        public float DeltaTimeUncaled
        {
            get { return DeltaTime; }
        }

        public float TotalTimeElapsed
        {
            get;
            private set;
        }

        public GameTime()
        {
        }

        public void Update(float deltaTime, float totalTimeElapsed)
        {
            DeltaTime = deltaTime;
            TotalTimeElapsed = totalTimeElapsed;
        }
    }
}
