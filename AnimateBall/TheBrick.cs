using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimateBall
{
    class TheBrick
    {
        double _brickTop = 0;
        double _brickLeft = 0;
        string _name;
        Boolean _flag;
        int _hitPoints;
        int _brickPos;

        public TheBrick(double top, double left, string name)
        {
            this._brickLeft = left;
            this._brickTop = top;
            this._name = name;
            this._flag = false;
            this._hitPoints = 0;
            this._brickPos = 0;
        }
        public int BrickPos
        {
            get { return this._brickPos; }
            set { this._brickPos = value; }
        }

        public int HitPoints
        {
            get { return this._hitPoints; }
            set { this._hitPoints = value; }
        }

        public Boolean flag
        {
            get { return this._flag; }
            set { this._flag = value; }
        }

        public string Name
        {
            get { return this._name; }
            set { this._name = value; }
        }

        public double Top
        {
            get { return this._brickTop; }
            set { this._brickTop = value; }
        }

        public double Left
        {
            get { return this._brickLeft; }
            set { this._brickLeft = value; }
        }
    }
}
