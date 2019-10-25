using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ViewPort.UserInteraction;

namespace ViewPort.Wpf.UserInteraction
{
    /// <summary>
    /// WPF implementation of the <see cref="IUserInteractionProvider"/> interface.
    /// </summary>
    public class WpfUserInteractionProvider : IUserInteractionProvider
    {
        /// <inheritdoc />
        public Task<UserInputResult> GetUserInputAsync(string title, string message, UserInputOption options, UserInputResult defaultResult)
        {
            var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
            return Task.Factory.StartNew(
                () =>
                {
                    MessageBoxResult result = MessageBox.Show(Application.Current.MainWindow, message, title, (MessageBoxButton)options, MessageBoxImage.None, (MessageBoxResult)defaultResult);
                    return (UserInputResult)result;
                },
                CancellationToken.None,
                TaskCreationOptions.None,
                scheduler);
        }

        /// <inheritdoc />
        public Task<UserFile> GetFileToReadAsync(params FileType[] fileTypes)
        {
            var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
            return Task.Factory.StartNew<UserFile>(
                delegate
                {
                    FileDialog dialog = new OpenFileDialog()
                    {
                        AddExtension = true,
                        DefaultExt = fileTypes.First().Extensions.First(),
                        Filter = string.Join("|", fileTypes.Select(f => f.ToFilter()))
                    };

                    if (dialog.ShowDialog() == true)
                    {
                        var fileName = dialog.FileName;
                        return new UserFile(fileName, () => Task.FromResult<Stream>(File.OpenRead(fileName)));
                    }
                    else
                    {
                        return null;
                    }
                },
                CancellationToken.None,
                TaskCreationOptions.None,
                scheduler);
        }

        /// <inheritdoc />
        public Task<UserFile> GetFileToWriteAsync(string suggestedFileName, params FileType[] fileTypes)
        {
            var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
            return Task.Factory.StartNew(
                () =>
                {
                    FileDialog dialog = new SaveFileDialog()
                    {
                        AddExtension = true,
                        DefaultExt = fileTypes.First().Extensions.First(),
                        FileName = suggestedFileName,
                        Filter = string.Join("|", fileTypes.Select(f => f.ToFilter()))
                    };

                    if (dialog.ShowDialog() == true)
                    {
                        var fileName = dialog.FileName;
                        return new UserFile(fileName, () => Task.FromResult<Stream>(File.Open(fileName, FileMode.Create)));
                    }
                    else
                    {
                        return null;
                    }
                },
                CancellationToken.None,
                TaskCreationOptions.None,
                scheduler);
        }

        /// <inheritdoc />
        public Task<string> GetFolderAsync()
        {
            var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
            return Task.Factory.StartNew(
                delegate
                {
                    var dialog = new System.Windows.Forms.FolderBrowserDialog()
                    {
                        Description = Resources.SelectFolder
                    };

                    return (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) ? dialog.SelectedPath : null;
                },
                CancellationToken.None,
                TaskCreationOptions.None,
                scheduler);
        }

        /// <inheritdoc />
        public Task<UserFile> GetFileToReadAsync(KnownFolder startFolder, params FileType[] fileTypes)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<UserFile> GetFileToWriteAsync(string suggestedFileName, KnownFolder startFolder, params FileType[] fileTypes)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<string> GetFolderAsync(KnownFolder startFolder)
        {
            throw new NotImplementedException();
        }
    }
}
