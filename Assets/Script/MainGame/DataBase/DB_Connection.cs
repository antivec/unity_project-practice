using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using UnityEngine.Networking;
using System.Linq.Expressions;
using UnityEngine.Analytics;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class DB_Connection 
{
    private SqliteConnection m_dbConnection;
    private SqliteCommand m_dbCommand;
    private SqliteDataReader m_dbDataReader;

    public DB_Connection(string db_filePath)
    {
        OpenDatabase("data source = " + db_filePath);
    }

    public void OpenDatabase(string connectionString)
    {
        try
        {
            m_dbConnection = new SqliteConnection(connectionString);
            m_dbConnection.Open();

            if (m_dbConnection.State == ConnectionState.Open)
            {
                Debug.Log("Connected to database");
                
            }
            else
            {
                Debug.Log("Failed Connection");
                
            }
        }

        catch (Exception e)
        {
            Debug.Log(e);

        }

    }
    public int LoginAttempt(string _ID, string _PW)
    {
        SqliteDataReader reader;
        string cmpStr = _PW;
        string tmpStr;
        string LoginQuery = string.Format("Select PW from Player_Data where ID = \"{0}\"",_ID);
        try
        {
            reader = ExecuteSQLQuery(LoginQuery);
            tmpStr = reader["PW"].ToString();
            if(tmpStr == cmpStr)
            {
                Debug.Log("Account Available!");
                return 1;
            }
            else
            {
                Debug.Log("Not Found!");
                return 0;
            }

        }
        catch(Exception e)
        {
            Debug.Log(e);
            return -1;
        }
    }

    public void RegisterID(string newID, string newPW)
    {
        string testQuery = string.
            Format("Insert into Player_Data(ID,PW,Coin_Held) values(\"{0}\",\"{1}\",0)",newID,newPW);

        SqliteDataReader reader;
        try
        {
            Debug.Log(testQuery);
            reader = ExecuteSQLQuery(testQuery);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void UpdatePlayerCoin(string _targetID, int _value)
    {
        string query = string.
            Format("Update Player_data set Coin_Held = {0} where ID = \"{1}\"",_value,_targetID);
        SqliteDataReader reader;
        try
        {
            Debug.Log(query);
            reader = ExecuteSQLQuery(query);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public SqliteDataReader ExecuteSQLQuery(string query)
    {
        m_dbCommand = m_dbConnection.CreateCommand();
        m_dbCommand.CommandText = query;

        m_dbDataReader = m_dbCommand.ExecuteReader();
        return m_dbDataReader;
    }

    public void CloseDBConnection()
    {
        if(m_dbDataReader != null)
        {
            m_dbDataReader.DisposeAsync();
            m_dbDataReader.Close();
        }
        m_dbDataReader = null;

        if (m_dbCommand != null)
        {
            m_dbCommand.Dispose();
        }
        m_dbCommand = null;

        if(m_dbConnection != null)
        {
            m_dbConnection.Close();
        }
        m_dbConnection = null;

        Debug.Log("Disconnected from Database");
    }

   


}
