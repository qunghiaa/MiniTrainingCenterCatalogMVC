using System.ComponentModel.DataAnnotations;

namespace MiniTrainingCenterCatalog.Mvc.ViewModels;

public class AdjustSeatViewModel
{
    public int Id { get; set; }

    public string CourseName { get; set; } = "";

    public int CurrentCapacity { get; set; }

    [Range(-1000,1000)]
    public int SeatChange { get; set; }

    public byte[] RowVersion { get; set; }
        = Array.Empty<byte>();
}