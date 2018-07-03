using System;
using System.IO;

namespace Chip8Emu
{
    public class RAM 
    {
        private const ushort MEMORY_SIZE = 4096;
        public const ushort SPRITES_OFFSET = 0x000;
        public const ushort SPRITE_SIZE = 5;
        public const ushort ROM_OFFSET = 0x200;

        public byte[] Memory;

        public RAM()
        {
            Memory = new byte[MEMORY_SIZE];
            LoadSprites();
        }

        public void LoadToMemory(byte[] data, uint offset)
        {
            for (uint i = 0; i < data.Length; ++i)
            {
                Memory[i + offset] = data[i];
            }
        }

        public void LoadROM(string path)
        {
            byte[] data = File.ReadAllBytes(path);
            LoadToMemory(data, ROM_OFFSET);
        }

        public void LoadSprites()
        {
            // Numbers
            byte[] zero = { 0xF0, 0x90, 0x90, 0x90, 0xF0 };
            byte[] one = { 0x20, 0x60, 0x20, 0x20, 0x70 };
            byte[] two = { 0xF0, 0x10, 0xF0, 0x80, 0xF0 };
            byte[] three = { 0xF0, 0x10, 0xF0, 0x10, 0xF0 };
            byte[] four = { 0x90, 0x90, 0xF0 ,0x10, 0x10 };
            byte[] five = { 0xF0, 0x80, 0xF0, 0x10, 0xF0 };
            byte[] six = { 0xF0, 0x80, 0xF0, 0x90, 0xF0 };
            byte[] seven = { 0xF0, 0x10, 0x20, 0x40, 0x40 };
            byte[] eight = { 0xF0, 0x90, 0xF0, 0x90, 0xF0 };
            byte[] nine = { 0xF0, 0x90, 0xF0, 0x10, 0xF0 };

            // Characters
            byte[] a = { 0xF0, 0x90, 0xF0, 0x90, 0x90 };
            byte[] b = { 0xE0, 0x90 ,0xE0, 0x90, 0xE0 };
            byte[] c = { 0xF0, 0x80, 0x80, 0x80, 0xF0 };
            byte[] d = { 0xE0, 0x90, 0x90, 0x90, 0xE0 };
            byte[] e = { 0xF0, 0x80, 0xF0, 0x80, 0xF0 };
            byte[] f = { 0xF0, 0x80, 0xF0, 0x80, 0x80 };

            byte[][] sprites = {zero, one, two, three, four, five, six, seven, eight, nine,
                                a, b, c, d, e, f};

            for (int i = 0; i < sprites.Length; ++i)
            {
                LoadToMemory(sprites[i], SPRITES_OFFSET + (uint)i * 5);
            }
        }

        public byte[] Read(uint from, uint to)
        {
            uint size = to - from + 1;
            byte[] data = new byte[size];

            for (uint i = 0; i < size; ++i)
            {
                data[i] = Memory[from + i];
            }

            return data;
        }

        public void DisplayMemory()
        {
            string hex = BitConverter.ToString(Memory).Replace("-", " ");
            Console.WriteLine(hex);
        }
    }
}
