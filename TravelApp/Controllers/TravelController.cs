using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TravelApp.Models;
using System.Drawing;

namespace TravelApp.Controllers
{
    public class TravelController : Controller
    {
        string ConnectionString = @"Data Source=VOIDEK7\SQLEXPRESS;Initial Catalog=TravelDb;Integrated Security=True";
        [HttpGet]

        public ActionResult TravelDetails()
        {
            TravelHeader travelHeader = new TravelHeader();
            travelHeader = AddDropDown();           
            return View(travelHeader);
        }

        [HttpPost]
        public ActionResult TravelDetails(TravelHeader travelHeader)
        {
            if (!ModelState.IsValid)
            {
                TravelHeader travelHeaders = new TravelHeader();
                travelHeaders = AddDropDown();
                return View(travelHeaders);
            }
            else
            {
                SqlConnection cn = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand("AddTraveldetails", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ResultID", SqlDbType.Int);
                cmd.Parameters["@ResultID"].Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@TravelReson", travelHeader.TravelReson);
                cmd.Parameters.AddWithValue("@TravellingFrom", travelHeader.TravellingFrom);
                cmd.Parameters.AddWithValue("@TravellingToDate", travelHeader.TravellingToDate);
                cmd.Parameters.AddWithValue("@ExplainationTravel", travelHeader.ExplainationTravel);
                cmd.Parameters.AddWithValue("@TravellingDetails", travelHeader.TravellingDetails);

                cn.Open();
                cmd.ExecuteNonQuery();
                int mainId = Convert.ToInt32(cmd.Parameters["@ResultID"].Value);
                if (travelHeader.TravelPreDetailsdata.Count != 0)
                {
                    for (int i = 0; i < travelHeader.TravelPreDetailsdata.Count; i++)
                    {
                        string myQuery = "insert into tblTrvelPreDetails values('" + travelHeader.TravelPreDetailsdata[i].Currency + "','" + travelHeader.TravelPreDetailsdata[i].Amount + "','" + travelHeader.TravelPreDetailsdata[i].ExchangeRate + "','" + travelHeader.TravelPreDetailsdata[i].AmountINR + "','" + travelHeader.TravelPreDetailsdata[i].Description + "','" + mainId + "')";
                        SqlCommand cmd2 = new SqlCommand(myQuery, cn);
                        cmd2.ExecuteNonQuery();
                    }
                }
                cn.Close();
            }
           
            
            return Json("true", JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowDetails()
        {
            List<TravelHeader> listTraveldata = new List<TravelHeader>();
            SqlConnection cn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand("select * from tblTravelHeader", cn);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            cn.Open();
            adp.Fill(dt);
            cn.Close();
            foreach (DataRow dr in dt.Rows)
            {
                listTraveldata.Add(
                    new TravelHeader
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        TravelReson = Convert.ToString(dr["TravelReson"]),
                        TravellingToDate = Convert.ToString(dr["TravellingToDate"]),
                        TravellingFrom = Convert.ToString(dr["TravellingFrom"]),
                        ExplainationTravel = Convert.ToString(dr["ExplainationTravel"]),
                        TravellingDetails = Convert.ToString(dr["TravellingDetails"]),
                    }
                    );
            }
            return View(listTraveldata);

        }

        public ActionResult ShowPreDetails(int Id)
        {
            List<TravelPreDetails> listTravelPredetails = new List<TravelPreDetails>();
            SqlConnection cn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand("select * from tblTrvelPreDetails where TravelId =" + Id, cn);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            cn.Open();
            adp.Fill(dt);
            cn.Close();
            foreach (DataRow dr in dt.Rows)
            {
                listTravelPredetails.Add(
                    new TravelPreDetails
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Currency = Convert.ToString(dr["Currency"]),
                        Amount = Convert.ToInt32(dr["Amount"]),
                        ExchangeRate = Convert.ToInt32(dr["ExchangeRate"]),
                        AmountINR = Convert.ToInt32(dr["AmountINR"]),
                        Description = Convert.ToString(dr["Description"]),
                    }
                    );
            }
            return View(listTravelPredetails);
        }

        public ActionResult EditTravelDetails(int Id)
        {
            TravelHeader traveldata = new TravelHeader();
            traveldata = AddDropDown();
            SqlConnection cn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand("select * from tblTravelHeader where tblTravelHeader.Id=" + Id, cn);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            cn.Open();
            adp.Fill(dt);
            cn.Close();            
            foreach (DataRow dr in dt.Rows)
            {
                traveldata.Id = Convert.ToInt32(dr["Id"]);
                traveldata.TravelReson = Convert.ToString(dr["TravelReson"]);
                traveldata.TravellingToDate = Convert.ToString(dr["TravellingToDate"]);
                traveldata.TravellingFrom = Convert.ToString(dr["TravellingFrom"]);
                traveldata.ExplainationTravel = Convert.ToString(dr["ExplainationTravel"]);
                traveldata.TravellingDetails = Convert.ToString(dr["TravellingDetails"]);
                traveldata.TravelPreDetailsdata = travelpredetails(Id);                                                                         
            }
            return View(traveldata);
        }


        public List<TravelPreDetails> travelpredetails(int Id)
        {
            List<TravelPreDetails> listtravelPreDetails = new List<TravelPreDetails>();
            SqlConnection cn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand("select * from tblTrvelPreDetails where TravelId =" + Id, cn);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            cn.Open();
            adp.Fill(dt);
            cn.Close();
            foreach (DataRow dr in dt.Rows)
            {
                listtravelPreDetails.Add(
                    new TravelPreDetails
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Currency = Convert.ToString(dr["Currency"]),
                        Amount = Convert.ToInt32(dr["Amount"]),
                        ExchangeRate = Convert.ToInt32(dr["ExchangeRate"]),
                        AmountINR = Convert.ToInt32(dr["AmountINR"]),
                        Description = Convert.ToString(dr["Description"]),
                    }
                    );                

            }
            return listtravelPreDetails;
        }

        [HttpPost]
        public ActionResult UpdateTravelDetails(TravelHeader travelHeader)
        {
            SqlConnection cn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand("UpdateTraveldetails", cn);
            cmd.CommandType = CommandType.StoredProcedure;            
            cmd.Parameters.AddWithValue("@Id", travelHeader.Id);
            cmd.Parameters.AddWithValue("@TravelReson", travelHeader.TravelReson);
            cmd.Parameters.AddWithValue("@TravellingFrom", travelHeader.TravellingFrom);
            cmd.Parameters.AddWithValue("@TravellingToDate", travelHeader.TravellingToDate);
            cmd.Parameters.AddWithValue("@ExplainationTravel", travelHeader.ExplainationTravel);
            cmd.Parameters.AddWithValue("@TravellingDetails", travelHeader.TravellingDetails);

            cn.Open();
            cmd.ExecuteNonQuery();            
            if (travelHeader.TravelPreDetailsdata.Count != 0)
            {
                for (int i = 0; i < travelHeader.TravelPreDetailsdata.Count; i++)
                {
                    string myQuery = "Update tblTrvelPreDetails set Currency='" + travelHeader.TravelPreDetailsdata[i].Currency + "',Amount='" + travelHeader.TravelPreDetailsdata[i].Amount + "',ExchangeRate='" + travelHeader.TravelPreDetailsdata[i].ExchangeRate + "',AmountINR='" + travelHeader.TravelPreDetailsdata[i].AmountINR + "',Description='" + travelHeader.TravelPreDetailsdata[i].Description + "' where Id= '" + travelHeader.TravelPreDetailsdata[i].Id + "'";                    
                    SqlCommand cmd2 = new SqlCommand(myQuery, cn);
                    cmd2.ExecuteNonQuery();
                }
            }
            cn.Close();
            return Json("true", JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteTravelDetails(int Id)
        {
            SqlConnection cn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand("DeleteTravelDetails", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", Id);
            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
            return RedirectToAction("ShowDetails");
        }

        public ActionResult DeletePreTravelDetails(int Id)
        {
            SqlConnection cn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand("DeleteTravelPredetails", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", Id);
            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
            return RedirectToAction("ShowDetails");
        } 
        
        public TravelHeader AddDropDown()
        {
            TravelHeader travelHeader = new TravelHeader();
            travelHeader.TravelReasonList.Add(new SelectListItem { Text = "Travel to relax", Value = "Travel to relax" });
            travelHeader.TravelReasonList.Add(new SelectListItem { Text = "Travel to explore", Value = "Travel to explore" });
            travelHeader.TravelReasonList.Add(new SelectListItem { Text = "Travel for humility", Value = "Travel for humility" });

            travelHeader.CurrencyList.Add(new SelectListItem { Text = "EURO", Value = "EURO" });
            travelHeader.CurrencyList.Add(new SelectListItem { Text = "USD", Value = "USD" });
            return travelHeader;

        }
    }
}
