namespace ShinraManager.Helpers
{
    public interface IClosingHack
    {
        /// <summary>
        /// Executes when window is closing
        /// </summary>
        /// <returns>Whether the windows should be closed by the caller</returns>
        bool OnClosing();
    }
}