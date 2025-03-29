using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using System.Collections.Generic;
using System.Linq;
using Wazzifni.Authorization.Users;

namespace Wazzifni.Authorization;

public class DeactivatedUsersSet : ISingletonDependency
{
    private readonly HashSet<long> bannedUsers;

    public DeactivatedUsersSet(IIocManager iocManager)
    {
        using var uowManager = iocManager.ResolveAsDisposable<IUnitOfWorkManager>();

        using var uow = uowManager.Object.Begin();

        using var usersRepo = iocManager.ResolveAsDisposable<IRepository<User, long>>();

        bannedUsers = usersRepo.Object.GetAll()
            .Where(u => u.IsActive == false)
            .Select(u => u.Id)
            .ToHashSet();

        uow.Complete();
    }

    public void Add(long userId) => bannedUsers.Add(userId);
    public void Remove(long userId) => bannedUsers.Remove(userId);

    public bool IsDeactivated(long userId) => bannedUsers.Contains(userId);
}