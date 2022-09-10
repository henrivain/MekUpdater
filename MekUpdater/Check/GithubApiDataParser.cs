// Copyright 2021 Henri Vainio 
using MekUpdater.Exceptions;
using MekUpdater.UpdateRunner;
using Newtonsoft.Json;

namespace MekUpdater.Check;

/// <summary>
/// Parse version info from github api. Get data with Parse()
/// </summary>
internal class GithubApiDataParser
{
    internal GithubApiDataParser(string responseJson)
    {
        ResponseJson = responseJson;
    }

    /// <summary>
    /// Json string from github api response
    /// </summary>
    public string ResponseJson { get; }

    /// <summary>
    /// Parse json into ParsedVersionData
    /// </summary>
    /// <returns>
    /// DataParseResult where 'Success' property indicates wheather action was successful or not. 
    /// Parsed data result is part of DataParseResult
    /// </returns>
    internal DataParseResult Parse()
    {
        ParsedVersionData? data;
        try
        {
            data = JsonConvert.DeserializeObject<ParsedVersionData>(ResponseJson);
            if (data is null)
            {
                throw new DataParseException($"Bad {nameof(ResponseJson)}. Can't parse. Json: {ResponseJson}");
            }
        }

        catch (Exception ex)
        {
            return new(false)
            {
                Message = $"Failed to parse string into {nameof(ParsedVersionData)} because of exception {ex}: {ex.Message}",
                UpdateMsg = UpdateMsg.ParseJson
            };
        }
        return new(true) { ParsedVersionData = data };
    }
}
