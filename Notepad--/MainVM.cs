using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var fisierInitial = new FileVM();
            FileDeschise.Add(fisierInitial);
            FileActiv = fisierInitial;
        }
    }
}
