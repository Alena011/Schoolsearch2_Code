public class SearchService
{
    public int GetUniqueStudentCount(List<Student> students)
    {
        return students
            .GroupBy(s => new { s.FirstName, s.LastName, s.Grade })
            .Select(group => group.First())
            .Count();
    }

    public List<(Student student, List<Teacher> teachers)> GetStudentClassAndTeacher(string lastName, List<Student> students, List<Teacher> teachers)
    {
        var matchingStudents = students
            .Where(s => s.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase))
            .ToList();

        var result = new List<(Student student, List<Teacher> teachers)>();

        foreach (var student in matchingStudents)
        {
            // Найти учителей, преподающих в классе, где учится студент
            var matchingTeachers = teachers
                .Where(t => t.Classroom == student.Classroom)
                .ToList();

            result.Add((student, matchingTeachers));
        }

        return result;
    }

    public List<int> GetBusRouteByStudentLastName(string lastName, List<Student> students)
    {
        return students
            .Where(s => s.LastName == lastName)
            .Select(s => s.Bus)
            .Distinct()
            .ToList();
    }

    public List<Student> GetStudentsByTeacher(string teacherLastName, string teacherFirstName, List<Student> students, List<Teacher> teachers)
    {
        var classrooms = teachers
            .Where(t => t.LastName == teacherLastName && t.FirstName == teacherFirstName)
            .Select(t => t.Classroom)
            .ToList();

        return students
            .Where(s => classrooms.Contains(s.Classroom))
            .ToList();
    }

    public List<Student> GetStudentsByBusRoute(int busRoute, List<Student> students)
    {
        return students
            .Where(s => s.Bus == busRoute)
            .ToList();
    }

    public List<Student> GetStudentsByGrade(int grade, List<Student> students)
    {
        return students
            .Where(s => s.Grade == grade)
            .ToList();
    }

    // Новый поиск всех учеников по номеру класса
    public List<Student> GetStudentsByClassroom(int classroom, List<Student> students)
    {
        return students
            .Where(s => s.Classroom == classroom)
            .ToList();
    }

    // Новый поиск учителей по номеру класса
    public List<Teacher> GetTeachersByClassroom(int classroom, List<Teacher> teachers)
    {
        return teachers
            .Where(t => t.Classroom == classroom)
            .ToList();
    }

    // Новый поиск учителей по оценке
    public List<Teacher> GetTeachersByGrade(int grade, List<Student> students, List<Teacher> teachers)
    {
        // Найти классы, в которых учатся студенты с заданной оценкой
        var classrooms = students
            .Where(s => s.Grade == grade)
            .Select(s => s.Classroom)
            .Distinct()
            .ToList();

        // Найти учителей, преподающих в этих классах
        return teachers
            .Where(t => classrooms.Contains(t.Classroom))
            .ToList();
    }
}
