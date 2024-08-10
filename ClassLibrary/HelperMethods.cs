using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using ClassLibrary;

namespace ClassLibrary
{
    public static class HelperMethods
    {
        public static string FilePath { get; private set; }

        /// <summary>
        /// Считывает данные из файла JSON.
        /// </summary>
        /// <param name="inputFPath">Путь к файлу JSON.</param>
        /// <returns>Список виджетов.</returns>
        public static List<Widget> ReadJson(string inputFPath)
        {
            List<Widget> widgets = new List<Widget>();

            string jsonData = File.ReadAllText(inputFPath);

            widgets = JsonSerializer.Deserialize<List<Widget>>(jsonData);

            return widgets;
        }

        /// <summary>
        /// Основное меню выбора действий.
        /// </summary>
        /// <param name="widgets">Список виджетов.</param>
        /// <param name="startChoice">Начальный выбор.</param>
        public static void MenuSelection(ref List<Widget> widgets, ref int startChoice)
        {
            int choice = HelperMethods.Choice();
            if (choice == 1)
            {
                HelperMethods.FirstMenuChoice(widgets);
                startChoice = HelperMethods.GetContinueOption();
            }
            if (choice == 2)
            {
                HelperMethods.SecondMenuChoice(widgets);
                startChoice = HelperMethods.GetContinueOption();
            }
        }

        /// <summary>
        /// Загружает данные из файла или выполняет другие действия с данными.
        /// </summary>
        /// <returns>Список виджетов.</returns>
        public static List<Widget> LoadData()
        {
            List<Widget> widgets = new List<Widget>();
            try
            {
                Console.Write("Введите путь к файлу:");
                string filePath = Console.ReadLine();

                while (!(File.Exists(filePath) && filePath.Length >= 5 && filePath.Substring(filePath.Length - 5, 5) == ".json"))
                {
                    Console.Write("Введен некорректный путь, повторите ввод: ");
                    filePath = Console.ReadLine();
                }

                FilePath = filePath;
                widgets = ReadJson(filePath);

                if (widgets.LongCount() != 0)
                {
                    return widgets;
                }
                else
                {
                    Console.WriteLine("Файл некорректен");
                    return null;
                }
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Произошла ошибка при открытии файла");
                throw new ArgumentNullException();
            }
            catch (System.Text.Json.JsonException)
            {
                Console.WriteLine("Файл некорректен");
                throw new System.Text.Json.JsonException();
            }
            catch (Exception)
            {
                Console.WriteLine("Произошла ошибка при открытии файла");
                throw new Exception();
            }
        }

        /// <summary>
        /// Получает выбор пользователя для основного меню.
        /// </summary>
        /// <returns>Выбор пользователя.</returns>
        public static int Choice()
        {
            Console.WriteLine("\nМеню:\n1. Отсортировать по полю\n2. Изменить виджет\n");

            int choice;
            bool validChoice = false;
            do
            {
                Console.Write("Введите номер действия: ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out choice) && choice >= 1 && choice <= 2)
                {
                    validChoice = true;
                }
                else
                {
                    Console.WriteLine("Введите 1 или 2.");
                }
            } while (!validChoice);



            return choice;
        }

        /// <summary>
        /// Получает выбор пользователя для поля, по которому нужно отсортировать виджеты.
        /// </summary>
        /// <returns>Номер выбранного поля для сортировки.</returns>
        public static int SortChoice()
        {
            Console.WriteLine("\nПоля, по которым можно отсортировать:\n" +
                              "1. widgetId\n" +
                              "2. name\n" +
                              "3. quantity\n" +
                              "4. price\n" +
                              "5. isAvailable\n" +
                              "6. manufactureDate\n");
            int choice;
            Console.Write("Введите номер поля: ");
            string input = Console.ReadLine();
            while (!int.TryParse(input, out choice) || choice < 1 && choice > 6)
            {
                Console.WriteLine("Некорректный ввод. Введите число от 1 до 6:");
                input = Console.ReadLine();
            }
            return choice;
        }

        /// <summary>
        /// Получает выбор пользователя для дальнейших действий после обработки данных.
        /// </summary>
        /// <returns>Выбор пользователя.</returns>
        public static int GetContinueOption()
        {
            Console.WriteLine("\nМеню:\n" +
                              "1. Продолжить с этим файлом\n" +
                              "2. Передать новый файл\n" +
                              "3. Завершить программу\n");
            Console.Write("Введите номер действия: ");
            string input = Console.ReadLine();
            int choice;
            while (!int.TryParse(input, out choice) || choice < 1 || choice > 3)
            {
                Console.WriteLine("Некорректный ввод. Введите 1, 2 или 3:");
                input = Console.ReadLine();
            }
            return choice;
        }

        /// <summary>
        /// Получает выбор пользователя для редактирования виджета.
        /// </summary>
        /// <param name="widgets">Список виджетов для редактирования.</param>
        /// <returns>Массив, содержащий выбранные пользователем номер виджета и номер поля.</returns>
        public static int[] EditChoice(List<Widget> widgets)
        {

            int cnt = 1;
            foreach (Widget widget in widgets)
            {
                Console.WriteLine($"{cnt}. {widget.WidgetId}");
                cnt++;
            }
            Console.Write("Введите номер виджета, который надо изменить: ");
            string firstInput = Console.ReadLine();
            int firstChoice;
            while (!int.TryParse(firstInput, out firstChoice) || firstChoice < 1 || firstChoice > widgets.LongCount())
            {
                Console.WriteLine($"Некорректный номер, введите число от 1 до {widgets.LongCount()}:");
                firstInput = Console.ReadLine();
            }
            firstChoice--;
            PrintData(widgets[firstChoice]);
            Console.WriteLine("Поля, которые можно изменить:\n" +
                              "1. name\n" +
                              "2. quantity\n" +
                              "3. isAvailable\n" +
                              "4. manufactureDate");


            for (int i = 0; i < widgets[firstChoice].Specifications.LongCount(); i++)
            {
                Console.WriteLine($"{5 + i}. specPrice в спецификации {widgets[firstChoice].Specifications[i].SpecName}");
            }

            Console.Write("Введите номер поля: ");
            string secondInput = Console.ReadLine();
            int secondChoice;
            while (!int.TryParse(secondInput, out secondChoice) || secondChoice < 1 || secondChoice > 4 + widgets[firstChoice].Specifications.LongCount())
            {
                Console.WriteLine($"Некорректный номер, введите число от 1 до {4 + widgets[firstChoice].Specifications.LongCount()}:");
                secondInput = Console.ReadLine();
            }

            int[] nums = {firstChoice, secondChoice};
            return nums;
        }

        /// <summary>
        /// Обрабатывает выбор пользователя для сортировки виджетов и выводит результат.
        /// </summary>
        /// <param name="widgets">Список виджетов для сортировки.</param>
        public static void FirstMenuChoice(List<Widget> widgets)
        {
            int choiceSort = HelperMethods.SortChoice();
            List<Widget> oldWidgets = new List<Widget>();
            oldWidgets = HelperMethods.SortWidgets(widgets, choiceSort);
            PrintData(oldWidgets);
            HelperMethods.HandleRewriteChoice(oldWidgets);
        }

        /// <summary>
        /// Обрабатывает выбор пользователя для редактирования виджета и выводит результат.
        /// </summary>
        /// <param name="widgets">Список виджетов для редактирования.</param>
        public static void SecondMenuChoice(List<Widget> widgets)
        {
            int[] choiceEdit = HelperMethods.EditChoice(widgets);
            List<Widget> oldWidgets = new List<Widget>();
            oldWidgets = HelperMethods.EditWidget(widgets, choiceEdit[0], choiceEdit[1]);
            PrintData(oldWidgets);
        }

        /// <summary>
        /// Обрабатывает выбор пользователя относительно перезаписи данных в файле.
        /// </summary>
        /// <param name="widgets">Список виджетов для возможной перезаписи.</param>
        public static void HandleRewriteChoice(List<Widget> widgets)
        {
            string choice = "";
            while (choice != "да" && choice != "нет")
            {
                Console.WriteLine("\nПерезаписать отсортированные данные в исходный файл? Введите да или нет: ");
                choice = Console.ReadLine().Trim().ToLower();
            }

            if (choice == "да")
            {
                var json = JsonSerializer.Serialize(widgets);
                File.WriteAllText(FilePath, json);
                Console.WriteLine($"\nИзменения сохранены в \"{FilePath}\"");
            }
        }

        /// <summary>
        /// Редактирует выбранный виджет в списке в соответствии с выбранным полем.
        /// </summary>
        /// <param name="widgets">Список виджетов для редактирования.</param>
        /// <param name="numWidget">Номер виджета в списке для редактирования.</param>
        /// <param name="numField">Номер поля для редактирования.</param>
        /// <returns>Список виджетов с отредактированными данными.</returns>
        public static List<Widget> EditWidget(List<Widget> widgets, int numWidget, int numField)
        {
            List<Widget> editedData = widgets;

            if (numField == 1)
            {
                InputNewName(ref editedData, numWidget);
            }
            else if (numField == 2)
            {
                InputNewQuantity(ref editedData, numWidget);
            }
            else if (numField == 3)
            {
                InputNewIsAvailable(ref editedData, numWidget);
            }
            else if (numField == 4)
            {
                InputNewManufactureDate(ref editedData, numWidget);
            }
            else if (numField >= 4)
            {
                InputNewSpecPrice(ref editedData, numWidget, numField - 5);
            }
            else
            {
                return null;
            }

            return editedData;
        }

        /// <summary>
        /// Получает новое значение для имени виджета от пользователя и обновляет его в списке.
        /// </summary>
        /// <param name="widgets">Список виджетов для обновления.</param>
        /// <param name="numWidget">Номер виджета в списке для обновления.</param>
        public static void InputNewName(ref List<Widget> widgets, int numWidget)
        {
            Console.WriteLine("Введите значение для имени:");
            string newValue = Console.ReadLine();
            while (!(newValue.Length != 0) || newValue == null)
            {
                Console.WriteLine("Введено некорректное значение. Введите значение для имени:");
                newValue = Console.ReadLine();
            }
            widgets[numWidget].UpdateName(newValue);
            Console.WriteLine("Значение обновлено");
        }

        /// <summary>
        /// Получает новое значение для количества виджета от пользователя и обновляет его в списке.
        /// </summary>
        /// <param name="widgets">Список виджетов для обновления.</param>
        /// <param name="numWidget">Номер виджета в списке для обновления.</param>
        public static void InputNewQuantity(ref List<Widget> widgets, int numWidget)
        {
            Console.WriteLine("Введите значение для количества:");
            string newValue = Console.ReadLine();
            int newQuantity;
            while (!int.TryParse(newValue, out newQuantity) || newQuantity < 0)
            {
                Console.WriteLine("Введено некорректное значение. Введите значение для количества:");
                newValue = Console.ReadLine();
            }
            widgets[numWidget].UpdateQuantity(newQuantity);
            Console.WriteLine("Значение обновлено");
        }

        /// <summary>
        /// Получает новое значение для доступности виджета от пользователя и обновляет его в списке.
        /// </summary>
        /// <param name="widgets">Список виджетов для обновления.</param>
        /// <param name="numWidget">Номер виджета в списке для обновления.</param>
        public static void InputNewIsAvailable(ref List<Widget> widgets, int numWidget)
        {
            Console.WriteLine("Введите новое значение для доступности true или false:");
            string newValue = Console.ReadLine();
            bool newAvailability;
            while (!bool.TryParse(newValue, out newAvailability))
            {
                Console.WriteLine("Введено некорректное значение для доступности. Введите новое значение для доступности true или false:");
                newValue = Console.ReadLine();
            }
            widgets[numWidget].UpdateIsAvailable(newAvailability);
            Console.WriteLine("Значение обновлено");
        }

        /// <summary>
        /// Получает новую дату производства виджета от пользователя и обновляет ее в списке.
        /// </summary>
        /// <param name="widgets">Список виджетов для обновления.</param>
        /// <param name="numWidget">Номер виджета в списке для обновления.</param>
        public static void InputNewManufactureDate(ref List<Widget> widgets, int numWidget)
        {
            Console.WriteLine("Введите новую дату производства в формате ДД.ММ.ГГГГ:");
            string newValue = Console.ReadLine();
            DateTime newManufactureDate;
            while (!DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out newManufactureDate))
            {
                Console.WriteLine("Введено некорректное значение. Введите новую дату производства в формате ДД.ММ.ГГГГ:");
                newValue = Console.ReadLine();
            }
            widgets[numWidget].UpdateManufactureDate(newManufactureDate);
            Console.WriteLine("Значение обновлено");
        }

        /// <summary>
        /// Получает новое значение цены для определенной спецификации виджета от пользователя и обновляет его в списке.
        /// </summary>
        /// <param name="oldWidgets">Список виджетов для обновления.</param>
        /// <param name="numWidget">Номер виджета в списке для обновления.</param>
        /// <param name="numSpec">Номер спецификации для обновления цены.</param>
        public static void InputNewSpecPrice(ref List<Widget> oldWidgets, int numWidget, int numSpec)
        {
            Console.WriteLine("Введите новое значение цены:");
            string newValue = Console.ReadLine();
            double newPrice;
            while (!double.TryParse(newValue, out newPrice) || newPrice < 0)
            {
                Console.WriteLine("Введено некорректное значение. Введите новое значение цены:");
                newValue = Console.ReadLine();
            }
            oldWidgets[numWidget].Specifications[numSpec].ChangePrice(newPrice);
            Console.WriteLine("Значение обновлено");
        }

        /// <summary>
        /// Выводит сообщение об изменении цены виджета.
        /// </summary>
        /// <param name="widgetId">Идентификатор виджета.</param>
        /// <param name="oldPrice">Старая цена виджета.</param>
        /// <param name="currentPrice">Текущая цена виджета.</param>
        public static void PrintDiff(string widgetId, double oldPrice, double currentPrice)
        {
            Console.WriteLine($"Цена виджета \"{widgetId}\" изменилась с {oldPrice} на {currentPrice}");
        }

        /// <summary>
        /// Сортирует список виджетов в соответствии с выбранным полем.
        /// </summary>
        /// <param name="widgetList">Список виджетов для сортировки.</param>
        /// <param name="num">Номер поля для сортировки.</param>
        /// <returns>Отсортированный список виджетов.</returns>
        public static List<Widget> SortWidgets(List<Widget> widgetList, int num)
        {
            switch (num)
            {
                case 1:
                    return widgetList.OrderBy(x => x.WidgetId).ToList();
                case 2:
                    return widgetList.OrderBy(x => x.Name).ToList();
                case 3:
                    return widgetList.OrderBy(x => x.Quantity).ToList();
                case 4:
                    return widgetList.OrderBy(x => x.Price).ToList();
                case 5:
                    return widgetList.OrderBy(x => x.IsAvailable).ToList();
                case 6:
                    return widgetList.OrderBy(x => x.ManufactureDate).ToList();
                default:
                    throw new ArgumentException("Неверный номер для сортировки");
            }
        }

        /// <summary>
        /// Выводит данные о списке виджетов в консоль в виде табличного формата.
        /// </summary>
        /// <param name="widgetList">Список виджетов для вывода.</param>
        public static void PrintData(List<Widget> widgetList)
        {
            Console.WriteLine();
            if (widgetList.Count != 0)
            {
                Console.WriteLine($"{"Widget ID",-36}|{"Name",-20}|{"Quantity",-10}|{"Price",-15}|{"Is Available",-15}|{"Manufacture Date",-25}");
                Console.WriteLine(new string('-', 128));

                foreach (var widget in widgetList)
                {
                    Console.WriteLine($"{widget.WidgetId,-36}|{widget.Name,-20}|{widget.Quantity,-10}|{widget.Price,-15}|{widget.IsAvailable,-15}|{widget.ManufactureDate,-25}");
                    Console.WriteLine(new string('-', 128));
                    Console.WriteLine("Specifications: ");
                    Console.WriteLine($"{"SpecName",-36}|{"SpecPrice",-20}|{"IsCustom",-15}");
                    Console.WriteLine(new string('-', 71));

                    if (widget.Specifications != null && widget.Specifications.Any())
                    {
                        foreach (var specification in widget.Specifications)
                        {
                            Console.WriteLine($"{specification.SpecName,-36}|{specification.SpecPrice,-20}|{specification.IsCustom,-15}");
                        }

                    }
                    else
                    {
                        Console.WriteLine($"{string.Empty,-36}|{string.Empty,-20}|{string.Empty,-10}{"Нет доступных specifications",-71}");
                    }

                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("Выборка пуста");
            }
        }

        /// <summary>
        /// Выводит данные о виджете в консоль.
        /// </summary>
        /// <param name="widget">Виджет для вывода данных.</param>
        public static void PrintData(Widget widget)
        {
            Console.WriteLine();
            Console.WriteLine($"{"Widget ID",-36}|{"Name",-20}|{"Quantity",-10}|{"Price",-15}|{"Is Available",-15}|{"Manufacture Date",-25}");
            Console.WriteLine(new string('-', 128));
            Console.WriteLine($"{widget.WidgetId,-36}|{widget.Name,-20}|{widget.Quantity,-10}|{widget.Price,-15}|{widget.IsAvailable,-15}|{widget.ManufactureDate,-25}");

            Console.WriteLine(new string('-', 128));
            Console.WriteLine("Specifications: ");
            Console.WriteLine($"{"SpecName",-36}|{"SpecPrice",-20}|{"IsCustom",-15}");
            Console.WriteLine(new string('-', 71));

            if (widget.Specifications != null && widget.Specifications.Any())
            {
                foreach (var specification in widget.Specifications)
                {
                    Console.WriteLine($"{specification.SpecName,-36}|{specification.SpecPrice,-20}|{specification.IsCustom,-15}");
                }
            }
            else
            {
                Console.WriteLine($"{string.Empty,-36}|{string.Empty,-20}|{string.Empty,-10}{"Нет доступных specifications",-71}");
            }

            Console.WriteLine();
        }
    }
}