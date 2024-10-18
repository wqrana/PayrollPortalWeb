using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeAide.Web.Models
{
    [Table("UserInformation")]
    public partial class UserInformation : BaseWithLoggingEntity
    {
        public UserInformation()
        {
           
        }

        [Column("UserInformationId")]
        public override int Id { get; set; }

        [Required]
        [Display(Name = "Employee Id")]       
        public int EmployeeId { get; set; }

        [Required]
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        [Required]
        public string FirstLastName { get; set; }
        public string SecondLastName { get; set; }

        [Required]
        [EmailAddress]
        public string LoginEmail { get; set; }
        [Required]
        public string LoginStatus { get; set; }

        public bool IsAdmin { get; set; }
        [NotMapped]
        public string FullUserName
        {
            get
            {               
              return (FirstName ?? "") + " " + (FirstLastName ?? "") + " " + (SecondLastName ?? "");
            }
        }

        [NotMapped]
        [DataType(DataType.Password)]
        public string LoginPassword { get; set; }
        [NotMapped]
        public string SelectedUserCompanyIds { get; set; }
    }
}
