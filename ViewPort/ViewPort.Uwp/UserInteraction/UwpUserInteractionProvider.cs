using System;
using System.IO;
using System.Threading.Tasks;
using ViewPort.UserInteraction;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using System.Linq;
using Windows.ApplicationModel.Resources;

namespace ViewPort.Uwp.UserInteraction
{
    /// <summary>
    /// UWP implementation of the <see cref="IUserInteractionProvider"/> interface.
    /// </summary>
    public class UwpUserInteractionProvider : IUserInteractionProvider
    {
        /// <inheritdoc />
        public async Task<UserInputResult> GetUserInputAsync(string title, string message, UserInputOption options, UserInputResult defaultResult)
        {
            var resources = ResourceLoader.GetForViewIndependentUse("ViewPort.Uwp/Resources");
            MessageDialog dialog = new MessageDialog(message, title);

            switch (options)
            {
                case UserInputOption.Ok:
                    dialog.Commands.Add(new UICommand(resources.GetString("OK")) { Id = UserInputResult.Ok });
                    break;

                case UserInputOption.OkCancel:
                    dialog.Commands.Add(new UICommand(resources.GetString("OK")) { Id = UserInputResult.Ok });
                    dialog.Commands.Add(new UICommand(resources.GetString("Cancel")) { Id = UserInputResult.Cancel });
                    dialog.CancelCommandIndex = 1;
                    if (defaultResult == UserInputResult.Ok)
                        dialog.DefaultCommandIndex = 0;
                    else
                        dialog.DefaultCommandIndex = 1;
                    break;

                case UserInputOption.YesNo:
                    dialog.Commands.Add(new UICommand(resources.GetString("Yes")) { Id = UserInputResult.Yes });
                    dialog.Commands.Add(new UICommand(resources.GetString("No")) { Id = UserInputResult.No });
                    dialog.CancelCommandIndex = 1;
                    if (defaultResult == UserInputResult.Yes)
                        dialog.DefaultCommandIndex = 0;
                    else
                        dialog.DefaultCommandIndex = 1;
                    break;

                case UserInputOption.YesNoCancel:
                    dialog.Commands.Add(new UICommand(resources.GetString("Yes")) { Id = UserInputResult.Yes });
                    dialog.Commands.Add(new UICommand(resources.GetString("No")) { Id = UserInputResult.No });
                    dialog.Commands.Add(new UICommand(resources.GetString("Cancel")) { Id = UserInputResult.Cancel });
                    dialog.CancelCommandIndex = 2;
                    switch (defaultResult)
                    {
                        case UserInputResult.Yes:
                            dialog.DefaultCommandIndex = 0;
                            break;
                        case UserInputResult.No:
                            dialog.DefaultCommandIndex = 1;
                            break;
                        default:
                            dialog.DefaultCommandIndex = 2;
                            break;
                    }
                    break;
            }

            IUICommand command = await dialog.ShowAsync();
            return (UserInputResult)command.Id;
        }

        /// <inheritdoc />
        public Task<string> GetFolderAsync()
        {
            return this.GetFolderAsync(default);
        }

        /// <inheritdoc />
        public Task<UserFile> GetFileToReadAsync(params FileType[] fileTypes)
        {
            return this.GetFileToReadAsync(default, fileTypes);
        }

        /// <inheritdoc />
        public Task<UserFile> GetFileToWriteAsync(string suggestedFileName, params FileType[] fileTypes)
        {
            return this.GetFileToWriteAsync(suggestedFileName, default, fileTypes);
        }

        /// <inheritdoc />
        public async Task<string> GetFolderAsync(KnownFolder startFolder)
        {
            var picker = new FolderPicker()
            {
                ViewMode = PickerViewMode.List,
                SuggestedStartLocation = (PickerLocationId)startFolder
            };

            picker.FileTypeFilter.Add("."); // folders only

            var folder = await picker.PickSingleFolderAsync();
            return folder.Path;
        }

        /// <inheritdoc />
        public async Task<UserFile> GetFileToReadAsync(KnownFolder startFolder, params FileType[] fileTypes)
        {
            var picker = new FileOpenPicker()
            {
                SuggestedStartLocation = (PickerLocationId)startFolder
            };

            foreach (var fileType in fileTypes.SelectMany(f => f.Extensions))
            {
                picker.FileTypeFilter.Add(fileType);
            }

            var file = await picker.PickSingleFileAsync();
            if (file == null)
            {
                return null;
            }
            else
            {
                return new UserFile(file.Path, file.OpenStreamForReadAsync);
            }
        }

        /// <inheritdoc />
        public async Task<UserFile> GetFileToWriteAsync(string suggestedFileName, KnownFolder startFolder, params FileType[] fileTypes)
        {
            var picker = new FileSavePicker()
            {
                SuggestedFileName = suggestedFileName,
                DefaultFileExtension = fileTypes.First().Extensions.First(),
                SuggestedStartLocation = (PickerLocationId)startFolder
            };

            foreach (var fileType in fileTypes)
            {
                picker.FileTypeChoices.Add(fileType.DisplayName, fileType.Extensions);
            }

            var file = await picker.PickSaveFileAsync();
            if (file == null)
            {
                return null;
            }
            else
            {
                return new UserFile(file.Path, file.OpenStreamForWriteAsync);
            }
        }
    }
}
