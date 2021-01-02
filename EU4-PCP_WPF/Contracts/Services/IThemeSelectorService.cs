using System;

using EU4_PCP_WPF.Models;

namespace EU4_PCP_WPF.Contracts.Services
{
    public interface IThemeSelectorService
    {
        void InitializeTheme();

        void SetTheme(AppTheme theme);

        AppTheme GetCurrentTheme();
    }
}
