# MazeMaker

MazeMaker is the editor application of [MazeSuite](http://www.mazesuite.com) for creating and editing 3D maze environments used in spatial navigation research. Mazes designed in MazeMaker are run in the companion [MazeWalker](https://github.com/AyazLab/MazeWalker) engine.

## Features

- **2D top-down editor** for designing 3D maze environments with walls, floors, curved walls, start positions, end regions, active regions, lights, and 3D models
- **Maze Wizard** for procedural generation of rectangular and circular mazes
- **MazeList Builder** for creating experiment protocols (sequences of mazes, text screens, images, multiple choice questions, audio, and shell commands)
- **Collection editors** for managing textures, 3D models (.obj), and audio files
- **Maze packaging** to bundle mazes with assets into portable .mazx archives
- **Log processor** for splitting MazeWalker log files into per-maze segments with timing and path length reports
- Undo/redo, copy/paste, multi-select, drag-and-drop, and keyboard shortcuts

## Requirements

- Windows with .NET Framework 4.8
- Visual Studio 2012 or later
- [MazeLib](https://github.com/AyazLab/MazeLib) (sibling project dependency)

## Building

1. Clone this repository and [MazeLib](https://github.com/AyazLab/MazeLib) as sibling directories
2. Open `MazeMaker.sln` in Visual Studio
3. Build the solution (Debug, Release, or NSIS configuration)

## File Formats

| Format | Description |
|--------|-------------|
| `.maz` | XML-based maze file (walls, floors, textures, models, regions) |
| `.mazx` | Packaged maze (zip archive with .maz + assets) |
| `.mel` | Maze list experiment protocol |

## License

MIT License - Copyright (c) 2026 Ayaz Lab
