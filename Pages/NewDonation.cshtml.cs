using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Disaster_Alleviation.Pages
{
    public class NewDonationModel : PageModel
    {
        public List<string[]> GetMonetary_DonationData()
        {
            List<string[]> monetary_donations = new List<string[]>();

            String connection = "Server=tcp:djpromo01.database.windows.net,1433;Initial Catalog=DJPromoDatabase;Persist Security Info=False;User ID=djadmin;Password=Stayship#34;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            string sqlQuery = "SELECT Date, Amount, Recognition from Monetary_Donation";

            using (SqlConnection c = new SqlConnection(connection))
            {
                c.Open();
                using (SqlCommand cmd = new SqlCommand(sqlQuery, c))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string Date = reader["Date"].ToString();
                            string Amount = reader["Amount"].ToString();
                            string Recognition = reader["Recognition"].ToString();

                            string[] dataRow = { Date, Amount, Recognition };
                            monetary_donations.Add(dataRow);
                        }
                    }
                }
            }
            return monetary_donations;
        }
        public IActionResult OnGet()
        {
            List<string[]> monetary_donations = GetMonetary_DonationData();
            ViewData["Monetary_Donation"] = monetary_donations;
            return Page();
        }
        public void OnPost() 
        {
            String date = Request.Form["monDate"];
            String amount = Request.Form["monAmount"];
            String name = Request.Form["monName"];

            String connection = "Server=tcp:djpromo01.database.windows.net,1433;Initial Catalog=DJPromoDatabase;Persist Security Info=False;User ID=djadmin;Password=Stayship#34;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            
            SqlConnection sql = new SqlConnection(connection);

            sql.Open();
            string sqlQuery = "INSERT into Monetary_Donation(Date, Amount, Recognition) VALUES(" +"'" + date +"'" +"," + "'" + amount + "'" + "," + "'" + name +"'" + ")";
            SqlCommand create = new SqlCommand(sqlQuery, sql);
            create.ExecuteNonQuery();
            sql.Close();
        }
    }
}
