using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ComicBookShopCore.Data
{
    public class ValidationClass : INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _propErrors = new Dictionary<string, List<string>>();

        [JsonIgnore]
        public bool HasErrors => _propErrors.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged = delegate{};

        public IEnumerable GetErrors(string propertyName)
        {
            if (propertyName != string.Empty && _propErrors.Keys.Any(x => x== propertyName))
            {
                return _propErrors[propertyName];
            }

            return null;
        }

        private void ValidateProperty<T>(string propertyName, T value)
        {
            if (propertyName == string.Empty) return;

            var results = new List<ValidationResult>();
            var context = new ValidationContext(this)
            {
                MemberName = propertyName
            };

            Validator.TryValidateProperty(value, context, results);

            if (results.Any())
            {
                _propErrors[propertyName] = results.Select(c => c.ErrorMessage).ToList();
            }
            else
            {
                _propErrors.Remove(propertyName);
            }

            ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public string GetFirstError(string propertyName)
        {
            return _propErrors.ContainsKey(propertyName) ? _propErrors[propertyName].First() : null;
        }

        public string GetFirstError()
        {

            return _propErrors?.FirstOrDefault().Value.FirstOrDefault();

        }

        public void Validate()
        {
            var obj = this;
            foreach (var prop in obj.GetType().GetProperties())
            {
                ValidateProperty(prop.Name, prop.GetValue(obj));
            }
        }
    }
}
