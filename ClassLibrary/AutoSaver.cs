using System;
using System.IO;
using System.Text.Json;
using ClassLibrary;

namespace ClassLibrary
{
    public class AutoSaver
    {
        private readonly List<Widget> widgets;

        private DateTime PreviousTime;

        private readonly string initialFileName;

        /// <summary>
        /// Конструктор класса AutoSaver без параметров.
        /// </summary>
        public AutoSaver()
        {
            PreviousTime = DateTime.MinValue;
        }

        /// <summary>
        /// Конструктор класса AutoSaver с параметрами.
        /// </summary>
        /// <param name="widgets">Список виджетов, изменения в котором будут сохраняться.</param>
        /// <param name="initialFileName">Имя файла для сохранения.</param>
        public AutoSaver(List<Widget> widgets, string initialFileName)
        {
            this.widgets = widgets;
            this.initialFileName = initialFileName;
            PreviousTime = DateTime.Now;
            Subscribe();
        }

        /// <summary>
        /// Подписывает обработчик события изменения виджета на каждый виджет в списке.
        /// </summary>
        public void Subscribe()
        {
            foreach (var widget in widgets)
            {
                widget.Updated += HandleWidgetUpdated;
            }
        }

        /// <summary>
        /// Обработчик события изменения виджета.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события, содержащие информацию об изменении.</param>
        private void HandleWidgetUpdated(object sender, EventArgs e)
        {
            var updatedEventArgs = e as WidgetsUpdatedEventArgs;
            DateTime currentTime = updatedEventArgs.UpdateDateTime;
            if ((currentTime - PreviousTime).TotalSeconds <= 15)
            {
                SaveToJson();
            }
        }

        /// <summary>
        /// Сохраняет текущий список виджетов в JSON файл.
        /// </summary>
        private void SaveToJson()
        {
            var json = JsonSerializer.Serialize(widgets);
            var fileName = $"{initialFileName}_tmp.json";
            File.WriteAllText(fileName, json);
            Console.WriteLine($"\nИзменения сохранены в \"{fileName}\"");
        }
    }
}

