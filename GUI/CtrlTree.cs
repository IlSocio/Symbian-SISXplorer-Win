using System;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SISX;
using SISX.Fields;
using Utility;


namespace SISXplorer
{
    public partial class CtrlTree : UserControl
    {
//        private bool _isDraggingFromTree;
        private SISXFileCollection fileList;
        private bool _showUsingTree = false;
        public delegate void AfterSelectEventHandler(object sender, string file);
        public event AfterSelectEventHandler AfterSelectFile;
//        public event AfterSelectEventHandler AfterSelectSISX;


/*        public bool IsDraggingFromTree
        {
            get
            {
                return _isDraggingFromTree;
            }
            private set
            {
                _isDraggingFromTree = value;
            }
        }*/


        public bool ShowUsingTree
        {
            get
            {                
                return _showUsingTree;
            }
            set
            {
                if (value == _showUsingTree) return;
                _showUsingTree = value;
                ArrayList files = new ArrayList();
                foreach (TreeNode node in treeView1.Nodes)
                    files.Add( node.ToolTipText );
                RemoveAllSIS();
                foreach (string sisFile in files)
                    AddSIS( sisFile );                
            }
        }



        static CtrlTree()
        {
        }


        public CtrlTree()
        {
            InitializeComponent();
            fileList = new SISXFileCollection();
//            IsDraggingFromTree = false;
        }


/*        public SISEntry this[string sisFullFilename]
        {
            get
            {
                return fileList[sisFullFilename] as SISEntry;
            }
        }*/


        /// <summary>
        /// Crea un TreeNode rappresentativo del Controller
        /// </summary>
        private TreeNode BuildTreeFromController(Composite ctrl, string fullName)
        {
            // Aggiunge Nodo SISXFile a TreeView
            TreeNode SISXNode = new TreeNode( ctrl.Name );
            SISXNode.ToolTipText = fullName;
            SISXNode.Tag = ctrl;

            foreach (Component comp in ctrl.GetChilds())
            {
                if (comp is Leaf)
                {
                    Leaf leaf = comp as Leaf;
                    TreeNode leafNode = new TreeNode( leaf.Name );
                    leafNode.Tag = leaf;
                    SISXNode.Nodes.Add( leafNode );     
                    // TODO: Estrai in NewDir...
                }
                if (comp is Composite)
                {
                    SISXNode.Nodes.Add( BuildTreeFromController( comp as Composite, comp.Name ) );
                }
            }
            return SISXNode;
        }


        /// <summary>
        /// Aggiunge un SISXFile analizzando il contenuto di sisFilename
        /// </summary>
        public TreeNode AddSIS(string fullSisFilename)
        {
            string sisFilename = System.IO.Path.GetFileName( fullSisFilename );
            if (fileList[fullSisFilename] != null) 
            {
                TreeNode[] nodes = treeView1.Nodes.Find( sisFilename, true );
                return nodes[0];
            }

            SISEntry sis = null;
            try
            {
                // Crea l'entry nella Collection di SISXFile
                sis = fileList.AddSISXFile( fullSisFilename );
            }
            catch (Exception ex)
            {
                GUI.ShowError( "Unknown File Format", "This is not a 3rd Edition file!\n"+ex.Message );
                return null;
            }

            // Aggiunge Nodo SISXFile a TreeView
            TreeNode SISXNode = BuildTreeFromController( sis.Root, sis.FileName );
            //TreeNode SISXNode = new TreeNode("prova");
            SISXNode.Expand();
            treeView1.Nodes.Add( SISXNode );
            return SISXNode;
        }


        public void RemoveAllSIS()
        {
            while (treeView1.Nodes.Count > 0)
            {
                RemoveSIS( treeView1.Nodes[0] );
            }
        }


        /// <summary>
        /// Rimuove il SISXFile associato a questo nodo
        /// </summary>
        public void RemoveSIS(TreeNode node)
        {
            if (node == null) return;
            TreeNode sisxNode = GetSISXNode( node );
            fileList.RemoveSISXFile( sisxNode.ToolTipText );
            treeView1.Nodes.Remove( sisxNode );
            // Se sono stati rimossi tutti i nodi genera un evento select...
            if (treeView1.Nodes.Count == 0)
            {
                if (AfterSelectFile != null)
                    AfterSelectFile( this, "" );
            }
        }


        /// <summary>
        /// Restituisce il nodo identificativo del file SISX a cui questo nodo appartiene
        /// </summary>
        private TreeNode GetSISXNode(TreeNode node)
        {
            if (node == null) return null;
            while (node.Parent != null) node = node.Parent;
            return node;
        }


/*        /// <summary>
        /// Restituisce il nodo identificativo del controller
        /// </summary>
        private TreeNode GetControllerNode(TreeNode node)
        {
            if (node == null) return null;
            while (node.Nodes.Count == 0) node = node.Parent;
            return node;
        }*/


        /// <summary>
        /// Restituisce True se l'elemento selezionato e' un archivio SISX.
        /// Nel caso di directory o file contenuto restituisce false.
        /// </summary>
        public bool IsSisSelected
        {
            get
            {
                if (treeView1.SelectedNode == null) return false;
                return (treeView1.SelectedNode.Nodes.Count > 0);
            }
        }


        /// <summary>
        /// Restituisce true nel caso in cui sia stato selezionato un SISX embedded.
        /// </summary>
        public bool IsSisEmbeddedSelected
        {
            get
            {
                return (IsSisSelected && treeView1.SelectedNode != null && treeView1.SelectedNode.Parent != null);
            }
        }


        /// <summary>
        /// Restituisce l'entry del file SISX selezionato
        /// </summary>
        public SISEntry SelectedSISEntry
        {
            get
            {
                TreeNode root = GetSISXNode( treeView1.SelectedNode );
                if (root == null) 
                    return null;
                return fileList[root.ToolTipText];
            }
        }


        /// <summary>
        /// Restituisce l'entry del file SISX selezionato
        /// </summary>
        public SISController SelectedSISController
        {
            get
            {
                TreeNode node = treeView1.SelectedNode;
                if (node == null) 
                    return null;
                if (node.Nodes.Count == 0) 
                    node = node.Parent;
                Composite comp = node.Tag as Composite;
                return comp.ctrl;
            }
        }


        public SISFileDescription SelectedSISFileDescription
        {
            get
            {
                TreeNode node = treeView1.SelectedNode;
                if (node == null) 
                    return null;
                if (node.Nodes.Count > 0) // Controller o sis
                    return null;
                SISEntry sisEntry = SelectedSISEntry;
                if (sisEntry == null) 
                    return null;
                Leaf leaf = node.Tag as Leaf;
                return leaf.fileDescr;
            }
        }

        /// <summary>
        /// Restituisce l'elenco (path fisici) di tutti i files selezionati.
        /// Se viene selezionato un .sisx restituisce l'insieme dei files contenuti nel .sisx
        /// </summary>
        public string[] SelectedFiles
        {
            get
            {
                return GetContainedFilesForNode( treeView1.SelectedNode );
            }
        }


        /// <summary>
        /// Restituisce la lista dei path fisici contenuti nel nodo selezionato
        /// Selezionando un SISX restituisce la lista di tutti i files contenuti nel SISX
        /// </summary>
        private string[] GetContainedFilesForNode(TreeNode mainNode)
        {
            List<string> lista = new List<string>();
            if (mainNode != null)
            {
                if (mainNode.Nodes.Count == 0)
                {
                    string fullFilename = GetPhysicalFileForNode( mainNode );
                    if (fullFilename != "")
                        lista.Add( fullFilename );
                }

                foreach (TreeNode node in mainNode.Nodes)
                {
                    string[] lista2 = GetContainedFilesForNode( node );
                    lista.AddRange( lista2 );
                }
            }

            string[] array = lista.ToArray();
            return array;
        }


        /// <summary>
        /// Restituisce il path fisico del nodo selezionato (SISX oppure file estratto)
        /// Selezionando un SISX restituisce il path del SISX
        /// </summary>
        private string GetPhysicalFileForNode(TreeNode node)
        {
            if (node == null) 
                return "";
            if (node.Nodes.Count > 0)
            {
                if (node.Parent == null) 
                    return node.ToolTipText; // E' un sisx
                return ""; // E' un controller
            }            
            // E' un file
            TreeNode rootNode = GetSISXNode( node );
            SISEntry entry = fileList[rootNode.ToolTipText];
            Leaf leaf = node.Tag as Leaf;
            return entry.TempPath + leaf.FullPath;
        }


        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (AfterSelectFile != null)
            {
                string fullFilename = GetPhysicalFileForNode( treeView1.SelectedNode );
                AfterSelectFile( this, fullFilename );
            }
        }


        private void treeView1_MouseUp(object sender, MouseEventArgs e)
        {
            //            if (e.Button == MouseButtons.Right)
            {
                TreeNode node = treeView1.GetNodeAt(e.X, e.Y);
                if (node != null)
                {
                    treeView1.SelectedNode = node;
                }
            }
        }

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
        }


        /// <summary>
        /// Estrae tutti gli archivi .sisx 
        /// </summary>
        public void ExtractAllTo(string path)
        {
            foreach (TreeNode node in treeView1.Nodes)
            {
                ExtractNodeTo(node, path );
            }
        }

        /// <summary>
        /// Estrae i files del nodo selezionato (directory, file .sis, file contenuto)
        /// </summary>
        public void ExtractSelectedTo(string path)
        {
            ExtractNodeTo( treeView1.SelectedNode, path );
        }

        /// <summary>
        /// Estrae i files interessati
        /// </summary>
        private void ExtractNodeTo(TreeNode node, string path)
        {
            if (node == null) return;
            Component comp = node.Tag as Component;
            comp.ExtractInDir( path, true );
        }


        /// <summary>
        /// Estrae i files dal SISX Selezionato
        /// </summary>
        private void extractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() != DialogResult.OK) return;
            string path = folderBrowserDialog1.SelectedPath;
            if (!path.EndsWith("\\")) path += "\\";
            ExtractNodeTo(treeView1.SelectedNode, path);
        }

        /// <summary>
        /// Chiude il SISX Selezionato
        /// </summary>
        private void closeSISXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveSIS( treeView1.SelectedNode );
        }

        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            string[] files = SelectedFiles;
            if (files != null)
            {
//                IsDraggingFromTree = true;
                DoDragDrop(new DataObject(DataFormats.FileDrop, files), DragDropEffects.Copy);
                //                DoDragDrop(filename, DragDropEffects.Copy);
            }
        }

        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
/*            if (IsDraggingFromTree)
            {
                IsDraggingFromTree = false;
                return;
            }*/
            string[] items = (string[])e.Data.GetData( DataFormats.FileDrop );
            foreach (string file in items)
                this.AddSIS( file );
        }

        private void treeView1_DragEnter(object sender, DragEventArgs e)
        {
            // If the data is a file or a bitmap, display the copy cursor.
            if (e.Data.GetDataPresent( DataFormats.FileDrop ))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

    }


    class SISXFileCollection
    {
        private Hashtable fileList;


        public SISXFileCollection()
        {
            fileList = new Hashtable();
        }

        public SISEntry this[string fullFilename]
        {
            get
            {
                return fileList[fullFilename] as SISEntry;
            }
        }

        public void RemoveSISXFile(string fullFilename)
        {
            fileList.Remove( fullFilename );
        }

        public SISEntry AddSISXFile(string fullFilename)
        {
            SISEntry sis = null;
            if (this[fullFilename] != null) 
                throw new Exception("Already Exists");

            sis = new SISEntry( fullFilename );
            fileList.Add( fullFilename, sis );
            return sis;
        }

/*        #region IEnumerable Membri di

        public IEnumerator GetEnumerator()
        {
            return fileList.Values.GetEnumerator();
        }

        #endregion*/
    }

}
