

using System;

using System.Data.SqlClient;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Device.Location;

using System.Data;

namespace WebApi
{
    public class SQLMigs
    {
        public static IEnumerable<string> GetEmergencyButton(string userid)
        {
            string[] ret = new string[2];
            ret[0] = "no";
            List<string> myList = new List<string>();


            SqlDataReader rdr = null;
            SqlConnection con = null;
            SqlCommand cmd = null;

            try
            {
                // Open connection to the database
                String ConnectionString = SQLAuth.dbase_DbDeveloper;
                con = new SqlConnection(ConnectionString);
                con.Open();

                string CommandText = " SELECT cms01_id, cms01_userid, cms01_aduan, cms01_Tarikhterima, cms01_Tarikhkemaskini, cms01_lokasi, cms01_telefon, cms01_kodpejabat_terima, cms01_gambar, cms01_status ";
                CommandText = CommandText + " FROM     cms01_Kecemasan  where cms01_userid=@userid ";

                cmd = new SqlCommand(CommandText);
                cmd.Connection = con;
                cmd.CommandText = CommandText;
                cmd.Parameters.AddWithValue("@userid", userid);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {

                    myList.Add(rdr["cms01_id"].ToString());
                    myList.Add(rdr["cms01_Tarikhterima"].ToString());
                    myList.Add(rdr["cms01_aduan"].ToString());
                    if (rdr["cms01_status"].ToString() == "1")
                    {
                        myList.Add("selesai");
                    }
                    else
                    {
                        myList.Add("belumselesai");
                    }


                }



                return myList;
            }
            catch (Exception)
            {
                return ret;
            }
            finally
            {
                if (rdr != null)
                    rdr.Close();

                if (con.State == ConnectionState.Open)
                    con.Close();
                // return twoDimensionalx;
            }
        }

        public static IEnumerable<string> GetDetailEmergencyButton(string userid, string cms01_id, string mlanguage)
        {
            string[] ret = new string[2];
            ret[0] = "no";
            List<string> myList = new List<string>();
            List<string> myList2 = new List<string>();
            int cnt = 0;
            SqlDataReader rdr = null;
            SqlConnection con = null;
            SqlCommand cmd = null;

            try
            {
                // Open connection to the database
                String ConnectionString = SQLAuth.dbase_DbDeveloper;
                con = new SqlConnection(ConnectionString);
                con.Open();

                string CommandText = " SELECT        cms02_id, cms01_id, cms02_tarikh, cms02_staffno_pegawai, cms02_ulasan, cms02_kodpejabat_ulasan ";
                CommandText = CommandText + " FROM     cms02_Log_kecemasan  where cms01_id = @cms01_idx ";

                cmd = new SqlCommand(CommandText);
                cmd.Connection = con;
                cmd.CommandText = CommandText;
                cmd.Parameters.AddWithValue("@userid", userid);
                cmd.Parameters.AddWithValue("@cms01_idx", cms01_id);
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (mlanguage == "0")
                    {
                        myList2.Add("Tarikh Kemaskini : " + rdr["cms02_tarikh"].ToString());
                        myList2.Add("Pegawai Bertugas : " + rdr["cms02_staffno_pegawai"].ToString());
                        myList2.Add("Ulasan : " + rdr["cms02_ulasan"].ToString());
                    }
                    else
                    {
                        myList2.Add("Date of Update : " + rdr["cms02_tarikh"].ToString());
                        myList2.Add("Officer on Duty : " + rdr["cms02_staffno_pegawai"].ToString() );
                        myList2.Add("Reviews : " + rdr["cms02_ulasan"].ToString());

                    }
                    myList.Add(rdr["cms02_tarikh"].ToString() + " : " + rdr["cms02_ulasan"].ToString());
                    cnt++;



                }

                if (cnt == 1)
                {
                    return myList2;
                }
                else
                {
                    if (cnt == 0)
                    {
                        if (mlanguage == "0")
                        {
                            myList2.Add("Ulasan : - ");
                        }
                        else
                        {
                            myList2.Add("Reviews : - ");

                        }
                        return myList2;
                    }
                    else
                    {
                        return myList;
                    }
                }


            }
            catch (Exception)
            {
                return ret;
            }
            finally
            {
                if (rdr != null)
                    rdr.Close();

                if (con.State == ConnectionState.Open)
                    con.Close();
                // return twoDimensionalx;
            }
        }
        public static IEnumerable<string> HantarIsuuTicket(string user, string aduan,  string lokasi, string telefon)
        {
            string[] ret = new string[1];
            ret[0] = "no";
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(SQLAuth.dbase_DbDeveloper))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {


                            cmd.CommandText = @"INSERT INTO    cms01_Kecemasan ( cms01_userid, cms01_aduan, cms01_lokasi, cms01_kodpejabat_terima,  cms01_status, cms01_telefon) values ( @USERID, @aduan,@lokasi,@kodpejabat, @status, @telefon)";
                            cmd.Connection = sqlConn;
                            cmd.Parameters.AddWithValue("@USERID", user);
                            cmd.Parameters.AddWithValue("@aduan", aduan);
                            cmd.Parameters.AddWithValue("@lokasi", lokasi);
                            cmd.Parameters.AddWithValue("@telefon", telefon);
                            cmd.Parameters.AddWithValue("@kodpejabat", "22");
                            cmd.Parameters.AddWithValue("@status", "0");
                      

                            try
                            {
                                sqlConn.Open();
                                cmd.ExecuteNonQuery();
                                ret[0] = "ok";
                            }
                            catch (SqlException e)
                            {
                                ret[0] = "A " + e.Message.ToString();
                            }
                   
                    }
                }
            }
            catch (Exception ex)
            {
                ret[0] = "B " + ex.Message.ToString();
            }

            return ret;
        }


    }
}