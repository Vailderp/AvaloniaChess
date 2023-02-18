namespace VailderChessDesktop.Game;

public interface IChessPlayer
{
    void Initialize(object initArg);
    string NextMove(object nextMoveArg);
    void Release(object releaseArg);
}