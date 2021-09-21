
using System;
using System.Windows.Forms;
using System.IO;


namespace Utility
{
	/// <summary>
	/// Summary description for GUI.
	/// </summary>
	public class GUI
	{
		public GUI()
		{
			//
			// TODO: Add constructor logic here
			//
		}


		public static void ShowInfo(string infomsg) 
		{            
			MessageBox.Show(infomsg, "Information", System.Windows.Forms.MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

        public static DialogResult ShowQuery(string query, MessageBoxButtons buttons)
        {
            return MessageBox.Show( query, "Confirm", buttons, MessageBoxIcon.Question );
        }

        public static DialogResult ShowQuery(string query) 
		{
            return ShowQuery( query, MessageBoxButtons.YesNoCancel );
		}

        public static void ShowError(string title, string err)
        {
//            File.AppendAllText( "errors.log", title + " " + err );
            MessageBox.Show(err, title, System.Windows.Forms.MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ShowError(string title, Exception ex)
        {
            ShowError( title, ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace );
        }

        public static void ShowError(string err)
        {
            ShowError("General Error", err);
        }

        public static void ShowFatalError(string title, string err) 
		{
            ShowError(title, err);
			Application.Exit();
		}

        public static void ShowFatalError(string err)
        {
            ShowFatalError("Fatal Error", err);
        }

        public static void ShowWarning(string warn) 
		{	
			MessageBox.Show(warn, "Warning", System.Windows.Forms.MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}
	}
}
