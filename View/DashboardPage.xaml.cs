using Memio.ViewModel; // <--- ЦЕ ОБОВ'ЯЗКОВО
using Microsoft.Maui.Controls;

namespace Memio.View
{
    public partial class DashboardPage : ContentPage
    {
        // Конструктор має називатися так само як клас (DashboardPage)
        public DashboardPage(DashboardViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}