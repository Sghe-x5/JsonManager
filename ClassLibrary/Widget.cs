using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using ClassLibrary;

namespace ClassLibrary
{
    public class WidgetsUpdatedEventArgs : EventArgs
    {
        public DateTime UpdateDateTime { get; }

        public WidgetsUpdatedEventArgs(DateTime updateDateTime)
        {
            UpdateDateTime = updateDateTime;
        }
    }

    /// <summary>
    /// Класс, представляющий виджет.
    /// </summary>
    public class Widget
    {
        [JsonPropertyName("widgetId")]
        public string WidgetId { get; private set; }

        [JsonPropertyName("name")]
        public string Name { get; private set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; private set; }

        [JsonPropertyName("price")]
        public double Price { get; private set; }

        [JsonPropertyName("isAvailable")]
        public bool IsAvailable { get; private set; }

        [JsonPropertyName("manufactureDate")]
        public DateTime ManufactureDate { get; private set; }

        [JsonPropertyName("specifications")]
        public List<Specification> Specifications { get; private set; }

        public event EventHandler<EventArgs> Updated;

        /// <summary>
        /// Конструктор класса Widget с параметрами.
        /// </summary>
        [JsonConstructor]
        public Widget(string widgetId, string name, int quantity, double price, bool isAvailable, DateTime manufactureDate, List<Specification> specifications)
        {
            WidgetId = widgetId;
            Name = name;
            Quantity = quantity;
            Price = price;
            IsAvailable = isAvailable;
            ManufactureDate = manufactureDate;
            Specifications = specifications;

            Subscribe();
        }

        /// <summary>
        /// Конструктор класса Widget без параметров.
        /// </summary>
        public Widget()
        {
            WidgetId = string.Empty;
            Name = string.Empty;
            Quantity = 0;
            Price = 0.0;
            IsAvailable = false;
            ManufactureDate = DateTime.MinValue;
            Specifications = new List<Specification>();
        }

        /// <summary>
        /// Подписывает обработчик события обновления цены на каждую спецификацию.
        /// </summary>
        private void Subscribe()
        {
            foreach (Specification spec in Specifications)
            {
                spec.UpdatePrice += HandleSpecPriceUpdated;
            }
        }

        /// <summary>
        /// Обработчик события обновления цены спецификации.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события, содержащие информацию об изменении цены.</param>
        private void HandleSpecPriceUpdated(object sender, EventArgs e)
        {
            SpecificationUpdatedEventArgs evArg = e as SpecificationUpdatedEventArgs;
            double oldPrice = Price;
            Price += evArg.priceDifference;
            HelperMethods.PrintDiff(WidgetId, oldPrice, Price);
            OnUpdated(new WidgetsUpdatedEventArgs(DateTime.Now));
        }

        /// <summary>
        /// Вызывает событие обновления виджета.
        /// </summary>
        /// <param name="e">Аргументы события обновления виджета.</param>
        public void OnUpdated(EventArgs e)
        {
            Updated?.Invoke(this, e);
        }

        /// <summary>
        /// Обновляет название виджета и вызывает событие обновления.
        /// </summary>
        public void UpdateName(string newValue)
        {
            Name = newValue;
            OnUpdated(new WidgetsUpdatedEventArgs(DateTime.Now));
        }

        /// <summary>
        /// Обновляет количество виджетов и вызывает событие обновления.
        /// </summary>
        public void UpdateQuantity(int newValue)
        {
            Quantity = newValue;
            OnUpdated(new WidgetsUpdatedEventArgs(DateTime.Now));
        }

        /// <summary>
        /// Обновляет доступность виджета и вызывает событие обновления.
        /// </summary>
        public void UpdateIsAvailable(bool newValue)
        {
            IsAvailable = newValue;
            OnUpdated(new WidgetsUpdatedEventArgs(DateTime.Now));
        }

        /// <summary>
        /// Обновляет дату производства виджета и вызывает событие обновления.
        /// </summary>
        public void UpdateManufactureDate(DateTime newValue)
        {
            ManufactureDate = newValue;
            OnUpdated(new WidgetsUpdatedEventArgs(DateTime.Now));
        }

        /// <summary>
        /// Представляет объект виджета в виде JSON строки.
        /// </summary>
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}