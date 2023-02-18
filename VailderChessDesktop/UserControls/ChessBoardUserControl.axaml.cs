using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using VailderChessDesktop.Game;

namespace VailderChessDesktop.UserControls;

public partial class ChessBoardUserControl : UserControl
{
    
    private readonly Grid _chessBoardGrid;
    
    private ChessFigure? _selectedChessFigure;

    private readonly ChessLogic _chessLogic;

    private PlayerColor _playerColor = PlayerColor.White;

    public delegate void OnPlayerMoved(string move);
    public event OnPlayerMoved PlayerMoved;

    public bool CanPlayerMove = true;


    private string _moves = "";
    
    public ChessBoardUserControl()
    {
        InitializeComponent();
        _chessBoardGrid = this.FindControl<Grid>("ChessBoardGrid");
        _chessLogic = new ChessLogic(this);
    }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    

    public PlayerColor PlayerColor
    {
        get => _playerColor;
        set
        {
            _playerColor = value;
            SetCoords();
            SetStartPosition();
            //NextMove("e2e4");
            //NextMove("h7h5");
        }
    }

    private Control GetChessFigureControlByProperties(FigureType figureType, Position position)
    {
        var chessFigure =  new ChessFigure
        {
            FigureType = figureType,
            Name = $"ChessFigure{position.X}{position.Y}"
        };
        chessFigure.SelectFigure += ChessFigureOnSelectFigure;
        chessFigure.UnselectFigure += ChessFigureOnUnselectFigure;
        Grid.SetColumn(chessFigure, position.X);
        Grid.SetRow(chessFigure, position.Y);
        return chessFigure;
    }

    public bool Rokerated = false;

    private void ChessFigureOnSelectFigure(object? sender, RoutedEventArgs e)
    {
        if (sender is not ChessFigure chessFigure) return;
        var pos = new Position(Grid.GetColumn(chessFigure), Grid.GetRow(chessFigure));
        var traceResult =
            _chessLogic.TraceFigure(chessFigure.FigureType, pos, true, true, true, PlayerColor);
        _selectedChessFigure = chessFigure;
        for (var y = 0; y < 8; y++)
        {
            for (var x = 0; x < 8; x++)
            {
                if (traceResult[y, x] != PlaceToMoveType.None)
                    SetChessPlaceToMove(new Position(x, y), traceResult[y, x]);
            }
        }
    }

    private void ChessFigureOnUnselectFigure(object? sender, RoutedEventArgs e)
    {
        ClearChessPlacesToMove();
    }
    
    private void PlaceToMoveControlOnGotFocus(object? sender, GotFocusEventArgs e)
    {
        if (!CanPlayerMove) return;
        if (_selectedChessFigure == null) return;
        if (sender is not PlaceToMove placeToMove) return;
        var toPos = new Position(Grid.GetColumn(placeToMove), Grid.GetRow(placeToMove));
        var fromPos = new Position(Grid.GetColumn(_selectedChessFigure), Grid.GetRow(_selectedChessFigure));
        _chessBoardGrid.Children.Remove(placeToMove);

        string nextMove;
        if (PlayerColor == PlayerColor.White)
        {
            nextMove = $"{(char)('a' + fromPos.X)}{7 - fromPos.Y + 1}{(char)('a' + toPos.X)}{7 - toPos.Y + 1}";
        }
        else
        {
            nextMove = $"{(char)('a' + (7 - fromPos.X))}{fromPos.Y + 1}{(char)('a' + (7 - toPos.X))}{toPos.Y + 1}";
        }

        NextMove(nextMove);
        OnPlayerMovedInvoke(nextMove);
    }

    private void SetChessFigure(FigureType figureType, Position position)
    {
        var chessFigureControl = GetChessFigureControlByProperties(figureType, position);
        _chessBoardGrid.Children.Add(chessFigureControl);
    }

    private void SetChessPlaceToMove(Position position, PlaceToMoveType placeToMoveType)
    {
        var chessFigureControl = new PlaceToMove
        {
            PlaceToMoveType = placeToMoveType
        };
        chessFigureControl.GotFocus += PlaceToMoveControlOnGotFocus;
        Grid.SetColumn(chessFigureControl, position.X);
        Grid.SetRow(chessFigureControl, position.Y);
        _chessBoardGrid.Children.Add(chessFigureControl);
    }
    
    private void SetCoords()
    {
        for (var y = 0; y < 8; y++)
        {
            var coord = PlayerColor == PlayerColor.White ? 7 - y + 1  : y + 1;
            var coordControl = new TextBlock()
            {
                Text = $"{coord}",
                Foreground = Brushes.PaleVioletRed,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = Thickness.Parse("2, 2, 0, 0")
            };
            Grid.SetColumn(coordControl, 0);
            Grid.SetRow(coordControl, y);
            _chessBoardGrid.Children.Add(coordControl);
        }
        for (var x = 0; x < 8; x++)
        {
            char coord = (char)(PlayerColor == PlayerColor.White ? 'a' + x : 'a' + (7 - x));
            var coordControl = new TextBlock()
            {
                Text = $"{coord}",
                Foreground = Brushes.PaleVioletRed,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = Thickness.Parse("0, 0, 2, 2")
            };
            Grid.SetColumn(coordControl, x);
            Grid.SetRow(coordControl, 7);
            _chessBoardGrid.Children.Add(coordControl);
        }
    }

    private void ClearChessPlacesToMove()
    {
        _chessBoardGrid.Children
            .RemoveAll(_chessBoardGrid.Children
                .Where(control => control is PlaceToMove));
    }

    private Control? GetControlAt(Position position)
    {

        return (Control?)_chessBoardGrid.Children.FirstOrDefault(
            x => Grid.GetColumn(x as Control) == position.X &&
                 Grid.GetRow(x as Control) == position.Y
        );
    }
    
    private List<Control?> GetControlsAt(Position position)
    {

        return _chessBoardGrid.Children
            .Where(
                x => Grid.GetColumn(x as Control) == position.X &&
                     Grid.GetRow(x as Control) == position.Y
            )
            .Select(x => x as Control)
            .ToList();
    }
    
    private ChessFigure? GetChessControlAt(Position position)
    {

        return (ChessFigure?)_chessBoardGrid.Children.FirstOrDefault(
            x => Grid.GetColumn(x as Control) == position.X &&
                 Grid.GetRow(x as Control) == position.Y && 
                 x is ChessFigure
        );
    }
    
    private List<ChessFigure?> GetChessControlsAt(Position position)
    {

        return _chessBoardGrid.Children
            .Where(
                x => Grid.GetColumn(x as Control) == position.X &&
                     Grid.GetRow(x as Control) == position.Y && 
                     x is ChessFigure
            )
            .Select(x => x as ChessFigure)
            .ToList();
    }

    private void RemoveControlAt(Position position)
    {
        var control = GetControlAt(position);
        if (control == null) return;
        if (_chessBoardGrid.Children.Contains(control)) 
            _chessBoardGrid.Children.Remove(control);
    }
    
    private void RemoveControl(Control? control)
    {
        if (control == null) return;
        if (_chessBoardGrid.Children.Contains(control)) 
            _chessBoardGrid.Children.Remove(control);
    }
    private void RemoveControls(IEnumerable<Control?> controls)
    {
        controls.ToList().ForEach(control =>
        {
            if (control == null) return;
            if (_chessBoardGrid.Children.Contains(control)) 
                _chessBoardGrid.Children.Remove(control);
        });
    }
    
    private void SetControlAt(Control? control, Position position)
    {
        if (control == null) return;
        Grid.SetColumn(control, 7 - position.X);
        Grid.SetRow(control, position.Y);
        if (!_chessBoardGrid.Children.Contains(control))
            _chessBoardGrid.Children.Add(control);
    }

    private void SwapControls(Position fromPos, Position toPos, char? type = null)
    {
        var chessFigure = GetChessControlAt(fromPos);
        if (chessFigure == null) return;
        FigureType? newFigureType = null;
        if (type != null)
        {
            var isWhiteFigure = ChessLogic.GetIsWhiteFigure(chessFigure.FigureType);
            newFigureType = type switch
            {
                'q' => isWhiteFigure ? FigureType.QueenWhite : FigureType.QueenBlack,
                'n' => isWhiteFigure ? FigureType.KnightWhite : FigureType.KnightBlack,
                'r' => isWhiteFigure ? FigureType.RookWhite : FigureType.RookBlack,
                'b' => isWhiteFigure ? FigureType.BishopWhite : FigureType.BishopBlack,
                _ => newFigureType
            };
        }
        var newControl = GetChessFigureControlByProperties(newFigureType ?? chessFigure.FigureType, toPos);
        RemoveControl(chessFigure);
        RemoveControls(GetChessControlsAt(new Position(7 - toPos.X, toPos.Y)));
        SetControlAt(newControl, toPos);
    }

    private void SetChessFigureFen(string fen)
    {
        var size = fen.Length;
        var iter = 0;
        int index = 0;
        for (; iter < size && fen[iter] != ' '; iter++)
        {
            if (iter >= size) return;
            var figure = fen[iter];
            
            if (fen[iter] == '/')
                continue;

            if (int.TryParse(figure.ToString(), out var spaceCount))
                index += spaceCount; // converts char digit to int. `5` to 5

            else
            {
                var p = GetPosition(new Position(index % 8, index / 8));
                switch (figure)
                {
                    case 'k': SetChessFigure(FigureType.KingBlack, p); break;
                    case 'K': SetChessFigure(FigureType.KingWhite, p); break;
                    case 'q': SetChessFigure(FigureType.QueenBlack, p); break;
                    case 'Q': SetChessFigure(FigureType.QueenWhite, p); break;
                    case 'b': SetChessFigure(FigureType.BishopBlack, p); break;
                    case 'B': SetChessFigure(FigureType.BishopWhite, p); break;
                    case 'n': SetChessFigure(FigureType.KnightBlack, p); break;
                    case 'N': SetChessFigure(FigureType.KnightWhite, p); break;
                    case 'p': SetChessFigure(FigureType.PawnBlack, p); break;
                    case 'P': SetChessFigure(FigureType.PawnWhite, p); break;
                    case 'r': SetChessFigure(FigureType.RookBlack, p); break;
                    case 'R': SetChessFigure(FigureType.RookWhite, p); break;
                }
                ++index;
            }
        }
    }

    private Position GetPosition(Position position)
    {
        if (PlayerColor == PlayerColor.White)
            return new Position(position.X, position.Y);
        return new Position(7 - position.X, 7 - position.Y);
    }

    public FigureType[, ] GetVirtualMap()
    {
        var virtualMap = new FigureType[8, 8];
        
        for (var i = 0; i < 8; i++)
            for (var l = 0; l < 8; l++)
                virtualMap[i, l] = FigureType.Empty;
        
        foreach (var control in _chessBoardGrid.Children.Select(control => control as Control))
        {
            if (control is not ChessFigure chessFigure) continue;
            var x = Grid.GetColumn(chessFigure);
            var y = Grid.GetRow(chessFigure);
            virtualMap[y, x] = chessFigure.FigureType;
        }
        
        return virtualMap;
    }

    public void SetStartPosition()
    {
        SetChessFigureFen("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR");
    }

    public void SetStartPosition(string moves)
    {
        var fen = moves.Split()[0];
        var movesSplit = moves.Split("moves ");
        if (movesSplit.Length == 0) return;
        movesSplit.ToList().ForEach(NextMove);
    }

    public void SetPosition(string fen)
    {
        SetChessFigureFen(fen);
    }

    public void SetPosition(string fen, string moves)
    {
        SetChessFigureFen(fen);
        var movesSplit = moves.Split();
        movesSplit.ToList().ForEach(NextMove);
    }

    public string GetPositionFen()
    {
        return _moves;
    }

    public string GetMoves()
    {
        return _moves;
    }

    public void NextMove(string nextMove)
    {
        Debug.WriteLine("MOVE: " + nextMove);
        
        var fromCoord = nextMove.Substring(0, 2);
        var toCoord = nextMove.Substring(2, 2);
        var fromX = fromCoord[0] - 'a';
        var toX = toCoord[0] - 'a';
        if (!int.TryParse(fromCoord[1].ToString(), out var fromY)) return;
        if (!int.TryParse(toCoord[1].ToString(), out var toY)) return;
        if (fromX > 7 || fromX < 0) return;
        if (toX > 7 || toX < 0) return;
        fromY -= 1;
        toY -= 1;

        char? newFigureTypeChar = null;
        if (nextMove.Length == 5)
        {
            newFigureTypeChar = nextMove[4];
        }
        
        if (PlayerColor == PlayerColor.White)
            SwapControls(new Position(fromX, 7 - fromY), new Position(7 - toX, 7 - toY), newFigureTypeChar);
        else
            SwapControls(new Position(7 - fromX, fromY), new Position(toX, toY), newFigureTypeChar);
        _moves += nextMove + " ";
        Debug.WriteLine("MOVES: " + _moves);
    }

    public void NextMove()
    {
        throw new System.NotImplementedException();
    }

    public void BackMove()
    {
        throw new System.NotImplementedException();
    }

    public void GetMovesCount()
    {
        throw new System.NotImplementedException();
    }

    public void OnPlayerMove()
    {
        throw new System.NotImplementedException();
    }
    
    protected virtual void OnPlayerMovedInvoke(string move)
    {
        PlayerMoved?.Invoke(move);
    }
}