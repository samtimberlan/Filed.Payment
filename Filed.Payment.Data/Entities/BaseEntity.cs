using System;

namespace Filed.Payments.Data.Entities
{
    public abstract class BaseEntity
    {
        public long Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public string CreatedBy { get; set; }
        public string CreatedByUserId { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedByUserId { get; set; }
    }
}
