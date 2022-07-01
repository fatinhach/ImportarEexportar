using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImportarEexportar.Controllers
{
    public class ExportarController : Controller
    {
        private SqlConnection _osqlconnection;

        // GET: Exportar
        public ActionResult Export()
        {
            return View();
        }
        public ActionResult Exportarxls(string Nome)
        {
            try
            {
                _osqlconnection = new SqlConnection(ConfigurationManager.ConnectionStrings["EscolaConnectionString"].ConnectionString);

                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();


                cmd = new SqlCommand("[dbo].[DadosAlunos]", _osqlconnection);

                cmd.Parameters.Add("@Nome", SqlDbType.VarChar).Value = Nome;

                cmd.CommandType = CommandType.StoredProcedure;

                da.SelectCommand = cmd;

                da.Fill(dt);
                if(dt.Rows.Count > 0)
                {
                    dt.TableName = "Dados_Do_Aluno";
                    using (XLWorkbook wp = new XLWorkbook())
                    {
                        wp.Worksheets.Add(dt);
                        wp.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        wp.Style.Font.Bold = true;
                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment; filename=TabelaAlunos.xlsx");
                        using(MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wp.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }else if(dt.Rows.Count <= 0)
                {
                    ViewBag.Mensagem = "Não existem dados a serem exportados";
                }
                return View();

            }
            catch(Exception ex)
            {
                ViewBag.Mensagem = "Erro: " + ex.Message + "" + ex.InnerException;
                return View();
            }
        }
    }
}