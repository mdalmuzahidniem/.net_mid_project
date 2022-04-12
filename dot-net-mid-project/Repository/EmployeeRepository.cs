using dot_net_mid_project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dot_net_mid_project.Repository
{
    public class EmployeeRepository
    {
        static Entities db;
        static EmployeeRepository()
        {
            db = new Entities();
        }
        public static User GetEmpInfo(int eid)
        {
            var e=(from emp in db.Users
                where emp.id==eid
                select emp).FirstOrDefault();
            return e;
        }
        public static User UpdateInfo(User s)
        {
            var entity = (from emp in db.Users
                           where emp.id == s.id
                           select emp).FirstOrDefault();
            entity.name = s.name.Trim();
            entity.phone = s.phone.Trim();
            entity.email = s.email.Trim();
            entity.password = s.password.Trim();
            entity.house = s.house.Trim();
            entity.road = s.road.Trim();
            //db.Entry(e).CurrentValues.SetValues(s);
            db.SaveChanges();
            return entity;
        }
        public static Employee GetEmpServiceInfo(int eid)
        {
            var e = (from emp in db.Employees
                     where emp.id == eid
                     select emp).FirstOrDefault();
            return e;
        }
        public static Employee ChangeStatus(string status,int id)
        {
            var e = (from emp in db.Employees
                          where emp.id == id
                          select emp).FirstOrDefault();
            e.work_status = status;
            db.SaveChanges();
            return e;
        }
        /*public static Order_Details GetOrderInfo(int empid)
        {
            var e = (from d in db.Order_Details
                     where d.employee_id == empid
                     select d);
            var info=e.ToList();
            return null;
        }*/
        public static Order GetInfo( int id)
        {
            var e = (from d in db.Orders
                     where d.id == id
                     select d).FirstOrDefault();
            return e;
        }
        public static Order ChangeOrderStatus(int id)
        {
            var e = (from o in db.Orders
                     where o.id == id
                     select o).FirstOrDefault();
            e.status = "completed";
            db.SaveChanges();
            return e;
        }
        public static Service Details(int id)
        {
            var e = (from d in db.Services
                     where d.id == id
                     select d).FirstOrDefault();
            return e;
        }
    }
}