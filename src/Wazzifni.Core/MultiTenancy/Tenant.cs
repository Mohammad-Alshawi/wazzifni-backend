﻿using Abp.MultiTenancy;
using Wazzifni.Authorization.Users;

namespace Wazzifni.MultiTenancy
{
    public class Tenant : AbpTenant<User>
    {
        public Tenant()
        {            
        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
        }
    }
}
