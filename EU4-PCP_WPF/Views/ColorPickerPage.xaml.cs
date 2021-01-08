using EU4_PCP_WPF.Models;
using EU4_PCP_WPF.Services;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static EU4_PCP_WPF.PCP_Implementations;
using static EU4_PCP_WPF.MainCode;
using static EU4_PCP_WPF.PCP_Data;
using static EU4_PCP_WPF.PCP_Const;
using EU4_PCP_WPF.Converters;

namespace EU4_PCP_WPF.Views
{
    public partial class ColorPickerPage : Page, INotifyPropertyChanged
    {
        readonly Style GreenStyle = Application.Current.FindResource("GreenBackground") as Style;
        readonly Style RedStyle = Application.Current.FindResource("RedBackground") as Style;

        private Color PickedColor = ColorPickerPickedColor.Convert();

        public ColorPickerPage()
        {
            InitializeComponent();
            DataContext = this;

            InitializeData();
            PickedColor.A = 255;
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
            ProvShownBlock.Text = ProvincesShown;

            BookmarkComboBox.IsEnabled = EnableBooks;
            BookmarkComboBox.ItemsSource = BookmarkComboBox.IsEnabled ? BookmarkList : null;
            BookmarkComboBox.SelectedIndex = SelectedBookmarkIndex;

            StartDateBlock.Text = StartDateStr;
            StartDateBlock.ToolTip = DATE_FORMAT;

            ProvCountColor();

            Lockdown = false;
        }

        private void ModSelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EnablePicker();
            if (Lockdown) return;
            SelectedModIndex = ModSelComboBox.SelectedIndex;
            EnactChange(CriticalScope.Mod);
        }

        private void EnablePicker()
        {
            bool isMod = ModSelComboBox.SelectedIndex > 0;

            RedSlider.IsEnabled =
            RedTextBox.IsEnabled =
            GreenSlider.IsEnabled =
            GreenTextBox.IsEnabled =
            BlueSlider.IsEnabled =
            BlueTextBox.IsEnabled =
            NewProvNameTextBox.IsEnabled =
            RandomizeButton.IsEnabled = isMod;

            if (!isMod)
            {
                RedSlider.Value = 0;
                GreenSlider.Value = 0;
                BlueSlider.Value = 0;
                NextProvBlock.Text = "";
                NewProvNameTextBox.Text = "";
            }
        }

        private async void EnactChange(CriticalScope scope)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            await Task.Delay(1);

            switch (scope)
            {
                case CriticalScope.Mod:
                    ChangeMod();
                    if (SelectedModIndex > 0) Randomize();
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

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            PickedColor.R = (byte)RedSlider.Value;
            PickedColor.G = (byte)GreenSlider.Value;
            PickedColor.B = (byte)BlueSlider.Value;
            
            ColorRectangle.Fill = new SolidColorBrush(PickedColor);
            ColorRectangle.ToolTip = PickedColor.ToString().Replace("FF", "");
            ColorPickerPickedColor = PickedColor.Convert();
        }

        private void RandomizeButton_Click(object sender, RoutedEventArgs e)
        {
            Randomize();
        }

        private void Randomize()
        {
            var tempColor = RandomProvColor(Provinces).Convert();
            RedSlider.Value = tempColor.R;
            GreenSlider.Value = tempColor.G;
            BlueSlider.Value = tempColor.B;

            NextProvBlock.Text = (Provinces.Last().Index + 1).ToString();
        }

        private void RedTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (byte.TryParse(RedTextBox.Text, out byte red))
                RedSlider.Value = red;
            
            RedTextBox.Text = RedSlider.Value.ToString();
        }

        private void GreenTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (byte.TryParse(GreenTextBox.Text, out byte green))
                GreenSlider.Value = green;

            GreenTextBox.Text = GreenSlider.Value.ToString();
        }

        private void BlueTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (byte.TryParse(BlueTextBox.Text, out byte blue))
                BlueSlider.Value = blue;

            BlueTextBox.Text = BlueSlider.Value.ToString();
        }
    }
}
