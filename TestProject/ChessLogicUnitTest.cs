using VailderChessDesktop.Game;
using VailderChessDesktop.UserControls;

namespace TestProject;

public class Tests
{

    [Test]
     public void GetIsWhiteFigure()
     {
         Assert.IsFalse(ChessLogic.GetIsWhiteFigure(FigureType.BishopBlack));
         Assert.IsTrue(ChessLogic.GetIsWhiteFigure(FigureType.BishopWhite));
         Assert.IsFalse(ChessLogic.GetIsWhiteFigure(FigureType.QueenBlack));
         Assert.IsTrue(ChessLogic.GetIsWhiteFigure(FigureType.RookWhite));
         Assert.IsTrue(ChessLogic.GetIsWhiteFigure(FigureType.QueenWhite));
         Assert.IsFalse(ChessLogic.GetIsWhiteFigure(FigureType.PawnBlack));
         Assert.IsFalse(ChessLogic.GetIsWhiteFigure(FigureType.KingBlack));
         Assert.IsTrue(ChessLogic.GetIsWhiteFigure(FigureType.KnightWhite));
     }

     [Test]
     public void TraceHorVer()
     {
         int xArg = 2, yArg = 2;
         FigureType[,] virtualMap = new FigureType[8, 8];
         for (var y = 0; y < 8; y++)
            for (var x = 0; x < 8; x++) 
                virtualMap[y, x] = FigureType.Empty;
         var traceResult = new PlaceToMoveType[8, 8];
         var traceResultReal = new PlaceToMoveType[8, 8];
         for (var y = 0; y < 8; y++)
            for (var x = 0; x < 8; x++)
                if ((x == xArg || y == yArg) && x != y)
                        traceResultReal[y, x] = PlaceToMoveType.Empty;
                else    traceResultReal[y, x] = PlaceToMoveType.None;
         Position position = new Position(2, 2);
         ChessLogic.TraceHorVer(virtualMap, traceResult, position);
         Assert.That(traceResultReal, Is.EqualTo(traceResult));
     }
    
}