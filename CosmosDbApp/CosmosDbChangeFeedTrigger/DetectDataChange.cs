using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace CosmosDbChangeFeedTrigger;

public static class DetectDataChange
{
    [FunctionName("ReadDataChange")]
    public static void Run([CosmosDBTrigger(
        databaseName: "appdb",
        collectionName: "course",
        ConnectionStringSetting = "cosmosdb",
        LeaseCollectionName = "leases",CreateLeaseCollectionIfNotExists =true)]IReadOnlyList<Document> input, ILogger log)
    {
        if (input != null && input.Count > 0)
        {
            foreach (var course in input)
            {
                log.LogInformation($"Id {course.Id}");
                log.LogInformation($"Course Id {course.GetPropertyValue<string>("courseId")}");
                log.LogInformation($"Course Name {course.GetPropertyValue<string>("courseName")}");
                log.LogInformation($"Ratng {course.GetPropertyValue<decimal>("rating")}");
            }
        }
    }
}
