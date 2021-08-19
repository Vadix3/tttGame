using System;
using System.Collections.Generic;
using System.Text;

namespace tttGame
{
    public class Players
    {
        public int id { get; set; }
        public string name { get; set; }
        public string last_name { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string confirmPassword { get; set; }
        public string num_of_games { get; set; }

        public Players(int id, string name, string last_name, string username, string password, string confirmPassword, string num_of_games)
        {
            this.id = id;
            this.name = name;
            this.last_name = last_name;
            this.username = username;
            this.password = password;
            this.confirmPassword = confirmPassword;
            this.num_of_games = num_of_games;
        }
    }
}
