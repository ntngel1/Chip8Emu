using SFML.System;

namespace Chip8Emu
{
    public class Timer
    {
        private float timeUntilTick;
        private float deltaTime;
        private Clock clock;

        public delegate void TimerTickHandler();
        public event TimerTickHandler OnTick;

        public Timer(uint frequency = 60)
        {
            timeUntilTick = 1.0f / frequency;
            clock = new Clock();
            clock.Restart();
        }

        public void Restart()
        {
            clock.Restart();
            deltaTime = 0;
        }

        public void Update()
        {
            deltaTime += clock.ElapsedTime.AsSeconds();
            clock.Restart();

            if (deltaTime >= timeUntilTick)
            {
                OnTick?.Invoke();
                deltaTime = 0;
            }
        }
    }
}