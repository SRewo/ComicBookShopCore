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
using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ComicBookShopCore.EmployeeModule.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly IRepository<User> _userRepository;
        private User _loggedUser;

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



        public LoginViewModel(IRegionManager manager, IRepository<User> userRepository)
        {

            SignInCommand = new DelegateCommand<object>(SignIn);

            _userRepository = userRepository;
            _regionManager = manager;
            
            CheckDb();

        }

        public void SignIn(object obj)
        {
            var passwordContainer = (IContainPassword) obj;
            if (passwordContainer != null)
            {
                var secureString = passwordContainer.Password;
                if (CheckUserExists(Username) && CheckPasswords(_loggedUser, secureString))
                {

                    GlobalVariables.LoggedUser = _loggedUser;
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
                
            _loggedUser = _userRepository.GetAll().FirstOrDefault(x => x.UserName == login);
            

            return _loggedUser != null;

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
    }
}
