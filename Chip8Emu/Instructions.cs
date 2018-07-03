using System;
using System.Collections.Generic;

namespace Chip8Emu
{
    public sealed class Instructions
    {
        private const ushort INSTRUCTIONS_COUNT = 36;

        private static Instructions instructions;
        public Dictionary<ushort, InstructionHandler> instructionHandlers;

        public delegate void InstructionHandler(CPUInstruction instruction, CPU cpu, RAM ram, Screen screen, Keyboard keyboard);

        public static Instructions GetInstructions()
        {
            if (instructions == null)
                instructions = new Instructions();
            return instructions;
        }

        private Instructions()
        {
            instructionHandlers = new Dictionary<ushort, InstructionHandler>()
            {
                { 0x0, new InstructionHandler(instruction_0x0_handler) },
                { 0x1, new InstructionHandler(instruction_0x1_handler) },
                { 0x2, new InstructionHandler(instruction_0x2_handler) },
                { 0x3, new InstructionHandler(instruction_0x3_handler) },
                { 0x4, new InstructionHandler(instruction_0x4_handler) },
                { 0x5, new InstructionHandler(instruction_0x5_handler) },
                { 0x6, new InstructionHandler(instruction_0x6_handler) },
                { 0x7, new InstructionHandler(instruction_0x7_handler) },
                { 0x8, new InstructionHandler(instruction_0x8_handler) },
                { 0x9, new InstructionHandler(instruction_0x9_handler) },
                { 0xA, new InstructionHandler(instruction_0xA_handler) },
                { 0xB, new InstructionHandler(instruction_0xB_handler) },
                { 0xC, new InstructionHandler(instruction_0xC_handler) },
                { 0xD, new InstructionHandler(instruction_0xD_handler) },
                { 0xE, new InstructionHandler(instruction_0xE_handler) },
                { 0xF, new InstructionHandler(instruction_0xF_handler) }
            };
        }

        private void instruction_0x0_handler(CPUInstruction instruction, CPU cpu, RAM ram, Screen screen, Keyboard keyboard)
        {
            if (instruction.KK == 0xE0)
            {
                screen.Clear();
                return;
            }

            if (instruction.KK == 0xEE)
            {
                cpu.PC = cpu.SubroutineStack.Pop();
                return;
            }
        }

        private void instruction_0x1_handler(CPUInstruction instruction, CPU cpu, RAM ram, Screen screen, Keyboard keyboard)
        {
            cpu.PC = instruction.NNN;
        }

        private void instruction_0x2_handler(CPUInstruction instruction, CPU cpu, RAM ram, Screen screen, Keyboard keyboard)
        {
            cpu.SubroutineStack.Push(cpu.PC);
            cpu.PC = instruction.NNN;
        }

        private void instruction_0x3_handler(CPUInstruction instruction, CPU cpu, RAM ram, Screen screen, Keyboard keyboard)
        {
            if (cpu.VX[instruction.X] == instruction.KK)
                cpu.SkipInstruction();
        }

        private void instruction_0x4_handler(CPUInstruction instruction, CPU cpu, RAM ram, Screen screen, Keyboard keyboard)
        {
            if (cpu.VX[instruction.X] != instruction.KK)
                cpu.SkipInstruction();
        }

        private void instruction_0x5_handler(CPUInstruction instruction, CPU cpu, RAM ram, Screen screen, Keyboard keyboard)
        {
            if (cpu.VX[instruction.X] == cpu.VX[instruction.Y])
                cpu.SkipInstruction();
        }

        private void instruction_0x6_handler(CPUInstruction instruction, CPU cpu, RAM ram, Screen screen, Keyboard keyboard)
        {
            cpu.VX[instruction.X] = instruction.KK;
        }

        private void instruction_0x7_handler(CPUInstruction instruction, CPU cpu, RAM ram, Screen screen, Keyboard keyboard)
        {
            cpu.VX[instruction.X] += instruction.KK;
        }

        private void instruction_0x8_handler(CPUInstruction instruction, CPU cpu, RAM ram, Screen screen, Keyboard keyboard)
        {
            if (instruction.N == 0x0)
            {
                cpu.VX[instruction.X] = cpu.VX[instruction.Y];
                return;
            }

            if (instruction.N == 0x1)
            {
                cpu.VX[instruction.X] = (byte)(cpu.VX[instruction.X] | cpu.VX[instruction.Y]);
                return;
            }

            if (instruction.N == 0x2)
            {
                cpu.VX[instruction.X] = (byte)(cpu.VX[instruction.X] & cpu.VX[instruction.Y]);
                return;
            }

            if (instruction.N == 0x3)
            {
                cpu.VX[instruction.X] = (byte)(cpu.VX[instruction.X] ^ cpu.VX[instruction.Y]);
                return;
            }

            if (instruction.N == 0x4)
            {
                if (cpu.VX[instruction.X] + cpu.VX[instruction.Y] > 255)
                {
                    cpu.VX[0xF] = 1;
                }
                else
                {
                    cpu.VX[0xF] = 0;
                }

                cpu.VX[instruction.X] = (byte)(cpu.VX[instruction.X] + cpu.VX[instruction.Y]);
                return;
            }

            if (instruction.N == 0x5)
            {
                if (cpu.VX[instruction.X] > cpu.VX[instruction.Y])
                    cpu.VX[0xF] = 1;
                else
                    cpu.VX[0xF] = 0;

                cpu.VX[instruction.X] = (byte)(cpu.VX[instruction.X] - cpu.VX[instruction.Y]);
                return;
            }

            if (instruction.N == 0x6)
            {
                if ((cpu.VX[instruction.X] & 0x1) == 0x1)
                    cpu.VX[0xF] = 1;
                else
                    cpu.VX[0xF] = 0;

                cpu.VX[instruction.X] = (byte)(cpu.VX[instruction.X] / 2);
                return;
            }

            if (instruction.N == 0x7)
            {
                if (cpu.VX[instruction.Y] > cpu.VX[instruction.X])
                    cpu.VX[0xF] = 1;
                else
                    cpu.VX[0xF] = 0;

                cpu.VX[instruction.X] = (byte)(cpu.VX[instruction.Y] - cpu.VX[instruction.X]);
                return;
            }

            if (instruction.N == 0xE)
            {
                if ((cpu.VX[instruction.X] & 0x80) == 0x80)
                    cpu.VX[0xF] = 1;
                else
                    cpu.VX[0xF] = 0;

                cpu.VX[instruction.X] = (byte)(cpu.VX[instruction.X] * 2);
                return;
            }
        }

        private void instruction_0x9_handler(CPUInstruction instruction, CPU cpu, RAM ram, Screen screen, Keyboard keyboard)
        {
            if (cpu.VX[instruction.X] != cpu.VX[instruction.Y])
                cpu.SkipInstruction();
        }

        private void instruction_0xA_handler(CPUInstruction instruction, CPU cpu, RAM ram, Screen screen, Keyboard keyboard)
        {
            cpu.I = instruction.NNN;
        }

        private void instruction_0xB_handler(CPUInstruction instruction, CPU cpu, RAM ram, Screen screen, Keyboard keyboard)
        {
            cpu.PC = (ushort)(instruction.NNN + cpu.VX[0]);
        }

        private void instruction_0xC_handler(CPUInstruction instruction, CPU cpu, RAM ram, Screen screen, Keyboard keyboard)
        {
            byte randomVal = (byte)cpu.Randomizer.Next(0, byte.MaxValue + 1);
            cpu.VX[instruction.X] = (byte)(randomVal & instruction.KK);
        }

        private void instruction_0xD_handler(CPUInstruction instruction, CPU cpu, RAM ram, Screen screen, Keyboard keyboard)
        {
            byte[] sprites = ram.Read(cpu.I, (uint)cpu.I + instruction.N - 1);
            uint x = cpu.VX[instruction.X];
            uint y = cpu.VX[instruction.Y];
            cpu.VX[0xF] = screen.XorPixel(sprites, x, y) ? (byte)1 : (byte)0;
        }

        private void instruction_0xE_handler(CPUInstruction instruction, CPU cpu, RAM ram, Screen screen, Keyboard keyboard)
        {
            if (instruction.KK == 0x9E)
            {
                byte key = cpu.VX[instruction.X];
                if (keyboard.IsKeyPressed(key))
                    cpu.SkipInstruction();
                return;
            }

            if (instruction.KK == 0xA1)
            {
                byte key = cpu.VX[instruction.X];
                if (!keyboard.IsKeyPressed(key))
                    cpu.SkipInstruction();
                return;
            }
        }

        private void instruction_0xF_handler(CPUInstruction instruction, CPU cpu, RAM ram, Screen screen, Keyboard keyboard)
        {
            if (instruction.KK == 0x07)
            {
                cpu.VX[instruction.X] = cpu.DelayTimer;
                return;
            }

            if (instruction.KK == 0x0A)
            {
                bool pressedKey = false;
                while (!pressedKey)
                {
                    for (byte key = 0; key <= 0xF; ++key)
                    {
                        pressedKey = keyboard.IsKeyPressed(key);
                        if (pressedKey)
                        {
                            cpu.VX[instruction.X] = key;
                            break;
                        }
                    }
                }
                return;
            }

            if (instruction.KK == 0x15)
            {
                cpu.DelayTimer = cpu.VX[instruction.X];
                return;
            }

            if (instruction.KK == 0x18)
            {
                cpu.SoundTimer = cpu.VX[instruction.X];
                return;
            }

            if (instruction.KK == 0x1E)
            {
                cpu.I += cpu.VX[instruction.X];
                return;
            }

            if (instruction.KK == 0x29)
            {
                cpu.I = (ushort)(RAM.SPRITES_OFFSET + cpu.VX[instruction.X] * RAM.SPRITE_SIZE);
                return;
            }

            if (instruction.KK == 0x33)
            {
                byte number = cpu.VX[instruction.X];
                byte hundredsDigit = (byte)(Math.Floor((double)number / 100) % 10);
                byte tensDigit = (byte)(Math.Floor((double)number / 10) % 10);
                byte onesDigit = (byte)(number % 10);

                byte[] data = { hundredsDigit, tensDigit, onesDigit };
                ram.LoadToMemory(data, cpu.I);

                return;
            }

            if (instruction.KK == 0x55)
            {
                byte[] data = new byte[instruction.X + 1];
                for (uint i = 0; i <= instruction.X; ++i)
                {
                    data[i] = cpu.VX[i];
                }
                ram.LoadToMemory(data, cpu.I);
                return;
            }

            if (instruction.KK == 0x65)
            {
                byte[] data = ram.Read(cpu.I, (uint)cpu.I + instruction.X);

                for (uint i = 0; i < data.Length; ++i)
                {
                    cpu.VX[i] = data[i];
                }

                return;
            }
        }

    }
}
