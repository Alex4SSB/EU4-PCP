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

        private static bool EnableBooks => Security.RetrieveBoolEnum(ProvinceNames.Dynamic)
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
            EnablePicker();
            UpdatePicker();
            
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
            LockRedButton.IsEnabled =
            GreenSlider.IsEnabled =
            GreenTextBox.IsEnabled =
            LockGreenButton.IsEnabled =
            BlueSlider.IsEnabled =
            BlueTextBox.IsEnabled =
            LockBlueButton.IsEnabled =
            NewProvNameTextBox.IsEnabled =
            RandomizeButton.IsEnabled = isMod;

            if (!isMod)
            {
                RedSlider.Value = 
                GreenSlider.Value = 
                BlueSlider.Value = 0;

                RedTextBox.Background = 
                GreenTextBox.Background = 
                BlueTextBox.Background = null;

                NextProvBlock.Text = 
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
                    ProvincesShown = Provinces.Count(prov => prov && prov.Show).ToString();
                    UpdatePicker();
                    break;
                case CriticalScope.Bookmark:
                    EnactBook();
                    break;
            }
            InitializeData();

            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void UpdatePicker()
        {
            if (SelectedModIndex < 1) return;

            if (ChosenProv)
            {
                ClearButton.Visibility = Visibility.Visible;
                NewProvNameTextBox.IsReadOnly = true;
                NewProvBlock.Text = "Existing Province";
                AddProvButton.Content = "Update Province";
                OriginalColorBlock.Visibility = Visibility.Visible;
                OriginalColorBlock.Text = $"Original color: ({ChosenProv.Color.R}, {ChosenProv.Color.G}, {ChosenProv.Color.B})";
                OpenProv();
            }
            else
            {
                ClearButton.Visibility = Visibility.Collapsed;
                NewProvNameTextBox.Text = "";
                NewProvNameTextBox.IsReadOnly = false;
                NewProvBlock.Text = "New Province";
                AddProvButton.Content = "Add Province";
                OriginalColorBlock.Visibility = Visibility.Collapsed;
                Randomize();
            }
        }

        private void OpenProv()
        {
            NewProvNameTextBox.Text = ChosenProv.Name.Definition;
            if (string.IsNullOrEmpty(NewProvNameTextBox.Text))
                NewProvNameTextBox.Text = ChosenProv.Name.ToString();

            NextProvBlock.Text = ChosenProv.Index.ToString();
            RedSlider.Value = ChosenProv.Red;
            GreenSlider.Value = ChosenProv.Green;
            BlueSlider.Value = ChosenProv.Blue;
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
            HxValueBlock.Text = PickedColor.ToString().Replace("#FF", "#");
            ColorPickerPickedColor = PickedColor.Convert();

            RedTextBox.Background =
            GreenTextBox.Background =
            BlueTextBox.Background = new SolidColorBrush(Provinces.Any(prov => prov.Color.Equals(PickedColor.Convert())) ? RedBackground : GreenBackground);

            if (ChosenProv)
            {
                AddProvButton.IsEnabled = !ChosenProv.Color.Equals(PickedColor.Convert());
            }
        }

        private void RandomizeButton_Click(object sender, RoutedEventArgs e)
        {
            Randomize();
        }

        private void Randomize()
        {
            int r = RedSlider.IsEnabled ? -1 : (int)RedSlider.Value;
            int g = GreenSlider.IsEnabled ? -1 : (int)GreenSlider.Value;
            int b = BlueSlider.IsEnabled ? -1 : (int)BlueSlider.Value;

            var tempColor = RandomProvColor(Provinces, r, g, b).Convert();
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

        private void NewProvNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            AddProvButton.IsEnabled = !string.IsNullOrWhiteSpace(NewProvNameTextBox.Text)
                && !NewProvNameTextBox.Text.Any(c => c > 255);
        }

        private void LockRedButton_Click(object sender, RoutedEventArgs e)
        {
            RedSlider.IsEnabled = !RedSlider.IsEnabled;
            RedTextBox.IsEnabled = RedSlider.IsEnabled;
            LockRedButton.Content = RedSlider.IsEnabled ? "\uE785" : "\uE72E";

            EnableRandomize();
        }

        private void LockGreenButton_Click(object sender, RoutedEventArgs e)
        {
            GreenSlider.IsEnabled = !GreenSlider.IsEnabled;
            GreenTextBox.IsEnabled = GreenSlider.IsEnabled;
            LockGreenButton.Content = GreenSlider.IsEnabled ? "\uE785" : "\uE72E";

            EnableRandomize();
        }

        private void LockBlueButton_Click(object sender, RoutedEventArgs e)
        {
            BlueSlider.IsEnabled = !BlueSlider.IsEnabled;
            BlueTextBox.IsEnabled = BlueSlider.IsEnabled;
            LockBlueButton.Content = BlueSlider.IsEnabled ? "\uE785" : "\uE72E";

            EnableRandomize();
        }

        private void EnableRandomize()
        {
            RandomizeButton.IsEnabled = RedSlider.IsEnabled || GreenSlider.IsEnabled || BlueSlider.IsEnabled;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ChosenProv = null;
            InitializeData();
        }

        private void AddProvButton_Click(object sender, RoutedEventArgs e)
        {
            AddProv();
        }

        private bool AddProv()
        {
            if (ChosenProv)
            {
                Provinces.Find(prov => prov.Index == ChosenProv.ID).Color = PickedColor.Convert();
            }
            else
            {
                var newProv = new Province(NextProvBlock.Text.ToInt(), new CompositeName(NewProvNameTextBox.Text), PickedColor.Convert());
                newProv.IsRNW();
                Provinces.Add(newProv);

                ModMaxProvinces = Inc(ModMaxProvinces, 1);
                if (!WriteDefines(ModMaxProvinces))
                    return false;

                ModProvinceCount = Inc(ModProvinceCount, 1);
                ProvincesShown = Provinces.Count(prov => prov && prov.Show).ToString();
            }

            if (!WriteProvinces())
                return false;

            ChosenProv = null;
            InitializeData();
            DupliPrep();

            return true;
        }

        private void HxValueBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HxValueBlock.FontWeight = FontWeights.Bold;
            Clipboard.SetText(HxValueBlock.Text);
        }

        private void HxValueBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            HxValueBlock.TextDecorations.Add(TextDecorations.Underline);
        }

        private void HxValueBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            HxValueBlock.FontWeight = FontWeights.Normal;
        }

        private void HxValueBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            HxValueBlock.TextDecorations.Clear();
        }
    }
}
