namespace MiniTrainingCenterCatalog.Mvc.ViewModels;

public class CourseFilterViewModel
{
    public int? CategoryId { get; set; }

    public decimal? MinFee { get; set; }

    public decimal? MaxFee { get; set; }
}