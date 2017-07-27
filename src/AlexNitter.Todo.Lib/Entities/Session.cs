using AlexNitter.Todo.Lib.DataLayer.Repositories;
using System;

namespace AlexNitter.Todo.Lib.Entities
{
    public class Session
    {
        public String Id { get; set; }
        public Int32 UserId { get; set; }
        private User _user;
        public User User
        {
            get
            {
                if(_user == null)
                {
                    var repo = new UserRepository();
                    _user = repo.FindById(UserId);
                }

                return _user;
            }
        }
        public Boolean Aktiv { get; set; }
        public DateTime Erstellungsdatum { get; set; }
    }
}
