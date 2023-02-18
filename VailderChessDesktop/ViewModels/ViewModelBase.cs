﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using ReactiveUI;

namespace VailderChessDesktop.ViewModels
{
    public class ViewModelBase : ReactiveObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}