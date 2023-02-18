using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Windows.Input;
using ReactiveUI;
using Splat;
using VailderChessDesktop.Game;
using VailderChessDesktop.Resources;

namespace VailderChessDesktop.ViewModels;

public class SettingsViewModel : ViewModelBase, IRoutableViewModel
{
    public IScreen HostScreen { get; }

    public string UrlPathSegment => "/settings";
    public SettingsViewModel(IScreen? hostScreen = null)
    {

            if (ResourceManager.LanguageSelectedIndex == 0)
            {
                LanguageItemStrings = new() { "Русский", "Английский" };
                AppDesignItemStrings = new() { "Баклажановый", "Апельсиновый" };
                ChessDesignItemStrings = new() { "Обычный", "Современный" };
                ChessboardDesignItemStrings = new() { "Ежевиковый", "Ананасовый" };
            }
            else if (ResourceManager.LanguageSelectedIndex == 1)
            {
                LanguageItemStrings = new() { "Russian", "English" };
                AppDesignItemStrings = new() { "Eggplant", "Orange fruit" };
                ChessDesignItemStrings = new() { "Obence", "Modern" };
                ChessboardDesignItemStrings = new() { "Blackberry", "Pineapple" };
            }

            _languageSelectedIndex = ResourceManager.LanguageSelectedIndex;
            _appDesignSelectedIndex = ResourceManager.AppDesignSelectedIndex;
            _chessDesignSelectedIndex = ResourceManager.ChessDesignSelectedIndex;
            _chessboardDesignSelectedIndex = ResourceManager.ChessboardDesignSelectedIndex;

        HostScreen = hostScreen ?? Locator.Current.GetService<IScreen>();

        // _saveSettings =
        //     ReactiveCommand.Create(
        //         () =>
        //         {
        //             //LanguageManager.SetLanguageAtShortName(LanguageItemViews.ElementAt(SelectedIndex).ShortName);
                     //LanguageManager.ApplyChanges();
        //         }
        //     );
        
    }
    

    public ObservableCollection<string> LanguageItemStrings { get; private set; }
    public ObservableCollection<string> AppDesignItemStrings { get; private set; }
    public ObservableCollection<string> ChessDesignItemStrings { get; private set; }
    public ObservableCollection<string> ChessboardDesignItemStrings { get; private set; }

    
    private int _languageSelectedIndex = 0;
    private int _appDesignSelectedIndex = 0;
    private int _chessDesignSelectedIndex = 0;
    private int _chessboardDesignSelectedIndex = 0;

    public int AppDesignSelectedIndex
    {
        get => _appDesignSelectedIndex;
        set
        {
            _appDesignSelectedIndex = value;
            if (_appDesignSelectedIndex == 0)
                ResourceManager.AppDesignResourceDictionary = new AppDesignEggplant();
            else if (_appDesignSelectedIndex == 1)
                ResourceManager.AppDesignResourceDictionary = new AppDesignOrangeFruit();
            ResourceManager.AppDesignSelectedIndex = _appDesignSelectedIndex;
        }
    }
    
    public int ChessDesignSelectedIndex
    {
        get => _chessDesignSelectedIndex;
        set
        {
            _chessDesignSelectedIndex = value;
            if (_chessDesignSelectedIndex == 0)
                ResourceManager.AppDesignChessResourceDictionary = new AppStyleChessFiguresStandard();
            else if (_chessDesignSelectedIndex == 1)
                ResourceManager.AppDesignChessResourceDictionary = new AppStyleChessFiguresModern();
            ResourceManager.ChessDesignSelectedIndex = _chessDesignSelectedIndex;
        }
    }
    
    public int ChessboardDesignSelectedIndex
    {
        get => _chessboardDesignSelectedIndex;
        set
        {
            _chessboardDesignSelectedIndex = value;
            if (_chessboardDesignSelectedIndex == 0)
                ResourceManager.AppDesignChessboardResourceDictionary = new AppStyleChessboardBlackberry();
            else if (_chessboardDesignSelectedIndex == 1)
                ResourceManager.AppDesignChessboardResourceDictionary = new AppStyleChessboardPineapple();
            ResourceManager.ChessboardDesignSelectedIndex = _chessboardDesignSelectedIndex;
        }
    }
    
    public int LanguageSelectedIndex
    {
        get => _languageSelectedIndex;
        set
        {
            _languageSelectedIndex = value;
            if (_languageSelectedIndex == 0)
                ResourceManager.AppLanguageResourceDictionary = new AppLanguageRussian();
            else if (_languageSelectedIndex == 1)
                ResourceManager.AppLanguageResourceDictionary = new AppLanguageEnglish();
            if (_languageSelectedIndex != ResourceManager.LanguageSelectedIndex)
            {
                ResourceManager.LanguageSelectedIndex = _languageSelectedIndex;
                ResourceManager.MainWindowViewModel.Router.Navigate.Execute(
                    new SettingsViewModel()
                );
            }
        }
    }
}