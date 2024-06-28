namespace EU4_PCP.Models;

public class Notifiable : ViewModelBase
{
    private string filterText = "";
    public string FilterText
    {
        get => filterText;
        set => Set(ref filterText, value);
    }

    private bool workDirsExpanded;
    public bool WorkDirsExpanded
    {
        get => workDirsExpanded;
        set => Set(ref workDirsExpanded, value);
    }

    private bool provTableExpanded;
    public bool ProvTableExpanded
    {
        get => provTableExpanded;
        set => Set(ref provTableExpanded, value);
    }

    private bool colorPickerExpanded;
    public bool ColorPickerExpanded
    {
        get => colorPickerExpanded;
        set => Set(ref colorPickerExpanded, value);
    }

    private bool personalizationExpanded;
    public bool PersonalizationExpanded
    {
        get => personalizationExpanded;
        set => Set(ref personalizationExpanded, value);
    }

    private bool aboutExpanded;
    public bool AboutExpanded
    {
        get => aboutExpanded;
        set => Set(ref aboutExpanded, value);
    }

    private AppTheme theme;
    public AppTheme Theme
    {
        get => theme;
        set => Set(ref theme, value);
    }

    private string versionDescription;
    public string VersionDescription
    {
        get => versionDescription;
        set => Set(ref versionDescription, value);
    }
}
