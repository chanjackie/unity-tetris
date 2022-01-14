using System.Collections.Generic;
using UnityEngine;

public static class Data
{
    public static readonly float cos = Mathf.Cos(Mathf.PI / 2f);
    public static readonly float sin = Mathf.Sin(Mathf.PI / 2f);
    public static readonly float[] RotationMatrix = new float[] { cos, sin, -sin, cos };

    public static readonly Dictionary<Tetromino, Vector2Int[]> Cells = new Dictionary<Tetromino, Vector2Int[]>()
    {
        { Tetromino.I, new Vector2Int[] { new Vector2Int(-1, 1), new Vector2Int( 0, 1), new Vector2Int( 1, 1), new Vector2Int( 2, 1) } },
        { Tetromino.J, new Vector2Int[] { new Vector2Int(-1, 1), new Vector2Int(-1, 0), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        { Tetromino.L, new Vector2Int[] { new Vector2Int( 1, 1), new Vector2Int(-1, 0), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        { Tetromino.O, new Vector2Int[] { new Vector2Int( 0, 1), new Vector2Int( 1, 1), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        { Tetromino.S, new Vector2Int[] { new Vector2Int( 0, 1), new Vector2Int( 1, 1), new Vector2Int(-1, 0), new Vector2Int( 0, 0) } },
        { Tetromino.T, new Vector2Int[] { new Vector2Int( 0, 1), new Vector2Int(-1, 0), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        { Tetromino.Z, new Vector2Int[] { new Vector2Int(-1, 1), new Vector2Int( 0, 1), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
    };

    private static readonly Vector2Int[,] WallKicksI = new Vector2Int[,] {
        { new Vector2Int(0, 0), new Vector2Int(-2, 0), new Vector2Int( 1, 0), new Vector2Int(-2,-1), new Vector2Int( 1, 2) },
        { new Vector2Int(0, 0), new Vector2Int( 2, 0), new Vector2Int(-1, 0), new Vector2Int( 2, 1), new Vector2Int(-1,-2) },
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int( 2, 0), new Vector2Int(-1, 2), new Vector2Int( 2,-1) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int(-2, 0), new Vector2Int( 1,-2), new Vector2Int(-2, 1) },
        { new Vector2Int(0, 0), new Vector2Int( 2, 0), new Vector2Int(-1, 0), new Vector2Int( 2, 1), new Vector2Int(-1,-2) },
        { new Vector2Int(0, 0), new Vector2Int(-2, 0), new Vector2Int( 1, 0), new Vector2Int(-2,-1), new Vector2Int( 1, 2) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int(-2, 0), new Vector2Int( 1,-2), new Vector2Int(-2, 1) },
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int( 2, 0), new Vector2Int(-1, 2), new Vector2Int( 2,-1) },
    };

    private static readonly Vector2Int[,] WallKicksJLOSTZ = new Vector2Int[,] {
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0,-2), new Vector2Int(-1,-2) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( 1,-1), new Vector2Int(0, 2), new Vector2Int( 1, 2) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( 1,-1), new Vector2Int(0, 2), new Vector2Int( 1, 2) },
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0,-2), new Vector2Int(-1,-2) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( 1, 1), new Vector2Int(0,-2), new Vector2Int( 1,-2) },
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1,-1), new Vector2Int(0, 2), new Vector2Int(-1, 2) },
        { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1,-1), new Vector2Int(0, 2), new Vector2Int(-1, 2) },
        { new Vector2Int(0, 0), new Vector2Int( 1, 0), new Vector2Int( 1, 1), new Vector2Int(0,-2), new Vector2Int( 1,-2) },
    };

    public static readonly Dictionary<Tetromino, Vector2Int[,]> WallKicks = new Dictionary<Tetromino, Vector2Int[,]>()
    {
        { Tetromino.I, WallKicksI },
        { Tetromino.J, WallKicksJLOSTZ },
        { Tetromino.L, WallKicksJLOSTZ },
        { Tetromino.O, WallKicksJLOSTZ },
        { Tetromino.S, WallKicksJLOSTZ },
        { Tetromino.T, WallKicksJLOSTZ },
        { Tetromino.Z, WallKicksJLOSTZ },
    };

    // Tetromino gravity speeds specified by: https://tetris.fandom.com/wiki/Tetris_(NES,_Nintendo)
    public static readonly List<float> DropSpeeds = new List<float>() {
        0.8f, 0.7167f, 0.6333f, 0.55f, 0.4667f, 
        0.3833f, 0.3f, 0.2167f, 0.1333f, 0.1f, 
        0.0833f, 0.0833f, 0.0833f, 0.0667f, 0.0667f,
        0.0667f, 0.05f, 0.05f, 0.05f, 0.0333f,
        0.0333f, 0.0333f, 0.0333f, 0.0333f, 0.0333f, 
        0.0333f, 0.0333f, 0.0333f, 0.0333f, 0.0167f
    };

    // Scoring modeled after: https://tetris.wiki/Scoring
    public enum ClearType {
        NONE, 
        SINGLE,
        DOUBLE,
        TRIPLE,
        TETRIS,
        TSPIN_MINI,
        TSPIN,
        TSPIN_MINI_SINGLE,
        TSPIN_SINGLE,
        TSPIN_MINI_DOUBLE,
        TSPIN_DOUBLE,
        TSPIN_TRIPLE,
        SINGLE_PERFECT,
        DOUBLE_PERFECT,
        TRIPLE_PERFECT,
        TETRIS_PERFECT,
        B2B_TETRIS_PERFECT
    }

    public static readonly List<ClearType> DifficultClears = new List<ClearType>() {
        ClearType.TETRIS, 
        ClearType.TSPIN_MINI, 
        ClearType.TSPIN,
        ClearType.TSPIN_MINI_SINGLE,
        ClearType.TSPIN_SINGLE,
        ClearType.TSPIN_MINI_DOUBLE,
        ClearType.TSPIN_DOUBLE,
        ClearType.TSPIN_TRIPLE
    };

    public static readonly Dictionary<ClearType, int> Scores = new Dictionary<ClearType, int>() {
        {ClearType.SINGLE, 100}, 
        {ClearType.DOUBLE, 300}, 
        {ClearType.TRIPLE, 500}, 
        {ClearType.TETRIS, 800}, 
        {ClearType.TSPIN_MINI, 100}, 
        {ClearType.TSPIN, 400}, 
        {ClearType.TSPIN_MINI_SINGLE, 200}, 
        {ClearType.TSPIN_SINGLE, 800}, 
        {ClearType.TSPIN_MINI_DOUBLE, 400}, 
        {ClearType.TSPIN_DOUBLE, 1200}, 
        {ClearType.TSPIN_TRIPLE, 1600}, 
        {ClearType.SINGLE_PERFECT, 800}, 
        {ClearType.DOUBLE_PERFECT, 1200}, 
        {ClearType.TRIPLE_PERFECT, 1800}, 
        {ClearType.TETRIS_PERFECT, 2000}, 
        {ClearType.B2B_TETRIS_PERFECT, 3200}
    };

    // Temp dictionary, doesn't support T-Spins
    public static readonly Dictionary<int, ClearType> LinesToClearType = new Dictionary<int, ClearType>() {
        {0, ClearType.NONE}, 
        {1, ClearType.SINGLE},
        {2, ClearType.DOUBLE},
        {3, ClearType.TRIPLE},
        {4, ClearType.TETRIS}
    };
}
