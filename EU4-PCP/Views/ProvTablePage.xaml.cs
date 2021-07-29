using EU4_PCP.Contracts.Views;
using EU4_PCP.Models;
using EU4_PCP.Services;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Shapes;
using static EU4_PCP.PCP_Const;
using static EU4_PCP.PCP_Data;
using static EU4_PCP.PCP_Implementations;

namespace EU4_PCP.Views
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
            PageTitle.Text = Properties.Resources.ProvTablePageTitle;
            if (SelectedMod)
                PageTitle.Text += $" - {SelectedMod}";

            ProvTable.ItemsSource = from prov in Provinces.Values
                                    where prov && prov.Show
                                    orderby prov.Index
                                    select new TableProvince(prov);

            ProvincesShown = ProvTable.Items.Count.ToString();

            PaintMarkers();
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

        /// <summary>
        /// The UI side of MarkerPrep
        /// </summary>
        private void PaintMarkers()
        {
            var markers = MarkerPrep(Provinces.Values, Storage.RetrieveBool(General.CheckDupli), Storage.RetrieveBool(General.ShowIllegalProv));

            foreach (var marker in markers)
            {
                var rect = new Rectangle() { Height = MarkerHeight, Fill = marker.Item2 };
                rect.Tag = marker.Item1;
                var grid = new Grid() { Children = { rect }, RowDefinitions = { new RowDefinition() { Height = new GridLength(marker.Item3, GridUnitType.Star), MinHeight = MarkerHeight }, new RowDefinition() { Height = new GridLength(1 - marker.Item3, GridUnitType.Star) } } };

                MarkerGrid.Children.Add(grid);
            }
        }

        private void ProvTable_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ProvTableIndex = e.VerticalOffset;
        }

        private void ProvTable_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MoveTableToIndex(ProvTableIndex);
        }

        private void MoveTableToIndex(double tableIndex)
        {
            if (ProvTable.Items.Count < 1) return;

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
            var provIndex = Provinces.Values.Where(p => p && p.Show).OrderBy(p => p.Index).ToList().IndexOf(prov);
            var offset = provIndex + (int)(ProvTable.RenderSize.Height / (ProvTable.MinRowHeight + 1) / 2) - 1;
            if (offset >= ProvTable.Items.Count) offset = ProvTable.Items.Count - 1;

            ProvTable.ScrollIntoView(ProvTable.Items[0]);
            ProvTable.ScrollIntoView(ProvTable.Items[offset]);
        }

        private void DataGridRow_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is DataGridCellsPresenter row && row.DataContext is TableProvince prov && prov.IsProvDupli)
            {
                ScrollToProv(prov.NextDupli);
            }
        }
    }
}
