using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wazzifni.Domain.Universities;

namespace Wazzifni.Models.TokenAuth.Dto
{
    public class TraineeCreate
    {
        public int? UniversityId { get; set; }
        public string? UniversityMajor { get; set; }

        public long TraineePhotoId { get; set; }

    }
}
