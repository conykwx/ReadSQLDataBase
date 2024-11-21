using System;
using System.Data;
using Microsoft.Data.SqlClient;

class Program
{
    static string connectionString = "Data Source=DESKTOP-9K56BQI\\SQLEXPRESS;Initial Catalog=StudentsGrades;Integrated Security=True;TrustServerCertificate=True";

    static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1. Ввести данные для нового сотрудника");
            Console.WriteLine("2. Вывести все данные сотрудников");
            Console.WriteLine("3. Выход");
            Console.Write("Введите номер действия: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                InsertEmployeeData();
            }
            else if (choice == "2")
            {
                DisplayAllEmployees();
            }
            else if (choice == "3")
            {
                Console.WriteLine("Завершение работы...");
                break; // Завершаем цикл и выходим из программы
            }
            else
            {
                Console.WriteLine("Неверный ввод, попробуйте снова.");
            }
        }
    }

    // Метод для ввода данных сотрудника
    static void InsertEmployeeData()
    {
        Console.Clear();
        Console.WriteLine("Введение данных для нового сотрудника:");

        Console.Write("ФИО сотрудника: ");
        string fullName = Console.ReadLine();

        Console.Write("Должность: ");
        string position = Console.ReadLine();

        Console.Write("Зарплата: ");
        float salary;
        while (!float.TryParse(Console.ReadLine(), out salary))
        {
            Console.Write("Некорректная зарплата, попробуйте снова: ");
        }

        Console.Write("Дата приема на работу (формат: yyyy-MM-dd): ");
        DateTime hireDate;
        while (!DateTime.TryParse(Console.ReadLine(), out hireDate))
        {
            Console.Write("Некорректная дата, попробуйте снова: ");
        }

        // Вставляем данные в базу
        string insertQuery = "INSERT INTO Employees (FullName, Position, Salary, HireDate) " +
                             "VALUES (@FullName, @Position, @Salary, @HireDate)";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@FullName", fullName);
                    command.Parameters.AddWithValue("@Position", position);
                    command.Parameters.AddWithValue("@Salary", salary);
                    command.Parameters.AddWithValue("@HireDate", hireDate);

                    command.ExecuteNonQuery();
                    Console.WriteLine("Данные успешно добавлены!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при добавлении данных: " + ex.Message);
            }
        }

        Console.WriteLine("Нажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }

    // Метод для отображения всех сотрудников
    static void DisplayAllEmployees()
    {
        Console.Clear();
        string selectQuery = "SELECT * FROM Employees";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        Console.WriteLine("Список сотрудников:");
                        while (reader.Read())
                        {
                            Console.WriteLine($"ID: {reader["EmployeeID"]}, " +
                                              $"ФИО: {reader["FullName"]}, " +
                                              $"Должность: {reader["Position"]}, " +
                                              $"Зарплата: {reader["Salary"]}, " +
                                              $"Дата приема на работу: {reader["HireDate"]}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Нет данных в базе.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при выводе данных: " + ex.Message);
            }
        }

        Console.WriteLine("Нажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }
}
