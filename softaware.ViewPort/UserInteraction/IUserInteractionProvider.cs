using System.Threading.Tasks;

namespace softaware.ViewPort.UserInteraction
{
    /// <summary>
    /// Abstraction for typical user interactions (showing messages, selecting files or folders, ...).
    /// </summary>
    public interface IUserInteractionProvider
    {
        /// <summary>
        /// Shows a message to the user and asks for his input.
        /// </summary>
        /// <param name="title">The message title.</param>
        /// <param name="message">The message.</param>
        /// <param name="options">The available options.</param>
        /// <param name="defaultResult">The default result.</param>
        /// <returns>The selected result.</returns>
        Task<UserInputResult> GetUserInputAsync(string title, string message, UserInputOption options, UserInputResult defaultResult);

        /// <summary>
        /// Asks the user to select a folder.
        /// </summary>
        /// <returns>The path to the folder.</returns>
        Task<string> GetFolderAsync();

        /// <summary>
        /// Asks the user to select a folder with a preselected known location.
        /// </summary>
        /// <param name="startFolder">The start folder.</param>
        /// <returns>The path to the folder.</returns>
        Task<string> GetFolderAsync(KnownFolder startFolder);

        /// <summary>
        /// Asks the user to select a file with read access.
        /// </summary>
        /// <param name="fileTypes">The file types.</param>
        /// <returns>A <see cref="UserFile"/> instance or <c>null</c>.</returns>
        Task<UserFile> GetFileToReadAsync(params FileType[] fileTypes);

        /// <summary>
        /// Asks the user to select a file with read access with a preselected known location.
        /// </summary>
        /// <param name="startFolder">The start folder.</param>
        /// <param name="fileTypes">The file types.</param>
        /// <returns>A <see cref="UserFile"/> instance or <c>null</c>.</returns>
        Task<UserFile> GetFileToReadAsync(KnownFolder startFolder, params FileType[] fileTypes);

        /// <summary>
        /// Asks the user to select a file with write access.
        /// </summary>
        /// <param name="suggestedFileName">The suggested file name.</param>
        /// <param name="fileTypes">The file types.</param>
        /// <returns>A <see cref="UserFile"/> instance or <c>null</c>.</returns>
        Task<UserFile> GetFileToWriteAsync(string suggestedFileName, params FileType[] fileTypes);

        /// <summary>
        /// Asks the user to select a file with write access with a preselected known location.
        /// </summary>
        /// <param name="suggestedFileName">The suggested file name.</param>
        /// <param name="startFolder">The start folder.</param>
        /// <param name="fileTypes">The file types.</param>
        /// <returns>A <see cref="UserFile"/> instance or <c>null</c>.</returns>
        Task<UserFile> GetFileToWriteAsync(string suggestedFileName, KnownFolder startFolder, params FileType[] fileTypes);
    }
}
