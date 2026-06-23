using MiniTrainingCenterCatalog.Mvc.Models;

namespace MiniTrainingCenterCatalog.Mvc.Repositories;

public interface IEnrollmentRepository
{
    void Add(Enrollment enrollment);

    void SaveChanges();
}