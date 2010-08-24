using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDC2010.Core.Domain
{
    public class User
    {
        public virtual int Id { get; set; }

        private string _email;
        public virtual string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
            }
        }

        public virtual string Name { get; set; }
    }
}
