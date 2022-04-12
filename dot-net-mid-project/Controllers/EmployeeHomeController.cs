using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using dot_net_mid_project.Models;
using dot_net_mid_project.Repository;


namespace dot_net_mid_project.Controllers
{
    [Authorize]
    public class EmployeeHomeController : Controller
    {
        // GET: EmployeeHome
        
        public ActionResult Home()
        {
            return View();
        }
        public ActionResult ServiceDetails(int id)
        {
            var OrderInfo = EmployeeRepository.GetInfo(id);
            return View(OrderInfo);
        }
        public ActionResult Dashboard()
        {
            object user = Session["userid"];
            var userid = (int)user;
            var info = EmployeeRepository.GetEmpInfo(userid);
            int a = info.id;
            using (Entities db = new Entities())
            {

                var e = (from d in db.Order_Details
                         where d.employee_id == a && d.Order.status == "ordered"
                         select d);

                return View(e.ToList());
            }
           
        }
        
        public ActionResult ChangeOrderStatus(int id)
        {
            var d= EmployeeRepository.ChangeOrderStatus(id);
            return RedirectToAction("Dashboard");
        }
           public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","Home");
        }
        public ActionResult WorkHistory()
        {
            object user = Session["userid"];
            var userid = (int)user;
            var info = EmployeeRepository.GetEmpInfo(userid);
            int a = info.id;
            using (Entities db = new Entities())
            {
                var e = (from d in db.Order_Details
                         where d.employee_id == a && d.Order.status == "completed"
                         select d);

                return View(e.ToList());
            }      

        }
        public ActionResult Wallet()
        {
            object user = Session["userid"];
            var userid = (int)user;
            Entities db = new Entities();
            
                var e = (from d in db.Order_Details
                         where d.employee_id == userid && d.Order.status=="completed"
                         select d);
                var amount = 0;
                foreach (var line in e)
                {
                    amount = (int)(amount +(@line.quantity* @line.unit_price));
                }
                ViewBag.money = amount;
                return View();
            
        }
        
        public ActionResult MyProfile()
        {
            object user = Session["userid"];
            var userid = (int)user;
            //ViewBag.id = userid;
            var info=EmployeeRepository.GetEmpInfo(userid);
            return View(info);
        }
        [HttpPost]
        public ActionResult Update(User s)
        {
            if (ModelState.IsValid)
            {
                
                var info = EmployeeRepository.UpdateInfo(s);
            }
            return RedirectToAction("MyProfile");
        }
        public ActionResult Service()
        {
            object user = Session["userid"];
            var userid = (int)user;
            var Info = EmployeeRepository.GetEmpServiceInfo(userid);

            return View(Info);
        }
        [HttpPost]
        public ActionResult ChangeWorkStatus()
        {
            object user = Session["userid"];
            var userid = (int)user;
            var status = Request["workStatus"];
            var Info = EmployeeRepository.ChangeStatus(status,userid);
            return RedirectToAction("Service");
        }
        public ActionResult Application()
        {
            object user = Session["userid"];
            var userid = (int)user;
            ViewBag.id = userid;
            return View();
        }
        public ActionResult SentApplication(Application s)
        {

            object user = Session["userid"];
            var userid = (int)user;

            Entities db = new Entities();
            Application a = new Application();

            a.start_date = s.start_date;
            a.end_date = s.end_date;
            a.application_subject = Request["application_subject"].Trim();
            a.application_status = "process";
            a.employee_id = userid;

            db.Applications.Add(a);
            db.SaveChanges();
            return RedirectToAction("Application");
        }
        public ActionResult ApplicationHistory()
        {
            Entities db = new Entities();
            object user = Session["userid"];
            var userid = (int)user;
            var e = (from h in db.Applications
                     where h.employee_id == userid && h.application_status == "process"
                     select h);
            return View(e.ToList());
        }
        public ActionResult AssignService(int id)
        {
            var detail = EmployeeRepository.Details(id);
            return View(detail);
        }

    }

    internal class EmployeeAccessAttribute : Attribute
    {
    }
}