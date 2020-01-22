using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WindowsFormsApplication1
{
    class databasecontrol
    {

         SqlConnection con;
         public databasecontrol()
           {
               con = new SqlConnection(ConfigurationManager.ConnectionStrings["WindowsFormsApplication1.Properties.Settings.testdatabaseConnectionString"].ConnectionString);
            //con.Open();

            //if (con.State == ConnectionState.Closed)
            //{
            //    con.Open();
            //}
        }

         //public mysqlconnection dbconnection()
         //{
         //    string connectionstring = "data source=laptop-avruc4c0;initial catalog=testdatabase;integrated security=true";
         //    sqlconnection dbconnect = new sqlconnection(connectionstring);
         //    try
         //    {
         //        dbconnect.open();
         //        return dbconnect;
         //    }
         //    catch (exception e)
         //    {
         //        console.writeline(e);
         //        return dbconnect;
         //    }
         //}

        public int InsertIntoEquipments(string v1, string v2, int v3, int v4,DateTime v5 , string v6, string v7)
        {

            con.Open();
            string query = "insert into equipments(itemname,type,quantity,damaged,date,roomno,facultyid) values('"+v1+"','"+v2+"','"+v3+"','"+v4+"','"+v5.ToString("yyyy-MM-dd")+"','"+v6+"','"+v7+"')";
            SqlCommand cmd =new SqlCommand(query,con);
            int results = cmd.ExecuteNonQuery();
            con.Close();
            return results;
            
        }


        public int InsertIntoHistory(string v1, string v2, int v3, int v4, string v5, string v6, string v7)
        {

            con.Open();
            string query = "insert into history(date,itemname,type,quantity,damaged,roomno,facultyid) values('" + v5 + "','" + v1 + "','" + v2 + "','" + v3 + "','" + v4 + "','" + v6 + "','" + v7 + "')";
            SqlCommand cmd = new SqlCommand(query, con);
            int results = cmd.ExecuteNonQuery();
            con.Close();
            return results;

        }

        public int updateRemaining(string name, int rem)
        {
            con.Open();
            string query = "update tools set Remaining='" + rem +  "'  where itemname='" + name + "'";
            
            SqlCommand cmd = new SqlCommand(query, con);
            int result = cmd.ExecuteNonQuery();
            con.Close();
            return result;

        }


        public int updatetoequipment(string name, string type, int quantity, int damaged, string roomno, string facultyid)
        {
            con.Open();
            string query = "update equipments set quantity=quantity+'" + quantity + "',damaged=damaged+'" + damaged + "',roomno='" + roomno + "',facultyid='" + facultyid + "' where itemname='" + name + "' and type = '" + type + "' ";
            SqlCommand cmd = new SqlCommand(query, con);
            int result = cmd.ExecuteNonQuery();
            con.Close();
            UpdateDamagedNumber(name, type, damaged);
            return result;

        }


        public int FindQuantity(string itemname, string itemtype)
        {
            con.Open();
            string query = "select * from tools where itemname = '" + itemname + "' and itemtype = '" + itemtype + "'";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader mdr = cmd.ExecuteReader();
            if (mdr.Read())
            {
                int quantity = int.Parse(mdr[3].ToString());
              
                con.Close();
                return quantity;

            }
            else
            {
                con.Close();
                return 0;

            }

        }

        public int FindRemaining(string itemname, string itemtype)
        {
            con.Open();
            string query = "select * from tools where itemname = '" + itemname + "' and itemtype = '" + itemtype + "'";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader mdr = cmd.ExecuteReader();
            if (mdr.Read())
            {
                int rem = int.Parse(mdr[5].ToString());

                con.Close();
                return rem;

            }
            else
            {
                con.Close();
                return 0;

            }

        }

        public int FindDamaged(string itemname, string itemtype)
        {
            con.Open();
            string query = "select * from tools where itemname = '" + itemname + "' and itemtype = '" + itemtype + "'";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader mdr = cmd.ExecuteReader();
            if (mdr.Read())
            {
                int damaged = int.Parse(mdr[4].ToString());
                con.Close();
                return damaged;

            }
            else
            {
                con.Close();
                return 0;

            }

        }

        public int UpdateDamagedNumber(string itemname, string itemtype, int damage)
        {
            con.Open();
            string query = "select * from tools where itemname = '" + itemname + "' and itemtype = '" + itemtype + "'";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader mdr = cmd.ExecuteReader();
            if (mdr.Read())
            {
                int damaged = int.Parse(mdr[4].ToString());
                int totalDamage = damaged + damage;
                con.Close();
                con.Open();
                string query1 = "update tools set damaged='" + totalDamage + "'  where itemname='" + itemname + "' and itemtype ='" + itemtype + "' ";
                SqlCommand cmd1 = new SqlCommand(query1, con);
                int result = cmd1.ExecuteNonQuery();
                con.Close();
                if (result > 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }

            }
            else
            {
                return 0;
            }

        }
        public void returnEquipmentsTable(string itemname,string itemtype,string facultyid) 
        {
            con.Open();
            string query = "delete from equipments where itemname='" + itemname + "' and type ='" + itemtype + "' and facultyid ='" + facultyid + "'";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public int returnEquipmentsRemaining(string name, string type, string facultyid,int quantity,int damaged)
        {

           
            con.Open();

            string query1 = "update tools set Remaining = Remaining+'" + quantity + "' -'" + damaged + "' where itemname = '" + name + "' and itemtype = '" + type + "' ";
            SqlCommand cmd1 = new SqlCommand(query1, con);
            cmd1.ExecuteNonQuery();
            con.Close();
            return 1;

        }

        

        public DataTable getAll() 
        {
            con.Open();
            string query = "select * from tools";
            SqlCommand cmd = new SqlCommand(query,con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            con.Close();
            return dt;
        }

        public DataTable getAllHistory()
        {
            con.Open();
            string query = "select * from history";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            con.Close();
            return dt;
        }
       

        public DataTable searchItemFromTools(string name) 
        {
            con.Open();
            string query = "select * from tools where itemname = '"+name+"' ";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            sda.Fill(dt);
            con.Close();
            return dt;
        }

        public int updateToTools(int id,string name,string type,int quantity,int damaged) 
        {
            con.Open();
            string query1 = "update tools set itemname = '" + name + "',itemtype = '" + type + "',quantity=quantity+'" + quantity + "',damaged=damaged+'" + damaged + "',Remaining = Remaining +'" + quantity + "'-'" + damaged + "' where id = '" + id + "'";
                SqlCommand cmd1 = new SqlCommand(query1,con);
                cmd1.ExecuteNonQuery();
                con.Close();
                return 1;
            
        }

        public int updateOrderTools(string name, string type, int quantity, int damaged)
        {
            con.Open();
            string query1 = "update tools set Remaining=Remaining-'" + quantity + "' where itemname = '" + name + "'and itemtype = '" + type + "'";
            SqlCommand cmd1 = new SqlCommand(query1, con);
            cmd1.ExecuteNonQuery();
            con.Close();
            return 1;

        }

        public int InsertIntoTools(string name , string type , int quantity , int damaged) 
        {
            con.Open();
            string query1 = "select * from tools where itemname ='"+name+"'and itemtype = '"+type+"' ";
            SqlCommand cmd1 = new SqlCommand(query1, con);
            SqlDataReader mdr = cmd1.ExecuteReader();
            if (mdr.Read())
            {
                con.Close();
                return 0;
            }
            else 
            {
                con.Close();
                con.Open();
                int rem = quantity - damaged;
                string query2 = "insert into tools(itemname,itemtype,quantity,damaged,Remaining) values('" + name + "','" + type + "','" + quantity + "','" + damaged + "','" + rem + "')";
                SqlCommand cmd2 = new SqlCommand(query2,con);
                int result=cmd2.ExecuteNonQuery();
                if (result > 0)
                {
                    con.Close();
                    return 1;
                }
                else 
                {
                    con.Close();
                    return 3;
                }
                
               
              
            }
            
        }
    }
}
