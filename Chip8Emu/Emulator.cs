using System;
using System.Collections.Generic;
using CommandLine;

namespace Chip8Emu
{
    class Emulator
    {
        class CLIOptions
        {
            [Option('f', "file", Required = true, HelpText = "ROM to emulate")]
            public string RomPath { get; set; }
            [Option('s', "scale", Required = false, Default = (uint)8, HelpText = "Scale of screen")]
            public uint Scale { get; set; }
            [Option('d', "delay", Required = false, Default = (uint)0, HelpText = "Delay of keyboard input")]
            public uint KeyboardDelay { get; set; }
        }

        static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<CLIOptions>(args)
                .WithParsed<CLIOptions>(opts => StartEmulating(opts))
                .WithNotParsed<CLIOptions>((errs) => HandleParseError(errs));
        }

        static void StartEmulating(CLIOptions options)
        {
            Screen screen = new Screen(options.Scale);
            Keyboard keyboard = new Keyboard(options.KeyboardDelay);

            RAM ram = new RAM();
            ram.LoadROM(options.RomPath);

            CPU cpu = new CPU(screen, ram, keyboard);
            Timer timer = new Timer(60);

            timer.OnTick += () => cpu.TimerTick();
            screen.OnResumed += () => timer.Restart();

            while (screen.Window.IsOpen)
            {
                timer.Update();
                cpu.Tick();
                screen.Render();
            }
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {
            foreach(Error err in errs)
            {
                Console.WriteLine(err.ToString());
            }
        }
    }
}
