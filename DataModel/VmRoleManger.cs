

using AppDatabase;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace POS
{
    public class VmRoleManger : BaseViewModel
    {
        
        IPermissions db;
        IRole dbr;
        public ObservableCollection<vmRoles> Roles { get; set; }
        public ObservableCollection<rolesToPermissions> permissions { get; set; } = new ObservableCollection<rolesToPermissions>();
        public string roleName { get; set; }
        public bool isSelected { get; set; }
        public vmRoles Role { get; set; }
        public string error { get; set; } = "";
        public string searchName { get; set; }
        public VmRoleManger()
        {
            db = new PermissionApp();
            dbr = new RoleApp();
            getAllRoles();
        }
        public void getAllRoles()
        {
            if(Roles!=null )
            {
                Roles.Clear();
            }
            Roles = new ObservableCollection<vmRoles>(setRolesList());
        }
        List<vmRoles> setRolesList()
        {
            List<vmRoles> r = new List<vmRoles>();
            foreach (var item in dbr.roles(searchName))
            {
                r.Add(setvmRole(item));
            }
            return r;
        }
        vmRoles setvmRole(Role r)
        {
            vmRoles rm = new vmRoles();
            rm.Name = r.Name;
            rm.RoleId = r.RoleId;

            return rm;

        }
        Role setRole(vmRoles r)
        {
            Role rm = new Role();
            rm.Name = r.Name;
            rm.RoleId = r.RoleId;
            return rm;
        }
        public bool addRoles()
        {
            if (validateA())
            {
                Role role = new Role();
                role.Name = setRole(Role).Name.Trim();
                dbr.addRole(role);
                return true;
            }
            return false;

        }
        public bool validateA()
        {
            error = string.Empty;
            if (string.IsNullOrWhiteSpace(Role.Name))
            {
                error = "please Enter role name";
                return false;
            }
            if (dbr.roleExist(Role.Name.Trim()))
            {
                error = "this role already exist";
                return false;
            }
            return true;
        }
        public bool validateE()
        {
            error = string.Empty;
            if (string.IsNullOrWhiteSpace(Role.Name))
            {
                error = "please Enter role name";
                return false;
            }
            return true;
        }
            public void getPermissions(int RoleId)
        {
            foreach (var item2 in db.Permissions())
            {
                rolesToPermissions permission = new rolesToPermissions();
                foreach (var item in db.getPermissionsForRole(RoleId))
                {
                    
                    if (item.PermissionsId == item2.PermissionsId)
                    {
                        permission.isSelected = true;
                    }
                } 
                permission.permissionName = item2.name;
                    permission.RoleId = RoleId;
                    permission.permissionId = item2.PermissionsId;
                    permissions.Add(permission);
            }
        }
        public void setPermissions()
        {
            List<permissionToRoles> permissionToRoles = new List<permissionToRoles>();
            foreach (var item in permissions.Where(x => x.isSelected))
            {
                permissionToRoles pr = new permissionToRoles();
                pr.RoleId = item.RoleId;
                pr.PermissionsId = item.permissionId;
                permissionToRoles.Add(pr);
            }
            db.setPermission(permissionToRoles);

        }
        public void deleteRole()
        {
            dbr.deleteRole(Role.RoleId);
        }
       
    } 
    public class rolesToPermissions:BaseViewModel
        {
            public int RoleId { get; set; }
            public string permissionName { get; set; }
            public int permissionId { get; set; }
            public bool isSelected { get; set; }
        }
    public class vmRoles:BaseViewModel
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        public bool isSelected { get; set; }
    }
}
