using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Battleship.Models;

namespace Battleship.Converters
{
    public class EnumToFillConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            SolidColorBrush scb = new SolidColorBrush(Colors.Transparent);

            if(value is Cell.CellType.Water)
            {
                scb = new SolidColorBrush(Colors.SkyBlue);
            }
            else if (value is Cell.CellType.Miss)
            {
                scb = new SolidColorBrush(Colors.White);
            }
            else if (value is Cell.CellType.Hit)
            {
                scb = new SolidColorBrush(Colors.Red);
            }
            else if (value is Cell.CellType.Ship)
            {
                scb = new SolidColorBrush(Colors.Gray);
            }
            return scb;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            //Cell c = new Cell();
            //if(value is SolidColorBrush)
            //{
            //    c.cellType = Cell.CellType.Ship;
            //}
            //return c;
            throw new NotImplementedException();
        }
    }
}
