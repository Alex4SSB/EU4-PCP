using EU4_PCP.Contracts.Services;
using EU4_PCP.Contracts.Views;
using EU4_PCP.Converters;
using EU4_PCP.Models;
using EU4_PCP.Services;
using static EU4_PCP.PCP_Implementations;
using static EU4_PCP.PCP_Logic;
using static EU4_PCP.PCP_Paths;

namespace EU4_PCP.Views;

public partial class SettingsPage : Page, INavigationAware
{
    private readonly AppConfig _appConfig;
    private readonly IThemeSelectorService _themeSelectorService;
    private readonly ISystemService _systemService;
    private bool _isInitialized;
    private bool _isBusy = false;
    
    public List<FrameworkElement> Controls = new();

    public SettingsPage(IOptions<AppConfig> appConfig, IThemeSelectorService themeSelectorService, ISystemService systemService, IApplicationInfoService applicationInfoService)
    {
        _appConfig = appConfig.Value;
        _themeSelectorService = themeSelectorService;
        _systemService = systemService;
        
        InitializeComponent();
        DataContext = this;

        AddControls();

        InitializeSettings();

        settingsPrivacyStatement.ToolTip = _appConfig.PrivacyStatement;
        settingsUserManual.ToolTip = _appConfig.UserManual;
        MahAppsLink.ToolTip = _appConfig.MahappsLink;

        PCP_Data.Notifiable.PropertyChanged += Notifiable_PropertyChanged;
    }

    private void Notifiable_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Notifiable.Theme))
        {
            _themeSelectorService.SetTheme(PCP_Data.Notifiable.Theme);
        }
    }

    public void OnNavigatedTo(object parameter)
    {
        PCP_Data.Notifiable.VersionDescription = $"{Properties.Resources.AppDisplayName} - {Properties.Resources.AppVersion}";
        PCP_Data.Notifiable.Theme = _themeSelectorService.GetCurrentTheme();
        _isInitialized = true;
    }

    public void OnNavigatedFrom()
    {

    }

    private void OnPrivacyStatementClick(object sender, RoutedEventArgs e)
        => _systemService.OpenInWebBrowser(_appConfig.PrivacyStatement);

    private void AddControls()
    {
        Controls.AddRange(new FrameworkElement[]
        {
            GamePathBlock,
            GamePathButton,
            ModPathBlock,
            ModPathButton,
            DisableLoadRadio,
            RememberLoadRadio,
            FullyLoadRadio,
            DefinitionNamesBox,
            LocalisationNamesBox,
            DynamicNamesBox,
            ShowAllBox,
            CheckDupliBox,
            UpdateMaxProvBox,
            IterateMaxProvBox,
            ShowIllegalProvBox,
            IgnoreIllegalBox,
            InCBox,
            OverrideModBooks
        });
    }

    private void PathButton_Click(object sender, RoutedEventArgs e)
    {
        ChangeSettings(sender as Control);
    }

    private void ChangeSettings(Control control)
    {
        string blockText = "";
        var tag = control.Tag.ToString();

        var dialog = new OpenFileDialog
        {
            Filter = Names.GlobalNames[tag + "Filter"],
        };

        if (dialog.ShowDialog() != true ||
            !dialog.FileName.Contains(dialog.Filter.Split('|')[1].TrimStart('*')))
            return;

        blockText = Directory.GetParent(dialog.FileName).ToString();
        Storage.StoreValue(blockText, tag);

        ((TextBlock)Controls.First(c => c.Tag.ToString() == tag && c is TextBlock)).Text = blockText;

        if (tag == General.GamePath.ToString())
        {
            PathHandler(Scope.Mod);
            if (!string.IsNullOrEmpty(ParadoxModPath))
                ModPathBlock.Text = ParadoxModPath;

            PCP_Main();
        }
    }

    private void RetrieveGroups()
    {
        var groups = Controls.Where(c => c.Tag.ToString().Contains('|')
                            ).Select(c => c.Tag.ToString().Split('|')[0]).Distinct();

        foreach (var item in groups)
        {
            ChangeGroup(item, Storage.RetrieveGroup(item));
        }
    }

    private void ChangeGroup(string group, long index)
    {
        var boxes = Controls.Where(c => c.Tag.ToString().Contains('|') && c.Tag.ToString().Split('|')[0] == group);

        foreach (ToggleButton item in boxes)
        {
            var i = item.GetIndex();
            item.IsChecked = i <= index; // (item is RadioButton ? i == index : i <= index)
        }
    }

    private void ChangeBox(ToggleButton control)
    {
        if (control.Tag.ToString().Contains('|'))
        {
            var property = control.Tag.ToString().Split('|');
            if (control is CheckBox)
            {
                var index = control.GetIndex();
                if (control.IsChecked == false) index--;

                property[1] = Enum.GetName(property[0].ToEnum(), index);
                ChangeGroup(property[0], index);
            }

            Storage.StoreValue(Enum.Parse(property[0].ToEnum(), property[1]), property[0]);
        }
        else
            Storage.StoreValue(control.IsChecked.ToString(), control.Tag);
    }

    private void InitializeSettings()
    {
        foreach (FrameworkElement item in Controls)
        {
            if (item.Tag.ToString().Contains('|')) continue;
            switch (item)
            {
                case StackPanel stack:
                    stack.Visible(Storage.RetrieveBool(stack.Tag));
                    break;
                case TextBlock box:
                    string value = Storage.RetrieveValue(item.Tag);
                    box.Text = string.IsNullOrEmpty(value) ? item.GetPlaceholder() : value;
                    break;
                case CheckBox box:
                    box.IsChecked = Storage.RetrieveBool(item.Tag);
                    break;
                default:
                    break;
            }
        }

        RetrieveGroups();
    }

    private void Box_Checked(object sender, RoutedEventArgs e)
    {
        if (_isInitialized && !_isBusy)
        {
            _isBusy = true;
            ChangeBox((ToggleButton)sender);
            UpdateProperties();
            _isBusy = false;
        }
    }

    private void UserManual_Click(object sender, RoutedEventArgs e)
        => _systemService.OpenInWebBrowser(_appConfig.UserManual);

    private void InCBox_Unchecked(object sender, RoutedEventArgs e)
    {
        ClearCache();

        Box_Checked(sender, e);
    }

    private void Hyperlink_Click(object sender, RoutedEventArgs e)
        => _systemService.OpenInWebBrowser(_appConfig.MahappsLink);
}
