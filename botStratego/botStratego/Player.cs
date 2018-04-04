using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stratego
{
    public class Player
    {
        private string _name;
        private SpaceType _playerType;
        private PlayerColor _playerColor;
        public Position flagPos; // location of FLAG
        public string name
        {
            get { return _name; }
        }
        public SpaceType playerType
        {
            get { return _playerType; }
        }
        public PlayerColor playerColor
        {
            get { return _playerColor; }
        }
        public Player(string name, SpaceType playerType, PlayerColor playerColor)
        {
            _name = name;
            _playerType = playerType;
            _playerColor = playerColor;
        }
        public static Boolean operator ==(Player p1, Player p2)
        {
            if (p1.name == p2.name)
                return true;
            else
                return false;
        }
        public static Boolean operator !=(Player p1, Player p2)
        {
            if (p1.name == p2.name)
                return false;
            else
                return true;
        }

    }

    public enum PlayerColor { Red = 1, Blue = 2 }
}
