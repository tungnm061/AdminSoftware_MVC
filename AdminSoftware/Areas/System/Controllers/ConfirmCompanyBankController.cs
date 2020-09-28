using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic.Hrm;
using BusinessLogic.System;
using Core.Enum;
using Core.Helper.Logging;
using Core.Singleton;
using AdminSoftware.Controllers;
using AdminSoftware.Models;
using DuyAmazone.Areas.Printify.Models;
using Entity.Common;
using Entity.System;
using Newtonsoft.Json;

namespace AdminSoftware.Areas.System.Controllers
{
    public class ConfirmCompanyBankController : BaseController
    {
        private readonly CompanyBankBll _companyBankBll;
        private readonly UserBll _userBll;
        private readonly ExpenseTypeBll _expenseTypeBll;
        private readonly DepartmentBll _departmentBll;

        public ConfirmCompanyBankController()
        {
            _expenseTypeBll = SingletonIpl.GetInstance<ExpenseTypeBll>();
            _userBll = SingletonIpl.GetInstance<UserBll>();
            _companyBankBll = SingletonIpl.GetInstance<CompanyBankBll>();
            _departmentBll = SingletonIpl.GetInstance<DepartmentBll>();
        }

        private List<TextNote> TextNoteMemoryConfirm
        {
            get
            {
                if (Session["TextNoteMemoryConfirm"] == null)
                    return new List<TextNote>();
                return (List<TextNote>)Session["TextNoteMemoryConfirm"];
            }
            set { Session["TextNoteMemoryConfirm"] = value; }
        }

        [SystemFilter]
        public ActionResult Index()
        {
            var users = _userBll.GetUsers(null);
            //var expenseTypes = _expenseTypeBll.GetExpenseTypes(true);
            var expenseTypesAll = _expenseTypeBll.GetExpenseTypes(null);
            var departments = _departmentBll.GetDepartments(0);
            ViewBag.ExpenseTypesAll = expenseTypesAll.Select(x => new KendoForeignKeyModel { value = x.ExpenseId.ToString(), text = x.ExpenseName });
            ViewBag.Users = users.Select(x => new KendoForeignKeyModel { value = x.UserId.ToString(), text = x.FullName });
            //ViewBag.ExpenseTypes = expenseTypes.Select(x => new KendoForeignKeyModel { value = x.ExpenseId.ToString(), text = x.ExpenseName });
            ViewBag.Departments = departments.Select(x => new KendoForeignKeyModel { value = x.Path.ToString(), text = x.DepartmentName }); ;
            ViewBag.TypeMoneys = from TypeMoneyEnum s in Enum.GetValues(typeof(TypeMoneyEnum))
                                 let singleOrDefault =
                                     (DescriptionAttribute)
                                     s.GetType()
                                         .GetField(s.ToString())
                                         .GetCustomAttributes(typeof(DescriptionAttribute), false)
                                         .SingleOrDefault()
                                 where singleOrDefault != null
                                 select new KendoForeignKeyModel { value = ((byte)s).ToString(), text = singleOrDefault.Description };
            ViewBag.StatusSearch = from StatusCompanyBankEnum s in Enum.GetValues(typeof(StatusCompanyBankEnum))
                                   let singleOrDefault =
                                       (DescriptionAttribute)
                                       s.GetType()
                                           .GetField(s.ToString())
                                           .GetCustomAttributes(typeof(DescriptionAttribute), false)
                                           .SingleOrDefault()
                                   where singleOrDefault != null
                                   select new KendoForeignKeyModel { value = ((byte)s).ToString(), text = singleOrDefault.Description };
            return View();
        }

        public ActionResult CompanyBanks(DateTime? fromDate, DateTime? toDate, int? expenseId, int? statusSearch)
        {
            var listObj = _companyBankBll.GetCompanyBanks(true, fromDate, toDate, expenseId, statusSearch);
            return Json(listObj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CompanyBank(int id)
        {
            TextNoteMemoryConfirm = null;
            if (id == 0)
            {
                return PartialView(new CompanyBank
                {
                    CompanyBankId = 0,
                    IsActive = true,
                    TypeMonney = 1,
                    TradingBy = UserLogin.UserId,
                    TradingDate = DateTime.Now,
                    Status = 1
                });
            }

            var obj = _companyBankBll.GetCompanyBank(id);
            if (!string.IsNullOrEmpty(obj.TextNote))
            {
                TextNoteMemoryConfirm = JsonConvert.DeserializeObject<List<TextNote>>(obj.TextNote);
            }
            return PartialView(_companyBankBll.GetCompanyBank(id));
        }

        public JsonResult Save(List<int> listId, int status)
        {
            try
            {
                if (listId == null || listId.Count == 0)
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty },
                        JsonRequestBehavior.AllowGet);

                List<CompanyBank> listObj = new List<CompanyBank>();
                listId.ForEach(x =>
                {
                    listObj.Add(new CompanyBank
                    {
                        CompanyBankId = x,
                        Status = (byte)status,
                        ConfirmBy = UserLogin.UserId,
                        ConfirmDate = DateTime.Now
                    });
                });
                BizResult rs = _companyBankBll.UpdateConfirm(listObj);
                if (rs.Status > 0)
                {
                    return Json(new { Status = rs.Status, Message = MessageAction.MessageUpdateSuccess },
                        JsonRequestBehavior.AllowGet);
                }

                return Json(new { Status = -1, Message = MessageAction.MessageActionFailed },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new { Status = 0, ex.Message },
                    JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SaveDetail(int companyBankId, int status)
        {
            try
            {
                if (companyBankId == 0)
                {
                    return Json(new { Status = -1, Message = MessageAction.DataIsEmpty },
                        JsonRequestBehavior.AllowGet);
                }
                CompanyBank model = new CompanyBank
                {
                    ConfirmBy = UserLogin.UserId,
                    ConfirmDate = DateTime.Now,
                    Status = (byte)status,
                    CompanyBankId = companyBankId
                };
                if (TextNoteMemoryConfirm != null)
                {
                    model.TextNote = JsonConvert.SerializeObject(TextNoteMemoryConfirm);
                }
                var rs = _companyBankBll.UpdateConfirmDetail(model);
                if (rs)
                {
                    return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
                        JsonRequestBehavior.AllowGet);
                }

                return Json(new { Status = -1, Message = MessageAction.MessageActionFailed },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new { Status = 0, ex.Message },
                    JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult TextNotes()
        {
            return Json(TextNoteMemoryConfirm, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TextNote(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return PartialView(new TextNote
                {
                    TextNoteId = Guid.Empty.ToString(),
                    CreateDate = DateTime.Now
                });
            }
            var obj = TextNoteMemoryConfirm.Find(
                x =>
                    x.TextNoteId ==
                    id);
            return PartialView(obj);
        }

        public JsonResult SaveTextNoteMemory(TextNote model)
        {
            try
            {
                if (model == null)
                {
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty }, JsonRequestBehavior.AllowGet);
                }
                var noteWorkings = TextNoteMemoryConfirm;
                var noteWorking =
                    noteWorkings.Find(
                        x =>
                            x.TextNoteId ==
                            model.TextNoteId);
                if (noteWorking == null)
                {
                    noteWorkings.Add(new TextNote
                    {
                        TextNoteId = Guid.NewGuid().ToString(),
                        CreateDate = model.CreateDate,
                        CreateBy = UserLogin.UserId,
                        Text = model.Text
                    });
                }
                else
                {
                    noteWorking.Text = model.Text;
                }
                TextNoteMemoryConfirm = noteWorkings;
                return
                    Json(
                        new
                        {
                            Status = 1,
                            Message = "Cập nhật ghi chú thành công!"
                        },
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new { Status = 0, ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteTextNote(string id)
        {
            try
            {
                var noteWorkings = TextNoteMemoryConfirm;
                noteWorkings.Remove(
                    noteWorkings.Find(
                        x =>
                            x.TextNoteId ==
                            id));
                TextNoteMemoryConfirm = noteWorkings;
                return
                    Json(
                        new
                        {
                            Status = 1,
                            Message = "Xóa chi tiết công việc thành công!"
                        },
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new { Status = 0, ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult ClearSession()
        {
            return null;
        }

    }
}