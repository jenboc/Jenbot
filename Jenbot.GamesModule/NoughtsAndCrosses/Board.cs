namespace Jenbot.GamesModule.NoughtsAndCrosses;

public class Board
{
    private Tile[,] _tiles;

    public int Size { get; private set; }
    private int _maxTiles;

    private Queue<int[]> _noughtPlacements;
    private Queue<int[]> _crossPlacements;

    public Board(int size, int maxTiles = 0)
    {
        if (size < 3 || size > 5)
        {
            throw new ArgumentOutOfRangeException(
                $"Invalid size passed ({size}), must be between 3 and 5 (inclusive)"
            );
        }

        if (maxTiles > 0 && maxTiles < size)
        {
            throw new ArgumentOutOfRangeException(
                $"Invalid max tiles passed ({maxTiles}), if enabled should be at least {maxTiles}"
            );
        }

        _tiles = new Tile[size, size];

        for (var i = 0; i < size; i++)
            for (var j = 0; j < size; j++)
                _tiles[i,j] = Tile.Empty;

        Size = size;
        _maxTiles = maxTiles;
        _noughtPlacements = new();
        _crossPlacements = new();
    }

    public Tile this[int i, int j]
    {
        get => _tiles[i,j];
    }

    public bool TryPlaceTile(Tile tile, int i, int j)
    {
        ValidateBoardCoordinate(i, j);

        if (_tiles[i,j] != Tile.Empty)
            return false;

        _tiles[i,j] = tile;

        // Record placement in one of the queues
        var queue = tile == Tile.Cross ? _crossPlacements : _noughtPlacements;
        queue.Enqueue(new[] { i, j });

        // Have we exceeded the number of allowed tiles?
        if (queue.Count > _maxTiles && _maxTiles > 0)
        {
            var toRemove = queue.Dequeue();
            _tiles[toRemove[0], toRemove[1]] = Tile.Empty;
        }

        return true;
    }

    public GameState EvaluateState()
    {
        var foundEmpty = false;

        var leadingDiagonalWin = true;
        var leadingDiagonalTile = _tiles[0,0];
        var otherDiagonalWin = true;
        var otherDiagonalTile = _tiles[Size - 1, 0];

        for (var i = 0; i < Size; i++)
        {
            // Check i-th row and column
            var rowWin = true;
            var rowTile = _tiles[i,0];
            var colWin = true;
            var colTile = _tiles[0,i];

            // Can skip one tile
            for (var j = 1; j < Size; j++)
            {
                var currentRow = _tiles[i,j];
                var currentCol = _tiles[j,i];

                // Only have to check rows for empties, since all spaces columns and diagonals check
                // are covered by rows too
                if (rowTile == Tile.Empty || currentRow == Tile.Empty)
                    foundEmpty = true;

                // Potential Win if the tiles match and they aren't empty
                rowWin = rowWin && rowTile == currentRow && currentRow != Tile.Empty;
                colWin = colWin && colTile == currentCol && currentCol != Tile.Empty;
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
            var currentOther = _tiles[Size - 1 - i, Size - 1 - i];

            leadingDiagonalWin = leadingDiagonalWin && leadingDiagonalTile == currentLeading && currentLeading != Tile.Empty;
            otherDiagonalWin = otherDiagonalWin && otherDiagonalTile == currentOther && currentOther != Tile.Empty;
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
        if (i < 0 || i >= Size)
            throw new ArgumentOutOfRangeException($"i is out of range; passed {i}");
        
        if (j < 0 || j >= Size)
            throw new ArgumentOutOfRangeException($"j is out of range; passed {j}");
    }
}
