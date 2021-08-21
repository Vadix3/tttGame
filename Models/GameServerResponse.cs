using System;
using System.Collections.Generic;
using System.Text;

namespace tttGame.Models
{
    class GameServerResponse
    {
        public int id { get; set; }
        public int participant { get; set; }
        public int num_of_turns { get; set; }
        public DateTime date { get; set; }
        public string user_win { get; set; }

        public GameServerResponse() { }
        public GameServerResponse(int id, int participant, int num_of_turns, DateTime date, string user_win)
        {
            this.id = id;
            this.participant = participant;
            this.num_of_turns = num_of_turns;
            this.date = date;
            this.user_win = user_win;
        }


    }
}
