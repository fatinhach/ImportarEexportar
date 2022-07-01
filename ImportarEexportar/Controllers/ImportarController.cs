using System;

using System.Collections.Generic;

using System.Linq;

using System.Web;

using System.Web.Mvc;

using System.Data;

using System.Data.SqlClient;

using System.Configuration;

using System.Data.OleDb;

using System.IO;

namespace ImportarEexportar.Controllers
{
    public class ImportarController : Controller
    {

        // GET: /Home/

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["EscolaConnectionString"].ConnectionString);

        OleDbConnection Econ;

        public ActionResult Index()

        {

            return View();

        }

        [HttpPost]

        public ActionResult Index(HttpPostedFileBase file)

        {

            string filename = Guid.NewGuid() + Path.GetExtension(file.FileName);

            string filepath = "/excelfolder/" + filename;

            file.SaveAs(Path.Combine(Server.MapPath("/excelfolder"), filename));

            InsertExceldata(filepath, filename);



            return View();

        }

        private void ExcelConn(string filepath)

        {

            string constr = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=YES;""", filepath);

            Econ = new OleDbConnection(constr);

        }

        private void InsertExceldata(string fileepath, string filename)

        {

            string fullpath = Server.MapPath("/excelfolder/") + filename;

            ExcelConn(fullpath);

            string query = string.Format("Select * from [{0}]", "Planilha1$");

            OleDbCommand Ecom = new OleDbCommand(query, Econ);

            Econ.Open();



            DataSet ds = new DataSet();

            OleDbDataAdapter oda = new OleDbDataAdapter(query, Econ);

            Econ.Close();

            oda.Fill(ds);



            DataTable dt = ds.Tables[0];



            SqlBulkCopy objbulk = new SqlBulkCopy(con);

            objbulk.DestinationTableName = "Alunos";

            objbulk.ColumnMappings.Add("Nome", "Nome");

            objbulk.ColumnMappings.Add("DataNascimento", "DataNascimento");

            objbulk.ColumnMappings.Add("NomeMae", "NomeMae");

            objbulk.ColumnMappings.Add("NomePai", "NomePai");

            objbulk.ColumnMappings.Add("Serie", "Serie");

            objbulk.ColumnMappings.Add("Turma", "Turma");

            objbulk.ColumnMappings.Add("Sala", "Sala");

            con.Open();

            objbulk.WriteToServer(dt);

            con.Close();

        }

    }

}








