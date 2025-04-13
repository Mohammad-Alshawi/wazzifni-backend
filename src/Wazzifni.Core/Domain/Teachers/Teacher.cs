using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using Wazzifni.Authorization.Users;
using Wazzifni.Domain.Universities;

namespace Wazzifni.Domain.Teachers
{
    public class Teacher : FullAuditedEntity
    {
        public string Name { get; set; }
        public string About { get; set; }

    }
}
