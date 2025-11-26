using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Memio.Model;
using Memio.Services;
using System.Collections.ObjectModel;

namespace Memio.ViewModel
{
    public partial class DashboardViewModel : ObservableObject
    {
        private readonly BoardService _boardService;

        [ObservableProperty] int totalTasks;
        [ObservableProperty] int doneTasks;
        [ObservableProperty] int pendingTasks;
        [ObservableProperty] ObservableCollection<TaskItem> todaysTasks = new();

        public DashboardViewModel(BoardService boardService)
        {
            _boardService = boardService;
            LoadStatsAsync();

            // Слухаємо зміни
            WeakReferenceMessenger.Default.Register<DataChangedMessage>(this, (r, m) =>
            {
                LoadStatsAsync();
            });
        }

        public async void LoadStatsAsync()
        {
            // Беремо назву останньої активної дошки, або "Main"
            string currentBoard = Preferences.Get("LastActiveBoard", "Main");

            var columns = await _boardService.LoadBoardAsync(currentBoard);
            
            // Важливо: перетворюємо в список, щоб працював .Count
            var allTasks = columns.SelectMany(c => c.Tasks).ToList();

            TotalTasks = allTasks.Count;
            
            // Рахуємо виконані (або IsDone == true, або знаходяться в колонці "Done")
            DoneTasks = allTasks.Count(t => t.IsDone || columns.FirstOrDefault(c => c.Tasks.Contains(t))?.Id.ToLower() == "done");
            
            PendingTasks = TotalTasks - DoneTasks;

            var today = DateTime.Today;
            TodaysTasks = new ObservableCollection<TaskItem>(
                allTasks.Where(t => t.DueDate.Date == today && !t.IsDone)
            );
        }
    }
}