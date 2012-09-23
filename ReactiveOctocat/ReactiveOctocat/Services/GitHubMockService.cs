using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ReactiveOctocat.Services
{
    internal class GitHubMockService : IGitHubService
    {
        private int _current = 1;
        Dictionary<int, Repository[]> _repositories = new Dictionary<int, Repository[]>
        {
            {1, new []
            {
                new Repository { Name = "jQuery", Language = "JavaScript"},
                new Repository { Name = "ReactiveUI", Language = "C#"},
                new Repository { Name = "Bootsrap", Language = "JavaScript"},
                new Repository { Name = "Homebrew", Language = "Ruby"},
            }},
                {2, new []
            {
                new Repository { Name = "Rails", Language = "Ruby"},
                new Repository { Name = "html5-boilerplate", Language = "JavaScript"},
                new Repository { Name = "SuiteValue.UI.Metro", Language = "C#"},
                new Repository { Name = "Node", Language = "JavaScript"},
            }},
                {3, new []
            {
                new Repository { Name = "ImpressJS", Language = "JavaScript"},
                new Repository { Name = "CodeCommander", Language = "C#"},
                new Repository { Name = "Backbone", Language = "JavaScript"},
                new Repository { Name = "Linux", Language = "C"},
            }}
        };

        public User Login(string userName, string password)
        {
            Thread.Sleep(2000);
            return new User {Name = userName};
        }

        public Repository[] GetRepositories(User user)
        {
            _current++;
            if (_current == 4) _current = 1;
            Thread.Sleep(2000);
            return _repositories[_current];
        }
    }

    internal interface IGitHubService
    {
        User Login(string userName, string password);
        Repository[] GetRepositories(User user);
    }

    internal class Repository
    {
        public string Name { get; set; }
        public string Language { get; set; }
    }

    internal class User
    {
        protected bool Equals(User other)
        {
            return string.Equals(Name, other.Name);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }

        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            return (obj as User).Name == Name;
        }
    }

}
