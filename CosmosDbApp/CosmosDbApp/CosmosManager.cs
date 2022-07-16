using Azure.Data.Tables;
using Microsoft.Azure.Cosmos;

namespace CosmosDbApp;
internal static class CosmosManager
{
    const string databaseName = "appdb";
    const string containerName = "course";
    const string partitionKey = "/courseId";
    const string cosmosDbConnectionString = "AccountEndpoint=https://akdconsmosdbaccount.documents.azure.com:443/;AccountKey=uPU7Em0dAgTY2Opl7wJHt8kE2gmsOABpDHnzyQlKN1tXOXQBk0C8ciiP3wzr67fS9cIXUYMfY00ZWCNVy2FvEg==;";

    public static async Task CreateCosmosDb()
    {
        var cosmosClient = new CosmosClient(cosmosDbConnectionString);
        await cosmosClient.CreateDatabaseAsync(databaseName);

        var database = cosmosClient.GetDatabase(databaseName);

        await database.CreateContainerAsync(containerName, partitionKey);

        Console.WriteLine($"Databse : {databaseName} and Container : {containerName} is created");
    }

    public static async Task AddItem()
    {
        var course = new Course()
        {
            Id = "33",
            CourseId = "C0001033",
            CourseName = "AZ-204-3",
            Rating = 4.5m
        };
        using (var client = new CosmosClient(cosmosDbConnectionString))
        {
            var container = client.GetContainer(databaseName, containerName);

            await container.CreateItemAsync(course, new PartitionKey(course.CourseId));

            Console.WriteLine("Item is created");
        }
    }

    public static async Task AddItemsInBulk()
    {
        var courses = new List<Course>()
                    {
                    new Course() { Id="1",CourseId="Course0001", CourseName = "AZ-204 Developing Azure solutions", Rating = 4.5m },
                    new Course() { Id="2",CourseId="Course0002", CourseName = "AZ-303 Architecting Azure solutions", Rating = 4.6m },
                    new Course() { Id="3", CourseId="Course0003", CourseName = "DP-203 Azure Data Engineer", Rating = 4.7m },
                    new Course() { Id="4",CourseId="Course0004", CourseName = "AZ-900 Azure Fundamentals", Rating = 4.6m },
                    new Course() { Id="5",CourseId="Course0005", CourseName = "AZ-104 Azure Administrator", Rating = 4.5m }};

        using (var client = new CosmosClient(cosmosDbConnectionString, new CosmosClientOptions() { AllowBulkExecution = true }))
        {
            var container = client.GetContainer(databaseName, containerName);

            var tasks = new List<Task>();

            foreach (var course in courses)
                tasks.Add(container.CreateItemAsync(course, new PartitionKey(course.CourseId)));

            await Task.WhenAll(tasks);

            Console.WriteLine("Bulk insert completed");
        }
    }

    public static async Task ReadDataFromCosmos()
    {
        using (var client = new CosmosClient(cosmosDbConnectionString))
        {
            var container = client.GetContainer(databaseName, containerName);

            string query = "SELECT * FROM c WHERE c.courseId='Course0002'";

            QueryDefinition definition = new QueryDefinition(query);

            FeedIterator<Course> iterator = container.GetItemQueryIterator<Course>(definition);

            while (iterator.HasMoreResults)
            {
                FeedResponse<Course> response = await iterator.ReadNextAsync();
                foreach (var course in response)
                {
                    Console.WriteLine($"Id is {course.Id}");
                    Console.WriteLine($"Course Id is {course.CourseId}");
                    Console.WriteLine($"Course Name is {course.CourseName}");
                    Console.WriteLine($"Course Rating is {course.Rating}");
                }
            }
        }
    }

    public static async Task UpdateItem()
    {
        var id = "3";
        var partitionKeyValue = "Course0003";
        using (var client = new CosmosClient(cosmosDbConnectionString))
        {
            var container = client.GetContainer(databaseName, containerName);
            ItemResponse<Course> response = await container.ReadItemAsync<Course>(id, new PartitionKey(partitionKeyValue));

            Course course = response.Resource;
            course.Rating = 4.8m;
            await container.ReplaceItemAsync(course, id, new PartitionKey(partitionKeyValue));

            Console.WriteLine("Item has been updated");
        }
    }

    public static async Task DeleteItem()
    {
        var id = "3";
        var partitionKeyValue = "Course0003";
        using (var client = new CosmosClient(cosmosDbConnectionString))
        {
            var container = client.GetContainer(databaseName, containerName);

            await container.DeleteItemAsync<Course>(id, new PartitionKey(partitionKeyValue));

            Console.WriteLine("Item has been Deleted");
        }
    }

    public static async Task ExecuteStoredProcedure()
    {
        using (var client = new CosmosClient(cosmosDbConnectionString))
        {
            var container = client.GetContainer(databaseName, containerName);

            string output = await container.Scripts.ExecuteStoredProcedureAsync<string>("demoStoredProcedure", new PartitionKey(string.Empty), null);

            Console.WriteLine(output);
        }
    }

    public static async Task AddItemUsingStoredProcedure()
    {
        using (var client = new CosmosClient(cosmosDbConnectionString))
        {
            var container = client.GetContainer(databaseName, containerName);

            dynamic[] courseItems = new dynamic[]
             {
                new {id="51",courseId="Course00071",courseName="AZ-500 Azure Security-1",rating=4.5m}
             };

            string output = await container.Scripts.ExecuteStoredProcedureAsync<string>("addCourseItem", new PartitionKey("Course00071"), new[] { courseItems });
            Console.WriteLine(output);

            Console.WriteLine("Item is created");
        }
    }

    public static async Task AddItemPreTrigger()
    {
        var course = new Course()
        {
            Id = "333",
            CourseName = "AZ-204-33",
            Rating = 4.5m
        };
        using (var client = new CosmosClient(cosmosDbConnectionString))
        {
            var container = client.GetContainer(databaseName, containerName);

            await container.CreateItemAsync(course, null, new ItemRequestOptions { PreTriggers = new List<string> { "trgCreateTimeStamp" } });

            Console.WriteLine("Item is created");
        }
    }

    public static async Task AddEmbeddedItem()
    {
        var course = new Course()
        {
            Id = "334",
            CourseId = "C0001033",
            CourseName = "AZ-204-3",
            Rating = 4.5m,
            Orders = new List<Order>() { new Order() { OrderId = "O2", Price = 50 }, new Order() { OrderId = "O3", Price = 80 } }
        };
        using (var client = new CosmosClient(cosmosDbConnectionString))
        {
            var container = client.GetContainer(databaseName, containerName);

            await container.CreateItemAsync(course, new PartitionKey(course.CourseId));

            Console.WriteLine("Item is created");
        }
    }

    private static string cosmosTableConnectionString = "DefaultEndpointsProtocol=https;AccountName=akdcosmosapptable;AccountKey=HW5ICvz6bbb4USns8lhxdOCphubDKhrJOQSxSRVip3hkJrqb1yxW3Xi4dRWSNqhicrOwCZHnZxRKnBow41yQ8A==;TableEndpoint=https://akdcosmosapptable.table.cosmos.azure.com:443/;";
    private static string tableName = "product";

    public static async Task AddItemToTable()
    {
        TableServiceClient tableServiceClient = new TableServiceClient(cosmosTableConnectionString);
        // New instance of TableClient class referencing the server-side table
        TableClient tableClient = tableServiceClient.GetTableClient(tableName: tableName);

        await tableClient.CreateIfNotExistsAsync();

        // Create new item using composite key constructor
        var prod1 = new Product()
        {
            RowKey = "68719518388",
            PartitionKey = "gear-surf-surfboards",
            Name = "Ocean Surfboard",
            Quantity = 8,
            Sale = true
        };

        // Add new item to server-side table
        await tableClient.AddEntityAsync<Product>(prod1);


        // Read a single item from container
        var product = await tableClient.GetEntityAsync<Product>(
            rowKey: "68719518388",
            partitionKey: "gear-surf-surfboards"
        );
        Console.WriteLine("Single product:");
        Console.WriteLine(product.Value.Name);

        // Read multiple items from container
        var prod2 = new Product()
        {
            RowKey = "68719518390",
            PartitionKey = "gear-surf-surfboards",
            Name = "Sand Surfboard",
            Quantity = 5,
            Sale = false
        };

        await tableClient.AddEntityAsync<Product>(prod2);

        var products = tableClient.Query<Product>(x => x.PartitionKey == "gear-surf-surfboards");

        Console.WriteLine("Multiple products:");
        foreach (var item in products)
        {
            Console.WriteLine(item.Name);
        }
    }
}
