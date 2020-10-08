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
        public ActionResult Index(DateTime? fromDate, int? gmailId = 0)
        {
            var gmails = _gmailBll.GetGmails();
            DateTime? toDate = null;
            ViewBag.Gmails =
                gmails.Select(x => new KendoForeignKeyModel { value = x.Id.ToString(), text = x.FullName });

            var listDay = new List<String>();
            if (fromDate == null)
                fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (toDate == null)
                toDate = fromDate.Value.AddMonths(1).AddDays(-1);

            ViewBag.FromDate = fromDate.Value.ToString("MM yyyy");
            ViewBag.ToDate = toDate.Value.ToString("dd/MM/yyyy");
            for (var start = fromDate; start <= toDate; start = start.Value.AddDays(1))
            {
                listDay.Add(start.Value.ToString("dd/MM/yyyy"));
            }
            ViewBag.Days = listDay;
            ViewBag.GmailId = gmailId;
            return View();
        }

        public JsonResult GmailOrders(DateTime? fromDate,  int? gmailId)
        {
            if (fromDate == null)
                fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var toDate = fromDate.Value.AddMonths(1).AddDays(-1);

            var data = _gmailOrderBll.GetGmailOrderAll(gmailId, fromDate, toDate);


            if (data.Count == 0)
            {
                return Json(data,
                    JsonRequestBehavior.AllowGet);
            }
            foreach (var item in data)
            {
                item.GmailOrderDetails = _gmailOrderBll.GetGmailOrderDetailByDate(item.GmailOrderId, fromDate.Value);
                item.TotalOrder = item.GmailOrderDetails.Sum(x => x.TotalOrder ?? 0);
                item.LinkOrder = HttpUtility.UrlDecode(item.LinkOrder);
            }
            return Json(data,
                JsonRequestBehavior.AllowGet);
        }

        #region Import Excel
        public ActionResult ViewExcel()
        {
            return PartialView();
        }

        public int? ParseInt(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                return int.Parse(text.Trim());
            }
            return null;
        }

        public JsonResult ImportExcel(DateTime fromDate)
        {
            if (Request.Files["files"] == null)
                return Json(new { Status = 0, Message = "Không có dữ liệu để import!" }, JsonRequestBehavior.AllowGet);
            var dateOrderStr = Request.Form["DateOrder"];
            var toDate  = fromDate.AddMonths(1).AddDays(-1);
            TimeSpan ts = toDate - fromDate;

            var numberDay = ts.TotalDays + 1;

            var checkData = _gmailOrderBll.GetGmailOrderAll(null, fromDate, fromDate);
            if (checkData.Count > 0)
            {
                return Json(new { Status = 0, Message = "Tháng import này đã có dữ liệu và hiện tại đang bị khóa!" },
                    JsonRequestBehavior.AllowGet);
            }
            try
            {
                var file = Request.Files["files"];
                var dataLst = ExcelHelper.ReadExcelDictionary(file.InputStream);
                var listObj = new List<GmailOrder>();
                var listObjDetail = new List<GmailOrderDetail>();
                if (dataLst.Count == 0)
                {
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty }, JsonRequestBehavior.AllowGet);

                }
                if (dataLst.First().Count != (int)(numberDay + 5))
                {
                    return Json(new { Status = 0, Message = "File import không đúng định dạng (Kiểm tra lại số ngày trong tháng)!" },
                        JsonRequestBehavior.AllowGet);
                }
                foreach (var data in dataLst)
                {
                    GmailOrder obj = new GmailOrder();
                    obj.OrderDate = fromDate;
                    obj.CreateBy = UserLogin.UserId;
                    obj.CreateDate = DateTime.Now;
                    if (
                        data.ContainsKey("Mail phân quyền") &&
                        data.ContainsKey("Link Đơn") &&
                        data.ContainsKey("đơn gửi lại, hủy đơn") &&
                        data.ContainsKey("Đơn refund") &&
                        data.ContainsKey("lý do refund") )
                    {
                        string gmailName = data["Mail phân quyền"];

                        var checkName = _gmailBll.GetGmailByName(gmailName.Trim());

                        if (checkName == null)
                        {
                            return Json(new { Status = 0, Message = "Tài khoản " + gmailName + " chưa được tạo trong hệ thống!" },
                                JsonRequestBehavior.AllowGet);
                        }

                        if (listObj.FirstOrDefault(x=>x.GmailId == checkName.Id) != null)
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

                        obj.CancelOrder = ParseInt(data["đơn gửi lại, hủy đơn"]);
                        obj.RefundOrder = ParseInt(data["Đơn refund"]);
                        obj.Description = data["lý do refund"];
                        var listData = data.Select(x=>x.Value).ToList();
                        listObj.Add(obj);
                        for (int i = 0; i < numberDay; i++)
                        {
                            listObjDetail.Add(new GmailOrderDetail
                            {
                                GmailOrderId = obj.GmailOrderId,
                                OrderDate = fromDate.AddDays(i),
                                TotalOrder = ParseInt(listData[i+5])
                            });
                        }
                    }
                    else
                    {
                        return Json(new { Status = 0, Message = "File import không đúng định dạng!" },
                            JsonRequestBehavior.AllowGet);
                    }
                }

                long insert = _gmailOrderBll.Insert(listObj, listObjDetail);
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