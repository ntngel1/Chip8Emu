using System;

namespace Chip8Emu
{
    public class CPUInstruction
    {
        private const ushort nnn_mask = 0xFFF;
        private const ushort n_mask = 0xF;
        private const ushort x_mask = 0xF00;
        private const ushort y_mask = 0xF0;
        private const ushort kk_mask = 0xFF;
        private const ushort command_mask = 0xF000;

        public ushort Instruction;
        public ushort Command;
        public ushort NNN;
        public ushort N;
        public ushort X;
        public ushort Y;
        public byte KK;

        public CPUInstruction(ushort instr)
        {
            Instruction = instr;
            Parse(instr);
        }

        private void Parse(ushort instr)
        {
            NNN = (ushort)(instr & nnn_mask);
            N = (ushort)(instr & n_mask);
            X = (ushort)((instr & x_mask) >> 8);
            Y = (ushort)((instr & y_mask) >> 4);
            KK = (byte)(instr & kk_mask);
            Command = (ushort)((instr & command_mask) >> 12);
        }
    }
}
