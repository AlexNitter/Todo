using System;
using System.Collections.Generic;
using System.Text;

namespace AlexNitter.Todo.Lib.DataModels
{
    public class LoginRequest
    {
        public String Username { get; set; }
        public String Passwort { get; set; }
        public String PasswortWiederholung { get; set; }
    }
}
