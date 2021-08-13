using EU4_PCP.Models;
using EU4_PCP.Services;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static EU4_PCP.PCP_Const;
using static EU4_PCP.PCP_Data;
using static EU4_PCP.PCP_Implementations;
using static EU4_PCP.PCP_Logic;

namespace EU4_PCP.Views
{
    public partial class ColorPickerPage : Page, INotifyPropertyChanged
    {
        private readonly P_Color PickedColor = ColorPickerPickedColor;

        private bool RedLock = false, GreenLock = false, BlueLock = false;

        public ColorPickerPage()
        {
            InitializeComponent();
            DataContext = this;

            InitializeData();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private static bool EnableBooks => Storage.RetrieveBoolEnum(ProvinceNames.Dynamic)
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

            GameProvCountBlock.Text = GameProvinceCount;
            GameMaxProvBlock.Text = GameMaxProvinces;
            ModProvCountBlock.Text = $" {ModProvinceCount} ";
            ModMaxProvBlock.Text = $" {ModMaxProvinces} ";

            IllegalProvCountGrid.Visibility = Visibility.Collapsed;
            DupliCountGrid.Visibility = Visibility.Collapsed;

            ProvShownBlock.Text = ProvincesShown;

            BookmarkComboBox.IsEnabled = EnableBooks;
            BookmarkComboBox.ItemsSource = BookmarkComboBox.IsEnabled ? BookmarkList : null;
            BookmarkComboBox.SelectedIndex = SelectedBookmarkIndex;
            BookmarkComboBox.ToolTip = CurrentDateFormat(true);

            if (SelectedMod)
            {
                string modPath = Directory.GetParent(PCP_Paths.SteamModPath).FullName;
                BookmarkBlock.Text = $"Bookmark Selection{(BookFiles.Any(b => Directory.GetParent(b.Path).FullName.Contains(modPath)) ? "" : " *")}";

                if (Storage.RetrieveBool(General.ShowIllegalProv))
                {
                    IllegalProvCountGrid.Visibility = Visibility.Visible;
                    ModIllegalProvCount.Text = ModIllegalProvinceCount;
                }

                if (Storage.RetrieveBool(General.CheckDupli))
                {
                    DupliCountGrid.Visibility = Visibility.Visible;
                    ModDupliCount.Text = ModDupliProvinceCount;
                }
            }

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

            HxValueBlock.IsEnabled =
            LockRedButton.IsEnabled =
            LockGreenButton.IsEnabled =
            LockBlueButton.IsEnabled =
            NewProvNameTextBox.IsEnabled =
            RandomizeButton.IsEnabled = isMod;

            RedSlider.IsEnabled =
            RedTextBox.IsEnabled = isMod && !RedLock;
            GreenSlider.IsEnabled =
            GreenTextBox.IsEnabled = isMod && !GreenLock;
            BlueSlider.IsEnabled =
            BlueTextBox.IsEnabled = isMod && !BlueLock;

            if (!isMod)
            {
                RedSlider.Value =
                GreenSlider.Value =
                BlueSlider.Value = -1;

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
                    ChosenProv = null;
                    ChangeMod();
                    ProvincesShown = Provinces.Values.Count(prov => prov && prov.Show).ToString();
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
                ClearButton.IsEnabled = true;
                NewProvBlock.Text = "Existing Province";
                AddProvButton.Content = "Update";
                OriginalColorBlock.Visibility = Visibility.Visible;
                OriginalColorBlock.Text = $"Original color: ({ChosenProv.Red}, {ChosenProv.Green}, {ChosenProv.Blue})";
                OpenProv();
            }
            else
            {
                ClearButton.IsEnabled = false;
                NewProvNameTextBox.Text = "";
                NewProvBlock.Text = "New Province";
                AddProvButton.Content = "Add";
                OriginalColorBlock.Visibility = Visibility.Collapsed;
                Randomize();
            }
        }

        private void OpenProv()
        {
            NewProvNameTextBox.Text = ChosenProv.Name;
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
            if (!string.IsNullOrWhiteSpace(ModMaxProvBlock.Text))
            {
                if (ModMaxProvBlock.Text.Gt(ModProvCountBlock.Text))
                {
                    ModMaxProvBlock.Style = GreenStyle;
                    ModMaxProvBlock.ToolTip = Names.GlobalNames["MaxProvPositive"];
                }
                else
                {
                    ModMaxProvBlock.Style = RedStyle;
                    ModMaxProvBlock.ToolTip = Names.GlobalNames["MaxProvNegative"];
                }

                if (ModProvCountBlock.Text.Ge(GameProvCountBlock.Text))
                {
                    ModProvCountBlock.Style = null;
                    ModProvCountBlock.ToolTip = null;
                }
                else
                {
                    ModProvCountBlock.Style = RedStyle;
                    ModProvCountBlock.ToolTip = Names.GlobalNames["ModProvNegative"];
                }
            }
            else
            {
                ModMaxProvBlock.Style =
                ModProvCountBlock.Style = null;
                ModMaxProvBlock.ToolTip =
                ModProvCountBlock.ToolTip = null;
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
            PickedColor.R = (short)RedSlider.Value;
            PickedColor.G = (short)GreenSlider.Value;
            PickedColor.B = (short)BlueSlider.Value;

            HxValueBlock.Text = PickedColor.AsHex();
            ColorPickerPickedColor = PickedColor;

            if (PickedColor.IsLegal())
            {
                ColorRectangle.Fill = new SolidColorBrush(PickedColor);
                RedTextBox.Background =
                GreenTextBox.Background =
                BlueTextBox.Background = SelectBG(PickedColor, ChosenProv?.province);
            }
            else
            {
                ColorRectangle.Fill = new SolidColorBrush(Colors.Transparent);
                RedTextBox.Background = LegalBG(PickedColor.R);
                GreenTextBox.Background = LegalBG(PickedColor.G);
                BlueTextBox.Background = LegalBG(PickedColor.B);
            }

            NewProvLegal();
        }

        private void NewProvLegal()
        {
            var enable = EnableAddProv();
            AddProvButton.IsEnabled = enable;
            NextIllegalButton.IsEnabled = enable
                && ChosenProv
                && !(ChosenProv.IsNameLegal() && ChosenProv.province.Color.IsLegal())
                && !Storage.RetrieveBool(General.IgnoreIllegal)
                && ModIllegalProvinceCount.ToInt() > 1;
        }

        private bool EnableAddProv()
        {
            if (string.IsNullOrWhiteSpace(NewProvNameTextBox.Text)) return false;

            if (NewProvNameTextBox.Text.Any(c => c > 255)) return false;

            if (!PickedColor.IsLegal()) return false;

            if (ChosenProv && (ChosenProv.Color.Equals(PickedColor)
                    && (ChosenProv.Name == NewProvNameTextBox.Text)))
                return false;

            return true;
        }

        private void RandomizeButton_Click(object sender, RoutedEventArgs e)
        {
            Randomize();
        }

        private void Randomize()
        {
            int r = RedLock ? (int)RedSlider.Value : -1;
            int g = GreenLock ? (int)GreenSlider.Value : -1;
            int b = BlueLock ? (int)BlueSlider.Value : -1;

            var tempColor = RandomProvColor(Provinces, r, g, b);
            RedSlider.Value = tempColor.R;
            GreenSlider.Value = tempColor.G;
            BlueSlider.Value = tempColor.B;

            int lastIndex;
            if (Storage.RetrieveBool(General.IgnoreIllegal))
            {
                var provs = Provinces.Values.OrderBy(p => p.Index);
                lastIndex = provs.Last(prov => prov.IsNameLegal() && prov.Color.IsLegal());
            }
            else 
                lastIndex = Provinces.Keys.Max();

            if (!ChosenProv)
                NextProvBlock.Text = (lastIndex + 1).ToString();
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
            NewProvLegal();
        }

        private void LockRedButton_Click(object sender, RoutedEventArgs e)
        {
            RedLock = !RedLock;

            RedSlider.IsEnabled = 
            RedTextBox.IsEnabled = !RedLock;
            LockRedButton.Content = RedLock ? "\uE72E" : "\uE785";

            EnableRandomize();
        }

        private void LockGreenButton_Click(object sender, RoutedEventArgs e)
        {
            GreenLock = !GreenLock;

            GreenSlider.IsEnabled = 
            GreenTextBox.IsEnabled = !GreenLock;
            LockGreenButton.Content = GreenLock ? "\uE72E" : "\uE785";

            EnableRandomize();
        }

        private void LockBlueButton_Click(object sender, RoutedEventArgs e)
        {
            BlueLock = !BlueLock;

            BlueSlider.IsEnabled = 
            BlueTextBox.IsEnabled = !BlueLock;
            LockBlueButton.Content = BlueLock ? "\uE72E" : "\uE785";

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
            NewProv();
        }

        private void NewProv(bool next = false)
        {
            int index = NextProvBlock.Text.ToInt();
            if (!AddProv(new Province(index, new CompositeName(NewProvNameTextBox.Text), PickedColor)))
                return;

            if (next)
                ChosenProv = new(Provinces[index + 1]);

            CountProv(Scope.Mod);
            InitializeData();
            DupliPrep();
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

        private void NextIllegalButton_Click(object sender, RoutedEventArgs e)
        {
            NewProv(true);
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
