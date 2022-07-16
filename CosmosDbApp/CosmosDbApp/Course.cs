using Newtonsoft.Json;

namespace CosmosDbApp;
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

    [JsonProperty(PropertyName = "orders")]
    public List<Order> Orders { get; set; }
}

internal class Order
{
    [JsonProperty(PropertyName = "orderId")]
    public string OrderId { get; set; }

    [JsonProperty(PropertyName = "price")]
    public int Price { get; set; }
}
