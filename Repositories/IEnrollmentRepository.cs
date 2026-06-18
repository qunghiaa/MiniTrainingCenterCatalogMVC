using MiniTrainingCenterCatalog.Mvc.Models;

namespace MiniTrainingCenterCatalog.Mvc.Repositories;

public interface IEnrollmentRepository
{
    Task CreateEnrollmentAsync(
        int studentId,
        int courseId);
}