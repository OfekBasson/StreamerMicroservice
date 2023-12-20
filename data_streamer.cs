namespace Streamer
{
    class DataStreamer
    {
        private int nextImageIndex;
        public int NextImageIndex
        {
            get { return nextImageIndex; }
            set { nextImageIndex = value; }
        }

        public DataStreamer()
        {
            this.NextImageIndex = 0;
            this.DestinationDir = "/Users/wpqbswn/Desktop/Ofek/8200-learning/final_project/data_destination";
        }

        private string destinationDir;
        public string DestinationDir
        {
            get { return destinationDir; }
            set { destinationDir = value; }
        }

        public void StreamData()
        {
            Console.WriteLine($"this.NextImageIndex is: {this.NextImageIndex}");
            string sourceDir = $"/Users/wpqbswn/Desktop/Ofek/8200-learning/final_project/data_origin/{this.NextImageIndex}";

            DirectoryInfo dir = new DirectoryInfo(sourceDir);
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            FileInfo[] filesInsideDir = dir.GetFiles();
            if (!this.DoesDirectoryContainRightFiles(filesInsideDir))
            {
                throw new Exception($"Directory contains wrong files/partial files/unnecessary files.");
            }

            string tragetDirPath = Path.Combine(this.DestinationDir, this.NextImageIndex.ToString());
            Directory.CreateDirectory(tragetDirPath);

            foreach (FileInfo file in filesInsideDir)
            {
                string targetFilePath = Path.Combine(tragetDirPath, file.Name);
                file.CopyTo(targetFilePath);
            }
            this.NextImageIndex += 1;
        }

        private bool DoesDirectoryContainRightFiles(FileInfo[] files)
        {
            Console.WriteLine("Entered the 'DoesDirectoryContainRightFiles' method");
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