﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace Wazzifni.Teachers.Dto
{
    public class UpdateTeacherDto : CreateTeacherDto, IEntityDto
    {
        [Required]
        public int Id { get ; set ; }
    }
}
