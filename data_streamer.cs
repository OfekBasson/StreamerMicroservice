namespace Streamer
{
    class DataStreamer
    {
        private int nextImageIndex;
        private string destinationDir;

        public DataStreamer()
        {
            nextImageIndex = 0;
            destinationDir = "/Users/wpqbswn/Desktop/Ofek/8200-learning/final_project/data_destination";
        }

        public string StreamDataAndReturnImageId()
        {
            string sourceDir = $"/Users/wpqbswn/Desktop/Ofek/8200-learning/final_project/data_origin/{nextImageIndex}";
            FileInfo[] filesInsideDir = GetFilesFromDir(sourceDir);
            string tragetDirPath = GetDestinationDirectoryPath();
            CopyFilesToDestinationDir(filesInsideDir, tragetDirPath);
            string imageID = nextImageIndex.ToString();
            nextImageIndex += 1;
            return imageID;
        }

        private void CopyFilesToDestinationDir(FileInfo[] files, string tragetDirPath)
        {
            foreach (FileInfo file in files)
            {
                string targetFilePath = Path.Combine(tragetDirPath, file.Name);
                file.CopyTo(targetFilePath);
            }
        }

        private string GetDestinationDirectoryPath()
        {
            string tragetDirPath = Path.Combine(destinationDir, nextImageIndex.ToString());
            if (IsDirectoryAlreadyCopiedToDestination(tragetDirPath))
                throw new Exception($"Directory already exists in the 'data_destination' folder");
            else
            {
                Directory.CreateDirectory(tragetDirPath);
            }
            return tragetDirPath;
        }

        private FileInfo[] GetFilesFromDir(string sourceDir)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDir);
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            FileInfo[] filesInsideDir = dir.GetFiles();
            if (!DoesDirectoryContainRightFiles(filesInsideDir))
                throw new Exception($"Directory contains wrong files/partial files/unnecessary files.");

            return filesInsideDir;
        }

        private bool IsDirectoryAlreadyCopiedToDestination(string dirPath)
        {
            DirectoryInfo dir = new DirectoryInfo(dirPath);
            return dir.Exists;
        }

        private bool DoesDirectoryContainRightFiles(FileInfo[] files)
        {
            bool doesDirectoryContainImage = false;
            bool doesDirectoryContainJson = false;
            bool doesDirectoryContainUnnecessaryFiles = false;
            bool doesDirectoryContainDSStore = false;

            foreach (FileInfo file in files)
            {
                if (file.Name.EndsWith(".json"))
                {
                    doesDirectoryContainJson = true;
                    continue;
                }
                else if (file.Name.EndsWith(".png") || file.Name.EndsWith(".jpg") || file.Name.EndsWith(".jpeg") || file.Name.EndsWith(".cog"))
                {
                    doesDirectoryContainImage = true;
                    continue;
                }

                else if (file.Name == ".DS_Store")
                {
                    doesDirectoryContainDSStore = true;
                    continue;
                }
                else
                    doesDirectoryContainUnnecessaryFiles = true;
            }

            return doesDirectoryContainImage
            & doesDirectoryContainJson
            & (files.Length == 2 || (files.Length == 3 && doesDirectoryContainDSStore))
            & !doesDirectoryContainUnnecessaryFiles
            ;
        }
    }
}