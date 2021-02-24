using Indexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Watson.ORM.Core;

namespace EronNew.Models
{        

    /// <summary>
    /// A document that has been indexed by the index engine.
    /// </summary>
    ///
    [Table("Search_RawData")]
    public class Document
    {

        #region Public-Members
        /// <summary>
        /// Integer uniquely representing the document; typically used if stored in a database.
        /// </summary>
        [Column("Id", true, DataTypes.Int, false)]
        [JsonIgnore]
        public int Id { get; set; } = 0;

        /// <summary>
        /// Title of the document.  Supplied by the caller.
        /// </summary>
        [Column("Title", false, DataTypes.Nvarchar, 256, true)]
        public string Title { get; set; } = null;

        /// <summary>
        /// Free-form text description of the document.  Supplied by the caller.
        /// </summary>
        [Column("Description", false, DataTypes.Nvarchar, 1024, true)]
        public string Description { get; set; } = null;

        /// <summary>
        /// URL or other handle to use when attempting to reach the content.  Supplied by the caller.
        /// </summary>
        [Column("Handle", false, DataTypes.Nvarchar, 256, true)]
        public string Handle { get; set; } = null;

        /// <summary>
        /// Source of the content, i.e. YouTube, Vimeo, web, etc.  Supplied by the caller.
        /// </summary>
        [Column("Source", false, DataTypes.Nvarchar, 32, true)]
        public string Source { get; set; } = null;

        /// <summary>
        /// Free-form text describing who added the document.  Supplied by the caller.
        /// </summary>
        [Column("AddedUser", false, DataTypes.Nvarchar, 32, true)]
        public string AddedBy { get; set; } = null;

        /// <summary>
        /// GUID for this document.  Assigned by the index engine.
        /// </summary>
        [Column("RawKey", false, DataTypes.Nvarchar, 64, false)]
        public string GUID { get; set; } = null;

        /// <summary>
        /// UTC timestamp when the document was added.  Assigned by the index engine.
        /// </summary>
        [Column("Added", false, DataTypes.DateTime, false)]
        public DateTime Added { get; set; } = DateTime.Now.ToUniversalTime();

        /// <summary>
        /// Document contents in byte array form.  
        /// </summary>
        public byte[] Data { get; set; } = null;

        [Column("RawText", false, DataTypes.Varchar, 4000, false)]
        public string RawText { get; set; }


        #endregion

        #region Private-Members

        #endregion

        #region Constructors-and-Factories

        /// <summary>
        /// Creates an empty Document object.  Creation of the object structure is relegated to the application using the object.
        /// </summary>
        public Document()
        {

        }

        /// <summary>
        /// Creates a populated Document object.  
        /// </summary>
        /// <param name="guid">Globally unique identifier for the document.  If one is not supplied, IndexEngine will supply one.</param>
        /// <param name="title">Non-nullable title of the document.</param>
        /// <param name="description">Description of the document.</param>
        /// <param name="handle">Non-nullable URL or other handle to access the document on persistent storage (managed by the caller).</param>
        /// <param name="source">Source of the document (managed by the caller)></param>
        /// <param name="addedBy">Name of the user adding the document (managed by the caller).</param>
        /// <param name="data">Byte array data from the source document.</param>
        public Document(
            string guid,
            string title,
            string description,
            string handle,
            string source,
            string addedBy,
            byte[] data)
        {
            if (String.IsNullOrEmpty(title)) throw new ArgumentNullException(nameof(title));
            if (String.IsNullOrEmpty(handle)) throw new ArgumentNullException(nameof(handle));

            Title = title;
            Description = description;
            Handle = handle;
            Source = source;
            AddedBy = addedBy;
            if (!String.IsNullOrEmpty(guid)) GUID = guid;
            else GUID = Guid.NewGuid().ToString();
            Added = DateTime.Now.ToUniversalTime();
            Data = data;
            string rawText = Regex.Replace(System.Text.Encoding.Default.GetString(data), @"[#*-_'%^]", "");
            if (rawText.Length > 4000) RawText = rawText.Replace("  ", "").Substring(0, 4000);
            else RawText = rawText.Replace("  ", "");
        }

        #endregion

        #region Public-Methods

        /// <summary>
        /// Retrieve a JSON string of the object.
        /// </summary>
        /// <returns>JSON string.</returns>
        public string ToJson()
        {
            return Common.SerializeJson(this, true);
        }

        #endregion

        #region Private-Methods

        private static char[] _TermDelimiters = new char[]
            {
                '!',
                '\"',
                '#',
                '$',
                '%',
                '&',
                '\'',
                '(',
                ')',
                '*',
                '+',
                ',',
                '-',
                '.',
                '/',
                ':',
                ';',
                '<',
                '=',
                '>',
                '?',
                '@',
                '[',
                '\\',
                ']',
                '^',
                '_',
                '`',
                '{',
                '|',
                '}',
                '~',
                ' ',
                '\'',
                '\"',
                '\u001a',
                '\r',
                '\n',
                '\t'
            };


        #endregion
    }
}
