using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ReactiveOctocat.Services;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace ReactiveOctocat.ViewModels
{
    class MainViewModel : ReactiveObject
    {
        private IGitHubService _gitHubService = new GitHubMockService();

        public MainViewModel()
        {
            IsInProgress = Visibility.Collapsed;

            LoginCommand = new ReactiveAsyncCommand(this.WhenAny(t => t.UserName, t => t.Password, (x, y) => !string.IsNullOrEmpty(x.Value) && !string.IsNullOrEmpty(y.Value)));
            LoginCommand.RegisterAsyncAction(_ =>
            {
                IsInProgress = Visibility.Visible;
                LoggedInUser = _gitHubService.Login(UserName, Password);
                IsInProgress = Visibility.Collapsed;
            });

            this.ObservableForProperty(x => x.LoggedInUser,
                                       user => user == null ? Visibility.Hidden : Visibility.Visible).Subscribe(v => IsUserLoggedIn = v);

            this.WhenAny(t => t.LoggedInUser, u => u.Value != null).Subscribe(filter =>
                
            {
                if (filter)
                {
                    Repositories = new ReactiveCollection<Repository>(_gitHubService.GetRepositories(LoggedInUser));
                }
            });
        }

        private string _UserName;

        public string UserName
        {
            get { return _UserName; }
            set { this.RaiseAndSetIfChanged(x => x.UserName, value);
            }
        }

        private string _Password;

        public string Password
        {
            get { return _Password; }
            set { this.RaiseAndSetIfChanged(x => x.Password, value); }
        }

        private User _LoggedInUser;

        public User LoggedInUser
        {
            get { return _LoggedInUser; }
            set { this.RaiseAndSetIfChanged(x => x.LoggedInUser, value); }
        }

        private Visibility _IsUserLoggedIn;

        public Visibility IsUserLoggedIn
        {
            get { return _IsUserLoggedIn; }
            set { this.RaiseAndSetIfChanged(x => x.IsUserLoggedIn, value); }
        }

        private Visibility _IsInProgress;

        public Visibility IsInProgress
        {
            get { return _IsInProgress; }
            set { this.RaiseAndSetIfChanged(x => x.IsInProgress, value); }
        }



        public ReactiveAsyncCommand LoginCommand { get; set; }

        private ReactiveCollection<Repository> _Repositories;

        public ReactiveCollection<Repository> Repositories
        {
            get { return _Repositories; }
            set { this.RaiseAndSetIfChanged(x => x.Repositories, value); }
        }

    }
}
