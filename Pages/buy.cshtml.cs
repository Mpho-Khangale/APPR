using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Disaster_Alleviation.Pages
{
    public class Buy_GoodsModel : PageModel
    {
        public decimal AvailableFunds { get; set; }
        public decimal RemainingAmount { get; set; }

        public List<string[]> GetDisasterData()
        {
            List<string[]> disaster = new List<string[]>();

            String connection = "Server=tcp:djpromo01.database.windows.net,1433;Initial Catalog=DJPromoDatabase;Persist Security Info=False;User ID=djadmin;Password=Stayship#34;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            string sqlQuery2 = "SELECT Description from Disasters";

            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                using (SqlCommand comm = new SqlCommand(sqlQuery2, con))
                {
                    using (SqlDataReader reading = comm.ExecuteReader())
                    {
                        while (reading.Read())
                        {
                            string Description = reading["Description"].ToString();

                            string[] dataRow = { Description };
                            disaster.Add(dataRow);
                        }
                    }
                }
            }
            return disaster;
        }

        public List<string[]> GetGoods_DonationData()
        {
            List<string[]> goods_donations = new List<string[]>();

            String connection = "Server=tcp:djpromo01.database.windows.net,1433;Initial Catalog=DJPromoDatabase;Persist Security Info=False;User ID=djadmin;Password=Stayship#34;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            string sqlQuery1 = "SELECT Categories from Goods_Donation";

            using (SqlConnection conn = new SqlConnection(connection))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(sqlQuery1, conn))
                {
                    using (SqlDataReader reading = command.ExecuteReader())
                    {
                        while (reading.Read())
                        {
                            string Categories = reading["categories"].ToString();

                            string[] dataRow1 = { Categories };
                            goods_donations.Add(dataRow1);
                        }
                    }
                }
            }
            return goods_donations;
        }

        public IActionResult OnGet()
        {
            List<string[]> disaster = GetDisasterData();
            ViewData["Disasters"] = disaster;


            List<string[]> goods_donations = GetGoods_DonationData();
            ViewData["Goods_Donation"] = goods_donations;

            AvailableFunds = CalculateAvailableFunds(); // You may need to call this method here.
            RemainingAmount = AvailableFunds;

            return Page();
        }

        public void OnPost()
        {
            string disaster = Request.Form["disasters"];
            string goods = Request.Form["goods"];
            string allocatedAmount = Request.Form["allocatedAmount"];

            // Calculate available funds by summing the "Amount" from the "Monetary_Donation" table.
            decimal availableFunds = CalculateAvailableFunds();

            // Calculate remaining amount by subtracting allocatedAmount from available funds.
            decimal remainingAmount = availableFunds - decimal.Parse(allocatedAmount);

            // Save the allocation data to the "allocations" table in your database using SQL queries.
            string connection = "Server=tcp:djpromo01.database.windows.net,1433;Initial Catalog=DJPromoDatabase;Persist Security Info=False;User ID=djadmin;Password=Stayship#34;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            using (SqlConnection sql = new SqlConnection(connection))
            {
                sql.Open();

                // Insert the allocation data into the "allocations" table, including available funds and remaining amount.
                string sqlQuery = "INSERT INTO Allocations (Disaster, Goods, Allocated_Amount, Available_Funds, Remaining_Amount) VALUES (@disaster, @goods, @allocatedAmount, @availableFunds, @remainingAmount)";
                using (SqlCommand cmd = new SqlCommand(sqlQuery, sql))
                {
                    cmd.Parameters.AddWithValue("@disaster", disaster);
                    cmd.Parameters.AddWithValue("@goods", goods);
                    cmd.Parameters.AddWithValue("@allocatedAmount", allocatedAmount);
                    cmd.Parameters.AddWithValue("@availableFunds", availableFunds);
                    cmd.Parameters.AddWithValue("@remainingAmount", remainingAmount);
                    cmd.ExecuteNonQuery();
                }

                sql.Close();
            }
            AvailableFunds = availableFunds;
            RemainingAmount = remainingAmount;
        }

        private decimal CalculateAvailableFunds()
        {
            // Perform a query to sum the "Amount" from the "Monetary_Donation" table.
            string connection = "Server=tcp:djpromo01.database.windows.net,1433;Initial Catalog=DJPromoDatabase;Persist Security Info=False;User ID=djadmin;Password=Stayship#34;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            using (SqlConnection sql = new SqlConnection(connection))
            {
                sql.Open();

                string sqlQuery = "SELECT SUM(Amount) FROM Monetary_Donation";
                using (SqlCommand cmd = new SqlCommand(sqlQuery, sql))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToDecimal(result);
                    }
                    return 0; // Default to 0 if no donations exist yet.
                }

                sql.Close();
            }
        }


    }
}