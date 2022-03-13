using PetsProject.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace PetsProject.Models.DAO
{
    public class userDao
    {
        private ProjectWebBanThuCungEntities2 db;

        public userDao()
        {
            this.db = new ProjectWebBanThuCungEntities2();
        }
        public bool checkExistUserName(string userName)
        {
            var user = db.accounts.FirstOrDefault(u => u.username == userName);
            if(user == null)
            {
                return false;
            }
            return true;
        }

        public APP_USER getInfUserByUserName(string username)
        {
            APP_USER user = new Models.APP_USER();
            return db.APP_USER.Where(u => u.USER_NAME.Equals(username)).FirstOrDefault();

        }
        public bool changNewPass(string username, string password)
        {
            bool success = false;
       
            var newPassword = encMd5PassWord(password);

            APP_USER userRecord = db.APP_USER.Where(user => user.USER_NAME.Equals(username)).FirstOrDefault();
            if (userRecord != null)
            {
                userRecord.ENCRYTED_PASSWORD = newPassword;
                db.SaveChanges();
                success = true;
            }
            return success;
        }
        public bool checkExistAccount(string username, string email)
        {
            bool existU = false;

            if(db.accounts.Where(acc => acc.email.Equals(email) && acc.username.Equals(username)).FirstOrDefault() != null)
            {
                existU = true;
            }

            return existU;
        }

        public string encMd5PassWord(string password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            System.Text.UTF8Encoding encoder = new UTF8Encoding();
            Byte[] originalBytes = encoder.GetBytes(password);
            Byte[] encodedBytes = md5.ComputeHash(originalBytes);
            password = BitConverter.ToString(encodedBytes).Replace("-", "");
            var result = password.ToLower();
            return result;
        }
        
        public bool sign(APP_USER user)
        {
            var passwordEncode = encMd5PassWord(user.ENCRYTED_PASSWORD);
            user.ENCRYTED_PASSWORD = passwordEncode;
      
            APP_USER userRecord = db.APP_USER.Where(u => u.USER_NAME.Equals(user.USER_NAME) && u.ENCRYTED_PASSWORD.Equals(user.ENCRYTED_PASSWORD) && u.ENABLED).FirstOrDefault();
            if(userRecord!= null)
            {
                return true;
            }
            return false;
        }

        public bool changePass(string userName, string oldPass, string newPass)
        {
            bool success = false;
            var currentPass = encMd5PassWord(oldPass);
            var newPassword = encMd5PassWord(newPass);

            APP_USER userRecord = db.APP_USER.Where(user => user.USER_NAME.Equals(userName) && user.ENCRYTED_PASSWORD.Equals(currentPass)).FirstOrDefault();
            if(userRecord != null)
            {
                userRecord.ENCRYTED_PASSWORD = newPassword;
                db.SaveChanges();
                success = true;
            }
            return success;
                
        }
        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public account getUserByUserName(string userame)
        {
            return db.accounts.Where(user => user.username.Equals(userame)).FirstOrDefault();
        }
 
        public bool signUp(string fullName, DateTime birthday,string address, string email, string phone, string username, string password)
        {
            if(checkExistUserName(username))
            {
                return false;
            }
            else
            {
                // Mặc định người dùng đăng ký sẽ chỉ có 1 quyền USER
                int roleUser = 2;
                long idUser = createIdUser();

                CreateRandomID cr = new CreateRandomID();

                account newAccount = new account();
                newAccount.username = username;
                newAccount.fullname = fullName;
                newAccount.email = email;
                newAccount.avatar = "img/account/default.jpg";
                newAccount.birthday = birthday;
                newAccount.phone = phone;
                newAccount.address = address;

                USER_ROLE userRole = new USER_ROLE();
                userRole.USER_ID = idUser;
                userRole.ROLE_ID = roleUser;

                long idRole = createId();
                userRole.ID = idRole;
                System.Diagnostics.Debug.WriteLine(idRole);


                APP_USER appUser = new APP_USER();
                appUser.USER_ID = idUser;
                appUser.USER_NAME = username;
                appUser.ENCRYTED_PASSWORD = encMd5PassWord(password);
                appUser.ENABLED = true;

                address_Book ab = new address_Book();
                ab.id = cr.newIDAddressBook();
                ab.username = username;
                ab.address = newAccount.address;
                // Mặc định khi tạo mới user sẽ lấy address mới nhập thành AB mặc định (tiện trong việc điền thông tin thanh toán)
                ab.status = "DEFAULT";

                db.accounts.Add(newAccount);
                db.APP_USER.Add(appUser);
                db.USER_ROLE.Add(userRole);
                db.address_Book.Add(ab);
                db.SaveChanges();

                return true;
            }
        }
        public  long createIdUser()
        {
            string idUser = "";
            Random random = new Random();
            for (var i = 0; i < 6; i++)
            {


                long r = random.Next(1, 9);
                idUser += r;
            }
            return Int32.Parse(idUser);
        }

        public long createId()
        {
            string id = "";
            Random random = new Random();
            for (var i = 0; i < 6; i++)
            {


                long r = random.Next(1, 9);
                id += r;
            }
            return Int32.Parse(id);
        }
        public long otpCode()
        {
            string otp = "";
            Random random = new Random();
            for (var i = 0; i < 6; i++)
            {


                long r = random.Next(1, 9);
                otp += r;
            }
            return Int32.Parse(otp);
        }






    }
}