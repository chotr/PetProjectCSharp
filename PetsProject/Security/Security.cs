using PetsProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace PetsProject.Security
{
    public class Security : RoleProvider
    {
        ProjectWebBanThuCungEntities2 db = new ProjectWebBanThuCungEntities2();
        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }
        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }
        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }
        public override string[] GetRolesForUser(string username)
        {
            APP_USER au=db.APP_USER.SingleOrDefault(x => x.USER_NAME == username);
            List<USER_ROLE> listUser = db.USER_ROLE.Where(x => x.USER_ID == au.USER_ID).ToList();

            String[] result = new string[listUser.Count];
            int count = 0;
            foreach (var item in listUser)
            {
                result[count] = db.APP_ROLE.Find(item.ROLE_ID).ROLE_NAME;
                count++;
            }
            return result;


        }
        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }
        public override bool IsUserInRole(string username, string roleName)
        {
            APP_USER au = db.APP_USER.SingleOrDefault(x => x.USER_NAME == username);
            List<USER_ROLE> listUser = db.USER_ROLE.Where(x => x.USER_ID == au.USER_ID).ToList();
            String checkRole = "";
            foreach (var item in listUser)
            {
               checkRole = db.APP_ROLE.Find(item.ROLE_ID).ROLE_NAME;
                if (checkRole == roleName)
                    return true;
            }
            return false;
        }
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }
        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}