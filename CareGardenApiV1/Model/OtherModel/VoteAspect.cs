using System.Text.Json.Serialization;

namespace CareGardenApiV1.Model.OtherModel
{
    public class VoteAspect
    {
        public double point { get; set; }
        public int index { get; set; }
        public string description { get; set; }

        public VoteAspect(double point, int index, string descriptionEn, string description, bool isTurkish)
        {
            this.index = index;
            this.point = point;
            this.description = isTurkish ? description : descriptionEn;
        }
    }
}
