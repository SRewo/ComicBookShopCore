using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using ComicBookShopCore.Data;
using ComicBookShopCore.Data.Repositories;
using ComicBookShopCore.EmployeeModule.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace ComicBookShopCore.EmployeeModule.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private SqlRepository<Employee> _employeeRepository;
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



        public LoginViewModel(IRegionManager manager)
        {

            SignInCommand = new DelegateCommand<object>(SignIn);
            _regionManager = manager;
            
            CheckDb();

        }

        private void SignIn(object obj)
        {
            var passwordContainer = (IContainPassword) obj;
            if (passwordContainer != null)
            {
                var secureString = passwordContainer.Password;
                if (CheckUserExists(Username) && CheckPasswords(_loggedEmployee,secureString))
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

        private void Redirect()
        {

            var activeRegion = _regionManager.Regions["content"].ActiveViews.FirstOrDefault();
            _regionManager.Regions["content"].Deactivate(activeRegion);

            _regionManager.RequestNavigate("menu", "MenuView");

        }

        private bool CheckUserExists(string login)
        {
                
            using (var context = new ShopDbEntities())
            {

                _employeeRepository = new SqlRepository<Employee>(context);

                _loggedEmployee = _employeeRepository.GetAll().FirstOrDefault(x => x.Login == login);
            }

            return _loggedEmployee != null;

        }

        private static bool CheckPasswords(Employee emp, SecureString securePassword)
        {

            return emp.Password == Encrypt(new System.Net.NetworkCredential(string.Empty, securePassword).Password);

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

        private void CheckDb()
        {
            using (var context = new ShopDbEntities())
            {
                if (!context.Database.CanConnect())
                {
                    ErrorMessage =
                        "Unable to connect with database. Please restart program and try again. If after few tries problem still persists, please call to: 999999";
                    CanLogIn = false;
                }
                else
                {
                    CanLogIn = true;
                }
            }
        }
    }
}
