using Abp.Domain.Services;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Wazzifni.Domain.Trainees
{
    public interface ITraineeManager : IDomainService
    {
        Task<long> InitateTrainee(long UserId, int? UniversityId , string? UniversityMajor ,long LogoAttchmentId,string EmailAddress);

        Task<Trainee> GetEntityByIdAsync(long TraineeId);

        Task<Trainee> GetEntityByUserIdAsync(long UserId);

        Task<long> GetTraineeIdByUserId(long userId);

        Task DeleteTraineeByUserId(long userId);
        Task<Trainee> GetEntityByIdWithUserAsync(long TraineeId);

    }
}
