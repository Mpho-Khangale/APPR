using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Disaster_Alleviation.Pages
{
    public class NewGoodsDonationModel : PageModel
    {
        public List<string[]> GetGoods_DonationData()
        {
            List<string[]> goods_donations = new List<string[]>();

            String connection = "Server=tcp:djpromo01.database.windows.net,1433;Initial Catalog=DJPromoDatabase;Persist Security Info=False;User ID=djadmin;Password=Stayship#34;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            string sqlQuery1 = "SELECT Date, Items, Categories, Description, Name from Goods_Donation";

            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(sqlQuery1, conn))
                {
                    using (SqlDataReader reading = command.ExecuteReader())
                    {
                        while (reading.Read())
                        {
                            string gDate = reading["Date"].ToString();
                            string Items = reading["Items"].ToString();
                            string Categories = reading["categories"].ToString();
                            string Description = reading["Description"].ToString();
                            string gName = reading["Name"].ToString();

                            string[] dataRow1 = { gDate, Items, Categories, Description, gName };
                            goods_donations.Add(dataRow1);
                        }
                    }
                }
            }
            return goods_donations;
        }
        public IActionResult OnGet()
        {
            List<string[]> goods_donations = GetGoods_DonationData();
            ViewData["Goods_Donation"] = goods_donations;
            return Page();
        }
        
        public void OnPost() 
        {
            String gDate = Request.Form["goodsDate"];
            String items = Request.Form["goodsItems"];
            String categories = Request.Form["category"];
            String desc = Request.Form["description"];
            String gName = Request.Form["goodsName"];

            String connection = "Server=tcp:djpromo01.database.windows.net,1433;Initial Catalog=DJPromoDatabase;Persist Security Info=False;User ID=djadmin;Password=Stayship#34;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            SqlConnection sql = new SqlConnection(connection);

            sql.Open();
            string sqlQuery = "INSERT into Goods_Donation(Date, Items, Categories, Description, Name) VALUES(" + "'" + gDate + "'" + "," + "'" + items + "'" + "," + "'" + categories + "'" + "," + "'" + desc + "'" + "," + "'" + gName + "'" + ")";
            SqlCommand create = new SqlCommand(sqlQuery, sql);
            create.ExecuteNonQuery();
            sql.Close();
        }
    }
}
