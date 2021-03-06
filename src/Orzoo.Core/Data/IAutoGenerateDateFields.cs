﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Orzoo.Core.Data
{
    public interface IMetadataEntity
    {
        [Display(Name = "创建日期")]
        DateTime? CreatedDate { get; set; }

        [Display(Name = "最后更新日期")]
        DateTime? LastModifiedDate { get; set; }

        [Display(Name = "创建者")]
        string CreatedUser { get; set; }

        [Display(Name = "最后更新者")]
        string ModifiedUser { get; set; }
    }
}
