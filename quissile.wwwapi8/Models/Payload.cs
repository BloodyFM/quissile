namespace quissile.wwwapi8.Models
{
    public class Payload<T> where T : class
    {
        public string Status { get; set; } = "Success";
        public T Data { get; set; }
    }
}
