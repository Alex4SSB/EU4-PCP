using EU4_PCP_WPF.Models;
using EU4_PCP_WPF.Services;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static EU4_PCP_WPF.MainCode;
using static EU4_PCP_WPF.PCP_Data;

namespace EU4_PCP_WPF.Views
{
    public partial class ColorPickerPage : Page, INotifyPropertyChanged
    {
        readonly Style GreenStyle = Application.Current.FindResource("GreenBackground") as Style;
        readonly Style RedStyle = Application.Current.FindResource("RedBackground") as Style;

        public ColorPickerPage()
        {
            InitializeComponent();
            DataContext = this;

            InitializeData();
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

        public bool EnableBooks => Security.RetrieveBoolEnum(ProvinceNames.Dynamic)
            && BookmarkList != null
            && BookmarkList.Any();

        private void InitializeData()
        {
            Lockdown = true;

            ModSelComboBox.ItemsSource = ModList;
            ModSelComboBox.IsEnabled = ModList.Count > 1;
            ModSelComboBox.SelectedIndex = SelectedModIndex;

            GameVerBlock.Text = GameVersion;
            ModVerBlock.Text = ModVersion;

            GameProvCountBlock.Text = $" {GameProvinceCount} ";
            ModProvCountBlock.Text = $" {ModProvinceCount} ";
            GameMaxProvBlock.Text = $" {GameMaxProvinces} ";
            ModMaxProvBlock.Text = $" {ModMaxProvinces} ";

            BookmarkComboBox.IsEnabled = EnableBooks;
            BookmarkComboBox.ItemsSource = BookmarkComboBox.IsEnabled ? BookmarkList : null;
            BookmarkComboBox.SelectedIndex = SelectedBookmarkIndex;

            StartDateBlock.Text = StartDateStr;

            ProvCountColor();

            Lockdown = false;
        }

        private void ModSelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Lockdown) return;
            SelectedModIndex = ModSelComboBox.SelectedIndex;
            EnactChange(CriticalScope.Mod);
        }

        private async void EnactChange(CriticalScope scope)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            await Task.Delay(1);

            switch (scope)
            {
                case CriticalScope.Mod:
                    ChangeMod();
                    break;
                case CriticalScope.Bookmark:
                    EnactBook();
                    break;
            }
            InitializeData();

            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Handles colorization of province count TextBlocks.
        /// </summary>
        private void ProvCountColor()
        {
            if (!string.IsNullOrWhiteSpace(GameMaxProvBlock.Text))
            {
                GameMaxProvBlock.Style = GameMaxProvBlock.Text.Gt(GameProvCountBlock.Text)
                    ? GreenStyle : RedStyle;
            }
            else
                GameMaxProvBlock.Style = null;

            if (!string.IsNullOrWhiteSpace(ModMaxProvBlock.Text))
            {
                ModMaxProvBlock.Style = ModMaxProvBlock.Text.Gt(ModProvCountBlock.Text)
                    ? GreenStyle : RedStyle;

                ModProvCountBlock.Style = ModProvCountBlock.Text.Ge(GameProvCountBlock.Text)
                    ? null : RedStyle;
            }
            else
            {
                ModMaxProvBlock.Style = null;
                ModProvCountBlock.Style = null;
            }
        }

        private void BookmarkComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Lockdown) return;
            SelectedBookmarkIndex = BookmarkComboBox.SelectedIndex;
            EnactChange(CriticalScope.Bookmark);
        }
    }
}
