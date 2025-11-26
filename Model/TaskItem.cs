using CommunityToolkit.Mvvm.ComponentModel;

namespace Memio.Model
{
    public partial class TaskItem : ObservableObject
    {
        [ObservableProperty] string id = Guid.NewGuid().ToString();
        [ObservableProperty] string title = string.Empty;
        [ObservableProperty] string description = string.Empty;
        [ObservableProperty] string tag = "General";
        [ObservableProperty] Color tagColor = Colors.LightBlue; // Можна брати з ресурсів
        [ObservableProperty] DateTime dueDate = DateTime.Now;
        
        // Нова властивість для статистики
        [ObservableProperty] bool isDone; 
    }
}