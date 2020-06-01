using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JayLib.WPF.BasicClass;

namespace DRJ.Models
{
    public class User : NotificationObject
    {
        private string account;

        public string Account
        {
            get { return account; }
            private set { account = value; }
        }

        private int authorizationLevel;

        public int AuthorizationLevel
        {
            get { return authorizationLevel; }
            set { authorizationLevel = value; }
        }

    }
}
