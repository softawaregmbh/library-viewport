using System.Collections.Generic;
using System.Linq;

namespace ViewPort.UserInteraction
{
    /// <summary>
    /// Describes a file type.
    /// </summary>
    public class FileType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileType"/> class.
        /// </summary>
        /// <param name="displayName">The display name (for example "Image files" or "All files").</param>
        /// <param name="extensions">The extensions including a dot (for example ".jpg", ".png" or ".*").</param>
        public FileType(string displayName, params string[] extensions)
        {
            this.DisplayName = displayName;
            this.Extensions = new List<string>(extensions);
        }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public string DisplayName { get; }

        /// <summary>
        /// Gets the extensions including a dot (for example ".jpg", ".png" or ".*").
        /// </summary>
        /// <value>
        /// The extensions.
        /// </value>
        public List<string> Extensions { get; }

        /// <summary>
        /// Creates a file type filter string.
        /// </summary>
        /// <returns></returns>
        public string ToFilter()
        {
            return string.Format($"{this.DisplayName}|{string.Join(";", this.Extensions.Select(e => "*" + e))}");
        }
    }
}
