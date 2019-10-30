using Objectia.Core.Messages;

namespace Objectia.Messages
{
    /// <summary>
    /// Class to represent file attachments loaded from in-memory byte arrays rather than from disk.
    /// </summary>
    public class FileAttachment : IFileAttachment
    {
        /// <summary>
        /// The byte data of the file.
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// The filename of the file to be sent.
        /// </summary>
        public string Name { get; set; }
    }
}