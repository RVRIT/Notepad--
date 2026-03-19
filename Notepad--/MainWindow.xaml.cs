
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Notepad
{
    public partial class MainWindow : Window
    {
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            var vm = (MainVM)DataContext;

            vm.SaveConfig();

            foreach (var f in vm.FileDeschise.ToList())
            {
                if (!vm.CloseFile(f))
                {
                    e.Cancel = true;
                    return;
                }
            }
            base.OnClosing(e);
        }
        private void ReplaceMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var vm = (MainVM)DataContext;
            var replaceWindow = new ReplaceWindow(vm, GetActiveTextBox);
            replaceWindow.Owner = this;
            replaceWindow.Show();
        }
        private void FindMenuItem_Click(object sender, RoutedEventArgs e) => OpenFind();

        private void OpenFind()
        {
            var vm = (MainVM)DataContext;
            var findWindow = new FindWindow(vm, GetActiveTextBox);
            findWindow.Owner = this;
            findWindow.Show();
        }

        private TextBox GetActiveTextBox()
        {
            MainTabControl.UpdateLayout();
            return FindVisualChild<TextBox>(MainTabControl);
        }

        private static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T t) return t;
                var result = FindVisualChild<T>(child);
                if (result != null) return result;
            }
            return null;
        }
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainVM();
        }
        private ExplorerNodeVM _copiedFolder = null;

        private ExplorerNodeVM GetClickedNode(object sender)
        {
            return (sender as FrameworkElement)?.DataContext as ExplorerNodeVM;
        }

        private void TreeNode_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount != 2) return;
            var node = GetClickedNode(sender);
            if (node == null || node.IsFolder) return;

            var vm = (MainVM)DataContext;

            var existing = vm.FileDeschise.FirstOrDefault(f => f.Path == node.FullPath);
            if (existing != null) { vm.FileActiv = existing; return; }

            var f = new FileVM();
            try
            {
                f.Path = node.FullPath;
                f.Name = node.Name;
                f.Content = File.ReadAllText(f.Path);
                f.IsEdited = false;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error"); return; }

            vm.FileDeschise.Add(f);
            vm.FileActiv = f;
            e.Handled = true;
        }

        private void TreeNewFile_Click(object sender, RoutedEventArgs e)
        {
            var node = GetClickedNode(sender);
            if (node == null) return;

            var name = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter file name:", "New File", "newfile.txt");
            if (string.IsNullOrWhiteSpace(name)) return;

            var fullPath = Path.Combine(node.FullPath, name);
            try
            {
                File.WriteAllText(fullPath, string.Empty);
                var vm = (MainVM)DataContext;
                var f = new FileVM { Path = fullPath, Name = name, Content = string.Empty };
                vm.FileDeschise.Add(f);
                vm.FileActiv = f;
                node.Refresh();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error"); }
        }

        private void TreeCopyPath_Click(object sender, RoutedEventArgs e)
        {
            var node = GetClickedNode(sender);
            if (node == null) return;
            Clipboard.SetText(node.FullPath);
        }

        private void TreeCopyFolder_Click(object sender, RoutedEventArgs e)
        {
            var node = GetClickedNode(sender);
            if (node == null || !node.IsFolder) return;
            var vm = (MainVM)DataContext;
            vm.CopiedFolder = node;
            MessageBox.Show($"Copied: {node.Name}", "Copy Folder");
        }

        private void TreePasteFolder_Click(object sender, RoutedEventArgs e)
        {
            var node = GetClickedNode(sender);
            var vm = (MainVM)DataContext;
            if (node == null || !node.IsFolder || vm.CopiedFolder == null) return;

            var dest = System.IO.Path.Combine(node.FullPath, vm.CopiedFolder.Name);
            try
            {
                CopyDirectory(vm.CopiedFolder.FullPath, dest);
                node.Refresh();
                MessageBox.Show($"Pasted into: {node.Name}", "Paste Folder");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error"); }
        }
        private void CopyDirectory(string source, string dest)
        {
            Directory.CreateDirectory(dest);

            foreach (var file in Directory.GetFiles(source))
                File.Copy(file, System.IO.Path.Combine(dest, System.IO.Path.GetFileName(file)), overwrite: true);

            foreach (var dir in Directory.GetDirectories(source))
            {
                string destSubDir = System.IO.Path.Combine(dest, System.IO.Path.GetFileName(dir));
                if (string.Equals(dir, dest, StringComparison.OrdinalIgnoreCase)) continue;

                CopyDirectory(dir, destSubDir);
            }
        }
        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var about = new AboutWindow();
            about.Owner = this;
            about.ShowDialog();
        }
    }
}