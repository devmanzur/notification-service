using FluentValidation.Results;

namespace OrganizationNotificationService.Utils;

public static class ErrorFormatterUtils
{
    public const string ValidationErrorSeparator = ";";
    public const string DefaultErrorParamKey = "Request";

    // generate serialized error string from fluent validation error
    public static string ToSerializedErrors(this List<ValidationFailure> errors)
    {
        if (!errors.Any())
        {
            return "Validation failed";
        }

        var errorMessages = errors.Select(x => $"{x.PropertyName}:{x.ErrorMessage}").ToList();
        return string.Join(ValidationErrorSeparator, errorMessages);
    }

    //generate problem detail style errors from serialized error string
    public static Dictionary<string, List<string>>? ToProblemDetailFormattedErrors(string? errorMessage)
    {
        if (string.IsNullOrEmpty(errorMessage))
        {
            return null;
        }

        var detailedError = new Dictionary<string, List<string>>();
        var errors = errorMessage.Split(ValidationErrorSeparator);

        foreach (var error in errors)
        {
            var split = error.Split(":");
            string key;
            string errorDetail;

            if (split.Length == 2)
            {
                key = split[0];
                errorDetail = split[1];
            }
            else
            {
                key = DefaultErrorParamKey;
                errorDetail = error;
            }

            if (detailedError.ContainsKey(key))
            {
                var errorList = detailedError[key];
                errorList.Add(errorDetail);
                detailedError[key] = errorList;
            }
            else
            {
                detailedError[key] = new List<string>()
                {
                    errorDetail
                };
            }
        }

        return detailedError;
    }
}