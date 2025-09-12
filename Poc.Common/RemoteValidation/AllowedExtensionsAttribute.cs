using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Poc.Common.RemoteValidation
{
    public class AllowedExtensionsAttribute(string[] extensions, int maxfileSizeInMB) : ValidationAttribute, IClientModelValidator
    {
        private readonly string[] _extensions = extensions.Select(e => e.ToLower()).ToArray();
        private readonly long _maxSizeBytes = maxfileSizeInMB * 1024 * 1024; // Convert MB to bytes

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            IFormFile? file = value as IFormFile;

            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);

                if (!_extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult($"Only {string.Join(", ", _extensions)} files are allowed.");
                }

                if (file.Length > _maxSizeBytes)
                {
                    return new ValidationResult(
                        $"Maximum allowed file size is {_maxSizeBytes / (1024 * 1024)} MB.");
                }
            }
            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true");

            MergeAttribute(context.Attributes, "data-val-allowedextensionsandsize",
            $"Only {string.Join(", ", _extensions)} files are allowed " +
            $"and max size is {_maxSizeBytes / (1024 * 1024)} MB.");

            MergeAttribute(context.Attributes, "data-val-allowedextensionsandsize-extensions",
                string.Join(",", _extensions));

            MergeAttribute(context.Attributes, "data-val-allowedextensionsandsize-maxsize",
                _maxSizeBytes.ToString());
        }

        private bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        {
            if (attributes.ContainsKey(key))
                return false;

            attributes.Add(key, value);
            return true;
        }
    }
}
