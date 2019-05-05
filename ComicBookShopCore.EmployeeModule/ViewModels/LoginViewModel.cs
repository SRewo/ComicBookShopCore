using System;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;
using ComicBookShopCore.Data.Repositories;
using ComicBookShopCore.EmployeeModule.Interfaces;
using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ComicBookShopCore.EmployeeModule.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private IRepository<Employee> _employeeRepository;
        private Employee _loggedEmployee;

        public DelegateCommand<object> SignInCommand { get; set; }

        private string _username;

        public string Username
        {
            get =>_username;
            set => SetProperty(ref _username, value);
        }

        private string _errorMessage;

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        private bool _canLogIn;

        public bool CanLogIn
        {
            get => _canLogIn;
            set => SetProperty(ref _canLogIn, value);
        }



        public LoginViewModel(IRegionManager manager, IRepository<Employee> employeeRepository)
        {

            SignInCommand = new DelegateCommand<object>(SignIn);

            _employeeRepository = employeeRepository;
            _regionManager = manager;
            
            CheckDb();

        }

        public void SignIn(object obj)
        {
            var passwordContainer = (IContainPassword) obj;
            if (passwordContainer != null)
            {
                var secureString = passwordContainer.Password;
                if (CheckUserExists(Username) && CheckPasswords(_loggedEmployee, secureString))
                {

                    GlobalVariables.LoggedEmployee = _loggedEmployee;
                    Redirect();

                }
                else
                {
                    ErrorMessage = "Invalid Username or password!";
                }
                
            }
            else
            {
                ErrorMessage = "Invalid Username or password!";
            }
            
        }

        public void Redirect()
        {
            try
            {
                var activeRegion = _regionManager.Regions["content"].ActiveViews.FirstOrDefault();
                _regionManager.Regions["content"].Deactivate(activeRegion);

                _regionManager.RequestNavigate("menu", "MenuView");
            }catch(Exception ex)
            {

            }
        }

        public bool CheckUserExists(string login)
        {
                
            _loggedEmployee = _employeeRepository.GetAll().FirstOrDefault(x => x.Login == login);

            return _loggedEmployee != null;

        }

        public static bool CheckPasswords(Employee emp, SecureString securePassword)
        {

            return emp.Password.ToUpper() == Encrypt(new System.Net.NetworkCredential(string.Empty, securePassword).Password);

        }

        public static string Encrypt(string password)
        {
            var stringBuilder = new StringBuilder();
            var data = Encoding.UTF8.GetBytes(password);
            data = new SHA256Managed().ComputeHash(data);
            foreach (var b in data)
            {
                stringBuilder.Append(b.ToString("x2"));
            }
            return stringBuilder.ToString().ToUpper();
        }

        public void CheckDb()
        {

            CanLogIn = _employeeRepository.CanOpen();
            if (!CanLogIn)
            {
                ErrorMessage = "Unable to connect with database. Please restart program and try again.";
            }

        }
    }
}
