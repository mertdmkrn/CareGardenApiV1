namespace CareGardenApiV1.Model.RequestModel
{
    public class PasswordChangeRequestModel
    {
        public string? currentPassword { get; set; }
        public string? newPassword { get; set; }
        public string? newRetryPassword { get; set; }
    }
}
