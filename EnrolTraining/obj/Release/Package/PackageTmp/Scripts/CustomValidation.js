//Service Side
//    rule.ValidationParameters.Add("param1", "value1");
//    rule.ValidationParameters.Add("param2", "value2");

//ClientSide
//jQuery.validator.unobtrusive.adapters.add(
//    'TestMethod', 
//    [ 'userid' ],
//    function (options) {
//        // simply pass the options.params here
//        options.rules['TestMethodCheck'] = options.params;
//        options.messages['TestMethodCheck'] = options.message;
//    }
//);

//jQuery.validator.addMethod('TestMethodCheck', function (value, element, params) {
//    // params here will equal { param1: 'value1', param2: 'value2' }
//    return true
//    },
//'');




//Another Sample

//client code:

//jQuery.validator.unobtrusive.adapters.add
//        ('datecomparer', ['firstdate', 'seconddate', 'compare'], function (options) {
//            var attribs = {
//                firstdate: options.params.firstdate,
//                seconddate: options.params.seconddate,
//                compare: options.params.compare
//            };
//            options.rules['datecomparercheck'] = attribs;

//            if (options.message) {
//                $.validator.messages.datecomparercheck = options.message;
//            }
//        });

//jQuery.validator.addMethod(
//    'datecomparercheck',
//    function (value, element, params) {
//        var result = false;
//        if (value && (params['firstdate'] != null && params['seconddate'] != null)) {
//            var startdatevalue = $('input[id="' + params['firstdate'] + '"]').datepicker().val();
//            var enddatevalue = $('input[id="' + params['seconddate'] + '"]').datepicker().val();
//            var dateFormat = $('input[id="' + params['firstdate'] + '"]').datepicker("option", "dateFormat");
//            var sDate = moment(startdatevalue, "DD-MMM-YYYY");
//            var eDate = moment(enddatevalue, "DD-MMM-YYYY");
//            if (params['compare'] == 'GreaterThan') {
//                result = sDate > eDate;
//            }
//            else if (params['compare'] == 'LessThan') {
//                result = sDate < eDate;
//            }
//            else if (params['compare'] == 'Equals') {
//                result = sDate == eDate;
//            }
//        }
//        return result;
//    }
//);
//server  code:

//public enum CompareType
//{
//    EqualsTo,
//    GreaterThan,
//    LessThan
//}

//public class DateComparerAttribute : ValidationAttribute, IClientValidatable
//{
//    public string FirstDate { get; private set; }
//    public string SecondDate { get; private set; }
//    public CompareType Compare { get; set; }

//    public DateComparerAttribute(string firstDate, string secondDate, CompareType type)
//        : base()
//{
//        FirstDate = firstDate;
//        SecondDate = secondDate;
//        Compare = type;
//}
//    public override string FormatErrorMessage(string name)
//{
//        return name;
//}

//    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
//    {
//        var firstDate = validationContext.ObjectInstance.GetType().GetProperty(FirstDate);
//        var secondDate = validationContext.ObjectInstance.GetType().GetProperty(SecondDate);
//        var firstDateValue = firstDate.GetValue(validationContext.ObjectInstance, null);
//        var secondDateValue = secondDate.GetValue(validationContext.ObjectInstance, null);

//        if (value is DateTime && ((firstDateValue is DateTime) && (secondDateValue is DateTime)))
//        {
//            if (Compare == CompareType.EqualsTo)
//        {
//            bool equals = ((DateTime)firstDateValue)==((DateTime)secondDateValue);
//            if (!equals)
//                return new ValidationResult(FormatErrorMessage(FirstDate + " must be equal to " + SecondDate));
//        }
//    else if (Compare == CompareType.GreaterThan)
//        {
//            bool equals = ((DateTime)firstDateValue) > ((DateTime)secondDateValue);
//            if (!equals)
//                return new ValidationResult(FormatErrorMessage(FirstDate + " must be greater than " + SecondDate));
//        }
//    else if (Compare == CompareType.LessThan)
//        {
//            bool equals = ((DateTime)firstDateValue) < ((DateTime)secondDateValue);
//            if (!equals)
//                return new ValidationResult(FormatErrorMessage(FirstDate + " must be less than " + SecondDate));
//        }
//    }
//    return ValidationResult.Success;
//    }        

//public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
//    {
//        var clientValidationRule = new ModelClientValidationRule()
//        {
//            ErrorMessage = FormatErrorMessage(FirstDate + " must be " + Enum.GetName(typeof(CompareType), Compare) + " " + SecondDate),
//            ValidationType = "datecomparer"
//        };
//        clientValidationRule.ValidationParameters.Add("firstdate", FirstDate);
//        clientValidationRule.ValidationParameters.Add("seconddate", SecondDate);
//        clientValidationRule.ValidationParameters.Add("compare", Compare);
//        return new[] { clientValidationRule };
//    }
//}


