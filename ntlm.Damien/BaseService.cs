namespace ntlm.Damien
{
    public class BaseService
    {
        #region Log

        /// <summary>
        /// The warnings.
        /// </summary>
        public static List<string> Warnings { get; } = [];

        /// <summary>
        /// Warns.
        /// </summary>
        /// <param name="text"></param>
        public void Warn(string text)
        {
            Log(text);
            Warnings.Add(text);
        }

        /// <summary>
        /// Log progress.
        /// </summary>
        /// <param name="text"></param>
        public void Log(string text)
            => Logger?.WriteLine(text);

        /// <summary>
        /// Logger.
        /// </summary>
        public TextWriter Logger { get; set; } = Console.Out;

        #endregion




        #region Progress

        public event EventHandler<int> ProgressChanged = delegate { };

        protected virtual void OnProgressChanged(object sender, int progress)
        {
            ProgressChanged?.Invoke(sender, progress);
        }

        public event EventHandler<string> Warned = delegate { };

        #endregion
    }

}
