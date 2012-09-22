using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReactiveOctocat.Services;
using ReactiveUI;

namespace ReactiveOctocat.ViewModels
{
    class MainViewModel : ReactiveObject
    {
        private IGitHubService _gitHubService = new GitHubMockService();

        public MainViewModel()
        {


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

    }
}
