using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using Wazzifni.Domain.WorkPosts;
using Wazzifni.NotificationService;

namespace Wazzifni.BackgroundJobs
{
    public class ArchiveWorkPosts : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly WorkPostManager WorkPostManager;
        private readonly INotificationService _InotificationService;
        public ArchiveWorkPosts(AbpTimer timer, IUnitOfWorkManager unitOfWorkManager, WorkPostManager WorkPostManager,
                    INotificationService inotificationService

                 ) : base(timer)
        {
            Timer.Period = 86400000;//24 Hours 
            _unitOfWorkManager = unitOfWorkManager;
            this.WorkPostManager = WorkPostManager;
            _InotificationService = inotificationService;
        }
        protected override async void DoWork()
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                var workPosts = await WorkPostManager.GetAllWorkPostInAvaibiltyStatuesAndClosed();

                await UnitOfWorkManager.Current.SaveChangesAsync();
                unitOfWork.Complete();
            }
        }
    }
}
