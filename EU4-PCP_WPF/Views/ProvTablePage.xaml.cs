using EU4_PCP_WPF.Contracts.Views;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using static EU4_PCP_WPF.PCP_Data;

namespace EU4_PCP_WPF.Views
{
    public partial class ProvTablePage : Page, INotifyPropertyChanged, INavigationAware
    {
        public ProvTablePage()
        {
            InitializeComponent();
            DataContext = this;
        }

        public void OnNavigatedTo(object parameter)
        {
            ProvTable.ItemsSource = from prov in Provinces
                                    where prov && prov.Show
                                    select new TableProvince(prov);

            ProvincesShown = ProvTable.Items.Count.ToString();

            PaintMarkers();
        }

        private void PaintMarkers()
        {
            foreach (var dupliProv in Provinces.Where(prov => prov.NextDupli))
            {
                var marker = new Rectangle() { Height = 4, VerticalAlignment = System.Windows.VerticalAlignment.Bottom, Margin = new System.Windows.Thickness(0, 0, 0, 0), Fill = new SolidColorBrush(Colors.Crimson) };
                marker.MouseLeftButtonUp += new MouseButtonEventHandler(Rectangle_MouseLeftButtonUp);
                marker.Tag = dupliProv;
                var shownProvs = Provinces.Where(p => p && p.Show).ToList();
                double ratio = shownProvs.IndexOf(dupliProv) / (double)shownProvs.Count;

                var grid = new Grid() { Children = { marker }, RowDefinitions = { new RowDefinition() { Height = new System.Windows.GridLength(ratio, System.Windows.GridUnitType.Star), MinHeight = 4 }, new RowDefinition() { Height = new System.Windows.GridLength(1 - ratio, System.Windows.GridUnitType.Star) } } };
                MarkerGrid.Children.Add(grid);
            }
        }

        public void OnNavigatedFrom()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void ProvTable_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ProvTableIndex = e.VerticalOffset;
        }

        private void ProvTable_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            MoveTableToIndex(ProvTableIndex);
        }

        private void MoveTableToIndex(double tableIndex)
        {
            // -2 rows for scroll offset, +1 pixel for horizontal grid line width
            int index = (int)(tableIndex - 2 + (ProvTable.RenderSize.Height / (ProvTable.MinRowHeight + 1)));
            if (index >= ProvTable.Items.Count) index = ProvTable.Items.Count - 1;
            ProvTable.ScrollIntoView(ProvTable.Items[index]);
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is DataGridRow row && row.Item is TableProvince prov && SelectedModIndex > 0)
            {
                ChosenProv = prov;
                NavigateToColorPicker = true;
            }
        }

        private void Rectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var marker = sender as Rectangle;
            ScrollToProv(marker.Tag as Province);
        }

        private void ScrollToProv(Province prov)
        {
            var provIndex = Provinces.Where(p => p && p.Show).ToList().IndexOf(prov);
            var offset = provIndex + (int)(ProvTable.RenderSize.Height / (ProvTable.MinRowHeight + 1) / 2) - 1;
            if (offset >= ProvTable.Items.Count) offset = ProvTable.Items.Count - 1;

            ProvTable.ScrollIntoView(ProvTable.Items[0]);
            ProvTable.ScrollIntoView(ProvTable.Items[offset]);
        }

        private void DataGridRow_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is DataGridCellsPresenter row && row.DataContext is TableProvince prov && prov.IsDupli)
            {
                ScrollToProv(prov.NextDupli);
            }
        }
    }
}
