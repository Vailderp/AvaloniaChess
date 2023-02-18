using System.Text.RegularExpressions;
using VailderChessDesktop.Game;

namespace TestProject;

public class UciConnectorTest
{
    [Test]
    public void UciConnectorConnectToEngineTest()
    {
        var chessEnginePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) +
                              @"\VailderChess\ChessEngines\stockfish_15_x64_avx2.exe";
        var uciConnector = new UciConnector();
        uciConnector.ConnectTo(chessEnginePath)
            .ContinueWith(task =>
            {
                Assert.That(task.Result, Is.True);
                Assert.That(uciConnector.Connected, Is.True);
            });
    }
    
    [Test]
    public void UciConnectorGoBestMoveTest()
    {
        var chessEnginePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) +
                              @"\VailderChess\ChessEngines\stockfish_15_x64_avx2.exe";
        var uciConnector = new UciConnector();
        uciConnector.ConnectTo(chessEnginePath)
            .ContinueWith(task => Assert.That(task.Result, Is.True));
        uciConnector.SetStartPosition();
        uciConnector
            .GoBestMoveAsync()
            .ContinueWith(task => Assert.That(Regex.IsMatch(
                task.Result, "[0-9]{1}[a-h]{1}[0-9]{1}[a-h]{1}"), Is.True));
    }
    
    [Test]
    public void UciConnectorSetDepthAndNodesTest()
    {
        var chessEnginePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) +
                              @"\VailderChess\ChessEngines\stockfish_15_x64_avx2.exe";
        var uciConnector = new UciConnector();
        uciConnector.ConnectTo(chessEnginePath)
            .ContinueWith(task => Assert.That(task.Result, Is.True));
        const int depth = 15;
        const int nodes = 1500000;
        uciConnector.Depth = depth;
        uciConnector.Nodes = nodes;
        Assert.That(uciConnector.GoParameter, Is.EqualTo($"go depth {depth} nodes {nodes}"));
    }
    
    [Test]
    public void UciConnectorQuitTest()
    {
        var chessEnginePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) +
                              @"\VailderChess\ChessEngines\stockfish_15_x64_avx2.exe";
        var uciConnector = new UciConnector();
        uciConnector.ConnectTo(chessEnginePath)
            .ContinueWith(task => Assert.That(task.Result, Is.True));
        uciConnector.Quit();
        Assert.That(uciConnector.Connected, Is.False);
    }
    
}