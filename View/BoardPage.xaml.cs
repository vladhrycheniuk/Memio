using Memio.ViewModel; // <--- ЦЕЙ РЯДОК ОБОВ'ЯЗКОВИЙ
using Microsoft.Maui.Controls;

namespace Memio.View
{
    public partial class BoardPage : ContentPage
    {
        public BoardPage(BoardViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            // Примусово оновлюємо дані при вході на сторінку
            if (BindingContext is BoardViewModel vm)
            {
                await vm.LoadDataAsync();
            }
        }
    }
}