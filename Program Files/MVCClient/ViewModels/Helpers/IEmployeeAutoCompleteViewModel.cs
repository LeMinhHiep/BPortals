using System.ComponentModel.DataAnnotations;

namespace MVCClient.ViewModels.Helpers
{
    public interface IEmployeeAutoCompleteViewModel
    {
        int EmployeeID { get; set; }
        [Display(Name = "Nhân viên thực hiện")]
        string EmployeeName { get; set; }
    }
}
