// Constants/HotkeyList.cs
using System.Collections.Generic;

namespace FenixTestAutomation.Constants
{
    public class HotkeyTool
    {
        public string Modifier { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
    }

    public static class HotkeyList
    {
        public static List<HotkeyTool> Tools => new List<HotkeyTool>
        {
            new HotkeyTool { Modifier = "Alt", Key = "B", Name = "Указатель" },
            new HotkeyTool { Modifier = "Alt", Key = "W", Name = "Стена" },
            new HotkeyTool { Modifier = "Alt", Key = "U", Name = "Твердое тело" },
            new HotkeyTool { Modifier = "Alt", Key = "D", Name = "Дверь" },
            new HotkeyTool { Modifier = "Alt", Key = "I", Name = "Окно" },
            new HotkeyTool { Modifier = "Alt", Key = "O", Name = "Перекрытие" },
            new HotkeyTool { Modifier = "Alt", Key = "R", Name = "Лестница" },
            new HotkeyTool { Modifier = "Alt", Key = "E", Name = "Выход" },
            new HotkeyTool { Modifier = "Alt", Key = "P", Name = "Человек" },
            new HotkeyTool { Modifier = "Alt", Key = "C", Name = "Помещение" },
            new HotkeyTool { Modifier = "Alt", Key = "L", Name = "Линейка" },
            new HotkeyTool { Modifier = "Alt", Key = "G", Name = "Очаг пожара" },
            new HotkeyTool { Modifier = "Alt", Key = "H", Name = "Регистратор" },
            new HotkeyTool { Modifier = "Alt", Key = "J", Name = "Область расчета" },
            new HotkeyTool { Modifier = "Alt", Key = "Q", Name = "Линейный размер" }
        };
    }
}
