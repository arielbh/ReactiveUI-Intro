using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
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
        private MemoizingMRUCache<User, Repository[]>  _cache;

        public MainViewModel()
        {
            IsInProgress = Visibility.Collapsed;

            LoginCommand = new ReactiveAsyncCommand(this.WhenAny(t => t.UserName, t => t.Password, (x, y) => !string.IsNullOrEmpty(x.Value) && !string.IsNullOrEmpty(y.Value)));

            LoginCommand.ItemsInflight.Select(x => x > 0 ? Visibility.Visible : Visibility.Collapsed).Subscribe(x => IsInProgress = x);
            LoginCommand.RegisterAsyncFunction(_ => _gitHubService.Login(UserName, Password)).Subscribe(
                u => LoggedInUser = u);

            this.ObservableForProperty(x => x.LoggedInUser,
                                       user => user == null ? Visibility.Hidden : Visibility.Visible).Subscribe(v => IsUserLoggedIn = v);


            _cache = new MemoizingMRUCache<User, Repository[]>((user,_) =>
                                                                          _gitHubService.GetRepositories(user), 3);

            this.WhenAny(t => t.LoggedInUser, u => u.Value != null).Where(filter => filter).Subscribe(_ =>
                
                    Repositories = new ReactiveCollection<Repository>(_cache.Get(LoggedInUser))
            );
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
