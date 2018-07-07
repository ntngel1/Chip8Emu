# Chip8Emu
CHIP-8 Emulator created using C# and SFML. It makes me really surprised that I have done this project.

## Usage
You can run any CHIP-8 ROM using through emulator's CLI:
```ruby
Chip8Emu -f "path to ROM"
```
Also you can specify some parameters:
```ruby
-s | Scale of the screen
-d | Delay of keyboard input in milliseconds (it's needs by some games
     like Tetris because by default Tetrominoes moves really fast 
     because of zero-delay input)
```

## Some interesting resources
[Here you can download a little pack of CHIP-8 games](https://www.zophar.net/pdroms/chip8/chip-8-games-pack.html)
Also I used [this documentation](http://devernay.free.fr/hacks/chip8/C8TECH10.HTM) to write my emulator.
