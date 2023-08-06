namespace Finaviaapi.Files
{
    /// <summary>
    /// Handles wrting to file
    /// </summary>
    static public class WriteToFile
    {
        /// <summary>
        /// Writes to file
        /// </summary>
        /// <param name="fileName">Name of the file</param>
        /// <param name="suffix">File type</param>
        /// <param name="stuffToWrite">What to write</param>
        static public void Write(string fileName, string suffix, string stuffToWrite)
        {
            string currentDir;

            currentDir = Directory.GetCurrentDirectory();
            File.WriteAllText(currentDir + "/" + fileName + suffix, stuffToWrite);
        }
    }
}