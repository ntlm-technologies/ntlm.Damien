namespace ntlm.Damien
{

    /// <summary>
    /// Github settings.
    /// </summary>
    public class GithubSettings
    {

        public string? Organization { get; set; }


        /// <summary>
        /// Name of the setting.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Url of txt files listing the urls of repositories to clone.
        /// </summary>
        public string? UrlUrls { get; set; }

        /// <summary>
        /// Base github url of an organization housing the repositories.
        /// </summary>
        public string? OrganizationUrl { get; set; }

        /// <summary>
        /// The clients.
        /// 
        /// A solution can have multiple repositories of multiple clients.
        /// 
        /// Take:
        /// lgi.Wines
        /// ntlm.Wines
        /// 
        /// - If Clients are empty 
        ///   both are cloned in base directory.
        /// 
        /// - If Clients = ["ntlm", "lgi"] 
        /// lgi.Wines is cloned in lgi, ntlm.Wines is cloned in ntlm.
        /// </summary>
        public string[] Clients { get; set; } = [];

        /// <summary>
        /// Branches.
        /// 
        /// Take ["dev", "dev2", "master"] 
        /// 
        /// Will try to checkout dev if exists. I not, dev2 if exists, etc.
        /// </summary>
        public string[] Branches { get; set; } = [];

        public override string ToString() => Name ?? string.Empty;
    }
}
