﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using OfficeOpenXml;
using Service;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public OwnerService Service=new OwnerService();

        public ActionResult Index()
        {
            IEnumerable<OwnerViewModel> model=null;
            model = Service.GetData();
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var model = Service.Delete(id);
            return null;
        }

        public FileStreamResult ExportPDF()
        {
            MemoryStream outputStream = new MemoryStream();
            MemoryStream workStream = new MemoryStream();
            Document document = new Document();
            PdfWriter.GetInstance(document, workStream);
            document.Open();

            List<OwnerViewModel> lst = Service.GetData().ToList();
            foreach (var item in lst)
            {
                document.Add(new Paragraph($"Nombre {item.Name}"));
                document.Add(new Paragraph($"Apellido 1 {item.Last1}"));
                document.Add(new Paragraph($"Apellido 2 {item.Last2}"));
                document.Add(new Paragraph("..............................."));
            }


            document.Close();

            byte[] byteInfo = workStream.ToArray();
            outputStream.Write(byteInfo, 0, byteInfo.Length);
            outputStream.Position = 0;

            return new FileStreamResult(outputStream, "application/pdf");

        }


        public ActionResult ExportExcel()
        {
            ExcelPackage container = new ExcelPackage();
            var workSheet = container.Workbook.Worksheets.Add("lista");
            try
            {
                workSheet.Cells["A1"].LoadFromCollection(Collection: Service.GetData().ToList(), PrintHeaders: true);
            }
            catch (Exception ex)
            {
                throw;
            }
            // Autoanchura columnas
            workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                container.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }

            // Note we are returning a filename as well as the handle
            var result = new { FileGuid = handle, FileName = "excel.xlsx" };
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        public virtual ActionResult DownloadDocument(string fileGuid, string fileName)
        {
            if (TempData[fileGuid] != null)
            {
                byte[] data = TempData[fileGuid] as byte[];
                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml", fileName);
            }
            else
            {
                return new EmptyResult();
            }
        }


        public ActionResult GetOneProduct(int id)
        {
            OwnerViewModel data = null;
            data = Service.GetOne(id);
            
            var newListToJson = new List<OwnerViewModel> { data };
            return Json(SerializeJson(newListToJson), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(int ID, string name, string last1, string last2)
        {
            var objEdit= new OwnerViewModel
            {
                ID = ID,
                Name = name,
                Last1 = last1,
                Last2 = last2
            };

            var model= Service.Edit(objEdit);
            return null;
        }

        [HttpPost]
        public ActionResult Add(string name, string last1, string last2)
        {
            var objEdit = new OwnerViewModel
            {
                Name = name,
                Last1 = last1,
                Last2 = last2
            };

            var model = Service.Add(objEdit);
            return null;
        }



        private static string SerializeJson<T>(List<T> itemJson)
        {
            var dataFormatted = new Dictionary<string, List<T>>();
            dataFormatted.Add("Doc", itemJson);

            return JsonConvert.SerializeObject(dataFormatted, Formatting.Indented);
        }


    }
}