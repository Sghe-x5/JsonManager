using ClassLibrary;

public class Program
{
    static void Main()
    {
        List<Widget> widgets = new List<Widget>();  // Инициализация списка виджетов
        int choice = 2;
        do
        {
            try
            {
                if (choice == 2)
                {
                    widgets = HelperMethods.LoadData(); // Загрузка данных при выборе 2
                }

                if (widgets == null)
                {
                    Console.WriteLine("Ошибка при считывании данных из файла"); // Вывод ошибки, если данные не удалось считать
                    continue;
                }

                AutoSaver autoSaver = new AutoSaver(widgets, HelperMethods.FilePath); // Создание объекта AutoSaver для автосохранения

                HelperMethods.MenuSelection(ref widgets, ref choice); // Вызов метода для обработки выбора пользователя
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Произошла ошибка: переданное значение было null");
                continue;
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Произошла ошибка: переданный аргумент был недопустим");
                continue;
            }
            catch (System.Text.Json.JsonException)
            {
                continue;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла непредвиденная ошибка: {ex.Message}");
                continue;
            }
            // Обработка различных ошибок 

        } while (choice != 3);
    }
}
