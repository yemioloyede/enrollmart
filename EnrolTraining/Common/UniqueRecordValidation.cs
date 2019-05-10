using EnrolTraining.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace EnrolTraining.Common
{


    public class UniqueEmailAttribute: ValidationAttribute//, IClientValidatable
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                User loginModel = (User)validationContext.ObjectInstance;
                string strValue = value.ToString();
                Context db = new Context();
                bool IsUnchangeInEditMode = db.User.Any(x => x.EmailAddress == loginModel.EmailAddress && x.UserID == loginModel.UserID);
                if (IsUnchangeInEditMode)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    bool IsDuplicate = db.User.Any(x => x.EmailAddress == loginModel.EmailAddress);
                    if (IsDuplicate)
                    {
                        return new ValidationResult("The Email is already in used");
                    }
                    else
                    {
                        return ValidationResult.Success;
                    }
                }

            }
            else
            {
                return new ValidationResult("The Email cannot be validated");
            }
        }

        //public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        //{
        //    var rule = new ModelClientValidationRule
        //    {
        //        ErrorMessage = this.ErrorMessage,
        //        ValidationType = "TestMethod",
        //    };
        //    rule.ValidationParameters.Add("param1", "value1");
        //    rule.ValidationParameters.Add("param2", "value2");
        //    yield return rule;
        //}

    }

    public class UniqueSubDomainAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return true;

            Context db = new Context();
            bool isValid = !db.EnrolSetting.ToList().Any(x => x.SiteName== value.ToString());
            return isValid;
        }
    }

    public class UniqueUserAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return true;

            Context db = new Context();
            bool isValid = !db.User.Any(x => x.UserName == value.ToString());
            return isValid;
        }
    }


}