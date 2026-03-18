using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Notepad__;

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
    }
}