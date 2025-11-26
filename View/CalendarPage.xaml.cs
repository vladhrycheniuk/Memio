using Memio.ViewModel;

namespace Memio.View
{
    public partial class CalendarPage : ContentPage
    {
        public CalendarPage(CalendarViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Оновити, коли сторінка стає видимою
            if (BindingContext is CalendarViewModel vm)
            {
                vm.LoadScheduleAsync();
            }
        }
    }
}