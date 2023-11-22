using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Disaster_Alleviation.Pages
{
    public class DisasterModel : PageModel
    {
        public void OnGet()
        {
        }

        public void OnPost() 
        {
            String startDate = Request.Form["start"];
            String endDate = Request.Form["end"];
            String loc = Request.Form["location"];
            String disDesc = Request.Form["disasterDesc"];
            String dCategory = Request.Form["disasterCategory"];

            String connection = "Server=tcp:djpromo01.database.windows.net,1433;Initial Catalog=DJPromoDatabase;Persist Security Info=False;User ID=djadmin;Password=Stayship#34;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            SqlConnection sql = new SqlConnection(connection);

            sql.Open();
            string sqlQuery = "INSERT into Disasters(Start_Date, End_Date, Location, Description, Aid_Type) VALUES(" + "'" + startDate + "'" + "," + "'" + endDate + "'" + "," + "'" + loc + "'" + "," + "'" + disDesc + "'" + "," + "'" + dCategory + "'" + ")";
            SqlCommand create = new SqlCommand(sqlQuery, sql);
            create.ExecuteNonQuery();
            sql.Close();
        }   
    }
}
