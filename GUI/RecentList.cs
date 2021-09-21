using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;


namespace SISXplorer
{

    class RecentList
    {
        private StringCollection recentFiles;
        private static int MAX_RECENT = 5;
        private UserRegistry registry;


        // Trasforma in non static utilizzando la posizione del registro come parametro della classe        
        public RecentList(string compName, string prodName)
        {
            registry = new UserRegistry( compName, prodName );
            recentFiles = new StringCollection();
            for (int i = 0; i < MAX_RECENT; i++)
            {
                string aFile = registry.Get_Key( "Recent", "File" + i, "" );
                if (aFile.Length <= 0) continue;
                recentFiles.Add( aFile );
            }
        }


        public StringCollection UpdateRecentFiles(string fileName)
        {
            recentFiles.Insert( 0, fileName );
            // Rimuove Duplicati 
            for (int last = recentFiles.Count - 1; last > 0; last--)
                for (int frst = 0; frst < last; frst++)
                    if (recentFiles[last] == recentFiles[frst])
                    {
                        recentFiles.RemoveAt( last );
                        break;
                    }
            // Aggiorna Registro
            for (int i = 0; (i < MAX_RECENT) && (i < recentFiles.Count); i++)
            {
                if (recentFiles[i] != null)
                    registry.Set_Key( "Recent", "File" + i, recentFiles[i] );
            }
            return recentFiles;
        }


        public StringCollection GetRecentList()
        {
            return recentFiles;
        }
    }

}
