using System.Text.Json;
using System.Xml.Serialization;

public class ImportService
{
    public List<Student> ImportStudents(string filePath)
    {
        var students = new List<Student>();
        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines)
        {
            var parts = line.Split(',');
            students.Add(new Student
            {
                LastName = parts[0].Trim(),
                FirstName = parts[1].Trim(),
                Grade = int.Parse(parts[2].Trim()),
                Classroom = int.Parse(parts[3].Trim()),
                Bus = int.Parse(parts[4].Trim())
            });
        }

        return students;
    }

    public List<Teacher> ImportTeachers(string filePath)
    {
        var teachers = new List<Teacher>();
        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines)
        {
            var parts = line.Split(',');
            teachers.Add(new Teacher
            {
                LastName = parts[0].Trim(),
                FirstName = parts[1].Trim(),
                Classroom = int.Parse(parts[2].Trim())
            });
        }

        return teachers;
    }
}
