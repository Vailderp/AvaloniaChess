using System;
using System.Threading.Tasks;

namespace VailderChessDesktop.Game;

public class ChessEnginePlayerNextMoveInfo
{
    public string? Moves;
    public string? Fen;
}

public class ChessEnginePlayer : IChessPlayer
{
    public ChessEnginePlayer()
    {
        _connector = new UciConnector();
    }
    
    private UciConnector _connector;
    
    public void Initialize(object initArg)
    {
        if (initArg is not string path)
        {
            throw new Exception("Initialize chess engine error: path of chess engine is null.");
        }
        _connector.ConnectTo(path);
    }

    public string NextMove(object nextMoveArg)
    {
        if (nextMoveArg is not ChessEnginePlayerNextMoveInfo nextMoveInfo)
        {
            throw new Exception("NextMove chess engine error: ChessEnginePlayerNextMoveInfo of chess engine is null.");
        }

        if (nextMoveInfo.Fen != null && nextMoveInfo.Moves != null)
        {
            _connector.SetPosition(nextMoveInfo.Fen, nextMoveInfo.Moves);
        }
        else if (nextMoveInfo.Fen != null && nextMoveInfo.Moves == null)
        {
            _connector.SetPosition(nextMoveInfo.Fen);
        }
        else if (nextMoveInfo.Fen == null && nextMoveInfo.Moves != null)
        {
            _connector.SetStartPosition(nextMoveInfo.Moves);
        }
        else
        {
            _connector.SetStartPosition();
        }

        return "";
    }

    public void Release(object releaseArg)
    {
        _connector.Quit();
    }
}