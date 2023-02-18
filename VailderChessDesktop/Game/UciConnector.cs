using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using Tmds.DBus;

namespace VailderChessDesktop.Game;

public class UciConnector
{
    //private static Dictionary<string, string>

    private readonly Process _chessEngineProcess = new();

    public delegate void DataReceivedHandler(string message);

    public event DataReceivedHandler? OutputDataReceived;
    
    public event DataReceivedHandler? InputDataReceived;
    
    private string? ChessEngineResponse => _chessEngineResponses.Count > 0 ? _chessEngineResponses.Pop() : null;

    private readonly Stack<string> _chessEngineResponses = new();

    private readonly string[] _chessEngineResults = new string[]
    {
        "uciok", "readyok", "bestmove"
    };
    
    public UciConnector() { }

    public async Task<bool> ConnectTo(string enginePath)
    {
        if (!File.Exists(enginePath))
            throw new Exception("Directory of engine is not exists");
        var result = await RunEngineProcessUci(enginePath);
        Connected = true;
        return result;
    }

    private async Task<bool> RunEngineProcessUci(string enginePath)
    {
        ProcessStartInfo processStartInfo = new ProcessStartInfo() {
            FileName = enginePath,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardError = true,
            RedirectStandardInput = true,
            RedirectStandardOutput = true
        };
        _chessEngineProcess.StartInfo = processStartInfo;
        try {
            _chessEngineProcess.PriorityClass = ProcessPriorityClass.BelowNormal;
        }
        catch { }
        _chessEngineProcess.OutputDataReceived += ChessEnginePrecessOutputDataReceived;
        _chessEngineProcess.Start();
        _chessEngineProcess.BeginErrorReadLine();
        _chessEngineProcess.BeginOutputReadLine();
        EmitCommandLine("uci");
        var task = await GetResultAsync();
        var uciok = task == "uciok";
        return uciok;
    }

    public bool Connected = false; 
        
    public int GetResultAsyncDelayMs { get; set; } = 250;
    
    public int? Depth { get; set; }
    
    public int? Nodes { get; set; }

    public string GoParameter => $"go depth {Depth} nodes {Nodes}";


    // public void EnableDebug() => EmitCommandLine("debug on");
    //
    // public void DisableDebug() => EmitCommandLine("debug off");

    public async Task<string> GoBestMoveAsync()
    {
        Go();
        var moveResult = await GetResultAsync();
        return moveResult.Split()[1];
    }
    
    public async Task<string> GetResultAsync()
    {
        string? responce;
        responce = ChessEngineResponse;
        while (!_chessEngineResults.Contains(responce?.Split()[0]))
        {
            await Task.Delay(GetResultAsyncDelayMs);
            responce = ChessEngineResponse;
        }
        return responce;
    }

    public async Task<bool> IsEngineReady()
    {
        EmitCommandLine("isready");
        var task = await GetResultAsync();
        return task == "readyok";
    }

    public void Stop() => EmitCommandLine("stop");

    public void Quit()
    {
        EmitCommandLine("quit");
        Connected = false;
    }
    
    public void NewGame() => EmitCommandLine("ucinewgame");

    public void SetStartPosition() => EmitCommandLine("position startpos");
    
    public void SetStartPosition(string moves) => EmitCommandLine($"position startpos move {moves}");
    
    public void SetPosition(string fen) => EmitCommandLine($"position fen {fen}");
    
    public void SetPosition(string fen, string moves) => EmitCommandLine($"position fen {fen} moves {moves}");

    public void Go()
    {
        EmitCommandLine(GoParameter);
    }

    private void SetOptionHash(int initial, int minimum, int maximum)
    {
        EmitSpinOption("Hash", initial, minimum, maximum);
    }

    // private void SetOptionNalimovPath(string initial)
    // {
    //     EmitStringOption("NalimovPath", initial);
    // }
    //
    // private void SetOptionNalimovCache(int initial, int minimum, int maximum)
    // {
    //     EmitSpinOption("NalimovCache", initial, minimum, maximum);
    // }
    
    public void EmitCommandLine(string command)
    {
        _chessEngineProcess.StandardInput.WriteLine(command);
        _chessEngineProcess.StandardInput.Flush();
       InputDataReceived?.Invoke(command);
    }

    private void EmitStringOption(string name, string initial)
    {
        EmitCommandLine($"option name {name} type string default {initial}");
    }
    
    private void EmitSpinOption(string name, int initial, int minimum, int maximum)
    {
        EmitCommandLine($"option name {name} type spin default {initial} min {minimum} max {maximum}");
    }
    
    private void ChessEnginePrecessOutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (e.Data == null) return;
        _chessEngineResponses.Push(e.Data);
        //Debug.WriteLine("CHESS ENGINE SAY: " + e.Data);
        Dispatcher.UIThread.InvokeAsync(() => OutputDataReceived?.Invoke(e.Data));

    }
    
}