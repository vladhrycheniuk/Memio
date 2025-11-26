namespace Memio.View
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void OnColorClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is string colorHex)
            {
                // Зберігаємо вибір у налаштуваннях пристрою
                Preferences.Set("BoardColor", colorHex);
                DisplayAlert("Theme", "Background updated! Restart or switch tabs to see changes.", "OK");
            }
        }
    }
}