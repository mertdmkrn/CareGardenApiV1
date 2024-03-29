namespace CareGardenApiV1.Model.RequestModel
{
    public class PasswordResetModel
    {
        public string? email { get; set; }
        public string? password { get; set; }
        public string? retryPassword { get; set; }
        public string? verifiedCode { get; set; }
    }
}
