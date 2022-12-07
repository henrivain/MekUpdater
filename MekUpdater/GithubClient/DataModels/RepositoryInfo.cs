using System.Text.Json.Serialization;
using MekUpdater.GithubClient.DataModels;
using Newtonsoft.Json;

namespace MekUpdater.GithubClient.DataModel;

/// <summary>
/// Content structure for github repository api response
/// </summary>
public class RepositoryInfo
{

    /// <summary>
    /// Repository Id
    /// </summary>
    [JsonProperty("id")]
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Repository name
    /// </summary>
    [JsonProperty("name")]
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Full name of repository (ownerName/repoName)
    /// </summary>
    [JsonProperty("full_name")]
    [JsonPropertyName("full_name")]
    public string? FullName { get; set; }
    
    /// <summary>
    /// Is repository set as private 
    /// </summary>
    [JsonProperty("private")]
    [JsonPropertyName("private")]
    public bool IsPrivate { get; set; }

    /// <summary>
    /// Url path to repositorys visual html page 
    /// </summary>
    [JsonProperty("html_url")]
    [JsonPropertyName("html_url")]
    public string? HtmlUrl { get; set; }

    /// <summary>
    /// Description of the repository 
    /// </summary>
    [JsonProperty("description")]
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Bool representing if repository is fork 
    /// </summary>
    [JsonProperty("fork")]
    [JsonPropertyName("fork")]
    public bool IsFork { get; set; }

    /// <summary>
    /// Repository's github api url path 
    /// </summary>
    [JsonProperty("url")]
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    /// <summary>
    /// Repository's url path to its forks 
    /// </summary>
    [JsonProperty("forks_url")]
    [JsonPropertyName("forks_url")]
    public string? ForksUrl { get; set; }

    /// <summary>
    /// Repository's url path to its events 
    /// </summary>
    [JsonProperty("events_url")]
    [JsonPropertyName("events_url")]
    public string? EventsUrl { get; set; }

    /// <summary>
    /// Repository's url path to release tags
    /// </summary>
    [JsonProperty("tags_url")]
    [JsonPropertyName("tags_url")]
    public string? ReleaseTagsUrl { get; set; }
    
    /// <summary>
    /// Repository's url path to list of programming languages used
    /// </summary>
    [JsonProperty("languages_url")]
    [JsonPropertyName("languages_url")]
    public string? ProgrammingLanguagesUsedUrl { get; set; }

    /// <summary>
    /// Repository's url path to list of users that have starred the repository
    /// </summary>
    [JsonProperty("stargazers_url")]
    [JsonPropertyName("stargazers_url")]
    public string? StarGazersUrl { get; set; }

    /// <summary>
    /// Repository's url path to list of users that have contributed to the repository
    /// </summary>
    [JsonProperty("contributors_url")]
    [JsonPropertyName("contributors_url")]
    public string? ContributorsUrl { get; set; }

    /// <summary>
    /// Url to use for viewing issues with issue number
    /// </summary>
    [JsonProperty("issues_url")]
    [JsonPropertyName("issues_url")]
    public string? IssuesUrl { get; set; }

    /// <summary>
    /// Datetime for when repository was created
    /// </summary>
    [JsonProperty("created_at")]
    [JsonPropertyName("created_at")]
    public DateTime? CreationDate { get; set; }

    /// <summary>
    /// Datetime for when repository was last time updated
    /// </summary>
    [JsonProperty("updated_at")]
    [JsonPropertyName("updated_at")]
    public DateTime? LastUpdateDate { get; set; }

    /// <summary>
    /// Datetime for when repository had its last push
    /// </summary>
    [JsonProperty("pushed_at")]
    [JsonPropertyName("pushed_at")]
    public DateTime? LastPushDate { get; set; }

    /// <summary>
    /// Url to use for git actions
    /// </summary>
    [JsonProperty("git_url")]
    [JsonPropertyName("git_url")]
    public string? GitUrl { get; set; }

    /// <summary>
    /// Url to use with ssh
    /// </summary>
    [JsonProperty("ssh_url")]
    [JsonPropertyName("ssh_url")]
    public string? SshUrl { get; set; }

    /// <summary>
    /// Url to use when cloning repository
    /// </summary>
    [JsonProperty("clone_url")]
    [JsonPropertyName("clone_url")]
    public string? CloneUrl { get; set; }

    /// <summary>
    /// Link to repository home page (outside github)
    /// </summary>
    [JsonProperty("homepage")]
    [JsonPropertyName("homepage")]
    public object? HomePageUrl { get; set; }

    /// <summary>
    /// Size of this page maybe ???
    /// </summary>
    [JsonProperty("size")]
    [JsonPropertyName("size")]
    public int Size { get; set; }

    /// <summary>
    /// Number of stargazers
    /// </summary>
    [JsonProperty("stargazers_count")]
    [JsonPropertyName("stargazers_count")]
    public int StargazerCount { get; set; }

    /// <summary>
    /// Number of people watching repository currently
    /// </summary>
    [JsonProperty("watchers_count")]
    [JsonPropertyName("watchers_count")]
    public int WatcherCount { get; set; }

    /// <summary>
    /// Programming language used in repository
    /// </summary>
    [JsonProperty("language")]
    [JsonPropertyName("language")]
    public string? ProgrammingLanguage { get; set; }

    /// <summary>
    /// Does repository have any issues
    /// </summary>
    [JsonProperty("has_issues")]
    [JsonPropertyName("has_issues")]
    public bool HasIssues { get; set; }

    /// <summary>
    /// Does repository have wiki pages
    /// </summary>
    [JsonProperty("has_wiki")]
    [JsonPropertyName("has_wiki")]
    public bool HasWiki { get; set; }

    /// <summary>
    /// Does repository have discussions enabled
    /// </summary>
    [JsonProperty("has_discussions")]
    [JsonPropertyName("has_discussions")]
    public bool HasDiscussions { get; set; }

    /// <summary>
    /// Number of forks
    /// </summary>
    [JsonProperty("forks_count")]
    [JsonPropertyName("forks_count")]
    public int ForkCount { get; set; }

    /// <summary>
    /// Is repository archived by owner
    /// </summary>
    [JsonProperty("archived")]
    [JsonPropertyName("archived")]
    public bool IsArchived { get; set; }
    
    /// <summary>
    /// Is repository disabled by owner
    /// </summary>
    [JsonProperty("disabled")]
    [JsonPropertyName("disabled")]
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Number of open issues
    /// </summary>
    [JsonProperty("open_issues_count")]
    [JsonPropertyName("open_issues_count")]
    public int OpenIssueCount { get; set; }

    /// <summary>
    /// Can users fork the repository
    /// </summary>
    [JsonProperty("allow_forking")]
    [JsonPropertyName("allow_forking")]
    public bool IsForkingAllowed { get; set; }

    /// <summary>
    /// Bool representing if repository is a template repository
    /// </summary>
    [JsonProperty("is_template")]
    [JsonPropertyName("is_template")]
    public bool IsTemplateRepository { get; set; }

    /// <summary>
    /// Repository visibility public/private
    /// </summary>
    [JsonProperty("visibility")]
    [JsonPropertyName("visibility")]
    public string? Visibility { get; set; }

    /// <summary>
    /// Name of the default branch
    /// </summary>
    [JsonProperty("default_branch")]
    [JsonPropertyName("default_branch")]
    public string? DefaultBranchName { get; set; }


    /// <summary>
    /// Owner of the repository
    /// </summary>
    [JsonProperty("owner")]
    [JsonPropertyName("owner")]
    public Owner? Owner { get; set; }

    /// <summary>
    /// Licence of github repository
    /// </summary>
    [JsonProperty("licence")]
    [JsonPropertyName("licence")]
    public Licence? Licence { get; set; }

}
