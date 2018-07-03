using System;

namespace Chip8Emu
{
    public class Keyboard
    {
        private const ushort KEYBOARD_DELAY_MS = 75;

        private class UnrecognizedKeyException : Exception
        {
            public UnrecognizedKeyException(string message) : base(message)
            {
            }
        }

        public byte lastKeyPressed = 0xFF;
        private uint inputDelay;

        public Keyboard(uint inputDelay = 0)
        {
            this.inputDelay = inputDelay;
        }

        public bool IsKeyPressed(byte key)
        {
            switch (key)
            {
                case 0x0:
                    return CheckKey(SFML.Window.Keyboard.Key.Num0, key);
                case 0x1:
                    return CheckKey(SFML.Window.Keyboard.Key.Num1, key);
                case 0x2:
                    return CheckKey(SFML.Window.Keyboard.Key.Num2, key);
                case 0x3:
                    return CheckKey(SFML.Window.Keyboard.Key.Num3, key);
                case 0x4:
                    return CheckKey(SFML.Window.Keyboard.Key.Num4, key);
                case 0x5:
                    return CheckKey(SFML.Window.Keyboard.Key.Num5, key);
                case 0x6:
                    return CheckKey(SFML.Window.Keyboard.Key.Num6, key);
                case 0x7:
                    return CheckKey(SFML.Window.Keyboard.Key.Num7, key);
                case 0x8:
                    return CheckKey(SFML.Window.Keyboard.Key.Num8, key);
                case 0x9:
                    return CheckKey(SFML.Window.Keyboard.Key.Num9, key);
                case 0xA:
                    return CheckKey(SFML.Window.Keyboard.Key.A, key);
                case 0xB:
                    return CheckKey(SFML.Window.Keyboard.Key.B, key);
                case 0xC:
                    return CheckKey(SFML.Window.Keyboard.Key.C, key);
                case 0xD:
                    return CheckKey(SFML.Window.Keyboard.Key.D, key);
                case 0xE:
                    return CheckKey(SFML.Window.Keyboard.Key.E, key);
                case 0xF:
                    return CheckKey(SFML.Window.Keyboard.Key.F, key);
                default:
                    throw new UnrecognizedKeyException("Key: " + key.ToString("X"));
            }
        }

        private bool CheckKey(SFML.Window.Keyboard.Key key, byte hexKey)
        {
            if (SFML.Window.Keyboard.IsKeyPressed(key))
            {
                if (inputDelay != 0)
                    System.Threading.Thread.Sleep((int)inputDelay);
                return true;
            }

            return false;
        }


    }
}
