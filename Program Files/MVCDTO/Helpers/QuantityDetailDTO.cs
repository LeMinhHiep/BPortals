using System.ComponentModel.DataAnnotations;

using MVCModel;

namespace MVCDTO.Helpers
{
    public interface IQuantityDetailDTO : IBaseModel
    {
        decimal Quantity { get; set; }
    }

    public abstract class QuantityDetailDTO : BaseModel, IQuantityDetailDTO
    {
        [Display(Name = "SL")]
        [UIHint("Decimal")]
        [Range(0, 99999999999, ErrorMessage = "Số lượng không hợp lệ")]
        public virtual decimal Quantity { get; set; }
    }
}
