using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ClassLibrary
{
    public class SpecificationUpdatedEventArgs : EventArgs
    {
        public double priceDifference { get; }

        /// <summary>
        /// Конструктор класса SpecificationUpdatedEventArgs без параметров.
        /// </summary>
        public SpecificationUpdatedEventArgs() { }

        /// <summary>
        /// Конструктор класса SpecificationUpdatedEventArgs с параметром разницы в цене.
        /// </summary>
        /// <param name="priceDifference">Разница в цене.</param>
        public SpecificationUpdatedEventArgs(double priceDifference)
        {
            this.priceDifference = priceDifference;
        }
    }

    /// <summary>
    /// Класс, представляющий спецификацию виджета.
    /// </summary>
    public class Specification
    {
        public event EventHandler<EventArgs> UpdatePrice; // Событие обновления цены спецификации.

        /// <summary>
        /// Вызывает событие обновления цены спецификации.
        /// </summary>
        /// <param name="e">Аргументы события обновления цены.</param>
        public void OnUpdatePrice(EventArgs e)
        {
            UpdatePrice?.Invoke(this, e);
        }

        /// <summary>
        /// Обновляет цену спецификации и вызывает событие обновления цены.
        /// </summary>
        public void ChangePrice(double newPrice)
        {
            double oldPrice = SpecPrice;
            SpecPrice = newPrice;
            OnUpdatePrice(new SpecificationUpdatedEventArgs(newPrice - oldPrice));
        }

        [JsonPropertyName("specName")]
        public string SpecName { get; }

        [JsonPropertyName("specPrice")]
        public double SpecPrice { get; private set; }

        [JsonPropertyName("isCustom")]
        public bool IsCustom { get; }


        /// <summary>
        /// Конструктор класса Specification с параметрами.
        /// </summary>
        /// <param name="specName">Название спецификации.</param>
        /// <param name="specPrice">Цена спецификации.</param>
        /// <param name="isCustom">Пользовательская спецификация.</param>
        [JsonConstructor]
        public Specification(string specName, double specPrice, bool isCustom)
        {
            SpecName = specName;
            SpecPrice = specPrice;
            IsCustom = isCustom;
        }

        /// <summary>
        /// Конструктор класса Specification без параметров.
        /// </summary>
        public Specification()
        {
            SpecName = "";
            SpecPrice = 0.0;
            IsCustom = false;
        }

        /// <summary>
        /// Представляет объект спецификации в виде JSON строки.
        /// </summary>
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}

