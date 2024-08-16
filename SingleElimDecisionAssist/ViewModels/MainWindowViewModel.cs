using SingleElimDecisionAssist.Enums;
using SingleElimDecisionAssist.Interfaces;
using SingleElimDecisionAssist.Models;
using SingleElimDecisionAssist.View;
using SingleElimDecisionAssist.ViewModels.Utils;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

#nullable enable
namespace SingleElimDecisionAssist.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region properties
        private BitmapImage _leftImage = new BitmapImage();
        public BitmapImage LeftImage
        {
            get => _leftImage;
            private set => SetProperty(ref _leftImage, value);
        }

        private BitmapImage _rightImage = new BitmapImage();
        public BitmapImage RightImage
        {
            get => _rightImage;
            private set => SetProperty(ref _rightImage, value);
        }

        private RelayCommand? _loadCommand;
        public ICommand LoadCommand
        {
            get
            {
                _loadCommand ??= new RelayCommand(e => Load(), e => CanLoad());
                return _loadCommand;
            }
        }

        private RelayCommand? _selectLeftCommand;
        public ICommand SelectLeftCommand
        {
            get
            {
                _selectLeftCommand ??= new RelayCommand(e => SelectLeft(), e => CanSelect());
                return _selectLeftCommand;
            }
        }
        private RelayCommand? _selectRightCommand;
        public ICommand SelectRightCommand
        {
            get
            {
                _selectRightCommand ??= new RelayCommand(e => SelectRight(), e => CanSelect());
                return _selectRightCommand;
            }
        }
        #endregion // properties

        private readonly IElimination<string> elim;
        private bool locked = true;

        public MainWindowViewModel(IElimination<string> _elim)
        {
            elim = _elim;
            elim.WinnerCallback = OnWinner;
            elim.NextCallback = OnNextPair;
            elim.Shuffle = true;
        }

        private void Load()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Select images",
                Multiselect = true,
                Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png"
            };
            if (dialog.ShowDialog() ?? false)
            {

                var files = dialog.FileNames;
                if (files.Length < 2)
                {
                    MessageBox.Show("Select at least two files.", "Error");
                    return;
                }
                //elim = new SingleElim<string>(files, OnNextPair, OnWinner);
                elim.LoadNew(files);
                locked = false;
            }
        }
        private bool CanLoad() { return true; }
        private void SelectLeft() => elim?.Choose(ElimChoice.First);
        private void SelectRight() => elim?.Choose(ElimChoice.Second);
        private bool CanSelect() => !locked;

        private void OnNextPair((string, string) pair)
        {
            LeftImage = new BitmapImage(new Uri(pair.Item1));
            RightImage = new BitmapImage(new Uri(pair.Item2));
        }

        private void OnWinner(string winner)
        {
            locked = true;
            if (LeftImage?.UriSource?.OriginalString == winner)
            {
                RightImage = new BitmapImage();
            }
            else if (RightImage?.UriSource?.OriginalString == winner)
            {
                LeftImage = new BitmapImage();
            }
            new Winner(new BitmapImage(new Uri(winner)), winner).Show();
        }
    }
}
