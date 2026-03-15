using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using Microsoft.Win32;

namespace Notepad__
{
    public class FileVM : INotifyPropertyChanged
    {
        private string _name;
        private string _path;
        private string _content;

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        public string Path
        {
            get => _path;
            set { _path = value; OnPropertyChanged(nameof(Path)); }
        }

        public string Content
        {
            get => _content;
            set { _content = value; OnPropertyChanged(nameof(Content)); }
        }

        public ICommand NewFileCommand { get; }
        public ICommand OpenFileCommand { get; }
        public ICommand SaveFileCommand { get; }
        public ICommand SaveAsFileCommand { get; }

        public FileVM()
        {
            NewFileCommand = new RelayCommand(_ => NewFile());
            OpenFileCommand = new RelayCommand(_ => OpenFile());
            SaveFileCommand = new RelayCommand(_ => SaveFile(), _ => !string.IsNullOrEmpty(Content));
            SaveAsFileCommand = new RelayCommand(_ => SaveAsFile(), _ => !string.IsNullOrEmpty(Content));
        }

        private void NewFile()
        {
            Name = "Nou";
            Path = "";
            Content = "";
        }

        private void OpenFile()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Fisiere text (*.txt)|*.txt|Toate (*.*)|*.*"
            };

            if (dialog.ShowDialog() == true)
            {
                Path = dialog.FileName;
                Name = System.IO.Path.GetFileName(Path);
                Content = File.ReadAllText(Path);
            }
        }

        private void SaveFile()
        {
            if (string.IsNullOrEmpty(Path))
            {
                SaveAsFile();
                return;
            }
            File.WriteAllText(Path, Content);
        }

        private void SaveAsFile()
        {
            var dialog = new SaveFileDialog
            {
                Filter = "Fisiere text (*.txt)|*.txt",
                FileName = Name
            };

            if (dialog.ShowDialog() == true)
            {
                Path = dialog.FileName;
                Name = System.IO.Path.GetFileName(Path);
                File.WriteAllText(Path, Content);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string nume) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nume));
    }
}