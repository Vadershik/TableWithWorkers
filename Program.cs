using System;
using System.Text;
using System.Linq;
using System.IO;

public class Logger {
    // Если файл Log.log не существует,то при вызове функции он сам создаётся
    // Функция для записи лога в файл
    public void LogAction(string Action) {
        using(StreamWriter sw = new StreamWriter("Log.log", true)) {
            sw.WriteLine($"{DateTime.Now.ToString()} - {Action}");
        }
    }
}

public class Program {
    public static Logger log = new Logger();
    public static string dbFile = "database.txt"; //DataBase file
    public static string logFile = "Log.log"; //Log file
    public static void Main(string[] args) {
        ShowMenu();
    }

    public static void ShowMenu() {
        /* Функция для вывода меню программы.
         * Является входной точкой всей программы для дальнейшей её работы.
         */
        Console.WriteLine("\n\n");
        Console.WriteLine("1 - Просмотр таблицы");
        Console.WriteLine("2 - Добавить запись");
        Console.WriteLine("3 - Удалить запись");
        Console.WriteLine("4 - Обновить запись");
        Console.WriteLine("5 - Поиск записей");
        Console.WriteLine("6 - Просмотреть лог");
        Console.WriteLine("7 - Выход");
        Console.Write("Выберите действие: ");
        int Action = Convert.ToInt32(Console.ReadLine());
        switch(Action) {
            case 1:
                ShowTable();
                break;
            case 2:
                AddToTable();
                break;
            case 3:
                DelFromTable();
                break;
            case 4:
                UpdateTable();
                break;
            case 5:
                SearchInTable();
                break;
            case 6:
                WatchLogs();
                break;
            default:
                break;
        }
    }
    public static void ShowTable() {
        /* Функция для вывода текущей таблицы.
         * Выводит значения из файла с данными.
         */
        Console.WriteLine("\nОтдел кадров");
        Console.WriteLine("Фамилия     Должность  Год рожд    Оклад(грн)");
        using(StreamReader sr = File.OpenText(dbFile)) {
            string s = String.Empty;
            while((s=sr.ReadLine()) != null) {
                string[] str = s.Split(" ");
                Console.WriteLine($"{str[0],-11} {str[1],-10} {str[2],-11} {str[3],-4}");
            }
        }
        log.LogAction("Показ таблицы");
        ShowMenu();
    }
    public static void AddToTable() {
        /* Функция для добавления записи в таблицу.
         * Перезаписывает таблицу в файле с новыми данными.
         */
        string[] note = new string[4];
        Console.Write("Введите фамилию работника: ");
        note[0] = Console.ReadLine();
        Console.Write("Введите должность работника: ");
        note[1] = Console.ReadLine();
        Console.Write("Введите год рождения работника: ");
        note[2] = Console.ReadLine();
        Console.Write("Введите оклад(грн) работника: ");
        note[3] = Console.ReadLine();
        using(StreamWriter sw = new StreamWriter(dbFile, true)) {
            sw.WriteLine($"{note[0]} {note[1]} {note[2]} {note[3]}");
        }
        log.LogAction($"Добавлена запись \"{note[0]}\"");
        ShowMenu();
    }
    public static void DelFromTable() {
        /* Функция для удаления записи из таблицы.
         * Перезаписывает таблицу в файле без n-ой записи.
         */
        string[] lines = File.ReadLines(dbFile).ToArray();
        int linetodelete = 0;
        for(int i = 0; i < lines.Length; i++) {
            Console.WriteLine($"{i} - {lines[i].Split(" ")[0]}");
        }
        Console.Write("Напишите строку: ");
        linetodelete = Convert.ToInt32(Console.ReadLine());

        log.LogAction($"Удалена запись \"{lines[linetodelete].Split(" ")[0]}\"");

        lines = lines.Where(val => val != lines[linetodelete]).ToArray();
        File.WriteAllLines(dbFile, lines);
        ShowMenu();
    }
    public static void UpdateTable() {
        /* Функция для перезаписи записи в таблице.
         * Перезаписывает запись в таблице и сохраняет таблицу.
         */
        string[] lines = File.ReadLines(dbFile).ToArray();
        string[] note = new string[4];
        string param;

        //Выводим каждую строку с её индексом в массиве
        for(int i = 0; i < lines.Length; i++) Console.WriteLine($"{i} - {lines[i].Split(" ")[0]}");
        
        Console.WriteLine("Введите номер записи для редактирования:");
        Console.Write(">>> ");
        int n = Convert.ToInt32(Console.ReadLine());
        
        //Не забываем про логи
        log.LogAction($"Обновление записи \"{lines[n].Split(" ")[0]}\"");

        //Перезаписываем данные
        //Если поле остаётся пустым,то присваиваем полю значение из оригинальной строки
        Console.Write("Обновите фамилию работника(либо оставьте пустым): ");
        param = Console.ReadLine();
        note[0] = (param=="") ? lines[n].Split(" ")[0] : param;
        Console.Write("Обновите должность работника(либо оставьте пустым): ");
        param = Console.ReadLine();
        note[1] = (param=="") ? lines[n].Split(" ")[1] : param;
        Console.Write("Обновите год рождения работника(либо оставьте пустым): ");
        param = Console.ReadLine();
        note[2] = (param=="") ? lines[n].Split(" ")[2] : param;
        Console.Write("Обновите оклад(грн) работника(либо оставьте пустым): ");
        param = Console.ReadLine();
        note[3] = (param=="") ? lines[n].Split(" ")[3] : param;

        lines[n]=String.Join(' ',note);

        //Перезаписываем файл database.txt
        //Без передачи аргумента true файл перезаписывается
        using(StreamWriter sw = new StreamWriter(dbFile)) {
            foreach(var s in lines) sw.WriteLine(s);
        }
        ShowMenu();
    }
    public static void SearchInTable() {
        /* Функция для поиска записи в таблице.
         * Предположительно работает через какой-то фильтр.
         */
        //Читаем все данные с database.txt
        string[] lines = File.ReadLines(dbFile).ToArray();

        //Запрашиваем данные
        Console.WriteLine("Введите 1 из предложенных фильтров:");
        Console.WriteLine("1 - Имя");
        Console.WriteLine("2 - Должность");
        Console.WriteLine("3 - Год рождения");
        Console.WriteLine("4 - Оклад");
        Console.Write(">>> ");
        int n = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Введите данные:");
        Console.Write(">>> ");
        string s = Console.ReadLine();
        
        //Фильтруем массив по данным,которые были переданы
        //Т.к. нужно искать похожие,то делаем строки с маленькими символами
        lines = lines.Where(x => x.ToLower().Split(" ")[n-1].Contains(s.ToLower())).ToArray();
        //Выводим все найденные данные, если они найдены
        if (lines.Length==0) {
            Console.WriteLine("Ничего не найдено");
        }
        else {
            Console.WriteLine("Фамилия     Должность  Год рожд    Оклад(грн)");
            foreach(var x in lines) {
                string[] str = x.Split(" ");
                Console.WriteLine($"{str[0],-11} {str[1],-10} {str[2],-11} {str[3],-4}");
            }
        }
        //Добавляем лог.
        log.LogAction($"Поиск \"{s}\"");
        ShowMenu();
    }
    public static void WatchLogs() {
        /* Функция для просмотра действий в программе.
         * Читает файл со всеми совершёнными действиями в программе.
         */
        //Читаем файл с логами и выводим данные из него.
        using(StreamReader sr = File.OpenText(logFile)) {
            string s = String.Empty;
            while((s=sr.ReadLine()) != null) {
                Console.WriteLine(s);
            }
        }
        //Добавляем лог этому действию.
        log.LogAction("Просмотр логов");
        ShowMenu();
    }
}
