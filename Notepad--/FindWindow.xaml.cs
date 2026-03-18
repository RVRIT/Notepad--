using System;
using System.Windows;
using System.Windows.Controls;

namespace Notepad
{
    public partial class FindWindow : Window
    {
        private readonly MainVM _vm;
        private readonly Func<TextBox> _getTextBox;

        public FindWindow(MainVM vm, Func<TextBox> getTextBox)
        {
            InitializeComponent();
            _vm = vm;
            _getTextBox = getTextBox;
        }

        private int _lastIndex = -1;
        private string _lastTerm = string.Empty;
    
        private void Find_Click(object sender, RoutedEventArgs e)
        {
            var term = SearchBox.Text;
            if (string.IsNullOrEmpty(term)) return;

            if (term != _lastTerm)
            {
                _lastIndex = -1;
                _lastTerm = term;
            }

            var tb = _getTextBox();
            if (tb == null) return;
            string text = tb.Text;

            int start = _lastIndex < 0 ? 0 : _lastIndex + term.Length;
            int idx = text.IndexOf(term, start, StringComparison.OrdinalIgnoreCase);
            if (idx < 0) 
                idx = text.IndexOf(term, 0, StringComparison.OrdinalIgnoreCase);

            if (idx >= 0)
            {
                _lastIndex = idx;
                tb.Focus();
                tb.Select(idx, term.Length);
                tb.ScrollToLine(tb.GetLineIndexFromCharacterIndex(idx));
            }
            else MessageBox.Show("Not found.", "Find");
        }

        private void FindPrevious_Click(object sender, RoutedEventArgs e)
        {
            var term = SearchBox.Text;
            if (string.IsNullOrEmpty(term)) return;

            if (term != _lastTerm) { _lastIndex = -1; _lastTerm = term; }

            var tb = _getTextBox();
            if (tb == null) return;
            string text = tb.Text;

            int start = _lastIndex <= 0 ? text.Length - 1 : _lastIndex - 1;
            int idx = text.LastIndexOf(term, start, StringComparison.OrdinalIgnoreCase);
            if (idx < 0)
                idx = text.LastIndexOf(term, text.Length - 1, StringComparison.OrdinalIgnoreCase);

            if (idx >= 0)
            {
                _lastIndex = idx;
                tb.Focus();
                tb.Select(idx, term.Length);
                tb.ScrollToLine(tb.GetLineIndexFromCharacterIndex(idx));
            }
            else MessageBox.Show("Not found.", "Find");
        }
    }
}