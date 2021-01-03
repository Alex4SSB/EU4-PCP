using EU4_PCP_WPF.Contracts.Views;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
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
            // -2 rows for scroll offset, +1 pixel for horizontal grid line width
            ProvTable.ScrollIntoView(ProvTable.Items[(int)(ProvTableIndex - 2 + ProvTable.RenderSize.Height / (ProvTable.MinRowHeight + 1))]);
        }

        private void DataGridRow_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
    }
}
