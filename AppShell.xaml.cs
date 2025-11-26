using Memio.Services;
using Memio.View;

namespace Memio
{
    public partial class AppShell : Shell
    {
        private readonly BoardService _boardService;

        public AppShell()
        {
            InitializeComponent();
            _boardService = new BoardService();
            Routing.RegisterRoute(nameof(BoardPage), typeof(BoardPage));
            
            // Безпечний запуск завантаження меню
            try {
                LoadBoardsMenu();
            } catch { /* Ігноруємо помилки меню при старті */ }
        }

        private void LoadBoardsMenu()
        {
            try
            {
                var itemsToRemove = Items.Where(i => i.Title != null && i.Title.StartsWith("📂")).ToList();
                foreach (var item in itemsToRemove) Items.Remove(item);

                var boards = _boardService.GetKnownBoards();

                foreach (var board in boards)
                {
                    var flyoutItem = new FlyoutItem { Title = $"📂 {board}" };
                    var shellContent = new ShellContent
                    {
                        ContentTemplate = new DataTemplate(() => 
                        {
                            Preferences.Set("LastActiveBoard", board);
                            return IPlatformApplication.Current?.Services.GetService<BoardPage>();
                        })
                    };
                    flyoutItem.Items.Add(shellContent);
                    Items.Add(flyoutItem);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MENU ERROR: {ex.Message}");
            }
        }

        private async void OnCreateBoardClicked(object sender, EventArgs e)
        {
            string newName = await DisplayPromptAsync("New Board", "Enter board name:");
            if (!string.IsNullOrWhiteSpace(newName))
            {
                Preferences.Set("LastActiveBoard", newName);
                await Current.GoToAsync($"//{nameof(BoardPage)}");
                LoadBoardsMenu();
            }
        }
    }
}