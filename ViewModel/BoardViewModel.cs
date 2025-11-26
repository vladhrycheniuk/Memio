using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Memio.Model;
using Memio.Services;
using Memio.View; 
using System.Collections.ObjectModel;

namespace Memio.ViewModel
{
    public partial class BoardViewModel : ObservableObject
    {
        private readonly BoardService _boardService;

        [ObservableProperty]
        private ObservableCollection<ColumnModel> columns = new();

        [ObservableProperty]
        private string currentBoardName = "Main"; 

        [ObservableProperty]
        private Color boardBackground = Colors.White;

        private TaskItem? _draggedItem;
        private ColumnModel? _sourceColumn;

        public BoardViewModel(BoardService boardService)
        {
            _boardService = boardService;
            LoadDataAsync(); // Завантажуємо одразу
        }

        public void ApplyTheme()
        {
            string hex = Preferences.Get("BoardColor", "#FDFBF7");
            BoardBackground = Color.FromArgb(hex);
        }

        public async Task LoadDataAsync()
        {
            CurrentBoardName = Preferences.Get("LastActiveBoard", "Main");
            ApplyTheme();
            
            var data = await _boardService.LoadBoardAsync(CurrentBoardName);
            
            // ФІКС: Якщо дошка нова і пуста — створюємо дефолтні колонки
            if (data == null || data.Count == 0)
            {
                data = new List<ColumnModel>
                {
                    new ColumnModel { Id = "todo", Title = "To Do" },
                    new ColumnModel { Id = "doing", Title = "In Progress" },
                    new ColumnModel { Id = "done", Title = "Done" }
                };
                // Одразу зберігаємо, щоб файл створився фізично
                await _boardService.SaveBoardAsync(data, CurrentBoardName);
            }

            Columns = new ObservableCollection<ColumnModel>(data);
        }

        // --- ЛОГІКА РЕДАГУВАННЯ ТА ІНШЕ ЗАЛИШАЄТЬСЯ БЕЗ ЗМІН ---
        [RelayCommand]
        private async Task OpenTaskDetails(TaskItem? task)
        {
            if (task == null) return;
            await Shell.Current.Navigation.PushAsync(new TaskDetailPage(task));
            await SaveAsync();
        }

        [RelayCommand]
        private async Task AddTask(ColumnModel? column)
        {
             if (column == null) return;
             string title = await Shell.Current.DisplayPromptAsync("New Task", "Enter title:");
             if (string.IsNullOrWhiteSpace(title)) return;
             
             string action = await Shell.Current.DisplayActionSheet("Select Priority Color", "Cancel", null, "High Priority (Red)", "Medium (Orange)", "Normal (Blue)", "Low (Green)");
            
             Color selectedColor = Color.FromArgb("#C7CEEA");
             if (action == "High Priority (Red)") selectedColor = Color.FromArgb("#FFB7B2");
             else if (action == "Medium (Orange)") selectedColor = Color.FromArgb("#FFDAC1");
             else if (action == "Low (Green)") selectedColor = Color.FromArgb("#B5EAD7");
             else if (action == "Cancel") return;

             column.Tasks.Add(new TaskItem { Title = title, TagColor = selectedColor, DueDate = DateTime.Now });
             await SaveAsync();
        }

        [RelayCommand]
        private async Task DeleteTask(TaskItem? task)
        {
            if (task == null) return;
            bool confirm = await Shell.Current.DisplayAlert("Delete", "Remove this task?", "Yes", "No");
            if (!confirm) return;

            foreach (var col in Columns) {
                if (col.Tasks.Contains(task)) { col.Tasks.Remove(task); break; }
            }
            await SaveAsync();
        }

        [RelayCommand]
        private void DragStarted(TaskItem? item)
        {
            if (item == null) return;
            _draggedItem = item;
            _sourceColumn = Columns.FirstOrDefault(c => c.Tasks.Contains(item));
        }

        [RelayCommand]
        private async Task DragOver(ColumnModel? targetColumn)
        {
             if (_draggedItem == null || targetColumn == null || _sourceColumn == null) return;
             if (_sourceColumn == targetColumn) return;
             if (targetColumn.Tasks.Contains(_draggedItem)) return;

             _sourceColumn.Tasks.Remove(_draggedItem);
             if (targetColumn.Id.ToLower() == "done") _draggedItem.IsDone = true;
             else _draggedItem.IsDone = false;
             targetColumn.Tasks.Add(_draggedItem);
             _sourceColumn = targetColumn;
             await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _boardService.SaveBoardAsync(Columns.ToList(), CurrentBoardName);
            WeakReferenceMessenger.Default.Send(new DataChangedMessage("Updated"));
        }
    }
}