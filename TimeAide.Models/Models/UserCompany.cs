using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("UserCompany")]
    public partial class UserCompany : BaseWithLoggingEntity
    {
        public UserCompany()
        {

        }

        [Column("UserCompanyId")]
        public override int Id { get; set; }
        public int UserInformationId { get; set; }
        public int CompanyId { get; set; }

        public virtual Company Company { get; set; }
    }
}
