using System;
using System.Collections.Generic;
using System.IO;

namespace StudentGradingSystem
{
    // Student class
    public class Student
    {
        public int Id { get; }
        public string FullName { get; }
        public int Score { get; }

        public Student(int id, string fullName, int score)
        {
            Id = id;
            FullName = fullName;
            Score = score;
        }

        public string GetGrade()
        {
            if (Score >= 80 && Score <= 100)
                return "A";
            else if (Score >= 70 && Score <= 79)
                return "B";
            else if (Score >= 60 && Score <= 69)
                return "C";
            else if (Score >= 50 && Score <= 59)
                return "D";
            else
                return "F";
        }

        public override string ToString()
        {
            return $"{FullName} (ID: {Id}): Score = {Score}, Grade = {GetGrade()}";
        }
    }

    // Custom Exception Classes
    public class InvalidScoreFormatException : Exception
    {
        public InvalidScoreFormatException(string message) : base(message) { }
    }

    public class MissingFieldException : Exception
    {
        public MissingFieldException(string message) : base(message) { }
    }

    // StudentResultProcessor class
    public class StudentResultProcessor
    {
        public List<Student> ReadStudentsFromFile(string inputFilePath)
        {
            var students = new List<Student>();

            using (var reader = new StreamReader(inputFilePath))
            {
                string line;
                int lineNumber = 0;

                while ((line = reader.ReadLine()) != null)
                {
                    lineNumber++;
                    
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    var fields = line.Split(',');

                    // Check for missing fields
                    if (fields.Length != 3)
                    {
                        throw new MissingFieldException($"Line {lineNumber}: Expected 3 fields but found {fields.Length}. Line content: '{line}'");
                    }

                    try
                    {
                        // Parse ID
                        if (!int.TryParse(fields[0].Trim(), out int id))
                        {
                            throw new InvalidScoreFormatException($"Line {lineNumber}: Invalid ID format '{fields[0].Trim()}'");
                        }

                        // Get name
                        string fullName = fields[1].Trim();
                        if (string.IsNullOrEmpty(fullName))
                        {
                            throw new MissingFieldException($"Line {lineNumber}: Student name is missing or empty");
                        }

                        // Parse score
                        if (!int.TryParse(fields[2].Trim(), out int score))
                        {
                            throw new InvalidScoreFormatException($"Line {lineNumber}: Invalid score format '{fields[2].Trim()}'");
                        }

                        // Validate score range
                        if (score < 0 || score > 100)
                        {
                            throw new InvalidScoreFormatException($"Line {lineNumber}: Score {score} is out of valid range (0-100)");
                        }

                        students.Add(new Student(id, fullName, score));
                    }
                    catch (InvalidScoreFormatException)
                    {
                        throw; // Re-throw as is
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Line {lineNumber}: Unexpected error - {ex.Message}");
                    }
                }
            }

            return students;
        }

        public void WriteReportToFile(List<Student> students, string outputFilePath)
        {
            using (var writer = new StreamWriter(outputFilePath))
            {
                writer.WriteLine("=== Student Grade Report ===");
                writer.WriteLine($"Generated on: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                writer.WriteLine($"Total Students: {students.Count}");
                writer.WriteLine();

                foreach (var student in students)
                {
                    writer.WriteLine(student.ToString());
                }

                writer.WriteLine();
                writer.WriteLine("=== Grade Distribution ===");
                
                int gradeA = 0, gradeB = 0, gradeC = 0, gradeD = 0, gradeF = 0;
                
                foreach (var student in students)
                {
                    switch (student.GetGrade())
                    {
                        case "A": gradeA++; break;
                        case "B": gradeB++; break;
                        case "C": gradeC++; break;
                        case "D": gradeD++; break;
                        case "F": gradeF++; break;
                    }
                }

                writer.WriteLine($"A: {gradeA} students");
                writer.WriteLine($"B: {gradeB} students");
                writer.WriteLine($"C: {gradeC} students");
                writer.WriteLine($"D: {gradeD} students");
                writer.WriteLine($"F: {gradeF} students");
            }
        }

        public void CreateSampleInputFile(string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine("101,Alice Smith,84");
                writer.WriteLine("102,Bob Johnson,76");
                writer.WriteLine("103,Carol Davis,92");
                writer.WriteLine("104,David Brown,58");
                writer.WriteLine("105,Emma Wilson,45");
                writer.WriteLine("106,Frank Miller,88");
                writer.WriteLine("107,Grace Lee,71");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Student Grading System ===");
            Console.WriteLine();

            var processor = new StudentResultProcessor();
            string inputFile = "students.txt";
            string outputFile = "grade_report.txt";

            try
            {
                // Create sample input file if it doesn't exist
                if (!File.Exists(inputFile))
                {
                    Console.WriteLine("Creating sample input file...");
                    processor.CreateSampleInputFile(inputFile);
                    Console.WriteLine($"Sample file '{inputFile}' created.");
                    Console.WriteLine();
                }

                // Read students from file
                Console.WriteLine($"Reading student data from '{inputFile}'...");
                var students = processor.ReadStudentsFromFile(inputFile);
                Console.WriteLine($"Successfully read {students.Count} student records.");

                // Write report to file
                Console.WriteLine($"Generating report to '{outputFile}'...");
                processor.WriteReportToFile(students, outputFile);
                Console.WriteLine("Report generated successfully!");

                // Display summary
                Console.WriteLine("\n=== Processing Summary ===");
                foreach (var student in students)
                {
                    Console.WriteLine(student);
                }

                Console.WriteLine($"\nDetailed report saved to: {outputFile}");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"File Error: Input file not found - {ex.Message}");
            }
            catch (InvalidScoreFormatException ex)
            {
                Console.WriteLine($"Score Format Error: {ex.Message}");
            }
            catch (MissingFieldException ex)
            {
                Console.WriteLine($"Missing Field Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}