namespace MiniTrainingCenterCatalog.Mvc.Services;

public interface IEnrollmentService
{
    bool Register(
        int studentId,
        int courseId);
}