using System;
using System.Collections.Generic;
using System.Text;

namespace tttGame.Models
{
    public class GameHistory
    {
        public int id { get; set; }
        public string moves { get; set; }

        public GameHistory(int id, string moves)
        {
            this.id = id;
            this.moves = moves;
        }
        public GameHistory() { }

    }
}
