using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Model.ResponseModel
{
    public class FaqResponseModel
    {
        public List<string> categories { get; set; } = new List<string>();
        public IList<Faq> faqs { get; set; } = new List<Faq>();

    }
}
