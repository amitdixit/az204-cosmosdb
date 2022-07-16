using Newtonsoft.Json;

namespace CosmosDbChangeFeedTrigger;
internal class Course
{
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    [JsonProperty(PropertyName = "courseId")]
    public string CourseId { get; set; }

    [JsonProperty(PropertyName = "courseName")]
    public string CourseName { get; set; }

    [JsonProperty(PropertyName = "rating")]
    public decimal Rating { get; set; }
}
