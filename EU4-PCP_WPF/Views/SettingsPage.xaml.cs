using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using EU4_PCP_WPF.Contracts.Services;
using EU4_PCP_WPF.Contracts.Views;
using EU4_PCP_WPF.Converters;
using EU4_PCP_WPF.Models;
using EU4_PCP_WPF.Services;
using Microsoft.Extensions.Options;
using Microsoft.Win32;
using static EU4_PCP_WPF.MainCode;
using static EU4_PCP_WPF.PCP_Paths;

namespace EU4_PCP_WPF.Views
{
    public partial class SettingsPage : Page, INotifyPropertyChanged, INavigationAware
    {
        private readonly AppConfig _appConfig;
        private readonly IThemeSelectorService _themeSelectorService;
        private readonly ISystemService _systemService;
        private readonly IApplicationInfoService _applicationInfoService;
        private bool _isInitialized;
        private bool _isBusy = false;
        private AppTheme _theme;
        private string _versionDescription;

        public List<FrameworkElement> Controls = new List<FrameworkElement>();

        public AppTheme Theme
        {
            get { return _theme; }
            set { Set(ref _theme, value); }
        }

        public string VersionDescription
        {
            get { return _versionDescription; }
            set { Set(ref _versionDescription, value); }
        }

        public SettingsPage(IOptions<AppConfig> appConfig, IThemeSelectorService themeSelectorService, ISystemService systemService, IApplicationInfoService applicationInfoService)
        {
            _appConfig = appConfig.Value;
            _themeSelectorService = themeSelectorService;
            _systemService = systemService;
            _applicationInfoService = applicationInfoService;
            InitializeComponent();
            DataContext = this;

            AddControls();

            InitializeSettings();
        }

        public void OnNavigatedTo(object parameter)
        {
            VersionDescription = $"{Properties.Resources.AppDisplayName} - {Properties.Resources.AppVersion}";
            Theme = _themeSelectorService.GetCurrentTheme();
            _isInitialized = true;
        }

        public void OnNavigatedFrom()
        {
        }

        private void OnLightChecked(object sender, RoutedEventArgs e)
        {
            if (_isInitialized)
            {
                _themeSelectorService.SetTheme(AppTheme.Light);
            }
        }

        private void OnDarkChecked(object sender, RoutedEventArgs e)
        {
            if (_isInitialized)
            {
                _themeSelectorService.SetTheme(AppTheme.Dark);
            }
        }

        private void OnDefaultChecked(object sender, RoutedEventArgs e)
        {
            if (_isInitialized)
            {
                _themeSelectorService.SetTheme(AppTheme.Default);
            }
        }

        private void OnPrivacyStatementClick(object sender, RoutedEventArgs e)
            => _systemService.OpenInWebBrowser(_appConfig.PrivacyStatement);

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
                CheckDupliBox
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

            blockText = System.IO.Directory.GetParent(dialog.FileName).ToString();
            Security.StoreValue(blockText, tag);

            ((TextBlock)Controls.First(c => c.Tag.ToString() == tag && c is TextBlock)).Text = blockText;

            if (tag == General.GamePath.ToString())
            {
                PathHandler(Scope.Mod);
                if (!string.IsNullOrEmpty(ParadoxModPath))
                    ModPathBlock.Text = ParadoxModPath;
            }
        }

        private void RetrieveGroups()
        {
            var groups = Controls.Where(c => c.Tag.ToString().Contains('|')
                                ).Select(c => c.Tag.ToString().Split('|')[0]).Distinct();

            foreach (var item in groups)
            {
                ChangeGroup(item, Security.RetrieveGroup(item));
            }
        }

        private void ChangeGroup(string group, long index)
        {
            var boxes = Controls.Where(c => c.Tag.ToString().Contains('|') && c.Tag.ToString().Split('|')[0] == group);

            foreach (ToggleButton item in boxes)
            {
                var i = item.GetIndex();
                item.IsChecked = (item is RadioButton ? i == index : i <= index);
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

                Security.StoreValue(Enum.Parse(property[0].ToEnum(), property[1]), property[0]);
            }
            else
                Security.StoreValue(control.IsChecked.ToString(), control.Tag);
        }

        private void InitializeSettings()
        {
            foreach (FrameworkElement item in Controls)
            {
                if (item.Tag.ToString().Contains('|')) continue;
                switch (item)
                {
                    case TextBlock box:
                        string value = Security.RetrieveValue(item.Tag);
                        box.Text = string.IsNullOrEmpty(value) ? item.GetPlaceholder() : value;
                        break;
                    case CheckBox box:
                        box.IsChecked = Security.RetrieveBool(item.Tag);
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
    }
}
