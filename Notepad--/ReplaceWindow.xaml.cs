using System;
using System.Windows;
using System.Windows.Controls;

namespace Notepad
{
    public partial class ReplaceWindow : Window
    {
        private readonly MainVM _vm;
        private readonly Func<TextBox> _getTextBox;
        private int _lastIndex = -1;
        private string _lastTerm = string.Empty;

        public ReplaceWindow(MainVM vm, Func<TextBox> getTextBox)
        {
            InitializeComponent();
            _vm = vm;
            _getTextBox = getTextBox;
        }

        private void Replace_Click(object sender, RoutedEventArgs e)
        {
            var term = SearchBox.Text;
            var replacement = ReplaceBox.Text;
            if (string.IsNullOrEmpty(term)) return;

            if (term != _lastTerm) { _lastIndex = -1; _lastTerm = term; }

            bool allTabs = AllTabsBox.IsChecked == true;

            if (allTabs)
            {
                foreach (var file in _vm.FileDeschise)
                {
                    string content = file.Content ?? string.Empty;
                    int idx = content.IndexOf(term, StringComparison.OrdinalIgnoreCase);
                    if (idx < 0) continue;
                    file.Content = content.Remove(idx, term.Length).Insert(idx, replacement);
                    _lastIndex = idx;
                    _vm.FileActiv = file;
                    return;
                }
                MessageBox.Show("Not found in any tab.", "Replace");
            }
            else
            {
                var tb = _getTextBox();
                if (tb == null) return;
                string text = tb.Text;

                int start = _lastIndex < 0 ? 0 : _lastIndex + term.Length;
                int idx = text.IndexOf(term, start, StringComparison.OrdinalIgnoreCase);
                if (idx < 0)
                    idx = text.IndexOf(term, 0, StringComparison.OrdinalIgnoreCase);

                if (idx >= 0)
                {
                    tb.Focus();
                    tb.Select(idx, term.Length);
                    tb.SelectedText = replacement; 
                    _lastIndex = idx;
                }
                else MessageBox.Show("Not found.", "Replace");
            }
        }

        private void ReplaceAll_Click(object sender, RoutedEventArgs e)
        {
            var term = SearchBox.Text;
            var replacement = ReplaceBox.Text;
            if (string.IsNullOrEmpty(term)) return;

            bool allTabs = AllTabsBox.IsChecked == true;
            var targets = allTabs ? _vm.FileDeschise.ToList() :
                          new List<Notepad.FileVM> { _vm.FileActiv };

            int totalCount = 0;
            foreach (var file in targets)
            {
                if (string.IsNullOrEmpty(file.Content)) continue;
                int count = 0;
                string content = file.Content;
                int idx;
                while ((idx = content.IndexOf(term, StringComparison.OrdinalIgnoreCase)) >= 0)
                {
                    content = content.Remove(idx, term.Length).Insert(idx, replacement);
                    count++;
                }
                if (count > 0) { file.Content = content; totalCount += count; }
            }

            _lastIndex = -1;    
            MessageBox.Show($"Replaced {totalCount} occurrence(s).", "Replace All");
        }
    }
}