using EU4_PCP.Models;
using EU4_PCP.Services;
using static EU4_PCP.PCP_Const;
using static EU4_PCP.PCP_Data;
using static EU4_PCP.PCP_Implementations;
using static EU4_PCP.PCP_Logic;

namespace EU4_PCP.Views;

public partial class ColorPickerPage : Page
{
    private const string externDef = "[ External Definition ]";
    private readonly P_Color PickedColor = ColorPickerPickedColor;

    public ColorPickerPage()
    {
        InitializeComponent();
        DataContext = this;

        InitializeData();

        PCP_Data.Notifiable.PropertyChanged += Notifiable_PropertyChanged;
    }

    private void Notifiable_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(PCP_Data.Notifiable.ExternalDefinition) && PCP_Data.Notifiable.ExternalDefinition is not null)
        {
            if (ModList.Last() is not externDef)
                ModList.Add(externDef);

            ModSelComboBox.Items.Refresh();
            ModSelComboBox.SelectedItem = externDef;
        }
    }

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

        NoBooks();
        BookmarkComboBox.ItemsSource = BookmarkList;
        BookmarkComboBox.SelectedIndex = SelectedBookmarkIndex;
        BookmarkBlock.Text = "Bookmark Selection";

        if (SelectedMod)
        {
            BookmarkBlock.Text += AreBooksOverridden ? " *" : "";

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

    private void NoBooks()
    {
        BookmarkComboBox.IsEnabled = false;

        if (!EnableBooks)
        {
            BookmarkList = new() { new() { Date = StartDate, Name = Properties.Resources.NoBookmarkPlaceholder } };
        }
        else if (BookmarkList.Count > 1)
        {
            BookmarkComboBox.IsEnabled = true;
        }
        else if (BookmarkList.Count == 1 && string.IsNullOrEmpty(BookmarkList[0].Name))
        {
            BookmarkList[0].Name = Properties.Resources.NoBookmarkPlaceholder;
        }
    }

    private void ModSelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        EnablePicker();

        if (Lockdown)
            return;

        if (ModSelComboBox.SelectedItem is externDef)
        {
            Mods.Add(new()
            {
                Name = externDef,
                Path = PCP_Data.Notifiable.ExternalDefinition,
                GameVer = null,
                Replace = new()
            });

            //ModProvCountBlock.Visibility =
            //ModVerBlock.Visibility = Visibility.Hidden;
        }
        else
        {
            ModList.Remove(externDef);
            Mods.RemoveAll(m => m.Name is externDef);
            PCP_Data.Notifiable.ExternalDefinition = null;

            
        }

        ModProvCountBlock.Visibility =
        ModVerBlock.Visibility = Visibility.Visible;

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
        RedTextBox.IsEnabled = isMod && !PCP_Data.Notifiable.RedLock;
        GreenSlider.IsEnabled =
        GreenTextBox.IsEnabled = isMod && !PCP_Data.Notifiable.GreenLock;
        BlueSlider.IsEnabled =
        BlueTextBox.IsEnabled = isMod && !PCP_Data.Notifiable.BlueLock;

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
        NextIllegalButton.Background = AddProvButton.Background;
        if (SelectedModIndex < 1)
            return;

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
        NewProvNameTextBox.Text = ChosenProv.province.Name.Definition;

        NextProvBlock.Text = ChosenProv.Index.ToString();
        RedSlider.Value = ChosenProv.Red;
        GreenSlider.Value = ChosenProv.Green;
        BlueSlider.Value = ChosenProv.Blue;

        if (ChosenProv.IsProvDupli)
        {
            NextIllegalButton.Background = new SolidColorBrush(RedBackground);
            NextIllegalButton.ToolTip = Properties.Resources.NextButtonDupli;
        }
        else
        {
            NextIllegalButton.Background = new SolidColorBrush(PurpleBackground);
            NextIllegalButton.ToolTip = Properties.Resources.NextButtonIllegal;
        }
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

        RedTextBox.ChannelTooltip(PickedColor.R);
        GreenTextBox.ChannelTooltip(PickedColor.G);
        BlueTextBox.ChannelTooltip(PickedColor.B);

        NewProvLegal();
    }

    private void NewProvLegal()
    {
        var enable = EnableAddProv();
        AddProvButton.IsEnabled = enable;
        NextIllegalButton.IsEnabled = enable
            && ChosenProv
            && EnableNext();
    }

    private static bool EnableNext()
    {
        if (ChosenProv.IsProvDupli)
        {
            return ChosenProv.IsProvDupli
                && ModDupliProvinceCount.ToInt() > 1;
        }
        else
        {
            return (!ChosenProv.IsNameLegal() || !ChosenProv.province.Color.IsLegal())
                && !Storage.RetrieveBool(General.IgnoreIllegal)
                && ModIllegalProvinceCount.ToInt() > 1;
        }
    }

    private bool EnableAddProv()
    {
        string provName = NewProvNameTextBox.Text;
        if (string.IsNullOrWhiteSpace(provName)) return false;

        if (provName.Any(c => c > 255)) return false;

        if (!PickedColor.IsLegal()) return false;

        if (ChosenProv && (ChosenProv.province.Color.Equals(PickedColor)
                && (ChosenProv.province.Name.Definition == provName)))
            return false;

        return true;
    }

    private void RandomizeButton_Click(object sender, RoutedEventArgs e)
    {
        Randomize();
    }

    private void Randomize()
    {
        int r = PCP_Data.Notifiable.RedLock ? (int)RedSlider.Value : -1;
        int g = PCP_Data.Notifiable.GreenLock ? (int)GreenSlider.Value : -1;
        int b = PCP_Data.Notifiable.BlueLock ? (int)BlueSlider.Value : -1;

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
        PCP_Data.Notifiable.RedLock ^= true;

        RedSlider.IsEnabled = 
        RedTextBox.IsEnabled = !PCP_Data.Notifiable.RedLock;

        EnableRandomize();
    }

    private void LockGreenButton_Click(object sender, RoutedEventArgs e)
    {
        PCP_Data.Notifiable.GreenLock ^= true;

        GreenSlider.IsEnabled = 
        GreenTextBox.IsEnabled = !PCP_Data.Notifiable.GreenLock;

        EnableRandomize();
    }

    private void LockBlueButton_Click(object sender, RoutedEventArgs e)
    {
        PCP_Data.Notifiable.BlueLock ^= true;

        BlueSlider.IsEnabled = 
        BlueTextBox.IsEnabled = !PCP_Data.Notifiable.BlueLock;

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

        DupliPrep();
        if (next)
            ChosenProv = SelectNextProv(index, ChosenProv.IsProvDupli);

        CountProv(Scope.Mod);
        InitializeData();
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

    private void Border_MouseDown(object sender, MouseButtonEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Filter = $"Definition file|{PCP_Paths.DEFIN_FILE}",
        };

        if (dialog.ShowDialog() is true)
            PCP_Data.Notifiable.ExternalDefinition = dialog.FileName;
    }

    private void Border_DragEnter(object sender, DragEventArgs e)
    {
        if (e.Data.GetData(DataFormats.FileDrop) is not string[] items || items.Length != 1 || !items[0].EndsWith(PCP_Paths.DEFIN_FILE))
            return;

        // # FFD4CF94
        ExternalDefBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(212, 207, 148));
    }

    private void Border_DragLeave(object sender, DragEventArgs e)
    {
        ExternalDefBorder.BorderBrush = new SolidColorBrush(Colors.Transparent);
    }

    private void Border_Drop(object sender, DragEventArgs e)
    {
        if (e.Data.GetData(DataFormats.FileDrop) is string[] items && items.Length == 1 && items[0].EndsWith(PCP_Paths.DEFIN_FILE))
            PCP_Data.Notifiable.ExternalDefinition = items[0];

        ExternalDefBorder.BorderBrush = new SolidColorBrush(Colors.Transparent);
    }
}
