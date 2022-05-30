using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System;

namespace IngatlanGyak
{

    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<Ingatlan> Ingatlanok { get; set; }
        public ObservableCollection<string> Varosok
        {
            get
            {
                ObservableCollection<string> varosok = new ObservableCollection<string>();

                foreach (Ingatlan ingatlan in Ingatlanok)
                    if (!varosok.Contains(ingatlan.Varos))
                        varosok.Add(ingatlan.Varos);

                return varosok;
            }
        }

        private IEnumerable<Ingatlan> keresettIngatlanok;
        public IEnumerable<Ingatlan> KeresettIngatlanok 
        {
            get => keresettIngatlanok;
            set { keresettIngatlanok = value; OnPropertyChanged(nameof(KeresettIngatlanok)); } 
        }

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
            Ingatlanok = new ObservableCollection<Ingatlan>()
            {
                new Ingatlan() { Id = 1, Varos = "Budapest", Utca = "Üteg utca 15", Alapterulet = 400 },
                new Ingatlan() { Id = 2, Varos = "Budapest", Utca = "Váci út 142", Alapterulet = 130 },
                new Ingatlan() { Id = 3, Varos = "Vác", Utca = "Petőfi Sándor utca 2", Alapterulet = 80 },
                new Ingatlan() { Id = 4, Varos = "Tiszaújváros", Utca = "Ady Endre köz 3", Alapterulet = 95 }
            };
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        
        private void CbVarosok_SelectionChanged(object sender, SelectionChangedEventArgs e)
            => KeresettIngatlanok = Ingatlanok.Where(x => x.Varos == (string)cbVarosok.SelectedValue);

        private void DataGrid_SelectionChanged(object sender, EventArgs e)
        {
            var ingatlan = (Ingatlan) ((DataGrid) sender).SelectedItem;
            var wnd = new EditWindow(ingatlan);

            Hide();
            wnd.ShowDialog();
            Show();

            OnPropertyChanged(nameof(Varosok));
        }
    }
}
