using System.Security.Cryptography.X509Certificates;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;

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
            string sourceDir = $"/Users/wpqbswn/Desktop/Ofek/8200-learning/final_project/data_origin/{this.NextImageIndex}";
            
            DirectoryInfo dir = new DirectoryInfo(sourceDir);
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            FileInfo[] filesInsideDir = dir.GetFiles();
            if(!this.DoesDirectoryContainRightFiles(filesInsideDir))
            {
                throw new Exception($"Directory contains wrong files/partial files/unnecessary files.");
            }
                
            string tragetDirPath = Path.Combine(this.DestinationDir, this.NextImageIndex.ToString());
            Directory.CreateDirectory(tragetDirPath);

            foreach (FileInfo file in filesInsideDir)
            {
                string targetFilePath = Path.Combine(tragetDirPath,file.Name);
                file.CopyTo(targetFilePath);
            }
            this.NextImageIndex += 1;
        }

        private bool DoesDirectoryContainRightFiles(FileInfo[] files)
        {
            bool doesDirectoryContainImage = false;
            bool doesDirectoryContainJson = false;
            bool doesDirectoryContainUnnecessaryFiles = false;
            
            foreach (FileInfo file in files)
            {
                if (file.Name.EndsWith(".json"))
                    doesDirectoryContainJson = true;
                else if (file.Name.EndsWith(".png") || file.Name.EndsWith(".jpg") || file.Name.EndsWith(".jpeg") || file.Name.EndsWith(".cog"))
                    doesDirectoryContainImage = true;
                else
                    doesDirectoryContainUnnecessaryFiles = true;
            }
            System.Console.WriteLine($"doesDirectoryContainJson: {doesDirectoryContainJson}, doesDirectoryContainImage: {doesDirectoryContainImage}, doesDirectoryContainUnnecessaryFiles: {doesDirectoryContainUnnecessaryFiles}");
            return doesDirectoryContainImage & doesDirectoryContainJson & !doesDirectoryContainUnnecessaryFiles;
        }
    }
}