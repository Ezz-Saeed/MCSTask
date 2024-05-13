using System.ComponentModel.DataAnnotations;

namespace MCSTask.ViewModels
{
    public class FilteringValueViewModel
    {
        [DataType(DataType.Currency)]
        public decimal XValue { get; set; }
    }
}
