using Filed.Payments.Data.Entities;
using System;

namespace Filed.Payment.Services.Extension_Methods
{
    public static class AuditFieldSetter
    {
            public static void SetCreatedFields(this BaseEntity entity, string userName, string userId)
            {
                entity.CreatedBy = userName ?? "Anonymous";
                entity.CreatedByUserId = userId ?? "Anonymous";
                entity.DateCreated = DateTime.Now;
            }

            public static void SetModifiedFields(this BaseEntity entity, string userName, string userId)
            {
                entity.ModifiedBy = userName ?? "Anonymous";
                entity.ModifiedByUserId = userId ?? "Anonymous";
                entity.DateModified = DateTime.Now;
            }
        }
}
