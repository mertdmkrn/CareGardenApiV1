using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model.OtherModel;

namespace CareGardenApiV1.Model.ResponseModel
{
    public class StarChipResponseModel
    {
        public List<VoteAspect> businessStarChips { get; set; } = Constants.businessStarChips;
        public List<VoteAspect> workerStarChips { get; set; } = Constants.workerStarChips;
    }
}
