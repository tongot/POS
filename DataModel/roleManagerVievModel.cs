using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS
{
    public class roleManagerVievModel:BaseViewModel
    {
        public string Role { get; set; }
        public List<string> permissions { get; set; }
    }
    public class listRoleManagerViewModel:BaseViewModel
    {
        public List<roleManagerVievModel> roles { set; get; }
    }
}
