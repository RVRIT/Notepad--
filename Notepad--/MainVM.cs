using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Notepad__
{
    public class MainVM : INotifyPropertyChanged
    {
        public ObservableCollection<FileVM> FileDeschise { get; } = new ObservableCollection<FileVM>();

        private FileVM _fileActiv;
        public FileVM FileActiv
        {
            get => _fileActiv;
            set { _fileActiv = value; OnPropertyChanged(nameof(FileActiv)); }
        }

        public MainVM()
        {
            var fisierInitial = new FileVM { Name = "Nou" };
            FileDeschise.Add(fisierInitial);
            FileActiv = fisierInitial;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string nume) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nume));
    }
}