using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReactiveOctocat.Services;

namespace ReactiveOctocat.ViewModels
{
    class MainViewModel 
    {
        private IGitHubService _gitHubService = new GitHubMockService();

        public MainViewModel()
        {
            
        }
    }
}
