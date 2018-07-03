using System;
using System.Collections.Generic;
using CommandLine;
using System.Threading.Tasks;

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

        static Timer timer;
        static async void StartEmulating(CLIOptions options)
        {
            Keyboard keyboard = new Keyboard(options.KeyboardDelay);
            Screen screen = new Screen(options.Scale);
            RAM ram = new RAM();
            ram.LoadROM(options.RomPath);

            CPU cpu = new CPU(screen, ram, keyboard);
            timer = new Timer(60);

            timer.OnTick += () => cpu.TimerTick();
            screen.OnResumed += () => timer.Restart();

            UpdateTimerAsync();

            while (screen.Window.IsOpen)
            {
                cpu.Tick();
                screen.Render();
            }
        }

        static async void UpdateTimerAsync()
        {
            await Task.Factory.StartNew(()=>
            {
                while (true)
                {
                    timer.Update();
                }
            });
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
