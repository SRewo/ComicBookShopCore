using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using Prism.Mvvm;

namespace ComicBookShopCore.Data
{
    public class ValidableBase : BindableBase, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _propErrors = new Dictionary<string, List<string>>();

        public bool HasErrors => _propErrors.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged = delegate{};

        public IEnumerable GetErrors(string propertyName)
        {
            if (propertyName != String.Empty && _propErrors.Keys.Any(x => x== propertyName))
            {
                return _propErrors[propertyName];
            }

                return null;
        }

        protected override bool SetProperty<T>(ref T member, T val,
            [CallerMemberName] string propertyName = null)
        {

            ValidateProperty(propertyName, val);

            return base.SetProperty<T>(ref member, val, propertyName);
        }

        private void ValidateProperty<T>(string propertyName, T value)
        {
            if (propertyName != String.Empty)
            {
                var results = new List<ValidationResult>();
                ValidationContext context = new ValidationContext(this)
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
        }

        public string GetFirstError(string propertyName)
        {
            if (_propErrors.ContainsKey(propertyName))
            {
                return _propErrors[propertyName].First();
            }

                return null;
        }
    }
}
