using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CareGardenApiV1.Model.RequestModel
{
    public class PasswordChangeModel
    {
        public string? currentPassword { get; set; }
        public string? newPassword { get; set; }
        public string? newRetryPassword { get; set; }
    }
}
