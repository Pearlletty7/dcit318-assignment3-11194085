using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthcareSystem
{
    // Generic Repository class
    public class Repository<T>
    {
        private List<T> items = new List<T>();

        public void Add(T item)
        {
            items.Add(item);
        }

        public List<T> GetAll()
        {
            return new List<T>(items);
        }

        public T? GetById(Func<T, bool> predicate)
        {
            return items.FirstOrDefault(predicate);
        }

        public bool Remove(Func<T, bool> predicate)
        {
            var item = items.FirstOrDefault(predicate);
            if (item != null)
            {
                items.Remove(item);
                return true;
            }
            return false;
        }
    }

    // Patient class
    public class Patient
    {
        public int Id { get; }
        public string Name { get; }
        public int Age { get; }
        public string Gender { get; }

        public Patient(int id, string name, int age, string gender)
        {
            Id = id;
            Name = name;
            Age = age;
            Gender = gender;
        }

        public override string ToString()
        {
            return $"Patient ID: {Id}, Name: {Name}, Age: {Age}, Gender: {Gender}";
        }
    }

    // Prescription class
    public class Prescription
    {
        public int Id { get; }
        public int PatientId { get; }
        public string MedicationName { get; }
        public DateTime DateIssued { get; }

        public Prescription(int id, int patientId, string medicationName, DateTime dateIssued)
        {
            Id = id;
            PatientId = patientId;
            MedicationName = medicationName;
            DateIssued = dateIssued;
        }

        public override string ToString()
        {
            return $"Prescription ID: {Id}, Medication: {MedicationName}, Date: {DateIssued:yyyy-MM-dd}";
        }
    }

    // HealthSystemApp class
    public class HealthSystemApp
    {
        private Repository<Patient> _patientRepo = new Repository<Patient>();
        private Repository<Prescription> _prescriptionRepo = new Repository<Prescription>();
        private Dictionary<int, List<Prescription>> _prescriptionMap = new Dictionary<int, List<Prescription>>();

        public void SeedData()
        {
            // Add patients
            _patientRepo.Add(new Patient(1, "John Doe", 35, "Male"));
            _patientRepo.Add(new Patient(2, "Jane Smith", 28, "Female"));
            _patientRepo.Add(new Patient(3, "Bob Johnson", 45, "Male"));

            // Add prescriptions
            _prescriptionRepo.Add(new Prescription(1, 1, "Aspirin", DateTime.Now.AddDays(-5)));
            _prescriptionRepo.Add(new Prescription(2, 1, "Ibuprofen", DateTime.Now.AddDays(-3)));
            _prescriptionRepo.Add(new Prescription(3, 2, "Antibiotics", DateTime.Now.AddDays(-7)));
            _prescriptionRepo.Add(new Prescription(4, 2, "Vitamins", DateTime.Now.AddDays(-1)));
            _prescriptionRepo.Add(new Prescription(5, 3, "Blood Pressure Medicine", DateTime.Now.AddDays(-10)));
        }

        public void BuildPrescriptionMap()
        {
            var allPrescriptions = _prescriptionRepo.GetAll();
            
            foreach (var prescription in allPrescriptions)
            {
                if (!_prescriptionMap.ContainsKey(prescription.PatientId))
                {
                    _prescriptionMap[prescription.PatientId] = new List<Prescription>();
                }
                _prescriptionMap[prescription.PatientId].Add(prescription);
            }
        }

        public List<Prescription> GetPrescriptionsByPatientId(int patientId)
        {
            return _prescriptionMap.ContainsKey(patientId) 
                ? _prescriptionMap[patientId] 
                : new List<Prescription>();
        }

        public void PrintAllPatients()
        {
            Console.WriteLine("=== All Patients ===");
            var patients = _patientRepo.GetAll();
            foreach (var patient in patients)
            {
                Console.WriteLine(patient);
            }
            Console.WriteLine();
        }

        public void PrintPrescriptionsForPatient(int patientId)
        {
            var patient = _patientRepo.GetById(p => p.Id == patientId);
            if (patient != null)
            {
                Console.WriteLine($"=== Prescriptions for {patient.Name} (ID: {patientId}) ===");
                var prescriptions = GetPrescriptionsByPatientId(patientId);
                
                if (prescriptions.Any())
                {
                    foreach (var prescription in prescriptions)
                    {
                        Console.WriteLine(prescription);
                    }
                }
                else
                {
                    Console.WriteLine("No prescriptions found for this patient.");
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine($"Patient with ID {patientId} not found.");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Healthcare System ===");
            Console.WriteLine();

            var app = new HealthSystemApp();
            
            // Seed data
            app.SeedData();
            
            // Build prescription map
            app.BuildPrescriptionMap();
            
            // Print all patients
            app.PrintAllPatients();
            
            // Print prescriptions for a specific patient
            app.PrintPrescriptionsForPatient(1);
            app.PrintPrescriptionsForPatient(2);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}