using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;

namespace Notepad
{
    public class FileVM : INotifyPropertyChanged
    {
        private string _name;
        private string _path;
        private string _content;
        private bool _isEdited;
        public bool IsEdited
        {
            get => _isEdited;
            set { _isEdited = value; OnPropertyChanged(nameof(IsEdited)); }
        }

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
            set { _content = value; IsEdited = true; OnPropertyChanged(nameof(Content)); }
        }

        public ICommand SaveFileCommand { get; }
        public ICommand SaveAsFileCommand { get; }

        public FileVM()
        {
            SaveFileCommand = new RelayCommand(_ => SaveFile(), _ => !string.IsNullOrEmpty(Content));
            SaveAsFileCommand = new RelayCommand(_ => SaveAsFile(), _ => !string.IsNullOrEmpty(Content));
        }


        private void SaveFile()
        {
            if (string.IsNullOrEmpty(Path))
            {
                SaveAsFile();
                return;
            }
            File.WriteAllText(Path, Content);
            IsEdited = false;
        }

        private void SaveAsFile()
        {
            var dialog = new SaveFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                FileName = Name
            };

            if (dialog.ShowDialog() == true)
            {
                Path = dialog.FileName;
                Name = System.IO.Path.GetFileName(Path);
                File.WriteAllText(Path, Content);
                IsEdited = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string nume) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nume));
    }
}