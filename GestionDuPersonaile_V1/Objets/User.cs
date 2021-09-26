using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GestionDuPersonaile_V1.Objets
{
    public class User
    {
        private string _username;
        private string _password;
        public int type; // 1=homme 2=femme
        private int _grade; // le grade va determiner par la suite si le user est un admin ou une secretaire !
        public string question;
        public int position;
        public bool tentative;

       

        public User()
        {
            this._username = "HamzaMouad";
            this._password = "tdm";
            this._grade = 10;
            this.type = 1;
            this.question = "test";
            this.position=1;
            this.tentative = true;
        }

        public User(string _username, string _password, int _grade, int type, string question, int position) 
        {
            this._username = _username;
            this._password = _password;
            this._grade = _grade;
            this.type = type;
            this.question = question;
            this.position = position;
            this.tentative = true;
        }





        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        public int Grade
        {
            get { return _grade; }
            set { _grade = value; }
        }
    }
}
