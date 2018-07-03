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
            // Close the window
            if (instruction.KK == 0x00)
            {
                screen.Window.Close();
            }

            // Clear the screen
            if (instruction.KK == 0xE0)
            {
                screen.Clear();
                return;
            }

            // Return from a subroutine
            if (instruction.KK == 0xEE)
            {
                cpu.PC = cpu.SubroutineStack.Pop();
                return;
            }
        }

        private void instruction_0x1_handler(CPUInstruction instruction, CPU cpu, RAM ram, Screen screen, Keyboard keyboard)
        {
            // Jump to location nnn
            cpu.PC = instruction.NNN;
        }

        private void instruction_0x2_handler(CPUInstruction instruction, CPU cpu, RAM ram, Screen screen, Keyboard keyboard)
        {
            // Call subroutine at nnn
            cpu.SubroutineStack.Push(cpu.PC);
            cpu.PC = instruction.NNN;
        }

        private void instruction_0x3_handler(CPUInstruction instruction, CPU cpu, RAM ram, Screen screen, Keyboard keyboard)
        {
            // Skip next instruction if Vx = kk
            if (cpu.VX[instruction.X] == instruction.KK)
                cpu.SkipInstruction();
        }

        private void instruction_0x4_handler(CPUInstruction instruction, CPU cpu, RAM ram, Screen screen, Keyboard keyboard)
        {
            // Skip next instruction if Vx != kk
            if (cpu.VX[instruction.X] != instruction.KK)
                cpu.SkipInstruction();
        }

        private void instruction_0x5_handler(CPUInstruction instruction, CPU cpu, RAM ram, Screen screen, Keyboard keyboard)
        {
            // Skip next instruction if Vx = Vy
            if (cpu.VX[instruction.X] == cpu.VX[instruction.Y])
                cpu.SkipInstruction();
        }

        private void instruction_0x6_handler(CPUInstruction instruction, CPU cpu, RAM ram, Screen screen, Keyboard keyboard)
        {
            // Set Vx = kk
            cpu.VX[instruction.X] = instruction.KK;
        }

        private void instruction_0x7_handler(CPUInstruction instruction, CPU cpu, RAM ram, Screen screen, Keyboard keyboard)
        {
            // Set Vx = Vx + kk
            cpu.VX[instruction.X] = (byte)(cpu.VX[instruction.X] + instruction.KK);
        }

        private void instruction_0x8_handler(CPUInstruction instruction, CPU cpu, RAM ram, Screen screen, Keyboard keyboard)
        {
            // Set Vx = Vy
            if (instruction.N == 0x0)
            {
                cpu.VX[instruction.X] = cpu.VX[instruction.Y];
                return;
            }

            // Set Vx = Vx OR Vy.
            if (instruction.N == 0x1)
            {
                cpu.VX[instruction.X] = (byte)(cpu.VX[instruction.X] | cpu.VX[instruction.Y]);
                return;
            }

            // Set Vx = Vx AND Vy
            if (instruction.N == 0x2)
            {
                cpu.VX[instruction.X] = (byte)(cpu.VX[instruction.X] & cpu.VX[instruction.Y]);
                return;
            }

            // Set Vx = Vx XOR Vy.
            if (instruction.N == 0x3)
            {
                cpu.VX[instruction.X] = (byte)(cpu.VX[instruction.X] ^ cpu.VX[instruction.Y]);
                return;
            }

            // Set Vx = Vx + Vy, set VF = carry.
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

            // Set Vx = Vx - Vy, set VF = NOT borrow.
            if (instruction.N == 0x5)
            {
                if (cpu.VX[instruction.X] > cpu.VX[instruction.Y])
                    cpu.VX[0xF] = 1;
                else
                    cpu.VX[0xF] = 0;

                cpu.VX[instruction.X] = (byte)(cpu.VX[instruction.X] - cpu.VX[instruction.Y]);
                return;
            }

            // Set Vx = Vx SHR 1
            if (instruction.N == 0x6)
            {
                if ((cpu.VX[instruction.X] & 0x1) == 0x1)
                    cpu.VX[0xF] = 1;
                else
                    cpu.VX[0xF] = 0;

                cpu.VX[instruction.X] = (byte)(cpu.VX[instruction.X] / 2);
                return;
            }

            // Set Vx = Vy - Vx, set VF = NOT borrow.
            if (instruction.N == 0x7)
            {
                if (cpu.VX[instruction.Y] > cpu.VX[instruction.X])
                    cpu.VX[0xF] = 1;
                else
                    cpu.VX[0xF] = 0;

                cpu.VX[instruction.X] = (byte)(cpu.VX[instruction.Y] - cpu.VX[instruction.X]);
                return;
            }

            // Set Vx = Vx SHL 1.
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
            // Skip next instruction if Vx != Vy
            if (cpu.VX[instruction.X] != cpu.VX[instruction.Y])
                cpu.SkipInstruction();
        }

        private void instruction_0xA_handler(CPUInstruction instruction, CPU cpu, RAM ram, Screen screen, Keyboard keyboard)
        {
            // Set I = nnn
            cpu.I = instruction.NNN;
        }

        private void instruction_0xB_handler(CPUInstruction instruction, CPU cpu, RAM ram, Screen screen, Keyboard keyboard)
        {
            // Jump to location nnn + V0
            cpu.PC = (ushort)(instruction.NNN + cpu.VX[0]);
        }

        private void instruction_0xC_handler(CPUInstruction instruction, CPU cpu, RAM ram, Screen screen, Keyboard keyboard)
        {
            // Set Vx = random byte AND kk.
            byte randomVal = (byte)cpu.Randomizer.Next(0, byte.MaxValue); // MAYBE WE NEED +1?
            cpu.VX[instruction.X] = (byte)(randomVal & instruction.KK);
        }

        private void instruction_0xD_handler(CPUInstruction instruction, CPU cpu, RAM ram, Screen screen, Keyboard keyboard)
        {
            // Display n-byte sprite starting at memory location I at (Vx, Vy), set VF = collision.
            byte[] sprites = ram.Read(cpu.I, (uint)cpu.I + instruction.N - 1);
            uint x = cpu.VX[instruction.X];
            uint y = cpu.VX[instruction.Y];
            cpu.VX[0xF] = screen.XorPixel(sprites, x, y) ? (byte)1 : (byte)0;
        }

        private void instruction_0xE_handler(CPUInstruction instruction, CPU cpu, RAM ram, Screen screen, Keyboard keyboard)
        {
            // Skip next instruction if key with the value of Vx is pressed.
            if (instruction.KK == 0x9E)
            {
                byte key = cpu.VX[instruction.X];
                if (keyboard.IsKeyPressed(key))
                    cpu.SkipInstruction();
                return;
            }

            // Skip next instruction if key with the value of Vx is not pressed.
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
            // Set Vx = delay timer value.
            if (instruction.KK == 0x07)
            {
                cpu.VX[instruction.X] = cpu.DelayTimer;
                return;
            }

            // Wait for a key press, store the value of the key in Vx.
            // I THINK I'VE FOUND A FUCKING ERROR!!!!
            if (instruction.KK == 0x0A)
            {
                /*
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
                }*/
                bool pressedKey = false;
                for (byte key = 0; key <= 0xF; ++key)
                {
                    pressedKey = keyboard.IsKeyPressed(key);
                    if (pressedKey)
                    {
                        cpu.VX[instruction.X] = key;
                        break;
                    }
                }

                if (!pressedKey)
                    cpu.PC -= 2;
                return;
            }

            // Set delay timer = Vx.
            if (instruction.KK == 0x15)
            {
                cpu.DelayTimer = cpu.VX[instruction.X];
                return;
            }

            // Set sound timer = Vx.
            if (instruction.KK == 0x18)
            {
                cpu.SoundTimer = cpu.VX[instruction.X];
                return;
            }

            // Set I = I + Vx.
            if (instruction.KK == 0x1E)
            {
                cpu.I += cpu.VX[instruction.X];
                return;
            }

            // Set I = location of sprite for digit Vx.
            if (instruction.KK == 0x29)
            {
                cpu.I = (ushort)(RAM.SPRITES_OFFSET + cpu.VX[instruction.X] * RAM.SPRITE_SIZE);
                return;
            }

            // Store BCD representation of Vx in memory locations I, I+1, and I+2.
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

            // Store registers V0 through Vx in memory starting at location I.
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

            // Read registers V0 through Vx from memory starting at location I.
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
