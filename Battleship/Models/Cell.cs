using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Models
{
    public class Cell : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public enum CellType { Water, Miss, Hit, Ship}; //Blue, White, Red, Gray

        private CellType privateCellType;

        public CellType cellType
        {
            get { return privateCellType; }
            set
            {
                privateCellType = value;
                FieldChanged();
            }
        }

        public enum ShipClass { None, Carrier, Battleship, Cruiser, Submarine, Destroyer };

        private ShipClass privateShipClass;

        public ShipClass shipClass
        {
            get { return privateShipClass; }
            set { privateShipClass = value; }
        }


        internal void SetAsWater()
        {
            this.cellType = CellType.Water;
        }

        internal void SetAsMiss()
        {
            this.cellType = CellType.Miss;
        }

        internal void SetAsHit()
        {
            this.cellType = CellType.Hit;
        }

        internal void SetAsShip()
        {
            this.cellType = CellType.Ship;
        }


        internal void SetAsCarrier()
        {
            this.shipClass = ShipClass.Carrier;
        }
        internal void SetAsBattleship()
        {
            this.shipClass = ShipClass.Battleship;
        }
        internal void SetAsCruiser()
        {
            this.shipClass = ShipClass.Cruiser;
        }
        internal void SetAsSub()
        {
            this.shipClass = ShipClass.Submarine;
        }
        internal void SetAsDestroyer()
        {
            this.shipClass = ShipClass.Destroyer;
        }


        private void FieldChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
