namespace VailderChessDesktop.Game;

public struct Position
{
    public Position()
    {
        X = 0;
        Y = 0;
    }
    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
    public int X;
    public int Y;
}