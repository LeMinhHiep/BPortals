using System.Linq;
using System.Collections.Generic;

using MVCModel;

namespace MVCDTO
{
    public abstract class BaseWithDetailDTO<TDtoDetail> : BaseDTO
        where TDtoDetail : IBaseModel
    {
        public virtual IEnumerable<TDtoDetail> DtoDetails() { return new List<TDtoDetail>(); }

        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();

            this.DtoDetails().ToList().ForEach(e => { e.LocationID = this.LocationID; e.EntryDate = this.EntryDate; });
        }
    }
}
