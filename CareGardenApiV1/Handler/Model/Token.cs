namespace CareGardenApiV1.Handler.Model
{
    public class Token
    {
        public string token { get; set; }
        public Guid id { get; set; } = Guid.Empty;
        public DateTime expireDate { get; set; }
    }
}
