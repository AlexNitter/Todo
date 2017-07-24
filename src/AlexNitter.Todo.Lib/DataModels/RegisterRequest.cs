using System;

namespace AlexNitter.Todo.Lib.DataModels
{
    public class RegisterRequest
    {
        public String Username { get; set; }
        public String Passwort { get; set; }
        public String PasswortWiederholung { get; set; }
    }
}
