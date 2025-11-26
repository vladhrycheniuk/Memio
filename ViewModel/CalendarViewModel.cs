using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Memio.Services;
using Memio.Model;
using System.Collections.ObjectModel;

namespace Memio.ViewModel
{
    public class DateGroup : ObservableCollection<TaskItem>
    {
        public string DateHeader { get; private set; }
        public DateGroup(string dateHeader, List<TaskItem> items) : base(items)
        {
            DateHeader = dateHeader;
        }
    }

    public partial class CalendarViewModel : ObservableObject
    {
        private readonly BoardService _boardService;

        [ObservableProperty]
        private ObservableCollection<DateGroup> tasksByDate = new();

        public CalendarViewModel(BoardService boardService)
        {
            _boardService = boardService;
            LoadScheduleAsync();

            WeakReferenceMessenger.Default.Register<DataChangedMessage>(this, (r, m) => LoadScheduleAsync());
        }

        public async void LoadScheduleAsync()
        {
            // Беремо активну дошку
            string currentBoard = Preferences.Get("LastActiveBoard", "Main");

            var columns = await _boardService.LoadBoardAsync(currentBoard);
            var allTasks = columns.SelectMany(c => c.Tasks).OrderBy(t => t.DueDate).ToList();

            var groups = allTasks
                .GroupBy(t => t.DueDate.Date)
                .Select(g => new DateGroup(g.Key.ToString("MMMM dd, dddd"), g.ToList()))
                .ToList();

            TasksByDate = new ObservableCollection<DateGroup>(groups);
        }
    }
}