/// Copyright 2021 Henri Vainio 
using Newtonsoft.Json;

namespace MekPathLibraryTests.Check
{
    /// <summary>
    /// Parse version info from github api. Get data with .Data
    /// </summary>
    internal class GithubApiDataParser
    {
        /// <summary>
        /// Parse github "latest version" api response. 
        /// Throws DataParseException if in debug and bad response json
        /// </summary>
        /// <param name="responseJson"></param>
        /// <exception cref="DataParseException"></exception>
        internal GithubApiDataParser(string responseJson)
        {
            Parse(responseJson);
        }

        /// <summary>
        /// Get parsed data, returns null if unsuccesful
        /// </summary>
        internal ParsedVersionData? Data { get; private set; } = null;

        /// <summary>
        /// Parse json into ParsedVersionData
        /// </summary>
        /// <param name="responseJson"></param>
        /// <exception cref="DataParseException"></exception>
        private void Parse(string responseJson)
        {
            try
            {
                Data = JsonConvert.DeserializeObject<ParsedVersionData>(responseJson);
            }

            catch (Exception ex)
            {
                Console.WriteLine(AppError.Text($"Exception was thrown whilst parsing: {ex.Message}"));
            }

            if (Data is null)
            {
                Console.WriteLine(AppError.Text($"Parse error => Data is null"));
#if DEBUG
                throw new DataParseException(AppError.Text($"Bad {nameof(responseJson)}. Can't parse."));
#endif
            }
        }
    }
}
