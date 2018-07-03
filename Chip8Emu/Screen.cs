using System;
using SFML.System;
using SFML.Window;
using SFML.Graphics;
using System.Collections;
using System.Threading.Tasks;

namespace Chip8Emu
{
    public class Screen 
    {
        private const uint PIXELS_WIDTH = 64;
        private const uint PIXELS_HEIGHT = 32;
        private const string WINDOW_TITLE = "CHIP8-EMU";

        public RenderWindow Window;
        public delegate void ResumedHandler();
        public event ResumedHandler OnResumed;

        private bool[,] pixels;
        private uint pixelSize;
        private bool isUpdated;
        public bool IsPaused;

        public Screen(uint scale = 8)
        {
            pixelSize = scale;

            pixels = new bool[PIXELS_WIDTH, PIXELS_HEIGHT];

            Window = new RenderWindow(new VideoMode(0, 0), WINDOW_TITLE);
            updateWindowSize();

            Window.Resized += Window_Resized;
            Window.Closed += Window_Closed;
            Window.KeyPressed += Window_KeyPressed;
        }

        private void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            // Handle pause
            if (e.Code.Equals(SFML.Window.Keyboard.Key.Escape))
            {
                IsPaused = true;
                DrawPause();
                Console.WriteLine("Set paused = {0}", true);
                WaitForResumed();
                Console.WriteLine("Set paused = {0}", false);
                IsPaused = false;
                OnResumed?.Invoke();
            }
        }

        private void DrawPause()
        {
            Window.Clear(Color.Blue);
            Font f = new Font("./Resources/font.ttf");
            Text t = new Text("PAUSED", f, 44);
            t.Color = Color.White;
            //t.Origin = new Vector2f(0.5f, 0.5f);
            t.Position = new Vector2f(0, 0);
            Window.Draw(t);
            Window.Display();
        }

        public void WaitForResumed()
        {
            while (!SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.Escape))
            {
                continue;
            }
        }

        public void Render()
        {
            if (!isUpdated)
                return;

            Window.DispatchEvents();
            Window.Clear(Color.Black);

            for (uint h = 0; h < PIXELS_HEIGHT; ++h)
            {
                for (uint w = 0; w < PIXELS_WIDTH; ++w)
                {
                    if (!pixels[w, h])
                        continue;

                    RectangleShape pixel = new RectangleShape(new Vector2f(pixelSize, pixelSize));
                    pixel.Position = new Vector2f(w * pixelSize, h * pixelSize);
                    Window.Draw(pixel);
                }
            }

            Window.Display();
            isUpdated = false;
        }

        public bool XorPixel(byte[] data, uint x, uint y)
        {
            bool collision = false;

            isUpdated = true;
            BitArray sprites = new BitArray(data);

            for (uint i = 0; i < sprites.Length / 8; ++i)
            {
                for (uint bit = 0; bit < 8; ++bit)
                {
                    int pixelPos = (int)(i * 8 + bit);
                    bool pixel = sprites[pixelPos];

                    int xPos = (int)(x + 7 - bit) % 64;
                    int yPos = (int)(y + i) % 32;

                    bool currentPixelValue = pixels[xPos, yPos];
                    pixels[xPos, yPos] ^= pixel;

                    
                    if (currentPixelValue == true && pixels[xPos, yPos] == false)
                        collision = true; 
                }
            }

            return collision;
        }

        public void Clear()
        {
            for (uint h = 0; h < PIXELS_HEIGHT; ++h)
            {
                for (uint w = 0; w < PIXELS_WIDTH; ++w)
                {
                    pixels[w, h] = false;
                }
            }
        }

        private void Window_Resized(object sender, SizeEventArgs e)
        {
            pixelSize = computePixelSize(e.Width, e.Height);
            updateWindowSize();

            if (IsPaused)
                DrawPause();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Window.Close();
        }

        private uint computePixelSize(uint width, uint height)
        {
            uint sizeByWidth = (uint)Math.Floor((double)width / PIXELS_WIDTH);
            if (sizeByWidth == 0)
                sizeByWidth = 1;

            uint sizeByHeight = (uint)Math.Floor((double)height / PIXELS_HEIGHT);
            if (sizeByHeight == 0)
                sizeByHeight = 1;

            if (sizeByHeight > sizeByWidth)
                return sizeByWidth;
            else
                return sizeByHeight;
        }

        private void updateWindowSize()
        {
            uint w = PIXELS_WIDTH * pixelSize;
            uint h = PIXELS_HEIGHT * pixelSize;
            Window.Size = new Vector2u(w, h);
            Window.SetView(new View(new FloatRect(0, 0, w, h)));
        }
    }
}
