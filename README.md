# TicTacToe CLI Game

This is a command-line interface (CLI) implementation of the classic Tic-Tac-Toe game, written in C#. The game offers both player-vs-player and player-vs-CPU modes, with customizable player symbols.

## Features

- Two game modes:
  - Player vs Player
  - Player vs CPU
- Customizable player symbols
- Interactive CLI interface with slow-print text for enhanced user experience
- CPU player with basic strategy (can detect winning moves and block opponent's winning moves)
- Option to play multiple games with the same or different configurations
- Detailed controls and help system

## How to Play

1. Run the game executable.
2. Choose your game mode (Player vs Player or Player vs CPU).
3. Decide whether to use default shapes (X and O) or custom shapes.
4. If using custom shapes, select your preferred symbols.
5. Follow the on-screen prompts to make your moves.
6. To make a move, you can either:
   - Enter a single number (1-9) corresponding to the grid position.
   - Enter coordinates in the format "column,row" or "column.row" (e.g., "1,1" or "1.1").
7. Type 'h' and press Enter at any time to display the controls.
8. Play until one player wins or the game ends in a draw.
9. Choose to play again with the same configuration, start a new game with different settings, or exit.

## Grid Positions

The grid positions are numbered as follows:

```
1 | 2 | 3
---------
4 | 5 | 6
---------
7 | 8 | 9
```

You can also use coordinates, where the top-left corner is (1,1) and the bottom-right corner is (3,3).

## Development

This game is implemented using object-oriented principles in C#. The main components include:

- `GameController`: Manages the game flow and user interactions.
- `Game`: Represents the game state and logic.
- `CpuGame`: Extends `Game` with CPU player functionality.
- `Player`: Represents a human player.
- `Cpu`: Represents the CPU player, extending `Player`.
- Various enums and helper classes for game options and move validation.

## Future Improvements

- Implement more advanced CPU strategies.
- Add a graphical user interface (GUI) version.
- Incorporate unit tests for better code reliability.
- Implement network play for remote multiplayer games.
- Add difficulty levels for the CPU player.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

[Add your chosen license here]
