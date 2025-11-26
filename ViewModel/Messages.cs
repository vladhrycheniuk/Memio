using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Memio.ViewModel
{
    // Цей клас відповідає за повідомлення про зміни даних
    public class DataChangedMessage : ValueChangedMessage<string>
    {
        public DataChangedMessage(string value) : base(value)
        {
        }
    }
}