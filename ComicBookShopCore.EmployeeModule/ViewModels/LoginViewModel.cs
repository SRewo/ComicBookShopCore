using System;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Interfaces;
using ComicBookShopCore.Data.Repositories;
using ComicBookShopCore.EmployeeModule.Interfaces;
using Microsoft.AspNetCore.Identity;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Unity;
using Unity;

namespace ComicBookShopCore.EmployeeModule.ViewModels
{
    public class LoginViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IOpenable<User> _userRepository;
        private User[] _users;
        public User LoggedUser { get; set; }
        private bool _loggedIn;

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


        public LoginViewModel(IRegionManager manager, IOpenable<User> userRepository, User[] users)
        {

            SignInCommand = new DelegateCommand<object>(SignIn);

            _userRepository = userRepository;
            _regionManager = manager;
            _users = users;

            CheckDb();

        }

        public void SignIn(object obj)
        {
            var passwordContainer = (IContainPassword) obj;
            if (passwordContainer != null)
            {
                var secureString = passwordContainer.Password;
                if (CheckUserExists(Username) && CheckPasswords(LoggedUser, secureString))
                {

                    _loggedIn = true;
                    _users[0] = LoggedUser;
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
            }
            catch (Exception)
            {

            }
        }

        public bool CheckUserExists(string login)
        {
                
            LoggedUser = _userRepository.GetAll().FirstOrDefault(x => x.UserName == login);
            

            return LoggedUser != null;

        }

        public static bool CheckPasswords(User emp, SecureString securePassword)
        {
            var hasher = new PasswordHasher<User>();
            return hasher.VerifyHashedPassword(emp, emp.PasswordHash, new System.Net.NetworkCredential(string.Empty, securePassword).Password) == PasswordVerificationResult.Success;

        }

        public void CheckDb()
        {

            CanLogIn = _userRepository.CanOpen();
            if (!CanLogIn)
            {
                ErrorMessage = "Unable to connect with database. Please restart program and try again.";
            }

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
    }
}
