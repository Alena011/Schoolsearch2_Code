using System.Diagnostics;

public class Application
{
    private const string importFolder = "./files/import/";
    private const string exportFolder = "./files/export/";

    private ImportService importService = new ImportService();
    private ExportService exportService = new ExportService();
    private SearchService searchService = new SearchService();

    public void Start()
    {
        var students = ImportStudents();
        var teachers = ImportTeachers();

        byte choise;
        do
        {
            PrintMenu();
            choise = ReadMenuChoise();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            ActOnChoise(choise, students, teachers);

            stopwatch.Stop();
            Console.WriteLine($"Час виконання: {stopwatch.ElapsedMilliseconds} мілісекунд");
            Console.WriteLine("----------------------------------------------------------------------");
        } while (choise != 0);
    }

    private List<Student> ImportStudents()
    {
        var filePath = importFolder + "students.txt";

        Console.WriteLine("Імпортуємо студентів з файлу: " + filePath);

        var students = importService.ImportStudents(filePath);

        Console.WriteLine("Імпорт завершено успішно");
        Console.WriteLine("----------------------------------------------------------------------");

        return students;
    }

    private List<Teacher> ImportTeachers()
    {
        var filePath = importFolder + "teachers.txt";

        Console.WriteLine("Імпортуємо викладачів з файлу: " + filePath);

        var teachers = importService.ImportTeachers(filePath);

        Console.WriteLine("Імпорт завершено успішно");
        Console.WriteLine("----------------------------------------------------------------------");

        return teachers;
    }

    private void PrintMenu()
    {
        Console.WriteLine("МЕНЮ:");
        Console.WriteLine("-------------------------------- Пошуки ------------------------------");
        Console.WriteLine("1)  Враховуючи прізвище студента, знайдіть клас студента, та клас викладача.");
        Console.WriteLine("2)  Враховуючи прізвище студента, знайдіть автобусний маршрут, яким їде студент.");
        Console.WriteLine("3)  Дан викладач, знайдіть список його учнів.");
        Console.WriteLine("4)  Враховуючи автобусний маршрут, знайдіть усіх учнів, які ним користуються.");
        Console.WriteLine("5)  Знайдіть усіх учнів на певному рівні класу.");
        Console.WriteLine("6)  Враховуючи номер класу, перелічіть усіх учнів, призначених йому.");
        Console.WriteLine("7)  Задавши номер класу, знайдіть викладача (або викладачів), які викладають у ньому.");
        Console.WriteLine("8)  Отримавши оцінку, знайдіть усіх викладачів, які її викладають.");
        Console.WriteLine("--------------------------------- Інше -------------------------------");
        Console.WriteLine("9)  Скількі студентів?");
        Console.WriteLine("10) Експорт у JSON та XML.");
        Console.WriteLine("0)  Вийти.");
    }

    private byte ReadMenuChoise()
    {
        try
        {
            Console.WriteLine("Оберіть пункт меню: ");

            var input = Console.ReadLine();
            var result = Convert.ToByte(input);

            return result;
        }
        catch (FormatException e)
        {
            Console.WriteLine("Ошибка: введено некорректное значение.");
            throw e;
        }
        catch (OverflowException e)
        {
            Console.WriteLine("Ошибка: введено число вне диапазона значений byte (0-255).");
            throw e;
        };
    }

    private void ActOnChoise(byte choise, List<Student> students, List<Teacher> teachers)
    {
        switch (choise)
        {
            case 0:
                break;
            case 1:
                {
                    Console.WriteLine("Введіть прізвище студента:");
                    var studentLastName = Console.ReadLine();

                    var result = searchService.GetStudentClassAndTeacher(studentLastName, students, teachers);
                    if (result.Count > 0)
                    {
                        foreach (var (student, studentTeachers) in result)
                        {
                            Console.WriteLine($"Cтудент: {student.FirstName} {student.LastName}, Клас студента: {student.Grade}, Автобус: {student.Bus}, Клас: {student.Classroom}");

                            if (studentTeachers.Count > 0)
                            {
                                Console.WriteLine("Викладачі:");
                                foreach (var teacher in studentTeachers)
                                {
                                    Console.WriteLine($"Викладач: {teacher.FirstName} {teacher.LastName}, Клас: {teacher.Classroom}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Для цього студента викладачів не знайдено.");
                            }

                            Console.WriteLine("----------------------------------------------------------------------");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Студентів з таким прізвищем не знайдено.");
                    }
                    break;
                }
            case 2:
                {
                    Console.WriteLine("Введіть прізвище студента:");
                    var busLastName = Console.ReadLine();

                    var result = searchService.GetBusRouteByStudentLastName(busLastName, students);
                    if (result.Count > 0)
                    {
                        Console.WriteLine($"Автобусні маршрути: {string.Join(", ", result)}");
                    }
                    else
                    {
                        Console.WriteLine("Студентів з таким прізвищем не знайдено.");
                    }
                    break;
                }
            case 3:
                {
                    Console.WriteLine("Введіть прізвище викладача:");
                    var teacherLastName = Console.ReadLine();

                    Console.WriteLine("Введіть ім'я викладача:");
                    var teacherFirstName = Console.ReadLine();

                    var result = searchService.GetStudentsByTeacher(teacherLastName, teacherFirstName, students, teachers);
                    if (result.Count > 0)
                    {
                        foreach (var student in result)
                        {
                            Console.WriteLine($"Учень: {student.FirstName} {student.LastName}, Клас: {student.Grade}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Учнів у викладача не знайдено.");
                    }
                    break;
                }
            case 4:
                {
                    Console.WriteLine("Введіть номер автобусного маршруту:");
                    var busRoute = Convert.ToInt32(Console.ReadLine());
                    var result = searchService.GetStudentsByBusRoute(busRoute, students);
                    if (result.Count > 0)
                    {
                        foreach (var student in result)
                        {
                            Console.WriteLine($"Учень: {student.FirstName} {student.LastName}, Клас: {student.Grade}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Учнів на цьому маршруті не знайдено.");
                    }
                    break;
                }
            case 5:
                {
                    Console.WriteLine("Введіть рівень класу:");
                    var grade = Convert.ToInt32(Console.ReadLine());
                    var result = searchService.GetStudentsByGrade(grade, students);
                    if (result.Count > 0)
                    {
                        foreach (var student in result)
                        {
                            Console.WriteLine($"Учень: {student.FirstName} {student.LastName}, Автобус: {student.Bus}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Учнів у цьому класі не знайдено.");
                    }
                    break;
                }
            case 6:
                {
                    Console.WriteLine("Введіть номер класу:");
                    var classroom = Convert.ToInt32(Console.ReadLine());
                    var result = searchService.GetStudentsByClassroom(classroom, students);
                    if (result.Count > 0)
                    {
                        foreach (var student in result)
                        {
                            Console.WriteLine($"Учень: {student.FirstName} {student.LastName}, Клас: {student.Grade}, Автобус: {student.Bus}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Учнів у цьому класі не знайдено.");
                    }
                    break;
                }
            case 7:
                {
                    Console.WriteLine("Введіть номер класу:");
                    var classroom = Convert.ToInt32(Console.ReadLine());
                    var result = searchService.GetTeachersByClassroom(classroom, teachers);
                    if (result.Count > 0)
                    {
                        foreach (var teacher in result)
                        {
                            Console.WriteLine($"Викладач: {teacher.FirstName} {teacher.LastName}, Клас: {teacher.Classroom}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Викладачів у цьому класі не знайдено.");
                    }
                    break;
                }
            case 8:
                {
                    Console.WriteLine("Введіть рівень класу:");
                    var grade = Convert.ToInt32(Console.ReadLine());
                    var result = searchService.GetTeachersByGrade(grade, students, teachers);
                    if (result.Count > 0)
                    {
                        foreach (var teacher in result)
                        {
                            Console.WriteLine($"Викладач: {teacher.FirstName} {teacher.LastName}, Клас: {teacher.Classroom}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Викладачів на цьому рівні не знайдено.");
                    }
                    break;
                }
            case 9:
                {
                    var uniqueStudentCount = searchService.GetUniqueStudentCount(students);
                    Console.WriteLine($"Унікальних студентів: {uniqueStudentCount}");
                    break;
                }
            case 10:
                {
                    exportService.ExportToJson(students, exportFolder + "students.json");
                    exportService.ExportToXml(students, exportFolder + "students.xml");
                    exportService.ExportToJson(teachers, exportFolder + "teachers.json");
                    exportService.ExportToXml(teachers, exportFolder + "teachers.xml");
                    break;
                }
            default:
                {
                    Console.WriteLine("Такого вибору не існує!");
                    break;
                }
        }
        Console.WriteLine("----------------------------------------------------------------------");
    }
}