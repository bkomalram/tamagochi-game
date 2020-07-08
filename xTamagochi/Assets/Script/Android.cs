using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Referencias extraidas por fuera
using Mono.Data.Sqlite;
using System;
using System.Data;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Android : MonoBehaviour
{
    private string conn, sqlQuery;
    IDbConnection dbconn;
    IDbCommand dbcmd;
    private IDataReader reader;
    public InputField t_name, t_Address, t_id;
    public Text data_staff;

    string DatabaseName = "Pet.s3db";
    // Start is called before the first frame update
    void Start()
    {
        //Application database Path android
        //string filepath = Application.persistentDataPath + "/" + DatabaseName;
        string filepath = Application.dataPath + "/" + DatabaseName;
        Debug.Log(filepath);
        if (!File.Exists(filepath))
        {
            // If not found on android will create Tables and database

            Debug.LogWarning("File \"" + filepath + "\" does not exist. Attempting to create from \"" +
                             Application.dataPath + "!/assets/Pet");



            // UNITY_ANDROID
            
            WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);
            while (!loadDB.isDone) { }
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDB.bytes);
            
            /*
            var loadDB = new UnityWebRequest("jar:file://" + Application.dataPath + "!/assets/"+ DatabaseName);
            loadDB.downloadHandler = new DownloadHandlerBuffer();
            while (!loadDB.isDone) { }
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDB.downloadHandler.data);
            */
        }

        conn = "URI=file:" + filepath;

        Debug.Log("Stablishing connection to: " + conn);
        dbconn = new SqliteConnection(conn);
        dbconn.Open();

        string query;
        query = "CREATE TABLE [PetParameter] ([COD] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,[DESCRIPTION]  VARCHAR(50) NOT NULL,[VALUE] INTEGER NOT NULL)";
        try
        {
            dbcmd = dbconn.CreateCommand(); // create empty command
            dbcmd.CommandText = query; // fill the command
            reader = dbcmd.ExecuteReader(); // execute command which returns a reader
        }
        catch (Exception e)
        {

            Debug.Log("Intentando crear tabla pero, "+e);

        }

        update_function("1", "15");
        Reader_function();
    }

    //Insert
    public void insert_button()
    {
        insert_function(t_name.text, t_Address.text);

    }
    //Search 
    public void Search_button()
    {
        data_staff.text = "";
        Search_function(t_id.text);

    }

    //Found to Update 
    public void F_to_update_button()
    {
        data_staff.text = "";
        F_to_update_function(t_id.text);

    }
    //Update
    public void Update_button()
    {
        update_function(t_id.text, t_name.text);

    }

    //Delete
    public void Delete_button()
    {
        data_staff.text = "";
        Delete_function(t_id.text);

    }

    //Insert To Database
    private void insert_function(string DESCRIPTION, string VALUE)
    {
        using (dbconn = new SqliteConnection(conn))
        {
            dbconn.Open(); //Open connection to the database.
            dbcmd = dbconn.CreateCommand();
            sqlQuery = string.Format("insert into PetParameter (DESCRIPTION, VALUE) values (\"{0}\",\"{1}\")", DESCRIPTION, VALUE);// table name
            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteScalar();
            dbconn.Close();
        }
        data_staff.text = "";
        Debug.Log("Insert Done  ");

        Reader_function();
    }
    //Read All Data For To Database
    private void Reader_function()
    {
        // int idreaders ;
        string DESCRIPTION, VALUE;
        using (dbconn = new SqliteConnection(conn))
        {
            dbconn.Open(); //Open connection to the database.
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "SELECT  DESCRIPTION, VALUE FROM PetParameter";// table name
            Debug.Log("ResultSet Query =  " + sqlQuery);
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                // idreaders = reader.GetString(1);
                DESCRIPTION = reader.GetString(0);
                VALUE = reader.GetInt32(1).ToString();

                //data_staff.text += Namereaders + " - " + Addressreaders + "\n";
                Debug.Log(" DESCRIPTION =" + DESCRIPTION + " / VALUE=" + VALUE);
            }
            reader.Close();
            reader = null;
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            //       dbconn = null;

        }
    }
    //Search on Database by ID
    private void Search_function(string Search_by_id)
    {
        using (dbconn = new SqliteConnection(conn))
        {
            string Name_readers_Search, Address_readers_Search;
            dbconn.Open(); //Open connection to the database.
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "SELECT DESCRIPTION, VALUE " + "FROM PetParameter where COD =" + Search_by_id;// table name
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                //  string id = reader.GetString(0);
                Name_readers_Search = reader.GetString(0);
                Address_readers_Search = reader.GetString(1);
                data_staff.text += Name_readers_Search + " - " + Address_readers_Search + "\n";

                Debug.Log(" name =" + Name_readers_Search + "Address=" + Address_readers_Search);

            }
            reader.Close();
            reader = null;
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();


        }

    }


    //Search on Database by ID
    private void F_to_update_function(string Search_by_id)
    {
        using (dbconn = new SqliteConnection(conn))
        {
            string Name_readers_Search, Address_readers_Search;
            dbconn.Open(); //Open connection to the database.
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "SELECT VALUE " + "FROM PetParameter where COD =" + Search_by_id;// table name
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {

                Name_readers_Search = reader.GetString(0);
                Address_readers_Search = reader.GetString(1);
                t_name.text = Name_readers_Search;
                t_Address.text = Address_readers_Search;

            }
            reader.Close();
            reader = null;
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();


        }

    }
    //Update on  Database 
    //private void update_function(string update_id, string update_name, string update_address)
    private void update_function(string update_id, string update_name)
    {
        using (dbconn = new SqliteConnection(conn))
        {
            dbconn.Open(); //Open connection to the database.
            dbcmd = dbconn.CreateCommand();
            sqlQuery = string.Format("UPDATE PetParameter set VALUE = @name  where COD = @id ");

            SqliteParameter P_update_name = new SqliteParameter("@name", update_name);
            //SqliteParameter P_update_address = new SqliteParameter("@address", update_address);
            SqliteParameter P_update_id = new SqliteParameter("@id", update_id);

            dbcmd.Parameters.Add(P_update_name);
            //dbcmd.Parameters.Add(P_update_address);
            dbcmd.Parameters.Add(P_update_id);

            dbcmd.CommandText = sqlQuery;
            dbcmd.ExecuteScalar();
            dbconn.Close();
            Debug.Log("Query Update is : " + sqlQuery);
            Debug.Log("UPDATE PetParameter set VALUE = "+ update_name + "  where COD = " + update_id);
            //Search_function(t_id.text);
        }

        // SceneManager.LoadScene("home");
    }



    //Delete
    private void Delete_function(string Delete_by_id)
    {
        using (dbconn = new SqliteConnection(conn))
        {

            dbconn.Open(); //Open connection to the database.
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "DELETE FROM Staff where id =" + Delete_by_id;// table name
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();


            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            data_staff.text = Delete_by_id + " Delete  Done ";

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
