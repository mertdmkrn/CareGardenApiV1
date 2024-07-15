namespace CareGardenApiV1.Model.RequestModel
{
    public class PasswordResetRequestModel
    {
        public string? email { get; set; }
        public string? password { get; set; }
        public string? retryPassword { get; set; }
        public string? verifiedCode { get; set; }
        public string? linkId { get; set; }
    }
}
