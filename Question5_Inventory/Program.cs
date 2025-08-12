using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace InventoryDataPersistence
{
    // Define immutable InventoryItem record
    public record InventoryItem(int Id, string Name, int Quantity, DateTime DateAdded) : IInventoryEntity;

    // Marker interface for logging
    public interface IInventoryEntity
    {
        int Id { get; }
    }

    // Generic Inventory Logger
    public class InventoryLogger<T> where T : IInventoryEntity
    {
        private List<T> _log = new List<T>();
        private string _filePath;

        public InventoryLogger(string filePath)
        {
            _filePath = filePath;
        }

        public void Add(T item)
        {
            _log.Add(item);
            Console.WriteLine($"Added item: {item}");
        }

        public List<T> GetAll()
        {
            return new List<T>(_log);
        }

        public void SaveToFile()
        {
            try
            {
                using (var stream = new FileStream(_filePath, FileMode.Create))
                using (var writer = new StreamWriter(stream))
                {
                    var json = JsonSerializer.Serialize(_log, new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });
                    writer.Write(json);
                }
                Console.WriteLine($"Data saved to {_filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to file: {ex.Message}");
            }
        }

        public void LoadFromFile()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    Console.WriteLine($"File {_filePath} does not exist. Starting with empty log.");
                    _log.Clear();
                    return;
                }

                using (var stream = new FileStream(_filePath, FileMode.Open))
                using (var reader = new StreamReader(stream))
                {
                    var json = reader.ReadToEnd();
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        var items = JsonSerializer.Deserialize<List<T>>(json);
                        _log = items ?? new List<T>();
                        Console.WriteLine($"Data loaded from {_filePath}. {_log.Count} items loaded.");
                    }
                    else
                    {
                        _log.Clear();
                        Console.WriteLine("File is empty. Starting with empty log.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading from file: {ex.Message}");
                _log.Clear();
            }
        }

        public void ClearLog()
        {
            _log.Clear();
            Console.WriteLine("Log cleared from memory.");
        }
    }

    // Integration Layer - InventoryApp
    public class InventoryApp
    {
        private InventoryLogger<InventoryItem> _logger;

        public InventoryApp()
        {
            _logger = new InventoryLogger<InventoryItem>("inventory.json");
        }

        public void SeedSampleData()
        {
            Console.WriteLine("=== Seeding Sample Data ===");

            _logger.Add(new InventoryItem(1, "Laptop", 10, DateTime.Now.AddDays(-30)));
            _logger.Add(new InventoryItem(2, "Mouse", 50, DateTime.Now.AddDays(-25)));
            _logger.Add(new InventoryItem(3, "Keyboard", 25, DateTime.Now.AddDays(-20)));
            _logger.Add(new InventoryItem(4, "Monitor", 15, DateTime.Now.AddDays(-15)));
            _logger.Add(new InventoryItem(5, "Printer", 8, DateTime.Now.AddDays(-10)));

            Console.WriteLine();
        }

        public void SaveData()
        {
            Console.WriteLine("=== Saving Data to File ===");
            _logger.SaveToFile();
            Console.WriteLine();
        }

        public void LoadData()
        {
            Console.WriteLine("=== Loading Data from File ===");
            _logger.LoadFromFile();
            Console.WriteLine();
        }

        public void PrintAllItems()
        {
            Console.WriteLine("=== All Inventory Items ===");
            var items = _logger.GetAll();

            if (items.Count == 0)
            {
                Console.WriteLine("No items found in inventory.");
            }
            else
            {
                Console.WriteLine($"Total items: {items.Count}");
                Console.WriteLine();

                foreach (var item in items)
                {
                    Console.WriteLine($"ID: {item.Id}, Name: {item.Name}, Quantity: {item.Quantity}, Date Added: {item.DateAdded:yyyy-MM-dd}");
                }
            }
            Console.WriteLine();
        }

        public void ClearMemory()
        {
            Console.WriteLine("=== Simulating Application Restart ===");
            _logger.ClearLog();
            Console.WriteLine("Memory cleared - simulating new application session.");
            Console.WriteLine();
        }

        public void Run()
        {
            Console.WriteLine("Starting Inventory Data Persistence Demo...");
            Console.WriteLine();

            // Seed sample data
            SeedSampleData();

            // Show current data
            PrintAllItems();

            // Save to file
            SaveData();

            // Clear memory to simulate application restart
            ClearMemory();

            // Verify memory is empty
            Console.WriteLine("=== Verifying Memory is Empty ===");
            PrintAllItems();

            // Load data from file
            LoadData();

            // Show recovered data
            Console.WriteLine("=== Verifying Data Recovery ===");
            PrintAllItems();

            Console.WriteLine("Demo completed successfully!");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Inventory Data Persistence System ===");
            Console.WriteLine();

            try
            {
                var app = new InventoryApp();
                app.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Application Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}