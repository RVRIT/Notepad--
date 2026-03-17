using Microsoft.Win32;
using Notepad;
using Notepad__;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

public class MainVM : INotifyPropertyChanged
{
    public ObservableCollection<FileVM> FileDeschise { get; } = new();
    public ObservableCollection<Notepad.ExplorerNodeVM> ExplorerRoots { get; } = new();
    public bool _showTreeView = true;
    public bool ShowTreeView
    {
        get => _showTreeView;
        set { _showTreeView = value; OnPropertyChanged(nameof(ShowTreeView)); }
    }
    private FileVM _fileActiv;
    public FileVM FileActiv
    {
        get => _fileActiv;
        set { _fileActiv = value; OnPropertyChanged(nameof(FileActiv)); }
    }

    public ICommand ToggleTreeViewCommand { get; }
    public ICommand NewFileCommand { get; }
    public ICommand OpenFileCommand { get; }
    public ICommand CloseFileCommand { get; }


    public MainVM()
    {
        NewFileCommand = new RelayCommand(_ => NewFile());
        OpenFileCommand = new RelayCommand(_ => OpenFile());
        CloseFileCommand = new RelayCommand(f => CloseFile(f as FileVM), f => FileDeschise.Count > 1);
        ToggleTreeViewCommand = new RelayCommand(_ => ToggleTreeView());
        LoadDrives();
        NewFile();
    }
    private void LoadDrives()
    {
        ExplorerRoots.Clear();
        foreach (var drive in DriveInfo.GetDrives())
        {
            if (!drive.IsReady) continue;
            var node = new ExplorerNodeVM
            {
                Name = drive.Name,
                FullPath = drive.RootDirectory.FullName,
                IsFolder = true
            };
            node.Children.Add(ExplorerNodeVM.CreateDummy());
            ExplorerRoots.Add(node);
        }
    }
    private void ToggleTreeView() => ShowTreeView = !ShowTreeView;
    private void NewFile()
    {
        var f = new FileVM { Name = "File " + (FileDeschise.Count()+1).ToString() };
        FileDeschise.Add(f);
        FileActiv = f;
    }

    private void OpenFile()
    {
        var dialog = new OpenFileDialog
        {
            Filter = "Fisiere text (*.txt)|*.txt|Toate (*.*)|*.*"
        };
        if (dialog.ShowDialog() != true) return;

        var existing = FileDeschise.FirstOrDefault(f => f.Path == dialog.FileName);
        if (existing != null) { FileActiv = existing; return; }

        var f = new FileVM();
        try
        {
            f.Path = dialog.FileName;
            f.Name = System.IO.Path.GetFileName(f.Path);
            f.Content = File.ReadAllText(f.Path);
            f.IsEdited = false;
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Error"); return; }

        FileDeschise.Add(f);
        FileActiv = f;
    }

    private void CloseFile(FileVM f)
    {
        if (f == null) return;
        if (f.IsEdited)
        {
            var r = MessageBox.Show("You have unsaved changes. Are you sure you want to continue?",
                                    f.Name, MessageBoxButton.YesNo);
            if (r != MessageBoxResult.Yes) return;
        }

        int idx = FileDeschise.IndexOf(f);
        FileDeschise.Remove(f);
        FileActiv = FileDeschise[Math.Max(0, idx - 1)];
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string p) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
}