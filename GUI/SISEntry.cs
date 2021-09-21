using System;
using System.Collections.Generic;
using System.Text;
using SISX;
using System.IO;
using SISX.Fields;
using System.Collections;
using Utility;



namespace SISXplorer
{
    
    // Rappresenta l'interfaccia con cui il client interagisce...
    public abstract class Component
    {
        private Component _parent=null;
        public abstract Component[] GetChilds();
        public abstract void ExtractInDir(string dir, bool useName);


        public Component Parent
        {
            get
            {
                return _parent;
            }
            protected set
            {
                _parent = value;
            }
        }

        /// <summary>
        /// Restituisce il nome completo a partire dalla radice...
        /// </summary>
        public abstract string FullPath
        {
            get;
        }

        /// <summary>
        /// Identificativo numerico univoco 
        /// All'interno dello stesso SIS non esistono 2 controller con stesso ID
        /// All'interno dello stesso Controller non esistono 2 files con stesso ID
        /// </summary>
        public abstract int ID
        {
            get;
        }

        /// <summary>
        /// Nome del file o del controller
        /// </summary>
        public abstract string Name
        {
            get;
        }
    }



    // Rappresenta un file
    public class Leaf : Component
    {
        public readonly SISFileDescription fileDescr;
        public readonly byte[] data;

        public Leaf(SISFileDescription fileDescription, byte[] aData)
        {
            fileDescr = fileDescription;
            data = aData;
        }

        public override int ID
        {
            get
            {
                return (int)fileDescr.fileIndex;
            }
        }

        public Component Find(int id)
        {
            if (id != ID) return null;
            return this;
        }

        public override string Name
        {
            get
            {
                if (fileDescr.target.aString == "")
                    return "FileIndex" + ID;
                return fileDescr.target.aString;
            }
        }

        public override string FullPath
        {
            get
            {
                string s = "";
                if (Parent != null)
                    s = Parent.FullPath;
                if (!s.EndsWith("\\")) s += "\\";
                return s + ID + "_" + System.IO.Path.GetFileName(Name);
            }
        }

        public override void ExtractInDir(string dir, bool useName)
        {
            if (data == null) return;
            System.IO.File.WriteAllBytes( dir + ID + "_" + System.IO.Path.GetFileName( Name ), data );
        }

        public override Component[] GetChilds()
        {
            return new Component[0];
        }/**/
    }


    // Rappresenta un controller del sisx (contiene files ed altri sisx)
    public class Composite : Component
    {
        private List<Component> childrens;
        public SISController ctrl;

        public Composite(SISArray dataUnits, SISController controller)
        {
            childrens = new List<Component>();
            ctrl = controller;
//            SISDataUnit dataUnit = dataUnits.fields[ID] as SISDataUnit;

            // Aggiunge installBlock
            AddInstallBlock( dataUnits, controller.installBlock );

            // Aggiunge il logo.
            if (controller.logo != null)
            {
                SISFileDescription fileDescr = controller.logo.logo;
                int pos = (int)fileDescr.fileIndex;
                SISDataUnit dataUnit = dataUnits.fields[ID] as SISDataUnit;
                SISFileData fileData = dataUnit.fileData.fields[pos] as SISFileData;
                Component leaf = new Leaf( fileDescr, fileData.fileData.data );
                Add(leaf);
            }
        }


        public override void ExtractInDir(string dir, bool useName)
        {
            if (!dir.EndsWith( "\\" )) dir += "\\";
            string newTemp = dir + ID + "\\";
            if (useName) newTemp = dir + Name + "\\";
            Dirs.CreateNewDir( newTemp );
            foreach (Component comp in childrens)
                comp.ExtractInDir( newTemp, useName );
        }


        public override string FullPath
        {
            get
            {
                string s = "";
                if (Parent != null)
                    s = Parent.FullPath;
                return s + ID + "\\";
            }
        }


        public override int ID
        {
            get
            {
                return (int)ctrl.dataIndex.dataIndex;
            }
        }


        public override string Name
        {
            get
            {
                string s = "Foo" + ID;
                if (ctrl.info.names.fields.Count > 0)
                  s = ctrl.info.names.fields[0].ToString();
                return s;
              //                return ID+"_"+s;
            }
        }

        // All'interno di un installBlock ci sono vari controllers
        public void AddInstallBlock(SISArray dataUnits, SISInstallBlock installBlock)
        {
            if (installBlock != null)
            {
                foreach (SISX.Fields.SISFileDescription fileDescr in installBlock.files.fields)
                {
                    // Rileva il puntatore ai dati del file...
                    int pos = (int)fileDescr.fileIndex;
                    SISDataUnit dataUnit = dataUnits.fields[ID] as SISDataUnit;
                    SISFileData fileData = dataUnit.fileData.fields[pos] as SISFileData;
                    Component leaf = null;
                    if (fileDescr.operation != (uint)TSISFileOperation.EOpNull)
                    {
                        leaf = new Leaf( fileDescr, fileData.fileData.data );
                        Add( leaf );
                    }
                    /*                    if (fileDescr.operation == (uint)TSISFileOperation.EOpNull)
                                        {
                                            leaf = new Leaf( fileDescr, new byte[0] );
                                        }
                                        else
                                        {
                                            leaf = new Leaf( fileDescr, fileData.fileData.data );
                                        }
                                        Add( leaf );*/
                }

                foreach (SISX.Fields.SISIf sisIf in installBlock.ifBlocks.fields)
                {
                    AddInstallBlock( dataUnits, sisIf.installBlock );

                    foreach (SISX.Fields.SISElseIf sisElseIf in sisIf.elseIfs.fields)
                    {
                        AddInstallBlock( dataUnits, sisElseIf.installBlock );
                    }
                }
            }

            foreach (SISController controller in installBlock.embeddedSIS.fields)
            {
                Component comp = new Composite( dataUnits, controller );
                Add( comp );
            }            
        }

        private void Add(Component comp)
        {
            System.Diagnostics.Debug.Assert( comp.Parent == null );
            comp.Parent = this;
            childrens.Add( comp );
        }

        public override Component[] GetChilds()
        {
            return childrens.ToArray();
        }

    }/**/



    /// Identifica un file sisx
    public class SISEntry
    {
        public readonly string FileName;
        public SISXFile sisFile;
        private Composite containedFiles; 
        public readonly string TempPath;
        private static int totOpenedSISX;
        public static string TempDir;


        static SISEntry() 
        {
            totOpenedSISX = 0;
            TempDir = System.IO.Path.GetTempPath() + "SISXplorer\\";
//            TempDir = "C:\\SISXplorer\\";
            Dirs.CreateNewDir(TempDir);            
        }

        public SISEntry(string aFilename)
        {
            FileName = aFilename;
            FileStream fs = new FileStream( aFilename, FileMode.Open, FileAccess.Read, FileShare.Read );
            BinaryReader br = new BinaryReader( fs );
            sisFile = new SISXFile( br );
            br.Close();
            fs.Close();
            containedFiles = new Composite( sisFile.cnt.data.dataUnits, sisFile.cnt._controller );

            // Estrae i files in una directory temporanea per fornire il drag&drop...

            totOpenedSISX++;
            TempPath = TempDir + totOpenedSISX + "\\";
            containedFiles.ExtractInDir( TempPath, false );
        }


        public string Name
        {
            get 
            {
                return containedFiles.Name;
            }
        }


        public Composite Root
        {
            get
            {
                return containedFiles;
            }
        }
    }
}
