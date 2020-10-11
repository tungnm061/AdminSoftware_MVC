using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogic.Sale;
using BusinessLogic.System;
using Core.Enum;
using Core.Helper.Logging;
using Core.Singleton;
using AdminSoftware.Areas.Sale.Models;
using AdminSoftware.Controllers;
using AdminSoftware.Helper;
using AdminSoftware.Models;
using Entity.Sale;
using Entity.System;

namespace AdminSoftware.Areas.Sale.Controllers
{
    public class GmailOrderController : BaseController
    {
        private readonly AutoNumberBll _autoNumberBll;
        private readonly GmailOrderBll _gmailOrderBll;
        private readonly UserBll _userBll;
        private readonly GmailBll _gmailBll;

        public GmailOrderController()
        {
            _autoNumberBll = SingletonIpl.GetInstance<AutoNumberBll>();
            _gmailOrderBll = SingletonIpl.GetInstance<GmailOrderBll>();
            _userBll = SingletonIpl.GetInstance<UserBll>();
            _gmailBll = SingletonIpl.GetInstance<GmailBll>();
        }
        [SaleFilter]
        public ActionResult Index(DateTime? fromDate, DateTime? toDate, int? gmailId = 0)
        {
            var gmails = _gmailBll.GetGmails();
            ViewBag.Gmails =
                gmails.Select(x => new KendoForeignKeyModel { value = x.Id.ToString(), text = x.FullName });

            var listDay = new List<String>();
            if (fromDate == null)
                fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (toDate == null)
                toDate = fromDate.Value.AddMonths(1).AddDays(-1);

            ViewBag.FromDate = fromDate.Value.ToString("dd/MM/yyyy");
            ViewBag.ToDate = toDate.Value.ToString("dd/MM/yyyy");
            for (var start = fromDate; start <= toDate; start = start.Value.AddDays(1))
            {
                listDay.Add(start.Value.ToString("dd/MM"));
            }
            ViewBag.Days = listDay;
            ViewBag.GmailId = gmailId;
            return View();
        }

        public JsonResult GmailOrders(DateTime fromDate, DateTime toDate, int? gmailId)
        {
            var gmails = _gmailOrderBll.GetGmailIdDistinct(fromDate, toDate);
            var listOrderStatistical = new List<GmailOrderStatistical>();

            if (gmails != null && gmails.Count > 0)
            {
                var data = _gmailOrderBll.GetGmailOrderAll(gmailId, fromDate, toDate);
                foreach (var item in gmails)
                {
                    var obj = new GmailOrderStatistical();
                    obj.GmailId = item;
                    var dataByGmailId = data.Where(x => x.GmailId == item).ToList();
                    var listDetail = new List<StatisticalOrderDetail>();
                    if (dataByGmailId.Any())
                    {
                        obj.LinkOrder = HttpUtility.UrlDecode(dataByGmailId[0].LinkOrder);
                    }
                    for (var start = fromDate; start <= toDate; start = start.AddDays(1))
                    {
                        var orders = dataByGmailId.FirstOrDefault(x=>x.OrderDate.ToShortDateString() == start.ToShortDateString());
                        var objDetail = new StatisticalOrderDetail();
                        objDetail.DateDay = start;
                        if (orders != null)
                        {
                            objDetail.CountOrder = orders.TotalOrder;
                            obj.TotalOrder += orders.TotalOrder;
                            obj.TotalCancelOrder += orders.CancelOrder??0;
                            obj.TotalRefundOrder += orders.RefundOrder??0;
                            obj.Description += (orders.OrderDate.ToString("dd/MM/yyyy") + " : " + orders.Description) + "  ";
                        }
                        listDetail.Add(objDetail);

                    }
                    obj.StatisticalDetails = listDetail;
                    listOrderStatistical.Add(obj);

//                    if (dataByGmailId.Any())
//                    {
//                        obj.LinkOrder = dataByGmailId[0].LinkOrder;
//                        obj.TotalCancelOrder = dataByGmailId.Sum(x => x.CancelOrder);
//                        obj.TotalRefundOrder = dataByGmailId.Sum(x => x.RefundOrder);
//                        var res = dataByGmailId.Aggregate("",
//(current, next) => current + ", " + next.OrderDate.ToString("dd/MM/yyyy") + " : " + next.Description);
//                        obj.TotalOrder = dataByGmailId.Sum(x => x.TotalOrder);
//                        var statisticalOrderDetails = dataByGmailId.OrderBy(x => x.OrderDate).Select(y => new StatisticalOrderDetail
//                        {
//                            CountOrder = y.TotalOrder,
//                            DateDay = y.OrderDate
//                        }).ToList();
//                        obj.StatisticalDetails = statisticalOrderDetails;
//                    }

//                    listOrderStatistical.Add(obj);
                }

            }
            return Json(listOrderStatistical,
                JsonRequestBehavior.AllowGet);
        }

        #region Import Excel
        public ActionResult ViewExcel()
        {
            return PartialView();
        }

        public int? ParseInt(string text)
        {
            int number;
            if (!string.IsNullOrEmpty(text))
            {
                bool result = Int32.TryParse(text.Trim(), out number);
                if (result)
                {
                    return number;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        //public JsonResult ImportExcel(DateTime fromDate)
        //{
        //    if (Request.Files["files"] == null)
        //        return Json(new { Status = 0, Message = "Không có dữ liệu để import!" }, JsonRequestBehavior.AllowGet);
        //    var dateOrderStr = Request.Form["DateOrder"];
        //    var toDate  = fromDate.AddMonths(1).AddDays(-1);
        //    TimeSpan ts = toDate - fromDate;

        //    var numberDay = ts.TotalDays + 1;
        //    var listGmail = new List<Gmail>();
        //    var checkData = _gmailOrderBll.GetGmailOrderAll(null, fromDate, fromDate);
        //    if (checkData.Count > 0)
        //    {
        //        return Json(new { Status = 0, Message = "Tháng import này đã có dữ liệu và hiện tại đang bị khóa!" },
        //            JsonRequestBehavior.AllowGet);
        //    }
        //    try
        //    {
        //        var file = Request.Files["files"];
        //        var fileExcel = ExcelHelper.ReadExcelDictionaryNew(file.InputStream);
        //        var dataLst = fileExcel.Data;
        //        var listObj = new List<GmailOrder>();
        //        var listObjDetail = new List<GmailOrderDetail>();
        //        if (dataLst.Count == 0)
        //        {
        //            return Json(new { Status = 0, Message = MessageAction.DataIsEmpty }, JsonRequestBehavior.AllowGet);
        //        }
        //        foreach (var data in dataLst)
        //        {
        //            GmailOrder obj = new GmailOrder();
        //            obj.OrderDate = fromDate;
        //            obj.CreateBy = UserLogin.UserId;
        //            obj.CreateDate = DateTime.Now;
        //            if (
        //                data.ContainsKey("GMAIL"))
        //            {
        //                string gmailName = data["GMAIL"];

        //                var checkName = _gmailBll.GetGmailByName(gmailName.Trim());

        //                if (checkName == null && !string.IsNullOrEmpty(gmailName))
        //                {
        //                    listGmail.Add(new Gmail
        //                    {
        //                        CreateBy = UserLogin.UserId,
        //                        CreateDate = DateTime.Now,
        //                        FullName = gmailName,
        //                        IsActive =  true
        //                    });
        //                }

        //                //if (checkName == null)
        //                //{
        //                //    return Json(new { Status = 0, Message = "Tài khoản " + gmailName + " chưa được tạo trong hệ thống!" },
        //                //        JsonRequestBehavior.AllowGet);
        //                //}

        //                //if (listObj.FirstOrDefault(x=>x.GmailId == checkName.Id) != null)
        //                //{
        //                //    return Json(new { Status = 0, Message = "Tài khoản " + gmailName + " xuất hiện nhiều hơn 1 lần trong danh sách!" },
        //                //        JsonRequestBehavior.AllowGet);
        //                //}

        //                //obj.GmailOrderId = Guid.NewGuid().ToString();
        //                //obj.GmailId = checkName.Id;
        //                //obj.LinkOrder = data["Link Đơn"];
        //                //if (!string.IsNullOrEmpty(obj.LinkOrder))
        //                //{
        //                //    obj.LinkOrder = HttpUtility.UrlEncode(obj.LinkOrder);
        //                //}

        //                //obj.CancelOrder = ParseInt(data["Đơn gửi lại, hủy đơn"]);
        //                //obj.RefundOrder = ParseInt(data["Đơn refund"]);
        //                //obj.Description = data["Lý do refund"];
        //                //var listData = data.Select(x=>x.Value).ToList();
        //                //listObj.Add(obj);
        //                //for (int i = 0; i < numberDay; i++)
        //                //{
        //                //    listObjDetail.Add(new GmailOrderDetail
        //                //    {
        //                //        GmailOrderId = obj.GmailOrderId,
        //                //        OrderDate = fromDate.AddDays(i),
        //                //        TotalOrder = ParseInt(listData[i+5])
        //                //    });
        //                //}
        //            }
        //            else
        //            {
        //                return Json(new { Status = 0, Message = "File import không đúng định dạng!" },
        //                    JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //        var insert = _gmailBll.Inserts(listGmail);
        //        //long insert = _gmailOrderBll.Insert(listObj, listObjDetail);
        //        if (insert)
        //        {
        //            return Json(new { Status = 1, Message = MessageAction.MessageImportSuccess },
        //                JsonRequestBehavior.AllowGet);
        //        }

        //        return Json(new { Status = 0, Message = MessageAction.MessageActionFailed }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logging.PutError(ex.Message, ex);
        //        return Json(new { Status = 0, ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        public JsonResult ImportExcel()
        {
            if (Request.Files["files"] == null)
                return Json(new { Status = 0, Message = "Không có dữ liệu để import!" }, JsonRequestBehavior.AllowGet);
            try
            {
                var file = Request.Files["files"];
                var fileExcel = ExcelHelper.ReadExcelDictionaryNew(file.InputStream);
                var dataLst = fileExcel.Data;
                var listObj = new List<GmailOrder>();
                if (dataLst.Count == 0)
                {
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty }, JsonRequestBehavior.AllowGet);
                }
                DateTime dateValue;
                if (!DateTime.TryParseExact(fileExcel.SheetName, "dd-MM-yyyy",
                              new CultureInfo("en-US"),
                              DateTimeStyles.None,
                              out dateValue))
                {
                    return Json(new { Status = 0, Message = "Tên sheet đầu tiên không đúng định dạng ngày" }, JsonRequestBehavior.AllowGet);
                }

                foreach (var data in dataLst)
                {
                    GmailOrder obj = new GmailOrder();
                    obj.OrderDate = dateValue;
                    obj.CreateBy = UserLogin.UserId;
                    obj.CreateDate = DateTime.Now;
                    if (
                        data.ContainsKey("Mail phân quyền") &&
                        data.ContainsKey("Link Đơn") &&
                        data.ContainsKey("Đơn gửi lại, hủy đơn") &&
                        data.ContainsKey("Đơn refund") &&
                        data.ContainsKey("Tổng đơn"))
                    {
                        string gmailName = data["Mail phân quyền"];
                        if (string.IsNullOrEmpty(gmailName))
                        {
                            return Json(new { Status = 0, Message = "Tài khoản Mail phân quyền không được để trống!" },
                                JsonRequestBehavior.AllowGet);
                        }
                        var checkName = _gmailBll.GetGmailByName(gmailName.Trim());


                        if (checkName == null)
                        {
                            return Json(new { Status = 0, Message = "Tài khoản " + gmailName + " chưa được tạo trong hệ thống!" },
                                JsonRequestBehavior.AllowGet);
                        }

                        if (listObj.FirstOrDefault(x => x.GmailId == checkName.Id) != null)
                        {
                            return Json(new { Status = 0, Message = "Tài khoản " + gmailName + " xuất hiện nhiều hơn 1 lần trong danh sách!" },
                                JsonRequestBehavior.AllowGet);
                        }

                        obj.GmailOrderId = Guid.NewGuid().ToString();
                        obj.GmailId = checkName.Id;
                        obj.LinkOrder = data["Link Đơn"];
                        if (!string.IsNullOrEmpty(obj.LinkOrder))
                        {
                            obj.LinkOrder = HttpUtility.UrlEncode(obj.LinkOrder);
                        }
                        obj.TotalOrder = ParseInt(data["Tổng đơn"]);
                        obj.CancelOrder = ParseInt(data["Đơn gửi lại, hủy đơn"]);
                        obj.RefundOrder = ParseInt(data["Đơn refund"]);
                        obj.Description = data["Lý do refund"];
                        listObj.Add(obj);
                    }
                    else
                    {
                        return Json(new { Status = 0, Message = "File import không đúng định dạng!" },
                            JsonRequestBehavior.AllowGet);
                    }
                }
                long insert = _gmailOrderBll.Insert(listObj);
                if (insert > 0)
                {
                    return Json(new { Status = 1, Message = MessageAction.MessageImportSuccess },
                        JsonRequestBehavior.AllowGet);
                }

                return Json(new { Status = 0, Message = MessageAction.MessageActionFailed }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new { Status = 0, ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}