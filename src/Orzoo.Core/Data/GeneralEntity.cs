﻿using System;
using Orzoo.Core.Enums;

namespace Orzoo.Core.Data
{
    public class GeneralEntity: Entity, IMetadataEntity, IFlagEntity
    {
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string CreatedUser { get; set; }
        public string ModifiedUser { get; set; }
        public DataFlag? Flag { get; set; }
    }
}