using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.DirectoryServices;
using System.Web;
using System.DirectoryServices.AccountManagement;



using System.Net;




//using System.DirectoryServices.AccountManagement;
namespace WebApi
{

    class SQLAuth
    {
        public static string dbase_dbclm = @"";
        public static string dbase_dbstaf = @"";
        public static string dbase_dbeqcas = @"";
        public static string dbase_dbmobile = @"Data Source='devmis.utem.edu.my\devstudent';Initial Catalog='DbDevMobile';User ID='mobdev';Password='MobDev@Student22'";
        public static string dbase_dbstudent = @"";
        public static string dbase_dbresearcher = @"";
        public static string dbase_dbpilihanraya = @"";
        public static string dbase_dbstudentpsh = @"";
        public static string dbase_dbpilihansenat = @"";
        public static string dbase_dbspku = @"";

        public static string dbase_devclm= @"";

        public static string dbase_DbDeveloper = @"Data Source='devmis.utem.edu.my\devstudent';Initial Catalog='DbDeveloper';User ID='mobdev';Password='MobDev@Student22'";

        //  public static string dbase_dbstudentdev = @"Data Source='devmis13.utem.edu.my\SQL_INS04';Initial Catalog='DBstudent';User ID='smp';Password='Smp@Dev2012'";
        //   public static string dbase_dbstudent = @"Data Source='V-SQL13.utem.edu.my\SQL_INS03';Initial Catalog='DBstudent';User ID='smp';Password='#utem07pksmp'";

        public static int rnd_seed = 327680;


        public static bool sqlvalidateUser2(string user, string passwordx)
        {
            bool valid = false;
            string passwordss = "";
            string passwordss24 = passwordx.Substring(3, passwordx.Length - 8) + "==";
           string passwordss44 = passwordx.Substring(3, passwordx.Length - 8) + "=";
            string passwordss64 = passwordx.Substring(3, passwordx.Length - 8);
            if (passwordss24.Length == 24)
            {
                passwordss = passwordss24;
            }
            else if (passwordss44.Length == 44)
            {
                passwordss = passwordss44;
            }
            else
            {
                passwordss = passwordss64;
            }
            string password = AESEncrytDecry.DecryptStringAES_notuser(passwordss,user);
            if ((password == "error"))
            {

            }
            else
            {
                try
                {
                    if (sqlvalidateUserCLM(user, password) == true)
                    {
                        valid = true;
                    }

                    if (valid == true) { }
                    else
                    {
                        using (PrincipalContext context = new PrincipalContext(ContextType.Domain))
                        {
                            valid = context.ValidateCredentials(user, password);
                        }
                    }


                }
                catch (Exception e)
                {

                }
            }



            return valid;
        }


        public static bool sqlvalidateUserCLM(string user, string password)
        {

            bool mybol = false;
            string mypass = "";
            SqlDataReader rdr = null;
            SqlConnection con = null;
            SqlCommand cmd = null;
            try
            {
                // Open connection to the database
                String ConnectionString = SQLAuth.dbase_dbmobile; 
                con = new SqlConnection(ConnectionString);
                con.Open();
                string CommandText = "SELECT username, password, name from  pengguna_api WHERE password = @CLM_loginPass and  username = @CLM_loginID ";
                cmd = new SqlCommand(CommandText);
                cmd.Connection = con;
                cmd.CommandText = CommandText;
                cmd.Parameters.AddWithValue("@CLM_loginID", user);
               // if (user == "00578")
              //  {
               //    
                 //   cmd.Parameters.AddWithValue("@CLM_loginPass", getPwD("Firebird##170674"));
              //  }
              //  else
              //  {
                    cmd.Parameters.AddWithValue("@CLM_loginPass", getPwD(password));
              //  }
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    mypass = rdr["username"].ToString().Trim();

                }
                if ( mypass == "")
                {

                }
                else
                {
                   // if (mypass == getPwD(password))
                    //{
                        mybol = true;
                    //}
                }

            }
            catch (Exception)
            {

            }
            finally
            {
                if (rdr != null)
                    rdr.Close();

                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            return mybol;



        }

      


      

      


 


        public static void updatedata(string userid, string clientid, string sessid, string clientip)
        {
            SqlDataReader rdr = null;
            SqlConnection con = null;
            SqlCommand cmd = null;
            var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        
                 con = new SqlConnection(SQLAuth.dbase_dbmobile);
                con.Open();
                string CommandText = "update      AspNetUsers set session_time=@SESSTIME, last_login=@LASTLOGIN, login_date=@LOGINDATE, clientid = @CLIENTID , sessionid = @SESSID ,  clientip  = @CLIENTIP  WHERE UserName = @USERID  ";
                cmd = new SqlCommand(CommandText);
                cmd.Connection = con;
                cmd.Parameters.Add(new SqlParameter("@CLIENTID", System.Data.SqlDbType.NVarChar, 500, "CLIENTID"));  // The name of the source column
                cmd.Parameters["@CLIENTID"].Value = clientid;
                cmd.Parameters.Add(new SqlParameter("@SESSID", System.Data.SqlDbType.NVarChar, 500, "SESSID"));  // The name of the source column
                cmd.Parameters["@SESSID"].Value = sessid;
                cmd.Parameters.Add(new SqlParameter("@CLIENTIP", System.Data.SqlDbType.NVarChar, 20, "CLIENTIP"));  // The name of the source column
                cmd.Parameters["@CLIENTIP"].Value = clientip;
                cmd.Parameters.Add(new SqlParameter("@USERID", System.Data.SqlDbType.NVarChar, 256, "USERID"));  // The name of the source column
                cmd.Parameters["@USERID"].Value = userid;

            cmd.Parameters.Add(new SqlParameter("@SESSTIME", System.Data.SqlDbType.BigInt));  // The name of the source column
            cmd.Parameters["@SESSTIME"].Value = Timestamp;
            cmd.Parameters.Add(new SqlParameter("@LASTLOGIN", System.Data.SqlDbType.BigInt));  // The name of the source column
            cmd.Parameters["@LASTLOGIN"].Value = Timestamp;
            cmd.Parameters.Add(new SqlParameter("@LOGINDATE", System.Data.SqlDbType.DateTime));  // The name of the source column
            cmd.Parameters["@LOGINDATE"].Value = DateTime.Now;


            rdr = cmd.ExecuteReader();
          //  int pilihanraya = updatedatabaru_sessionpilihanraya(userid, sessid);
            }
        public static int updatedatabaru (string userid, string clientid, string sessid, string clientip)
        {
            int mybol = 0;
            try { 
            
            SqlDataReader rdr = null;
            SqlConnection con = null;
            SqlCommand cmd = null;
            var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
       
            con = new SqlConnection(SQLAuth.dbase_dbmobile);
            con.Open();
            string CommandText = "update      AspNetUsers set session_time=@SESSTIME, last_login=@LASTLOGIN, login_date=@LOGINDATE, clientid = @CLIENTID , sessionid = @SESSID ,  clientip  = @CLIENTIP  WHERE UserName = @USERID  ";
            cmd = new SqlCommand(CommandText);
            cmd.Connection = con;
            cmd.Parameters.Add(new SqlParameter("@CLIENTID", System.Data.SqlDbType.NVarChar, 500, "CLIENTID"));  // The name of the source column
            cmd.Parameters["@CLIENTID"].Value = clientid;
            cmd.Parameters.Add(new SqlParameter("@SESSID", System.Data.SqlDbType.NVarChar, 500, "SESSID"));  // The name of the source column
            cmd.Parameters["@SESSID"].Value = sessid;
            cmd.Parameters.Add(new SqlParameter("@CLIENTIP", System.Data.SqlDbType.NVarChar, 20, "CLIENTIP"));  // The name of the source column
            cmd.Parameters["@CLIENTIP"].Value = clientip;
            cmd.Parameters.Add(new SqlParameter("@USERID", System.Data.SqlDbType.NVarChar, 256, "USERID"));  // The name of the source column
            cmd.Parameters["@USERID"].Value = userid;

            cmd.Parameters.Add(new SqlParameter("@SESSTIME", System.Data.SqlDbType.BigInt));  // The name of the source column
            cmd.Parameters["@SESSTIME"].Value = Timestamp;
            cmd.Parameters.Add(new SqlParameter("@LASTLOGIN", System.Data.SqlDbType.BigInt));  // The name of the source column
            cmd.Parameters["@LASTLOGIN"].Value = Timestamp;
            cmd.Parameters.Add(new SqlParameter("@LOGINDATE", System.Data.SqlDbType.DateTime));  // The name of the source column
            cmd.Parameters["@LOGINDATE"].Value = DateTime.Now;

            mybol = cmd.ExecuteNonQuery();
           // int pilihanraya = updatedatabaru_sessionpilihanraya(userid, sessid);
                // rdr = cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                return mybol;

            }
            return mybol;
        }
        public static int updatedatabaru_notmyutem(string userid, string clientid, string sessid, string clientip)
        {
            int mybol = 0;
            int mybol2 = 0;
            try
            {

                SqlDataReader rdr = null;
                SqlConnection con = null;
                SqlCommand cmd = null;
                var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
           
                con = new SqlConnection(SQLAuth.dbase_dbmobile);
                con.Open();
                string CommandText = "update      AspNetUsers set session_time=@SESSTIME, last_login=@LASTLOGIN, login_date=@LOGINDATE,   clientip  = @CLIENTIP  WHERE UserName = @USERID  ";
                cmd = new SqlCommand(CommandText);
                cmd.Connection = con;
                cmd.Parameters.Add(new SqlParameter("@CLIENTID", System.Data.SqlDbType.NVarChar, 500, "CLIENTID"));  // The name of the source column
                cmd.Parameters["@CLIENTID"].Value = clientid;
                cmd.Parameters.Add(new SqlParameter("@SESSID", System.Data.SqlDbType.NVarChar, 500, "SESSID"));  // The name of the source column
                cmd.Parameters["@SESSID"].Value = sessid;
                cmd.Parameters.Add(new SqlParameter("@CLIENTIP", System.Data.SqlDbType.NVarChar, 20, "CLIENTIP"));  // The name of the source column
                cmd.Parameters["@CLIENTIP"].Value = clientip;
                cmd.Parameters.Add(new SqlParameter("@USERID", System.Data.SqlDbType.NVarChar, 256, "USERID"));  // The name of the source column
                cmd.Parameters["@USERID"].Value = userid;

                cmd.Parameters.Add(new SqlParameter("@SESSTIME", System.Data.SqlDbType.BigInt));  // The name of the source column
                cmd.Parameters["@SESSTIME"].Value = Timestamp;
                cmd.Parameters.Add(new SqlParameter("@LASTLOGIN", System.Data.SqlDbType.BigInt));  // The name of the source column
                cmd.Parameters["@LASTLOGIN"].Value = Timestamp;
                cmd.Parameters.Add(new SqlParameter("@LOGINDATE", System.Data.SqlDbType.DateTime));  // The name of the source column
                cmd.Parameters["@LOGINDATE"].Value = DateTime.Now;

                mybol = cmd.ExecuteNonQuery();
                // rdr = cmd.ExecuteNonQuery();
              //  mybol2 = SQLKuliahStudent.updatedatabaru_notmyutem_attd(userid, clientid, sessid, clientip);
              //  if (mybol2 == 0)
              //  {
               //     mybol2 = SQLKuliahStudent.insertdatabaru_notmyutem_attd(userid, clientid, sessid, clientip);
               // }
            }
            catch (Exception e)
            {
                return mybol;

            }
            return mybol;
        }
        public static bool IsAspUserExist(string userid)
        {
            bool mybol = false;
            SqlDataReader rdr = null;
            SqlConnection con = null;
            SqlCommand cmd = null;
             con = new SqlConnection(SQLAuth.dbase_dbmobile);
            con.Open();
            string CommandText = "select UserName from AspNetUsers  WHERE UserName = @CLM_loginID  ";
            cmd = new SqlCommand(CommandText);
            cmd.Connection = con;
            cmd.Parameters.Add(new SqlParameter("@CLM_loginID", System.Data.SqlDbType.NVarChar, 256, "CLM_loginID"));  // The name of the source column
            cmd.Parameters["@CLM_loginID"].Value = userid;
            rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                      mybol = true;
                }
            return mybol;
        }


        //public static string finddatasmkb(string user, string jenis, string pass)
        //{
        //    string mybol = "yyxx";
        //    SqlDataReader rdr = null;
        //    SqlConnection con = null;
        //    SqlCommand cmd = null;
        //    //  string myEmail;
        //    // string myNama;
        //    // string myDepartment;server=V-SQL11.utem.edu.my\SQL_INS01;database=Dbstaf;UID=smsm;PWD=#smsm@kutkm07;"
        //    // SELECT        a.ms01_noakaun, b.NamaBank FROM ms01_peribadi as a, MS_Bank as b where a.ms01_kodbank = b.kodbank and a.ms01_nostaf = @CLM_loginID  ";
        //    try
        //    {
        //        // Open connection to the database
        //        String ConnectionString = SQLAuth.dbase_dbstaf;
        //        con = new SqlConnection(ConnectionString);
        //        con.Open();
        //        string CommandText = "SELECT        a.ms01_noakaun, b.NamaBank FROM ms01_peribadi as a, MS_Bank as b where a.ms01_kodbank = b.kodbank and a.ms01_nostaf = @CLM_loginID  ";
        //        cmd = new SqlCommand(CommandText);
        //        cmd.Connection = con;
        //        cmd.Parameters.Add(new SqlParameter("@CLM_loginID", System.Data.SqlDbType.NVarChar, 20, "CLM_loginID"));  // The name of the source column
        //        cmd.Parameters["@CLM_loginID"].Value = AESEncrytDecry.DecryptStringAES(user);
        //        //  cmd.Parameters.Add(new SqlParameter("@CLM_loginID", System.Data.SqlDbType.NVarChar, 20, "CLM_loginID"));  // The name of the source column
        //        //  cmd.Parameters["@CLM_loginID"].Value = user;
        //        // cmd.Parameters.Add(new SqlParameter("@CLM_StatusPG", System.Data.SqlDbType.NVarChar, 20, "CLM_StatusPG"));  // The name of the source column
        //        // cmd.Parameters["@CLM_StatusPG"].Value = password;
        //        // Execute the query
        //        rdr = cmd.ExecuteReader();
        //        while (rdr.Read())
        //        {
        //            // myEmail = rdr["emailaddress"].ToString();
        //            //  if (jenis == "nama") { mybol = rdr["firstname"].ToString(); }
        //            //   if (jenis == "dept") { mybol = rdr["department"].ToString(); }
        //            if (jenis == "nama") { mybol = rdr["NamaBank"].ToString() + " Acc No (" + rdr["ms01_noakaun"].ToString() +")"; }
        //          //  if (jenis == "dept") { mybol = rdr["department"].ToString(); }
        //          //  if (jenis == "email") { mybol = rdr["emailaddress"].ToString(); }
        //            //   lbFound.Items.Add(rdr["FirstName"].ToString() +
        //            //    " " + rdr["LastName"].ToString());
        //            //  mybol = "oo";
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        // Print error message
        //    }
        //    finally
        //    {
        //        //   return new string[] { myEmail.ToString(), user.UserName.ToString() };
        //        // Close data reader object and database connection
        //        if (rdr != null)
        //            rdr.Close();

        //        if (con.State == ConnectionState.Open)
        //            con.Close();
        //        //   return new string[] { myEmail.ToString(), user.UserName.ToString() };
        //    }

        //    return mybol;
        //}

       
      
        //////
        public  static string getPwD(string e)
        {
            //x is the variable to store pass after encryption
            string x = "";
            int a = 1;
            var b = "";
            var b2 = 0;
            var c = 0;
            var d = 0;

            Randomize(12345);

            for (a = 1; a <= e.Length; a++)
            {

                b = (e[a - 1]).ToString();//e.charCodeAt(a-1);


                b2 = Asc(Convert.ToChar(b));
                //Console.WriteLine("first " + b2 + "\n");
                decimal test = a / (decimal)2;
                int test2 = a / 2;
                //Console.WriteLine("CheckCalculation " + a + " - " + test + test2 );

                if (test == test2)
                    d = 5;
                else
                    d = -7;

                //Console.WriteLine("second " + d + "\n");


                //Console.WriteLine("rand " + Rnd(0) * 255);

                c = b2 ^ ((int)(Rnd(0) * 255) + a + d);
                //Console.WriteLine("third " + c + "\n");

                x = x + (char)c;
                //Console.WriteLine("fourth " + x + "\n\n");
            }

            return x;
        }

        public static int Asc(char String)
        {
            int num = Convert.ToInt32(String);
            if (num < 128)
            {
                return num;
            }
            try
            {
                Encoding fileIOEncoding = Encoding.Default;
                char[] chars = new char[1]
                {
      String
                };
                byte[] array;
                int bytes;
                if (fileIOEncoding.IsSingleByte)
                {
                    array = new byte[1];
                    bytes = fileIOEncoding.GetBytes(chars, 0, 1, array, 0);
                    return array[0];
                }
                array = new byte[2];
                bytes = fileIOEncoding.GetBytes(chars, 0, 1, array, 0);
                if (bytes == 1)
                {
                    return array[0];
                }
                if (BitConverter.IsLittleEndian)
                {
                    byte b = array[0];
                    array[0] = array[1];
                    array[1] = b;
                }
                return BitConverter.ToInt16(array, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static float Rnd(float Number)
        {

            int num = rnd_seed;
            checked
            {
                if ((double)Number != 0.0)
                {
                    if ((double)Number < 0.0)
                    {
                        num = BitConverter.ToInt32(BitConverter.GetBytes(Number), 0);
                        long num2 = num;
                        num2 &= uint.MaxValue;
                        num = (int)(num2 + (num2 >> 24) & 0xFFFFFF);
                    }
                    num = (int)(unchecked((long)num) * 1140671485L + 12820163 & 0xFFFFFF);
                }
                rnd_seed = num;
            }
            return (float)num / 16777216f;
        }

        static void  Randomize(double Number)
        {
            int rndSeed2 = rnd_seed;
            int num = (!BitConverter.IsLittleEndian) ? BitConverter.ToInt32(BitConverter.GetBytes(Number), 0) : BitConverter.ToInt32(BitConverter.GetBytes(Number), 4);
            num = ((num & 0xFFFF) ^ num >> 16) << 8;
            rndSeed2 = (rnd_seed = ((rndSeed2 & -16776961) | num));
        }
        public static void updatephonenumber_notmyutem(string userid, string mflag)
        {
            string mydat = "NULL";
            int mybol = 0;
            try
            {

                SqlDataReader rdr = null;
                SqlConnection con = null;
                SqlCommand cmd = null;
                var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
                 if (mflag == "1") { mydat = "1"; } else { mydat = ""; }
                con = new SqlConnection(SQLAuth.dbase_dbmobile);
                con.Open();
                string CommandText = "update      AspNetUsers set PhoneNumber=@CLIENTIP  WHERE UserName = @USERID  ";
                cmd = new SqlCommand(CommandText);
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@userid", userid);
                cmd.Parameters.AddWithValue("@CLIENTIP", mydat);
              //  cmd.Parameters.Add(new SqlParameter("@CLIENTIP", System.Data.SqlDbType.NVarChar, 20, "CLIENTIP"));  // The name of the source column
              //  cmd.Parameters["@CLIENTIP"].Value = "0";
              //  cmd.Parameters.Add(new SqlParameter("@USERID", System.Data.SqlDbType.NVarChar, 256, "USERID"));  // The name of the source column
             //   cmd.Parameters["@USERID"].Value = userid;


                mybol = cmd.ExecuteNonQuery();
          
            }
            catch (Exception e)
            {
               // return mybol;

            }
          //  return mybol;
        }
        public static string finddata(string user, string jenis, string pass)
        {
            string mybol = "yyxx";
            SqlDataReader rdr = null;
            SqlConnection con = null;
            SqlCommand cmd = null;
            //  string myEmail;
            // string myNama;
            // string myDepartment;
            try
            {
                // Open connection to the database
                String ConnectionString = @"Data Source='V-SQL14.utem.edu.my\SQL_INS04';Initial Catalog='eqcas';User ID='oas';Password='oas*pwd'";
                con = new SqlConnection(ConnectionString);
                con.Open();
                string CommandText = "SELECT emailaddress, firstname, password,  department FROM            live_user_staf WHERE att15 = @CLM_loginID  ";
                cmd = new SqlCommand(CommandText);
                cmd.Connection = con;
                cmd.Parameters.Add(new SqlParameter("@CLM_loginID", System.Data.SqlDbType.NVarChar, 20, "CLM_loginID"));  // The name of the source column
                cmd.Parameters["@CLM_loginID"].Value = user;
                //  cmd.Parameters.Add(new SqlParameter("@CLM_loginID", System.Data.SqlDbType.NVarChar, 20, "CLM_loginID"));  // The name of the source column
                //  cmd.Parameters["@CLM_loginID"].Value = user;
                // cmd.Parameters.Add(new SqlParameter("@CLM_StatusPG", System.Data.SqlDbType.NVarChar, 20, "CLM_StatusPG"));  // The name of the source column
                // cmd.Parameters["@CLM_StatusPG"].Value = password;
                // Execute the query
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    // myEmail = rdr["emailaddress"].ToString();
                    //  if (jenis == "nama") { mybol = rdr["firstname"].ToString(); }
                    //   if (jenis == "dept") { mybol = rdr["department"].ToString(); }
                    if (jenis == "nama") { mybol = rdr["firstname"].ToString(); }
                    if (jenis == "dept") { mybol = rdr["department"].ToString(); } // rdr["department"].ToString(); }
                    if (jenis == "email") { mybol = rdr["emailaddress"].ToString(); }
                    //   lbFound.Items.Add(rdr["FirstName"].ToString() +
                    //    " " + rdr["LastName"].ToString());
                    //  mybol = "oo";
                }
            }
            catch (Exception)
            {

                // Print error message
            }
            finally
            {
                //   return new string[] { myEmail.ToString(), user.UserName.ToString() };
                // Close data reader object and database connection
                if (rdr != null)
                    rdr.Close();

                if (con.State == ConnectionState.Open)
                    con.Close();
                //   return new string[] { myEmail.ToString(), user.UserName.ToString() };
            }

            return mybol;
        }
        public static bool IsAspPhoneUserExist(string userid)
        {
            bool mybol = false;
            SqlDataReader rdr = null;
            SqlConnection con = null;
            SqlCommand cmd = null;
            con = new SqlConnection(SQLAuth.dbase_dbmobile);
            con.Open();
            string CommandText = "select PhoneNumber from AspNetUsers  WHERE UserName = @CLM_loginID and PhoneNumber='1' ";
            cmd = new SqlCommand(CommandText);
            cmd.Connection = con;
            cmd.Parameters.Add(new SqlParameter("@CLM_loginID", System.Data.SqlDbType.NVarChar, 256, "CLM_loginID"));  // The name of the source column
            cmd.Parameters["@CLM_loginID"].Value = userid;
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                mybol = true;
            }
            return mybol;
        }
      
       

       
     
        /////
    }
}
