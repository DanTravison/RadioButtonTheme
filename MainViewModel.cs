using RadioButtonTheme.Resources.Themes;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace RadioButtonTheme
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        AppTheme _theme = AppTheme.Unspecified;

        public MainViewModel()
        {
            Application.Current.RequestedThemeChanged += OnRequestedThemeChanged;
            SetTheme(AppTheme.Unspecified);
        }

        public AppTheme Theme
        {
            get => _theme;
            set
            {
                if (_theme != value)
                {
                    _theme = value;
                    SetTheme(value);
                    OnPropertyChanged(ThemeChangedEventArgs);
                }
            }
        }

        private void OnRequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            // If the user has specified 'system' apply the theme changed.
            if (Theme == AppTheme.Unspecified)
            {
                Log("RequestedThemeChanged Applied:{0}", e.RequestedTheme);
                SetTheme(e.RequestedTheme);
            }
            else // otherwise, ignore it.
            {
                Log("RequestedThemeChanged Ignored:{0}", e.RequestedTheme);
            }
        }

        static void SetTheme(AppTheme theme)
        {
            Log("SetTheme Request:{0}", theme);
            if (theme == AppTheme.Unspecified)
            {
                theme = Application.Current.UserAppTheme;
                Log("SetTheme System Theme:{0}", theme);
                if (theme == AppTheme.Unspecified)
                {
                    Log("SetTheme Overriding System Theme:{0}", theme);
                }
            }

            Log("SetTheme Applied:{0}", theme);
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add
            (
                theme == AppTheme.Light ? new LightTheme() : new DarkTheme()
            );
        }

        void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        static void Log(string format, params object[] args)
        {
            string message = string.Format
            (
                CultureInfo.InvariantCulture,
                format,
                args
            );
            Trace.WriteLine(message);
        }

        static readonly PropertyChangedEventArgs ThemeChangedEventArgs = new(nameof(Theme));

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
