using System;
using System.IO;
using System.Threading.Tasks;

namespace ViewPort.UserInteraction
{
    public class UserFile
    {
        private readonly Func<Task<Stream>> openImplementation;

        public UserFile(string path, Func<Task<Stream>> openImplementation)
        {
            this.Path = path ?? throw new ArgumentNullException(nameof(path));
            this.openImplementation = openImplementation ?? throw new ArgumentNullException(nameof(openImplementation));
        }

        public string Path { get; }

        public Task<Stream> OpenAsync() => this.openImplementation();
    }
}