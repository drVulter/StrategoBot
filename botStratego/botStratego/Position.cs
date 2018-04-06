using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stratego
{
    [Serializable]
    public struct Position
    {
        public int _row,_col;
        public int row 
        {   get
            {
                return _row;
            }
            set 
            {
                this._row = value;
                if (value > 9) this._row = 9;
                if (value < 0) this._row = 0;
            } 
        }
        public int col 
        { 
            get
            {
                return _col;
            }
            set
            {
                this._col = value;
                if (value > 9) this._col = 9;
                if (value < 0) this._col = 0;
            }
        }

        public Position (int row, int col){
            //Use of possibly unassigned field error
            _row = 0;
            _col = 0;
            _row = row;
            _col = col;
        }

        public static  bool operator ==(Position p1, Position p2)
        {
            if(p1.col==p2.col && p1.row==p2.row)return true;
            return false;
        }
        public static  bool operator !=(Position p1, Position p2)
        {
            if (p1.col != p2.col || p1.row != p2.row) return true;
            return false;
        }
    }
}
