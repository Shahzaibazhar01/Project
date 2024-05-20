using Project.BusinessObject;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.BusinessLogic
{
    public class BL_Login
    {
        internal static object UserLogin(Models.User model)
        {
            using (WDMS_Entities db = new WDMS_Entities())
            {
                var user = db.Users.Where(x => x.UUserName == model.UUserName && x.UPassword == model.UPassword).FirstOrDefault();
                BusinessObject.User sessionUser = null;
                if (user != null)
                {
                    sessionUser = new BusinessObject.User();
                    sessionUser.UID = user.UID;
                    sessionUser.UAddress = user.UAddress;
                    sessionUser.UCNIC = user.UCNIC;
                    sessionUser.UName = user.UName;
                    sessionUser.UPhoneNumber = user.UPhoneNumber;
                    sessionUser.URoleID = user.URole;
                    sessionUser.URole = db.UserRoles.Where(x => x.RID == user.URole).Select(x => x.RName).FirstOrDefault();
                    sessionUser.UUserName = user.UUserName;
                }
                return sessionUser;
                //return db.GetLoginInfo(model.UUserName, model.UPassword).FirstOrDefault();
            }
        }

        internal static dynamic GetRoles()
        {
            using (WDMS_Entities db = new WDMS_Entities())
            {
                var temp = db.UserRoles.ToList();
                List<SelectListItem> roles = new List<SelectListItem>() {
                    new SelectListItem { Text = "Select", Value = "Select" }
                };
                foreach (var item in temp)
                    roles.Add(new SelectListItem { Text = item.RName, Value = item.RID.ToString() });
                return roles;
            }
        }

        internal static object CreateAccount(Models.User model)
        {
            using (WDMS_Entities db = new WDMS_Entities())
            {
                try {
                    var user = new Models.User();
                    user.UName = model.UName;
                    user.UAddress = model.UAddress;
                    user.UCNIC = model.UCNIC;
                    user.UPhoneNumber = model.UPhoneNumber;
                    user.UPassword = model.UPassword;
                    user.UPhoneNumber = model.UPhoneNumber;
                    user.URole = model.URole;
                    user.UUserName = model.UUserName;
                    db.Users.Add(user);
                    db.SaveChanges();
                    return "Success";
                }
                catch(DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
            }
        }
    }
}