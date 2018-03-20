﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ZaklepTo.Core.Domain
{
    public class Owner : User
    {
        public Restaurant Restaurant { get; private set; }

        public Owner(string login, string firstname, string lastname, string email,
            string phone, string password, string salt)
            : base(login, firstname, lastname, email, phone, password, salt)
        {
        }
    }
}
