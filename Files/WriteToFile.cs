namespace Finaviaapi.Files
{
    static public class WriteToFile
    {
        static public void Write(string fileName, string suffix, string stuffToWrite)
        {
            string currentDir;

            currentDir = Directory.GetCurrentDirectory();
            File.WriteAllText(currentDir + "/" + fileName + suffix, stuffToWrite);
        }
    }
}