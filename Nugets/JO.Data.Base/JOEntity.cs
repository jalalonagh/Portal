﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JO.Data.Base
{
    public abstract class JOEntity : IJOEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long? Editor { get; set; }
        public DateTime ModifiedTime { get; set; }
        [MaxLength(20)]
        public string ModifiedTimePersian { get; set; }
        public Guid ConcurrencyStatus { get; set; }
        [MaxLength(500)]
        public string? ModifiedDescription { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? Enabled { get; set; } = true;
        [NotMapped]
        private bool? IsSystem { get; set; }

        public void SetId(long id)
        {
            Id = id;
        }

        public void SetEditor(long? id)
        {
            Editor = id;
        }

        public void SetTimeNow()
        {
            ModifiedTime = DateTime.Now;
            ModifiedTimePersian = ModifiedTime.ToLocalTime().ToString("yyyy-MM-dd");
        }

        public void SetModifiedTime(DateTime time)
        {
            ModifiedTime = time;
            ModifiedTimePersian = ModifiedTime.ToLocalTime().ToString("yyyy-MM-dd");
        }

        public void ReNewConcurrency()
        {
            ConcurrencyStatus = Guid.NewGuid();
        }

        public abstract void Validate();

        public void SetDeleted()
        {
            IsDeleted = true;
        }

        public void SetIsSystemAction()
        {
            IsSystem = true;
        }

        public bool GetIsSystemAction()
        {
            return IsSystem ?? false;
        }

        public void SetEnabled(bool? enabled)
        {
            Enabled = enabled;
        }
    }
}