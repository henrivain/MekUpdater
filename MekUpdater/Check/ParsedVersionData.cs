/// Copyright 2021 Henri Vainio 


namespace MekUpdater.Check
{
    // everything on this class must be public
    // otherwise json can't be converted to this type of object
    // with Newtonsoft.Json.JsonConvert.DeserializeObject<ParsedVersionData>()

    /// <summary>
    /// Data structure for github version api. Some of the fields are removed
    /// </summary>
    public class ParsedVersionData
    {
        /// <summary>
        /// Link to version download page
        /// </summary>
        public string? html_url { get; set; }

        /// <summary>
        /// Version string?
        /// </summary>
        public string? tag_name { get; set; }

        /// <summary>
        /// Branch name 
        /// </summary>
        public string? target_commitish { get; set; }

        /// <summary>
        /// Whole version name
        /// </summary>
        public string? name { get; set; }

        /// <summary>
        /// Is version pre release
        /// </summary>
        public bool prerelease { get; set; }

        /// <summary>
        /// When version was firstly created
        /// </summary>
        public DateTime created_at { get; set; }

        /// <summary>
        /// Publish time
        /// </summary>
        public DateTime published_at { get; set; }

        /// <summary>
        /// Download link (starts download)
        /// </summary>
        public string? zipball_url { get; set; }
    }
}
