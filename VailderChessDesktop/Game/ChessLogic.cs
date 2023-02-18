using System;
using VailderChessDesktop.UserControls;

namespace VailderChessDesktop.Game;

public class ChessLogic
{
    public ChessLogic() { }

    public ChessLogic(ChessBoardUserControl chessBoard)
    {
        ChessBoard = chessBoard;
    }
    
    public ChessBoardUserControl? ChessBoard;
    
    public PlaceToMoveType[, ] TraceFigure(
        FigureType figureType, Position pos,
        bool king, bool leftRook, bool rightRook,
        PlayerColor playerColor)
    {
        var traceResult = new PlaceToMoveType[8, 8];

        for (var i = 0; i < 8; i++)
        {
            for (var l = 0; l < 8; l++)
            {
                traceResult[i, l] = PlaceToMoveType.None;
            }
        }
        
        if (ChessBoard == null) return traceResult;
        
        var virtualMap = ChessBoard.GetVirtualMap();

        switch (figureType)
        {
            case FigureType.BishopBlack or FigureType.BishopWhite:
                TraceDiagonal(virtualMap, traceResult, pos);
                break;
            case FigureType.RookBlack or FigureType.RookWhite:
                TraceHorVer(virtualMap, traceResult, pos);
                break;
            case FigureType.QueenBlack or FigureType.QueenWhite:
                TraceDiagonal(virtualMap, traceResult, pos);
                TraceHorVer(virtualMap, traceResult, pos);
                break;
            case FigureType.KnightBlack or FigureType.KnightWhite:
                TraceKnight(virtualMap, traceResult, pos);
                break;
            case FigureType.PawnBlack or FigureType.PawnWhite:
                TracePawn(virtualMap, traceResult, pos, playerColor);
                break;
        }

        return traceResult;
    }

    public static bool GetIsWhiteFigure(FigureType figureType)
    {
        switch (figureType)
        {
            case FigureType.RookWhite:
            case FigureType.KnightWhite:
            case FigureType.BishopWhite:
            case FigureType.PawnWhite:
            case FigureType.QueenWhite:
            case FigureType.KingWhite:
                return true;
            case FigureType.RookBlack:
            case FigureType.KnightBlack:
            case FigureType.BishopBlack:
            case FigureType.PawnBlack:
            case FigureType.QueenBlack:
            case FigureType.KingBlack:
            case FigureType.Empty:
            default:
                return false;
        }
    }

    public static void TraceDiagonal(
        FigureType[, ] virtualMap, PlaceToMoveType[, ] traceResult, 
        Position pos, bool once = false)
    {
        if (once)
        {
            var p = pos;
            if (p.X++ < 7 && p.Y++ < 7 && virtualMap[p.Y, p.X] == FigureType.Empty)
            {
                traceResult[p.Y, p.X] = PlaceToMoveType.Empty;
            }

            p = pos;
            if (p.X++ < 7 && p.Y-- > 0 && virtualMap[p.Y, p.X] == FigureType.Empty)
            {
                traceResult[p.Y, p.X] = PlaceToMoveType.Empty;
            }
            
            p = pos;
            if (p.X-- > 0 && p.Y++ < 7 && virtualMap[p.Y, p.X] == FigureType.Empty)
            {
                traceResult[p.Y, p.X] = PlaceToMoveType.Empty;
            }
            
            p = pos;
            if (p.X-- > 0 && p.Y-- > 0 && virtualMap[p.Y, p.X] == FigureType.Empty)
            {
                traceResult[p.Y, p.X] = PlaceToMoveType.Empty;
            }
        }
        else
        {
            var p = pos;
            while (p.X++ < 7 && p.Y++ < 7 && virtualMap[p.Y, p.X] == FigureType.Empty)
            {
                traceResult[p.Y, p.X] = PlaceToMoveType.Empty;
            }

            p = pos;
            while (p.X++ < 7 && p.Y-- > 0 && virtualMap[p.Y, p.X] == FigureType.Empty)
            {
                traceResult[p.Y, p.X] = PlaceToMoveType.Empty;
            }

            p = pos;
            while (p.X-- > 0 && p.Y++ < 7 && virtualMap[p.Y, p.X] == FigureType.Empty)
            {
                traceResult[p.Y, p.X] = PlaceToMoveType.Empty;
            }

            p = pos;
            while (p.X-- > 0 && p.Y-- > 0 && virtualMap[p.Y, p.X] == FigureType.Empty)
            {
                traceResult[p.Y, p.X] = PlaceToMoveType.Empty;
            }
        }
    }
    
    public static void TraceHorVer(
        FigureType[, ] virtualMap, PlaceToMoveType[, ] traceResult, 
        Position pos, bool once = false)
    {
        if (once)
        {
            var p = pos;
            if (p.Y++ < 7 && virtualMap[p.Y, p.X] == FigureType.Empty)
                traceResult[p.Y, p.X] = PlaceToMoveType.Empty;
            p = pos;
            if (p.X++ < 7 && virtualMap[p.Y, p.X] == FigureType.Empty)
                traceResult[p.Y, p.X] = PlaceToMoveType.Empty;
            p = pos;
            if (p.X-- > 0 && virtualMap[p.Y, p.X] == FigureType.Empty)
                traceResult[p.Y, p.X] = PlaceToMoveType.Empty;
            p = pos;
            if (p.Y-- > 0 && virtualMap[p.Y, p.X] == FigureType.Empty)
                traceResult[p.Y, p.X] = PlaceToMoveType.Empty;
        }
        else
        {
            var p = pos;
            while (p.Y++ < 7 && virtualMap[p.Y, p.X] == FigureType.Empty)
                traceResult[p.Y, p.X] = PlaceToMoveType.Empty;
            p = pos;
            while (p.X++ < 7 && virtualMap[p.Y, p.X] == FigureType.Empty)
                traceResult[p.Y, p.X] = PlaceToMoveType.Empty;
            p = pos;
            while (p.X-- > 0 && virtualMap[p.Y, p.X] == FigureType.Empty)
                traceResult[p.Y, p.X] = PlaceToMoveType.Empty;
            p = pos;
            while (p.Y-- > 0 && virtualMap[p.Y, p.X] == FigureType.Empty)
                traceResult[p.Y, p.X] = PlaceToMoveType.Empty;
        }
    }
    
    public static void TraceKnight(
        FigureType[, ] virtualMap, PlaceToMoveType[, ] traceResult, 
        Position pos)
    {
        var p = pos;
        if ((p.X += 2) < 8 && (p.Y += 1) < 8 && virtualMap[p.Y, p.X] == FigureType.Empty)
        {
            traceResult[p.Y, p.X] = PlaceToMoveType.Empty;
        }
        
        p = pos;
        if ((p.X += 1) < 8 && (p.Y += 2) < 8 && virtualMap[p.Y, p.X] == FigureType.Empty)
        {
            traceResult[p.Y, p.X] = PlaceToMoveType.Empty;
        }
        
        p = pos;
        if ((p.X -= 1) >= 0 && (p.Y += 2) < 8 && virtualMap[p.Y, p.X] == FigureType.Empty)
        {
            traceResult[p.Y, p.X] = PlaceToMoveType.Empty;
        }
        
        p = pos;
        if ((p.X -= 2) >= 0 && (p.Y += 1) < 8 && virtualMap[p.Y, p.X] == FigureType.Empty)
        {
            traceResult[p.Y, p.X] = PlaceToMoveType.Empty;
        }
        
        p = pos;
        if ((p.X += 2) < 8 && (p.Y -= 1) >= 0 && virtualMap[p.Y, p.X] == FigureType.Empty)
        {
            traceResult[p.Y, p.X] = PlaceToMoveType.Empty;
        }
        
        p = pos;
        if ((p.X += 1) < 8 && (p.Y -= 2) >= 0 && virtualMap[p.Y, p.X] == FigureType.Empty)
        {
            traceResult[p.Y, p.X] = PlaceToMoveType.Empty;
        }
        
        p = pos;
        if ((p.X -= 1) >= 0 && (p.Y -= 2) >= 0 && virtualMap[p.Y, p.X] == FigureType.Empty)
        {
            traceResult[p.Y, p.X] = PlaceToMoveType.Empty;
        }
                
        p = pos;
        if ((p.X -= 2) >= 0 && (p.Y -= 1) >= 0 && virtualMap[p.Y, p.X] == FigureType.Empty)
        {
            traceResult[p.Y, p.X] = PlaceToMoveType.Empty;
        }
    }
    
    public static void TracePawn(
        FigureType[, ] virtualMap, PlaceToMoveType[, ] traceResult, 
        Position pos, PlayerColor playerColor)
    {
        var p = pos;
        var figureType = virtualMap[p.Y, p.X];
        var isWhitePawn = GetIsWhiteFigure(figureType);
        if (figureType == (playerColor == PlayerColor.Black ? FigureType.PawnBlack : FigureType.PawnWhite))
        {
            if ((p.Y -= 1) < 8 && virtualMap[p.Y, p.X] == FigureType.Empty)
            {
                traceResult[p.Y, p.X] = PlaceToMoveType.Empty;
                p = pos;
                if (p.Y == 6 && virtualMap[p.Y - 2, p.X] == FigureType.Empty)
                {
                    traceResult[p.Y - 2, p.X] = PlaceToMoveType.Empty;
                }
            }
            
            p = pos;
            if ((p.X -= 1) >= 0 && (p.Y -= 1) >= 0 && virtualMap[p.Y, p.X] != FigureType.Empty)
            {
                if (isWhitePawn ? !GetIsWhiteFigure(virtualMap[p.Y, p.X]) : GetIsWhiteFigure(virtualMap[p.Y, p.X]))
                    traceResult[p.Y, p.X] = PlaceToMoveType.OnFigure;
                else
                    traceResult[p.Y, p.X] = PlaceToMoveType.OnFriendlyFigure;
            }
            
            p = pos;
            if ((p.X += 1) < 8 && (p.Y -= 1) >= 0 && virtualMap[p.Y, p.X] != FigureType.Empty)
            {
                if (isWhitePawn ? !GetIsWhiteFigure(virtualMap[p.Y, p.X]) : GetIsWhiteFigure(virtualMap[p.Y, p.X]))
                    traceResult[p.Y, p.X] = PlaceToMoveType.OnFigure;
                else
                    traceResult[p.Y, p.X] = PlaceToMoveType.OnFriendlyFigure;
            }
        }
        else
        {
            if ((p.Y += 1) >= 0 && virtualMap[p.Y, p.X] == FigureType.Empty)
            {
                traceResult[p.Y, p.X] = PlaceToMoveType.Empty;
                p = pos;
                if (p.Y == 1 && virtualMap[p.Y + 2, p.X] == FigureType.Empty)
                {
                    traceResult[p.Y + 2, p.X] = PlaceToMoveType.Empty;
                }
            }
            
            p = pos;
            if ((p.X += 1) < 8 && (p.Y += 1) < 8 && virtualMap[p.Y, p.X] != FigureType.Empty)
            {
                if (isWhitePawn ? !GetIsWhiteFigure(virtualMap[p.Y, p.X]) : GetIsWhiteFigure(virtualMap[p.Y, p.X]))
                    traceResult[p.Y, p.X] = PlaceToMoveType.OnFigure;
                else
                    traceResult[p.Y, p.X] = PlaceToMoveType.OnFriendlyFigure;
            }
            
            p = pos;
            if ((p.X -= 1) >= 0 && (p.Y += 1) < 8 && virtualMap[p.Y, p.X] != FigureType.Empty)
            {
                if (isWhitePawn ? !GetIsWhiteFigure(virtualMap[p.Y, p.X]) : GetIsWhiteFigure(virtualMap[p.Y, p.X]))
                    traceResult[p.Y, p.X] = PlaceToMoveType.OnFigure;
                else
                    traceResult[p.Y, p.X] = PlaceToMoveType.OnFriendlyFigure;
            }
        }

    }
    
}