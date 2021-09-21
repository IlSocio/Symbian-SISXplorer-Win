using System;
using System.Collections.Generic;
using System.Text;

namespace Utility
{
    public class Dirs
    {
        public static string CreateNewDir(string directory)
        {
            if (directory.EndsWith("" + System.IO.Path.DirectorySeparatorChar))
                directory = directory.Remove(directory.Length - 1);
            string newDirectory = directory;
            int retry = 1;
            while (System.IO.Directory.Exists(newDirectory))
            {
                newDirectory = directory + "_" + retry;
                retry++;
            }
            // DeleteDir( directory );
            Debug.WriteLine(newDirectory);
            System.IO.Directory.CreateDirectory(newDirectory);
            if (!newDirectory.EndsWith("" + System.IO.Path.DirectorySeparatorChar))
                newDirectory += System.IO.Path.DirectorySeparatorChar;
            return newDirectory;
        }


        public static void DeleteDir(string directory)
        {
            if (!directory.EndsWith("" + System.IO.Path.DirectorySeparatorChar))
                directory += System.IO.Path.DirectorySeparatorChar;
            if (System.IO.Directory.Exists( directory ))
            {
                System.IO.Directory.Delete( directory, true );
            }
        }
    }
}
