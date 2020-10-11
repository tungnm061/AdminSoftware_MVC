using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace AdminSoftware.Areas.Sale.Controllers
{
    public class ToolAmzSearchController : Controller
    {

        [SaleFilter]
        public ActionResult Index()
        {
            return View();
        }

        public class ElementCustom
        {
            //public IWebElement WebElement { get; set; }
            public int Id;
            public string UrlImage { get; set; }
            public string Title { get; set; }

        }


        public JsonResult GetDataSearch(string keySearch, int? page = 1)
        {

            var listElm = new List<ElementCustom>();

            if (string.IsNullOrEmpty(keySearch))
            {
                 return Json(listElm, JsonRequestBehavior.AllowGet);
            }
            ChromeOptions options = new ChromeOptions();
            var path = HostingEnvironment.ApplicationHost.GetPhysicalPath() + "/Tool/jkompbllimaoekaogchhkmkdogpkhojg-3.0.2-Crx4Chrome.com.crx";
            options.AddExtension(path);
            ChromeDriver driver = new ChromeDriver(options);
            //var test driver.FindElementByLinkText
            var url = "https://www.amazon.com/s?k={0}&ref=nb_sb_noss";
            url = string.Format(url, keySearch);
            driver.Navigate().GoToUrl(url);

            if (page != 1)
            {
                driver.FindElement(By.LinkText(page.ToString())).Click();

            }
            //var options = new ChromeOptions();
            //options.AddExtension(@"D:\Test\jkompbllimaoekaogchhkmkdogpkhojg-3.0.2-Crx4Chrome.com.crx");
            //var driver = new ChromeDriver(options);
            // test

            //var test = driver.FindElement(By.ClassName("a-pagination"));

            var a = driver.FindElements(By.ClassName("s-include-content-margin"));
            int i = 0;
            foreach (var item in a)
            {
                ElementCustom elmCustom = new ElementCustom();
                var elementRoot = item.FindElements(By.ClassName("xtaqv-root"));
                if (elementRoot.Count > 0)
                {
                    i++;
                    //elmCustom.WebElement = elementRoot[0];
                    //var x = item.FindElement(By.ClassName("s-image"));
                    elmCustom.Id = i;
                    elmCustom.Title = item.FindElement(By.ClassName("a-size-base-plus")).Text;
                    elmCustom.UrlImage = item.FindElement(By.ClassName("s-image")).GetAttribute("src");
                    listElm.Add(elmCustom);
                }
            }
            driver.Close();
            return Json(listElm, JsonRequestBehavior.AllowGet);
        }
    }
}