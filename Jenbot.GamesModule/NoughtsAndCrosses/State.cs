namespace Jenbot.GamesModule.NoughtsAndCrosses;

public class State
{
    private Tile[,] _tiles;
    private int[,] _placementTime;

    private int _clock;
    private int _size;

    public int CurrentTick => _clock;

    public State(int size)
    {
        if (size < 3)
        {
            throw new ArgumentOutOfRangeException(
                $"Invalid size passed ({size}), must be >= 3"
            );
        }

        _tiles = new Tile[size, size];
        _placementTime = new int[size, size];

        for (var i = 0; i < size; i++)
        {
            for (var j = 0; j < size; j++)
            {
                _tiles[i,j] = Tile.Empty;
                _placementTime[i,j] = -1;
            }
        }

        _clock = 0;
        _size = size;
    }

    public void EmptyTile(Tile tile, int i, int j)
    {
        ValidateBoardCoordinate(i, j);
        _tiles[i,j] = Tile.Empty;
        _placementTime[i,j] = -1;
    }

    public bool TryPlaceTile(Tile tile, int i, int j)
    {
        ValidateBoardCoordinate(i, j);

        if (_tiles[i,j] != Tile.Empty)
            return false;

        _clock++;

        _tiles[i,j] = tile;
        _placementTime[i,j] = _clock;

        return true;
    }

    public GameState EvaluateState()
    {
        var foundEmpty = false;

        var leadingDiagonalWin = true;
        var leadingDiagonalTile = _tiles[0,0];
        var otherDiagonalWin = true;
        var otherDiagonalTile = _tiles[_size - 1, 0];

        for (var i = 0; i < _size; i++)
        {
            // Check i-th row and column
            var rowWin = true;
            var rowTile = _tiles[i,0];
            var colWin = true;
            var colTile = _tiles[0,i];

            // Can skip one tile
            for (var j = 1; j < _size; j++)
            {
                var currentRow = _tiles[i,j];
                var currentCol = _tiles[j,i];

                // Only have to check rows for empties, since all spaces columns and diagonals check
                // are covered by rows too
                if (rowTile == Tile.Empty || currentRow == Tile.Empty)
                    foundEmpty = true;

                // Potential Win if the tiles match and they aren't empty
                rowWin = rowTile == currentRow && currentRow != Tile.Empty;
                colWin = colTile == currentCol && currentCol != Tile.Empty;
            }

            // Check if we found a row or column win
            if (rowWin)
            {
                return rowTile == Tile.Cross 
                    ? GameState.CrossWin 
                    : GameState.NoughtsWin;
            }

            if (colWin)
            {
                return colTile == Tile.Cross
                    ? GameState.CrossWin
                    : GameState.NoughtsWin;
            }

            // Don't check diagonal on first iteration
            if (i == 0)
                continue;

            // Continue checking diagonal
            var currentLeading = _tiles[i,i];
            var currentOther = _tiles[_size - 1 - i, _size - 1 - i];

            leadingDiagonalWin = leadingDiagonalTile == currentLeading && currentLeading != Tile.Empty;
            otherDiagonalWin = otherDiagonalTile == currentOther && currentOther != Tile.Empty;
        }

        // Check if we got a diagonal win
        if (leadingDiagonalWin)
        {
            return leadingDiagonalTile == Tile.Cross
                ? GameState.CrossWin
                : GameState.NoughtsWin;
        }

        if (otherDiagonalWin)
        {
            return otherDiagonalTile == Tile.Cross
                ? GameState.CrossWin
                : GameState.NoughtsWin;
        }

        // We didn't get a win, so we are either on going or drawn
        // This depends on whether we found an empty
        return foundEmpty ? GameState.Ongoing : GameState.Draw;
    }

    private void ValidateBoardCoordinate(int i, int j)
    {
        if (i < 0 || i >= _size)
            throw new ArgumentOutOfRangeException($"i is out of range; passed {i}");
        
        if (j < 0 || j >= _size)
            throw new ArgumentOutOfRangeException($"j is out of range; passed {j}");
    }
}
