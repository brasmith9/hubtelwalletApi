using System;

namespace Hubtel.Wallets.Api.DAL.Entities
{
    public class BaseEntity
    {

        public BaseEntity()
        {
            UpdatedDate = DateTime.UtcNow;
            CreatedDate = CreatedDate ?? UpdatedDate;
        }
        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}