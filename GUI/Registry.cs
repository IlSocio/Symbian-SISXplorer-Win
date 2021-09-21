using System;
using Microsoft.Win32;


namespace SISXplorer
{
	/// <summary>
	/// Medusa Registry is the class to access the System Registry to save a key
	/// </summary>
	public class UserRegistry
	{
        private string rootKey; // ="SOFTWARE\\SymbianToys\\";
 
		public UserRegistry(string companyName, string prodName)
		{
            rootKey = "SOFTWARE\\" + companyName+"\\"+ prodName +"\\";
		}

		//Method to set an integer value in a Key
		public void Set_Key(string key_name, string value_name, int value_data)
		{				
			string strRegKey = rootKey+ key_name;
			//create a subkey or open an existing key 
			RegistryKey key_utente =
							Registry.CurrentUser.OpenSubKey(strRegKey,true);
			if(key_utente == null)
				key_utente=Registry.CurrentUser.CreateSubKey(strRegKey);

			key_utente.SetValue(value_name,value_data);
			key_utente.Close();
		}

		//Method to set a string value in a Key
		public void Set_Key(string key_name, string value_name, string value_data)
		{				
			string strRegKey = rootKey+ key_name;
			//create a subkey or open an existing key 
			RegistryKey key_utente =
				Registry.CurrentUser.OpenSubKey(strRegKey,true);
			if(key_utente == null)
				key_utente=Registry.CurrentUser.CreateSubKey(strRegKey);

			key_utente.SetValue(value_name,value_data);
			key_utente.Close();

		}	

		//Get an integer value of a key
		public int Get_Key(string key_name, string value_name, int default_value)
		{				
			string strRegKey = rootKey+ key_name;
			//create a subkey or open an existing key 
			RegistryKey key_utente =
				Registry.CurrentUser.OpenSubKey(strRegKey);
			//if the key doesen't exist
			if(key_utente == null)
				return default_value;
			else
			{
				int value_key=(int)key_utente.GetValue(value_name,default_value);
				key_utente.Close();
				return value_key;
			}
		}
		//Get a string value of a key
		public string Get_Key(string key_name, string value_name, string default_value)
		{				
			string strRegKey = rootKey+ key_name;
			//create a subkey or open an existing key 
			RegistryKey key_utente =
				Registry.CurrentUser.OpenSubKey(strRegKey);
			//if the key doesen't exist
			if(key_utente == null)
				return default_value;
			else
			{
				string value_key =(string)key_utente.GetValue(value_name,default_value);
				key_utente.Close();
				return value_key;
			}
		}
	}
}

