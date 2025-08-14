# dcit318-assignment3-11194085

# What I Accomplished

I successfully built five different C# applications that demonstrate advanced object-oriented programming concepts. Each application taught me how to use records, interfaces, generics, collections, exception handling, and file operations to solve real-world programming problems.

For the Finance Management System, I learned how records create immutable data structures by building a Transaction record that cannot be changed once created. I implemented interfaces to define contracts that different payment processors must follow, showing how the same transaction can be processed through bank transfers, mobile money, or crypto wallets. The sealed SavingsAccount class taught me how to prevent inheritance when I want to lock down specific behavior, like enforcing insufficient funds checking.

The Healthcare System introduced me to the power of generics and collections. I created a generic Repository class that can store and manage any type of data, whether patients or prescriptions. This taught me how to write reusable code that works with different data types while maintaining type safety. Using dictionaries to group prescriptions by patient ID showed me how to organize related data efficiently.

In the Warehouse Inventory System, I mastered custom exception handling by creating specific exceptions for different error scenarios. When someone tries to add duplicate items, the system throws a DuplicateItemException. When items are missing, it throws ItemNotFoundException. This taught me how to provide meaningful error messages that help users understand what went wrong and how to fix it.

The Student Grading System taught me file input/output operations and data validation. I learned to read comma-separated values from text files, validate each field, and handle corrupted data gracefully. The system converts numerical scores to letter grades and generates detailed reports saved to output files. This showed me how programs can process external data and produce formatted output.

Finally, the Inventory Data Persistence System demonstrated how to make data survive between program sessions. Using C# records for immutable inventory items and JSON serialization for file storage, I learned how to save application state to disk and reload it later. This taught me the importance of data persistence in real-world applications.

Through these five projects, I gained deep understanding of advanced C# concepts including how records ensure data integrity, how interfaces define contracts for consistent behavior, how generics enable type-safe reusable code, how collections organize complex data relationships, how custom exceptions provide meaningful error handling, and how file operations enable data persistence. Each application builds on these concepts to solve practical programming challenges that mirror real-world software development scenarios.


