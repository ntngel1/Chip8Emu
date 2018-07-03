using System;
using System.Collections.Generic;

namespace Chip8Emu
{
    public class CPU
    {
        private const uint VX_SIZE = 16;
        private const uint STACK_SIZE = 16;

        public byte[] VX;
        public Stack<ushort> SubroutineStack;
        public ushort I;
        public ushort PC;
        public byte SP;
        public byte DelayTimer;

        public byte SoundTimer;

        public Random Randomizer;

        private CPUInstruction CurrentInstruction;
        private Instructions instructions;

        private RAM ram;
        private Screen screen;
        private Keyboard keyboard;

        public CPU(Screen screen, RAM ram, Keyboard keyboard)
        {
            this.screen = screen;
            this.ram = ram;
            this.keyboard = keyboard;

            VX = new byte[VX_SIZE];
            SubroutineStack = new Stack<ushort>((int)STACK_SIZE);
            Randomizer = new Random();
            instructions = Instructions.GetInstructions();

            PC = RAM.ROM_OFFSET;
        }

        public void Execute()
        {
            instructions.instructionHandlers[CurrentInstruction.Command]?
                .Invoke(CurrentInstruction, this, ram, screen, keyboard);
        }

        public void Tick()
        {
            LoadInstruction();
            Execute();
        }

        public void TimerTick()
        {
            if (DelayTimer > 0)
            {
                --DelayTimer;
            }

            if (SoundTimer > 0)
            {
                --SoundTimer;
            }
        }

        public void LoadInstruction()
        {
            ushort instr = (ushort)(ram.Memory[PC] << 8 | ram.Memory[PC + 1]);
            CurrentInstruction = new CPUInstruction(instr);

            PC += 2;
        }

        public void SkipInstruction()
        {
            PC += 2;
        }
    }
}
