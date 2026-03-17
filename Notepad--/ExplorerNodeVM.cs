using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
namespace Notepad
{


    public class ExplorerNodeVM : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string FullPath { get; set; }
        public bool IsFolder { get; set; }
        public ObservableCollection<ExplorerNodeVM> Children { get; } = new();

        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (_isExpanded == value) return;
                _isExpanded = value;
                OnPropertyChanged(nameof(IsExpanded));
                if (_isExpanded && Children.Count == 1 && Children[0].IsDummy)
                    LoadChildren();
            }
        }

        public bool IsDummy { get; private set; }

        public static ExplorerNodeVM CreateDummy() => new() { IsDummy = true };

        public static ExplorerNodeVM FromPath(string path)
        {
            var node = new ExplorerNodeVM
            {
                FullPath = path,
                Name = System.IO.Path.GetFileName(path),
                IsFolder = Directory.Exists(path)
            };
            if (node.IsFolder) node.Children.Add(CreateDummy());
            return node;
        }
        private void LoadChildren()
        {
            Children.Clear();
            try
            {
                foreach (var dir in Directory.GetDirectories(FullPath))
                {
                    var attrs = File.GetAttributes(dir);
                    if (attrs.HasFlag(FileAttributes.Hidden) ||
                        attrs.HasFlag(FileAttributes.System)) continue;
                    Children.Add(FromPath(dir));
                }
                foreach (var file in Directory.GetFiles(FullPath))
                {
                    var attrs = File.GetAttributes(file);
                    if (attrs.HasFlag(FileAttributes.Hidden) ||
                        attrs.HasFlag(FileAttributes.System)) continue;
                    Children.Add(FromPath(file));
                }
            }
            catch { }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string p) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
    }
}