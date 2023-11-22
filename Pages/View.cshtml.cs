using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Disaster_Alleviation.Pages
{
    public class ViewModel : PageModel
    {

        

        

        public List<string[]> GetDisastersData() 
        {
            List<string[]> disasters = new List<string[]>();

            String connection = "Server=tcp:djpromo01.database.windows.net,1433;Initial Catalog=DJPromoDatabase;Persist Security Info=False;User ID=djadmin;Password=Stayship#34;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            string sqlQuery2 = "SELECT Start_Date, End_Date, Location, Description, Aid_Type from Disasters";

            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                using (SqlCommand comm = new SqlCommand(sqlQuery2, con))
                {
                    using (SqlDataReader reading = comm.ExecuteReader())
                    {
                        while (reading.Read())
                        {
                            string Start_Date = reading["Start_Date"].ToString();
                            string End_Date = reading["End_Date"].ToString();
                            string Location = reading["Location"].ToString();
                            string Description = reading["Description"].ToString();
                            string Aid_Type = reading["Aid_Type"].ToString();

                            string[] dataRow = { Start_Date, End_Date, Location, Description, Aid_Type };
                            disasters.Add(dataRow);
                        }
                    }
                }
            }
            return disasters;
        }
        public IActionResult OnGet()
        {
            

            

            List<string[]> disasters= GetDisastersData();
            ViewData["Disasters"] = disasters;
            return Page();
        }

        
    }
}
