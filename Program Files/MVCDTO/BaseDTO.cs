﻿using System;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using MVCModel;
using MVCBase.Enums;

namespace MVCDTO
{
    public abstract class BaseDTO : BaseModel, IAccessControlAttribute 
    {
        protected BaseDTO()
        {
            // apply any DefaultValueAttribute settings to their properties
            var propertyInfos = this.GetType().GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                var attributes = propertyInfo.GetCustomAttributes(typeof(DefaultValueAttribute), true);
                if (attributes.Any())
                {
                    var attribute = (DefaultValueAttribute)attributes[0];
                    propertyInfo.SetValue(this, attribute.Value, null);
                }
            }
        }

        
        [Display(Name = "Số phiếu")]
        public string Reference { get; set; }



        public int UserID { get; set; }
        [Required]
        [Display(Name = "Người lập")]
        public virtual int PreparedPersonID { get; set; }
        public int OrganizationalUnitID { get; set; }


        [Display(Name = "Người duyệt")]
        public int ApproverID { get; set; }


        [Display(Name = "Diễn giải")]
        public string Description { get; set; }

        public bool Approved { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public bool InActive { get; set; }
        public bool InActivePartial { get; set; }
        public Nullable<System.DateTime> InActiveDate { get; set; }


        public bool Editable { get; set; }
        public bool Deletable { get; set; }



        //These properties are used as an implementation preservation of ISimpleViewModel for these ________ViewModel class (Those class ________ViewModel which is BOTH inheritance from this BaseDTO AND implementation of ISimpleViewModel)
        public virtual bool PrintAfterClosedSubmit { get; set; }
        public GlobalEnums.SubmitTypeOption SubmitTypeOption { get; set; }


        
        public virtual void PerformPresaveRule() { }
    }
}

