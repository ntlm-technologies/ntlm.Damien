namespace ntlm.Damien
{
    /// <summary>
    /// Represents a client.
    /// </summary>
    public class Client(string name)
    {

        /// <summary>
        /// Name of the client.
        /// </summary>
        public string Name { get; set; } = name;

        /// <summary>
        /// The extra repositories.
        /// Client will have access to all it's repositories
        /// (ie LGI has access to LGI.Wines, LGI.Wines.Web, etc.)
        /// and some extra repositories (ie ntlm.Wines, ntlm.Wines.Web, etc.)
        /// </summary>
        public string[] ExtraRepositories { get; set; } = [];

        /// <summary>
        /// Name of the team with read role.
        /// </summary>
        public string ReadTeam => $"{Name}.read";

        /// <summary>
        /// Name of the team with write role.
        /// </summary>
        public string WriteTeam => $"{Name}.write";

        /// <summary>
        /// Name of the team with admin role.
        /// </summary>
        public string AdminTeam => $"{Name}.admin";

        /// <summary>
        /// Teams of the client.
        /// </summary>
        public string[] Teams => [ReadTeam, WriteTeam, AdminTeam];


        public override string ToString() => Name;

    }
}
