    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;
using Microsoft.Win32;

    namespace Notepad__
    {
        public class FileVM : INotifyPropertyChanged
        {
        private string name;
        private string path;

        private string content;

        private bool isDirectory;
            public string Name
            {
                get => name;
                set { name = value; OnPropertyChanged(nameof(Name)); }
            }

            public string Path
            {
                get => path;
                set { path = value; OnPropertyChanged(nameof(Path)); }
            }

            public string Content
            {
                get => content;
                set { content = value; OnPropertyChanged(nameof(Content)); }
            }

            public bool IsDirectory
            {
                get => isDirectory;
                set { isDirectory = value; OnPropertyChanged(nameof(IsDirectory)); }
            }
        public ICommand NewFileCommand { get; }
        public ICommand OpenFileCommand { get; }
        public ICommand SaveFileCommand { get; }
        public FileVM()
        {
            NewFileCommand = new RelayCommand(_ => NewFile());
            OpenFileCommand = new RelayCommand(_ => OpenFile());
            SaveFileCommand = new RelayCommand(_ => SaveFile(), _ => !string.IsNullOrEmpty(Content));
        }
        public FileVM(string name, string path, bool isDirectory)
        {
            Name = name;
            Path = path;
            IsDirectory = isDirectory;

            NewFileCommand = new RelayCommand(_ => NewFile());
            OpenFileCommand = new RelayCommand(_ => OpenFile());
            SaveFileCommand = new RelayCommand(_ => SaveFile(), _ => !string.IsNullOrEmpty(Content));

            if (!isDirectory && File.Exists(path))
                Content = File.ReadAllText(path);
        }
        private void NewFile()
        {
            Name = "";
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
                var dialog = new SaveFileDialog
                {
                    Filter = "Fisiere text (*.txt)|*.txt"
                };

                if (dialog.ShowDialog() == true)
                    Path = dialog.FileName;
                else
                    return;
            }

            File.WriteAllText(Path, Content);
        }
        public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string nume) =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nume));
        }
    }
