using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

public class Class1
{
    SqlCommand cmd = new SqlCommand();
    SqlDataReader sdr;
    SqlDataAdapter da;
    DataSet ds;
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connect"].ConnectionString);

    public DataTable filldt(string query)
    {
        SqlCommand cmd = new SqlCommand(query, con);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        da.Fill(dt);
        return dt;
    }

    public DataSet fillds(string query)
    {
        SqlCommand cmd = new SqlCommand(query, con);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        cmd.CommandTimeout = 100000;
        DataSet ds = new DataSet();
        da.Fill(ds);
        return ds;
    }

    public SqlDataAdapter filldr(string query)
    {
        SqlCommand cmd = new SqlCommand(query, con);
        SqlDataAdapter da = new SqlDataAdapter(cmd);      
        return da;
    }
    
    public bool exeIUD(string query)
    {
        con.Open();
        SqlCommand cmd = new SqlCommand(query, con);
        if (Convert.ToBoolean(cmd.ExecuteNonQuery()) == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public SqlDataReader RetriveDataBaseQuery(string strQuery)
    {
        try
        {
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = strQuery;
            sdr = cmd.ExecuteReader();
            return sdr;
        }
        catch (Exception ex)
        {
            ex.ToString();
            con.Close();
            return sdr;
        }
    }

   //----------------------------------------------
    public string GenerateNewID(string query)
    {
        string newID = string.Empty;

        con.Open();
        SqlCommand cmd = new SqlCommand(query, con);
        SqlDataReader sdr = cmd.ExecuteReader();
        while (sdr.Read())
        {
            string i = sdr[0].ToString();

            if (string.IsNullOrEmpty(i))
            {
                newID = "EXM0001";
            }
            else
            {
                i = i.Substring(3);
                int j = Convert.ToInt32(i);
                j = j + 1;
                newID = "EXM" + j.ToString().PadLeft(4, '0');
            }
        }

        if (newID.Equals(""))
        {
            newID = "EXM0001";
        }

        con.Close();


        return newID;
    }
}
