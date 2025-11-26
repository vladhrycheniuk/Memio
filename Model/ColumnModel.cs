using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Memio.Model
{
    public partial class ColumnModel : ObservableObject
    {
        [ObservableProperty] string title = string.Empty;
        [ObservableProperty] string id = string.Empty;
        public ObservableCollection<TaskItem> Tasks { get; set; } = new();
    }
}