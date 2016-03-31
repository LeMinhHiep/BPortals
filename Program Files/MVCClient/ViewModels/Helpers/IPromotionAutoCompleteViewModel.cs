using System.ComponentModel.DataAnnotations;

namespace MVCClient.ViewModels.Helpers
{
    public interface IPromotionAutoCompleteViewModel
    {
        int PromotionID { get; set; }
        [Display(Name = "Chương trình khuyến mãi")]
        string PromotionCode { get; set; }
    }
}
