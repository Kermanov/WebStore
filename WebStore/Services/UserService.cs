using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.UnitOfWork;

namespace WebStore.Services
{
    public class UserService
    {
        private readonly WebStoreUnitOfWork unitOfWork;

        public UserService(WebStoreUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<IdentityUser> GetAll()
        {
            return unitOfWork.Users;
        }

        public Dictionary<IdentityUser, string> GetUsersWithRoles()
        {
            var users = new Dictionary<IdentityUser, string>();
            List<IdentityUser> allUsers = unitOfWork.Users.ToList();

            foreach (var user in allUsers)
            {
                var userRole = unitOfWork.UserRoles.Where(role => role.UserId == user.Id).FirstOrDefault();

                string roleName;
                if (userRole != null)
                {
                    roleName = unitOfWork.Roles.Where(role => role.Id == userRole.RoleId).FirstOrDefault().Name;
                }
                else
                {
                    roleName = "User";
                }

                users.Add(user, roleName);
            }

            return users;
        }

        public IdentityUser GetUser(string id)
        {
            return unitOfWork.Users.Find(id);
        }
    }
}
