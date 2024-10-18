using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TimeAide.Common.Helpers;

namespace TimeAide.Web.Models
{
  
    public class BaseEntity 
    {
        public BaseEntity()
        {           
            DataEntryStatus = 1;           
        }
        public virtual int Id
        {
            get;
            set;
        }      

        public int DataEntryStatus { get; set; }

    }     

    public class BaseWithLoggingEntity : BaseEntity
    {
        public BaseWithLoggingEntity()
        {
            if (SessionHelper.LoginId != 0) CreatedBy = SessionHelper.LoginId;
            CreatedDate = DateTime.Now;          
        }
        public int CreatedBy { get; set; }
      
        public DateTime CreatedDate { get; set; }
        
        public int? ModifiedBy { get; set; }
      
        public DateTime? ModifiedDate { get; set; }
    }
    
}