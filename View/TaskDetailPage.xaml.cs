using Memio.Model;

namespace Memio.View
{
    public partial class TaskDetailPage : ContentPage
    {
        public TaskDetailPage(TaskItem task)
        {
            InitializeComponent();
            BindingContext = task; // Прив'язуємо конкретне завдання
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            // Просто повертаємось назад, дані оновляться через Binding
            await Navigation.PopAsync();
        }
    }
}