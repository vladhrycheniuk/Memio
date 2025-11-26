using System.Text.Json;
using Memio.Model;

namespace Memio.Services
{
    public class BoardService
    {
        // Повертає шлях до конкретної дошки
        private string GetPath(string boardName) => 
            Path.Combine(FileSystem.Current.AppDataDirectory, $"board_{boardName}.json");

        public async Task SaveBoardAsync(List<ColumnModel> columns, string boardName)
        {
            var json = JsonSerializer.Serialize(columns);
            await File.WriteAllTextAsync(GetPath(boardName), json);
        }

        public async Task<List<ColumnModel>> LoadBoardAsync(string boardName)
        {
            var path = GetPath(boardName);
            if (!File.Exists(path))
            {
                // Дефолтні колонки для нової дошки
                return new List<ColumnModel>
                {
                    new ColumnModel { Id = "todo", Title = "To Do" },
                    new ColumnModel { Id = "doing", Title = "In Progress" },
                    new ColumnModel { Id = "done", Title = "Done" }
                };
            }
            var json = await File.ReadAllTextAsync(path);
            return JsonSerializer.Deserialize<List<ColumnModel>>(json) ?? new List<ColumnModel>();
        }

        // Отримати список усіх створених дошок (зберігаємо їх імена окремо)
        public List<string> GetKnownBoards()
        {
            // Простий хак: шукаємо файли в папці
            var files = Directory.GetFiles(FileSystem.Current.AppDataDirectory, "board_*.json");
            var boards = files.Select(f => Path.GetFileNameWithoutExtension(f).Replace("board_", "")).ToList();
            
            if (!boards.Contains("Main")) boards.Add("Main"); // Завжди є головна
            return boards;
        }
    }
}