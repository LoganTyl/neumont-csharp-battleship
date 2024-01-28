using Battleship.Converters;
using Battleship.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Battleship
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Board : Page
    {
        Rectangle rect;
        Random rand = new Random();

        int playerShipsPlaced = 0;

        bool horizontal = false;
        bool validShot = false;
        bool cheatModeOn = false;

        public Board()
        {
            this.InitializeComponent();
            SetUpGrids(PlayerGrid);
            SetUpGrids(AIGrid);
            AlignmentIndicator.Visibility = Visibility.Visible;
            Window.Current.CoreWindow.KeyDown += CoreWindow_RotateShip;
            FirstLoad();
        }

        private async void FirstLoad()
        {
            await new MessageDialog("The player's grid is on the left. The AI's grid is on the right.\nPlace a ship by left clicking on a space.\nRotate your ship by pressing Shift.\nShips are placed in " +
                "order of: Carrier, Battleship, Cruiser, Sub, Destroyer\nAfter all ships are placed, you can begin firing. Good luck!", "Welcome to Battleship!").ShowAsync();
        }

        private void CoreWindow_RotateShip(CoreWindow sender, KeyEventArgs args)
        {
            if (args.VirtualKey == VirtualKey.Shift)
            {
                if (AlignmentIndicator.Text == "Orientation: Vertical")
                {
                    AlignmentIndicator.Text = "Orientation: Horizontal";
                }
                else if (AlignmentIndicator.Text == "Orientation: Horizontal")
                {
                    AlignmentIndicator.Text = "Orientation: Vertical";
                }
                horizontal = !horizontal;
            }
        }

        private void SetUpGrids(Grid chosenGrid)
        {
            for (int r = 0; r < chosenGrid.RowDefinitions.Count; r++)
            {
                for (int c = 0; c < chosenGrid.ColumnDefinitions.Count; c++)
                {
                    //EnumToFillConverter enumToFillConverter = new EnumToFillConverter();
                    //Binding b = new Binding()
                    //{
                    //    Path = new PropertyPath("cellType"),
                    //    Mode = BindingMode.OneWay,
                    //    Converter = enumToFillConverter
                    //};

                    rect = new Rectangle();
                    rect.DataContext = new Cell();
                    (rect.DataContext as Cell).SetAsWater();
                    //rect.SetBinding(Rectangle.FillProperty, b);
                    rect.SetValue(Rectangle.FillProperty, new SolidColorBrush(Colors.SkyBlue));
                    rect.SetValue(Grid.RowProperty, r);
                    rect.SetValue(Grid.ColumnProperty, c);
                    rect.Stroke = new SolidColorBrush(Colors.Black);
                    if (chosenGrid == PlayerGrid)
                    {
                        rect.Tapped += SetShip;
                    }
                    chosenGrid.Children.Add(rect);
                }
            }
        }

        private void SetShip(object sender, TappedRoutedEventArgs e)
        {
            rect = (Rectangle)sender;
            try
            {
                switch (playerShipsPlaced)
                {
                    case 0:
                        if (!horizontal)
                        {
                            if ((int)rect.GetValue(Grid.RowProperty) + 5 > 10)
                            {
                                throw new ArgumentException("Ships placed must be in valid locations");
                            }
                            for (int i = 0; i < 5; i++)
                            {
                                if ((GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty) + i, (int)rect.GetValue(Grid.ColumnProperty)).DataContext as Cell).cellType != Cell.CellType.Water)
                                {
                                    throw new ArgumentException("Ships placed must be in valid locations");
                                }
                            }
                            for (int i = 0; i < 5; i++)
                            {
                                (GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty) + i, (int)rect.GetValue(Grid.ColumnProperty)).DataContext as Cell).SetAsShip();
                                (GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty) + i, (int)rect.GetValue(Grid.ColumnProperty)).DataContext as Cell).SetAsCarrier();
                                GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty) + i, (int)rect.GetValue(Grid.ColumnProperty)).Fill = new SolidColorBrush(Colors.Gray);
                            }
                        }
                        else
                        {
                            if ((int)rect.GetValue(Grid.ColumnProperty) + 5 > 10)
                            {
                                throw new ArgumentException("Ships placed must be in valid locations");
                            }
                            for (int i = 0; i < 5; i++)
                            {
                                if ((GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty), (int)rect.GetValue(Grid.ColumnProperty) + i).DataContext as Cell).cellType != Cell.CellType.Water)
                                {
                                    throw new ArgumentException("Ships placed must be in valid locations");
                                }
                            }
                            for (int i = 0; i < 5; i++)
                            {
                                (GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty), (int)rect.GetValue(Grid.ColumnProperty) + i).DataContext as Cell).SetAsShip();
                                (GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty), (int)rect.GetValue(Grid.ColumnProperty) + i).DataContext as Cell).SetAsCarrier();
                                GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty), (int)rect.GetValue(Grid.ColumnProperty) + i).Fill = new SolidColorBrush(Colors.Gray);
                            }
                        }
                        break;
                    case 1:
                        if (!horizontal)
                        {
                            if ((int)rect.GetValue(Grid.RowProperty) + 4 > 10)
                            {
                                throw new ArgumentException("Ships placed must be in valid locations");
                            }
                            for (int i = 0; i < 4; i++)
                            {
                                if ((GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty) + i, (int)rect.GetValue(Grid.ColumnProperty)).DataContext as Cell).cellType != Cell.CellType.Water)
                                {
                                    throw new ArgumentException("Ships placed must be in valid locations");
                                }
                            }
                            for (int i = 0; i < 4; i++)
                            {
                                (GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty) + i, (int)rect.GetValue(Grid.ColumnProperty)).DataContext as Cell).SetAsShip();
                                (GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty) + i, (int)rect.GetValue(Grid.ColumnProperty)).DataContext as Cell).SetAsBattleship();
                                GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty) + i, (int)rect.GetValue(Grid.ColumnProperty)).Fill = new SolidColorBrush(Colors.Gray);
                            }
                        }
                        else
                        {
                            if ((int)rect.GetValue(Grid.ColumnProperty) + 4 > 10)
                            {
                                throw new ArgumentException("Ships placed must be in valid locations");
                            }
                            for (int i = 0; i < 4; i++)
                            {
                                if ((GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty), (int)rect.GetValue(Grid.ColumnProperty) + i).DataContext as Cell).cellType != Cell.CellType.Water)
                                {
                                    throw new ArgumentException("Ships placed must be in valid locations");
                                }
                            }
                            for (int i = 0; i < 4; i++)
                            {
                                (GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty), (int)rect.GetValue(Grid.ColumnProperty) + i).DataContext as Cell).SetAsShip();
                                (GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty), (int)rect.GetValue(Grid.ColumnProperty) + i).DataContext as Cell).SetAsBattleship();
                                GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty), (int)rect.GetValue(Grid.ColumnProperty) + i).Fill = new SolidColorBrush(Colors.Gray);
                            }
                        }
                        break;
                    case 2:
                        if (!horizontal)
                        {
                            if ((int)rect.GetValue(Grid.RowProperty) + 3 > 10)
                            {
                                throw new ArgumentException("Ships placed must be in valid locations");
                            }
                            for (int i = 0; i < 3; i++)
                            {
                                if ((GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty) + i, (int)rect.GetValue(Grid.ColumnProperty)).DataContext as Cell).cellType != Cell.CellType.Water)
                                {
                                    throw new ArgumentException("Ships placed must be in valid locations");
                                }
                            }
                            for (int i = 0; i < 3; i++)
                            {
                                (GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty) + i, (int)rect.GetValue(Grid.ColumnProperty)).DataContext as Cell).SetAsShip();
                                (GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty) + i, (int)rect.GetValue(Grid.ColumnProperty)).DataContext as Cell).SetAsCruiser();
                                GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty) + i, (int)rect.GetValue(Grid.ColumnProperty)).Fill = new SolidColorBrush(Colors.Gray);
                            }
                        }
                        else
                        {
                            if ((int)rect.GetValue(Grid.ColumnProperty) + 3 > 10)
                            {
                                throw new ArgumentException("Ships placed must be in valid locations");
                            }
                            for (int i = 0; i < 3; i++)
                            {
                                if ((GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty), (int)rect.GetValue(Grid.ColumnProperty) + i).DataContext as Cell).cellType != Cell.CellType.Water)
                                {
                                    throw new ArgumentException("Ships placed must be in valid locations");
                                }
                            }
                            for (int i = 0; i < 3; i++)
                            {
                                (GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty), (int)rect.GetValue(Grid.ColumnProperty) + i).DataContext as Cell).SetAsShip();
                                (GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty), (int)rect.GetValue(Grid.ColumnProperty) + i).DataContext as Cell).SetAsCruiser();
                                GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty), (int)rect.GetValue(Grid.ColumnProperty) + i).Fill = new SolidColorBrush(Colors.Gray);
                            }
                        }
                        break;
                    case 3:
                        if (!horizontal)
                        {
                            if ((int)rect.GetValue(Grid.RowProperty) + 3 > 10)
                            {
                                throw new ArgumentException("Ships placed must be in valid locations");
                            }
                            for (int i = 0; i < 3; i++)
                            {
                                if ((GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty) + i, (int)rect.GetValue(Grid.ColumnProperty)).DataContext as Cell).cellType != Cell.CellType.Water)
                                {
                                    throw new ArgumentException("Ships placed must be in valid locations");
                                }
                            }
                            for (int i = 0; i < 3; i++)
                            {
                                (GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty) + i, (int)rect.GetValue(Grid.ColumnProperty)).DataContext as Cell).SetAsShip();
                                (GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty) + i, (int)rect.GetValue(Grid.ColumnProperty)).DataContext as Cell).SetAsSub();
                                GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty) + i, (int)rect.GetValue(Grid.ColumnProperty)).Fill = new SolidColorBrush(Colors.Gray);
                            }
                        }
                        else
                        {
                            if ((int)rect.GetValue(Grid.ColumnProperty) + 3 > 10)
                            {
                                throw new ArgumentException("Ships placed must be in valid locations");
                            }
                            for (int i = 0; i < 3; i++)
                            {
                                if ((GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty), (int)rect.GetValue(Grid.ColumnProperty) + i).DataContext as Cell).cellType != Cell.CellType.Water)
                                {
                                    throw new ArgumentException("Ships placed must be in valid locations");
                                }
                            }
                            for (int i = 0; i < 3; i++)
                            {
                                (GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty), (int)rect.GetValue(Grid.ColumnProperty) + i).DataContext as Cell).SetAsShip();
                                (GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty), (int)rect.GetValue(Grid.ColumnProperty) + i).DataContext as Cell).SetAsSub();
                                GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty), (int)rect.GetValue(Grid.ColumnProperty) + i).Fill = new SolidColorBrush(Colors.Gray);
                            }
                        }
                        break;
                    case 4:
                        if (!horizontal)
                        {
                            if ((int)rect.GetValue(Grid.RowProperty) + 2 > 10)
                            {
                                throw new ArgumentException("Ships placed must be in valid locations");
                            }
                            for (int i = 0; i < 2; i++)
                            {
                                if ((GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty) + i, (int)rect.GetValue(Grid.ColumnProperty)).DataContext as Cell).cellType != Cell.CellType.Water)
                                {
                                    throw new ArgumentException("Ships placed must be in valid locations");
                                }
                            }
                            for (int i = 0; i < 2; i++)
                            {
                                (GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty) + i, (int)rect.GetValue(Grid.ColumnProperty)).DataContext as Cell).SetAsShip();
                                (GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty) + i, (int)rect.GetValue(Grid.ColumnProperty)).DataContext as Cell).SetAsDestroyer();
                                GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty) + i, (int)rect.GetValue(Grid.ColumnProperty)).Fill = new SolidColorBrush(Colors.Gray);
                            }
                        }
                        else
                        {
                            if ((int)rect.GetValue(Grid.ColumnProperty) + 2 > 10)
                            {
                                throw new ArgumentException("Ships placed must be in valid locations");
                            }
                            for (int i = 0; i < 2; i++)
                            {
                                if ((GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty), (int)rect.GetValue(Grid.ColumnProperty) + i).DataContext as Cell).cellType != Cell.CellType.Water)
                                {
                                    throw new ArgumentException("Ships placed must be in valid locations");
                                }
                            }
                            for (int i = 0; i < 2; i++)
                            {
                                (GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty), (int)rect.GetValue(Grid.ColumnProperty) + i).DataContext as Cell).SetAsShip();
                                (GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty), (int)rect.GetValue(Grid.ColumnProperty) + i).DataContext as Cell).SetAsDestroyer();
                                GetPlayerGridRectangle((int)rect.GetValue(Grid.RowProperty), (int)rect.GetValue(Grid.ColumnProperty) + i).Fill = new SolidColorBrush(Colors.Gray);
                            }
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("Ships placed can only be between 1 and 4");
                }
            }
            catch (ArgumentException)
            {
                playerShipsPlaced--;
                IndicateInvalidPlacement();
            }
            playerShipsPlaced++;
            AlignmentIndicator.Text = "Orientation: Vertical";
            horizontal = false;

            if (playerShipsPlaced == 5)
            {
                AlignmentIndicator.Visibility = Visibility.Collapsed;
                SetAIShips();
            }
        }

        private async void IndicateInvalidPlacement()
        {
            await new MessageDialog("Your ship placement is not valid. Please put your ship in a valid location.", "Invalid Ship Placement").ShowAsync();
        }

        private void SetAIShips()
        {
            for (int j = 0; j < 5; j++)
            {
                int row = rand.Next(10);
                int col = rand.Next(10);
                int AIhorizontal = rand.Next(2);
                rect = (Rectangle)GetAIGridRectangle(row, col);
                switch (j)
                {
                    case 0:
                        if (AIhorizontal == 0)
                        {
                            if (row + 5 > 10)
                            {
                                goto Invalid;
                            }
                            for (int i = 0; i < 5; i++)
                            {
                                if ((GetAIGridRectangle(row + i, col).DataContext as Cell).cellType != Cell.CellType.Water)
                                {
                                    goto Invalid;
                                }
                            }
                            for (int i = 0; i < 5; i++)
                            {
                                (GetAIGridRectangle(row + i, col).DataContext as Cell).SetAsShip();
                                (GetAIGridRectangle((int)rect.GetValue(Grid.RowProperty) + i, (int)rect.GetValue(Grid.ColumnProperty)).DataContext as Cell).SetAsCarrier();
                                GetAIGridRectangle(row + i, col).SetValue(Rectangle.FillProperty, new SolidColorBrush(Colors.SkyBlue));
                            }
                            goto Finish;
                        }
                        else
                        {
                            if (col + 5 > 10)
                            {
                                goto Invalid;
                            }
                            for (int i = 0; i < 5; i++)
                            {
                                if ((GetAIGridRectangle(row, col + i).DataContext as Cell).cellType != Cell.CellType.Water)
                                {
                                    goto Invalid;
                                }
                            }
                            for (int i = 0; i < 5; i++)
                            {
                                (GetAIGridRectangle(row, col + i).DataContext as Cell).SetAsShip();
                                (GetAIGridRectangle((int)rect.GetValue(Grid.RowProperty), (int)rect.GetValue(Grid.ColumnProperty) + i).DataContext as Cell).SetAsCarrier();
                                GetAIGridRectangle(row, col + i).SetValue(Rectangle.FillProperty, new SolidColorBrush(Colors.SkyBlue));
                            }
                            goto Finish;
                        }
                    case 1:
                        if (AIhorizontal == 0)
                        {
                            if (row + 4 > 10)
                            {
                                goto Invalid;
                            }
                            for (int i = 0; i < 4; i++)
                            {
                                if ((GetAIGridRectangle(row + i, col).DataContext as Cell).cellType != Cell.CellType.Water)
                                {
                                    goto Invalid;
                                }
                            }
                            for (int i = 0; i < 4; i++)
                            {
                                (GetAIGridRectangle(row + i, col).DataContext as Cell).SetAsShip();
                                (GetAIGridRectangle((int)rect.GetValue(Grid.RowProperty) + i, (int)rect.GetValue(Grid.ColumnProperty)).DataContext as Cell).SetAsBattleship();
                                GetAIGridRectangle(row + i, col).SetValue(Rectangle.FillProperty, new SolidColorBrush(Colors.SkyBlue));
                            }
                            goto Finish;
                        }
                        else
                        {
                            if (col + 4 > 10)
                            {
                                goto Invalid;
                            }
                            for (int i = 0; i < 4; i++)
                            {
                                if ((GetAIGridRectangle(row, col + i).DataContext as Cell).cellType != Cell.CellType.Water)
                                {
                                    goto Invalid;
                                }
                            }
                            for (int i = 0; i < 4; i++)
                            {
                                (GetAIGridRectangle(row, col + i).DataContext as Cell).SetAsShip();
                                (GetAIGridRectangle((int)rect.GetValue(Grid.RowProperty), (int)rect.GetValue(Grid.ColumnProperty) + i).DataContext as Cell).SetAsBattleship();
                                GetAIGridRectangle(row, col + i).SetValue(Rectangle.FillProperty, new SolidColorBrush(Colors.SkyBlue));
                            }
                            goto Finish;
                        }
                    case 2:
                        if (AIhorizontal == 0)
                        {
                            if (row + 3 > 10)
                            {
                                goto Invalid;
                            }
                            for (int i = 0; i < 3; i++)
                            {
                                if ((GetAIGridRectangle(row + i, col).DataContext as Cell).cellType != Cell.CellType.Water)
                                {
                                    goto Invalid;
                                }
                            }
                            for (int i = 0; i < 3; i++)
                            {
                                (GetAIGridRectangle(row + i, col).DataContext as Cell).SetAsShip();
                                (GetAIGridRectangle((int)rect.GetValue(Grid.RowProperty) + i, (int)rect.GetValue(Grid.ColumnProperty)).DataContext as Cell).SetAsCruiser();
                                GetAIGridRectangle(row + i, col).SetValue(Rectangle.FillProperty, new SolidColorBrush(Colors.SkyBlue));
                            }
                            goto Finish;
                        }
                        else
                        {
                            if (col + 3 > 10)
                            {
                                goto Invalid;
                            }
                            for (int i = 0; i < 3; i++)
                            {
                                if ((GetAIGridRectangle(row, col + i).DataContext as Cell).cellType != Cell.CellType.Water)
                                {
                                    goto Invalid;
                                }
                            }
                            for (int i = 0; i < 3; i++)
                            {
                                (GetAIGridRectangle(row, col + i).DataContext as Cell).SetAsShip();
                                (GetAIGridRectangle((int)rect.GetValue(Grid.RowProperty), (int)rect.GetValue(Grid.ColumnProperty) + i).DataContext as Cell).SetAsCruiser();
                                GetAIGridRectangle(row, col + i).SetValue(Rectangle.FillProperty, new SolidColorBrush(Colors.SkyBlue));
                            }
                            goto Finish;
                        }
                    case 3:
                        if (AIhorizontal == 0)
                        {
                            if (row + 3 > 10)
                            {
                                goto Invalid;
                            }
                            for (int i = 0; i < 3; i++)
                            {
                                if ((GetAIGridRectangle(row + i, col).DataContext as Cell).cellType != Cell.CellType.Water)
                                {
                                    goto Invalid;
                                }
                            }
                            for (int i = 0; i < 3; i++)
                            {
                                (GetAIGridRectangle(row + i, col).DataContext as Cell).SetAsShip();
                                (GetAIGridRectangle((int)rect.GetValue(Grid.RowProperty) + i, (int)rect.GetValue(Grid.ColumnProperty)).DataContext as Cell).SetAsSub();
                                GetAIGridRectangle(row + i, col).SetValue(Rectangle.FillProperty, new SolidColorBrush(Colors.SkyBlue));
                            }
                            goto Finish;
                        }
                        else
                        {
                            if (col + 3 > 10)
                            {
                                goto Invalid;
                            }
                            for (int i = 0; i < 3; i++)
                            {
                                if ((GetAIGridRectangle(row, col + i).DataContext as Cell).cellType != Cell.CellType.Water)
                                {
                                    goto Invalid;
                                }
                            }
                            for (int i = 0; i < 3; i++)
                            {
                                (GetAIGridRectangle(row, col + i).DataContext as Cell).SetAsShip();
                                (GetAIGridRectangle((int)rect.GetValue(Grid.RowProperty), (int)rect.GetValue(Grid.ColumnProperty) + i).DataContext as Cell).SetAsSub();
                                GetAIGridRectangle(row, col + i).SetValue(Rectangle.FillProperty, new SolidColorBrush(Colors.SkyBlue));
                            }
                            goto Finish;
                        }
                    case 4:
                        if (AIhorizontal == 0)
                        {
                            if (row + 2 > 10)
                            {
                                goto Invalid;
                            }
                            for (int i = 0; i < 2; i++)
                            {
                                if ((GetAIGridRectangle(row + i, col).DataContext as Cell).cellType != Cell.CellType.Water)
                                {
                                    goto Invalid;
                                }
                            }
                            for (int i = 0; i < 2; i++)
                            {
                                (GetAIGridRectangle(row + i, col).DataContext as Cell).SetAsShip();
                                (GetAIGridRectangle((int)rect.GetValue(Grid.RowProperty) + i, (int)rect.GetValue(Grid.ColumnProperty)).DataContext as Cell).SetAsDestroyer();
                                GetAIGridRectangle(row + i, col).SetValue(Rectangle.FillProperty, new SolidColorBrush(Colors.SkyBlue));
                            }
                            goto Finish;
                        }
                        else
                        {
                            if (col + 2 > 10)
                            {
                                goto Invalid;
                            }
                            for (int i = 0; i < 2; i++)
                            {
                                if ((GetAIGridRectangle(row, col + i).DataContext as Cell).cellType != Cell.CellType.Water)
                                {
                                    goto Invalid;
                                }
                            }
                            for (int i = 0; i < 2; i++)
                            {
                                (GetAIGridRectangle(row, col + i).DataContext as Cell).SetAsShip();
                                (GetAIGridRectangle((int)rect.GetValue(Grid.RowProperty), (int)rect.GetValue(Grid.ColumnProperty) + i).DataContext as Cell).SetAsDestroyer();
                                GetAIGridRectangle(row, col + i).SetValue(Rectangle.FillProperty, new SolidColorBrush(Colors.SkyBlue));
                            }
                            goto Finish;
                        }
                    default:
                        throw new ArgumentOutOfRangeException("Ships placed can only be between 1 and 4");
                }
            Finish:
                continue;

            Invalid:
                j--;
                continue;
            }
            FinalSetUp();
        }

        private void FinalSetUp()
        {
            for (int r = 0; r < PlayerGrid.RowDefinitions.Count; r++)
            {
                for (int c = 0; c < PlayerGrid.ColumnDefinitions.Count; c++)
                {
                    GetPlayerGridRectangle(r, c).Tapped -= SetShip;
                    GetAIGridRectangle(r, c).Tapped += PlayerTurnClick;
                }
            }
            Window.Current.CoreWindow.KeyDown -= CoreWindow_RotateShip;
            Window.Current.CoreWindow.KeyDown += CoreWindow_SetCheatMode;
        }

        private void PlayerTurnClick(object sender, TappedRoutedEventArgs e)
        {
            rect = (Rectangle)sender;
            if ((rect.DataContext as Cell).cellType == Cell.CellType.Hit || (rect.DataContext as Cell).cellType == Cell.CellType.Miss)
            {
                IndicateInvalidMove();
                validShot = false;
            }
            else if ((rect.DataContext as Cell).cellType == Cell.CellType.Water)
            {
                (rect.DataContext as Cell).SetAsMiss();
                rect.Fill = new SolidColorBrush(Colors.White);
                validShot = true;
            }
            else if ((rect.DataContext as Cell).cellType == Cell.CellType.Ship)
            {
                (rect.DataContext as Cell).SetAsHit();
                rect.Fill = new SolidColorBrush(Colors.Red);
                switch((rect.DataContext as Cell).shipClass)
                {
                    case Cell.ShipClass.Carrier:
                        if (CheckForSameShip(Cell.ShipClass.Carrier))
                        {
                            AnnounceHit(Cell.ShipClass.Carrier);
                        }
                        else
                        {
                            AnnounceSink(Cell.ShipClass.Carrier);
                        }
                        break;
                    case Cell.ShipClass.Battleship:
                        if (CheckForSameShip(Cell.ShipClass.Battleship))
                        {
                            AnnounceHit(Cell.ShipClass.Battleship);
                        }
                        else
                        {
                            AnnounceSink(Cell.ShipClass.Battleship);
                        }
                        break;
                    case Cell.ShipClass.Cruiser:
                        if (CheckForSameShip(Cell.ShipClass.Cruiser))
                        {
                            AnnounceHit(Cell.ShipClass.Cruiser);
                        }
                        else
                        {
                            AnnounceSink(Cell.ShipClass.Cruiser);
                        }
                        break;
                    case Cell.ShipClass.Submarine:
                        if (CheckForSameShip(Cell.ShipClass.Submarine))
                        {
                            AnnounceHit(Cell.ShipClass.Submarine);
                        }
                        else
                        {
                            AnnounceSink(Cell.ShipClass.Submarine);
                        }
                        break;
                    case Cell.ShipClass.Destroyer:
                        if (CheckForSameShip(Cell.ShipClass.Destroyer))
                        {
                            AnnounceHit(Cell.ShipClass.Destroyer);
                        }
                        else
                        {
                            AnnounceSink(Cell.ShipClass.Destroyer);
                        }
                        break;
                }
                validShot = true;
                CheckWin();
            }
            if (validShot)
            {
                AIShoots();
            }
        }

        private async void AnnounceSink(Cell.ShipClass ship)
        {
            await new MessageDialog($"Sunk {ship.ToString()}", "Ship Sunk").ShowAsync();
        }

        private async void AnnounceHit(Cell.ShipClass ship)
        {
            await new MessageDialog($"Hit {ship.ToString()}", "Ship Hit").ShowAsync();
        }

        private bool CheckForSameShip(Cell.ShipClass ship)
        {
            bool sameShipsLeft = false;
            for (int r = 0; r < AIGrid.RowDefinitions.Count; r++)
            {
                for (int c = 0; c < AIGrid.ColumnDefinitions.Count; c++)
                {
                    if ((GetAIGridRectangle(r, c).DataContext as Cell).shipClass == ship && (GetAIGridRectangle(r, c).DataContext as Cell).cellType == Cell.CellType.Ship)
                    {
                        sameShipsLeft = true;
                        break;
                    }
                }
            }
            return sameShipsLeft;
        }

        private async void IndicateInvalidMove()
        {
            await new MessageDialog("You have already shot in this location. Please shoot again.", "Invalid shot").ShowAsync();
        }

        private void AIShoots()
        {
            do
            {
                int row = rand.Next(10);
                int col = rand.Next(10);
                rect = GetPlayerGridRectangle(row, col);
                if ((rect.DataContext as Cell).cellType == Cell.CellType.Hit || (rect.DataContext as Cell).cellType == Cell.CellType.Miss)
                {
                    validShot = false;
                }
                else if ((rect.DataContext as Cell).cellType == Cell.CellType.Water)
                {
                    (rect.DataContext as Cell).SetAsMiss();
                    rect.Fill = new SolidColorBrush(Colors.White);
                    validShot = true;
                }
                else if ((rect.DataContext as Cell).cellType == Cell.CellType.Ship)
                {
                    (rect.DataContext as Cell).SetAsHit();
                    rect.Fill = new SolidColorBrush(Colors.Red);
                    validShot = true;
                    CheckWin();
                }
            } while (!validShot);
            validShot = false;
        }

        private Rectangle GetPlayerGridRectangle(int row, int col)
        {
            foreach (FrameworkElement cell in PlayerGrid.Children)
            {
                if (Grid.GetRow(cell) == row && Grid.GetColumn(cell) == col)
                {
                    return (Rectangle)cell;
                }
            }
            throw new ArgumentOutOfRangeException("Row and column must be within PlayerGrid");
        }

        private FrameworkElement GetAIGridRectangle(int row, int col)
        {
            foreach (FrameworkElement cell in AIGrid.Children)
            {
                if (Grid.GetRow(cell) == row && Grid.GetColumn(cell) == col)
                {
                    return cell;
                }
            }
            throw new ArgumentOutOfRangeException("Row and column must be within AIGrid");
        }

        private Rectangle GetAnyGridRectangle(int row, int col, Grid chosenGrid)
        {
            foreach (FrameworkElement cell in chosenGrid.Children)
            {
                if (Grid.GetRow(cell) == row && Grid.GetColumn(cell) == col)
                {
                    return (Rectangle)cell;
                }
            }
            throw new ArgumentOutOfRangeException("Row and column must be within chosenGrid");
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            await new MessageDialog($"This feature is not implemented", "Not implemented").ShowAsync();
            //FileSavePicker fsp = new FileSavePicker();
            //DataContractSerializer ser = new DataContractSerializer(typeof(Grid));
            //fsp.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            //fsp.FileTypeChoices.Add("Battleship", new List<string>() { ".bshp" });
            //fsp.SuggestedFileName = "BattleShipGame";

            //var op = await fsp.PickSaveFileAsync();
            //if (op != null)
            //{
            //    var ms = new MemoryStream();
            //    ser.WriteObject(ms, PlayerGrid);
            //    ser.WriteObject(ms, AIGrid);
            //    var data = ms.ToArray();

            //    CachedFileManager.DeferUpdates(op);
            //    await FileIO.WriteBytesAsync(op, data);
            //    Windows.Storage.Provider.FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(op);
            //    ms.Close();
            //}
        }

        private void CheckWin()
        {
            if (NoShips(PlayerGrid))
            {
                DeclareWinner(AIGrid);
            }
            else if (NoShips(AIGrid))
            {
                DeclareWinner(PlayerGrid);
            }
        }

        private bool NoShips(Grid chosenGrid)
        {
            bool noShipsLeft = true;
            for (int r = 0; r < chosenGrid.RowDefinitions.Count; r++)
            {
                for (int c = 0; c < chosenGrid.ColumnDefinitions.Count; c++)
                {
                    if ((GetAnyGridRectangle(r, c, chosenGrid).DataContext as Cell).cellType == Cell.CellType.Ship)
                    {
                        noShipsLeft = false;
                    }
                }
            }
            return noShipsLeft;
        }

        private async void DeclareWinner(Grid winnerGrid)
        {
            string winner = null;
            if (winnerGrid == PlayerGrid)
            {
                winner = "Player 1";
            }
            else if (winnerGrid == AIGrid)
            {
                winner = "AI";
                SetCheatMode(true);
            }
            Window.Current.CoreWindow.KeyDown -= CoreWindow_SetCheatMode;
            await new MessageDialog($"The game is over. {winner} wins!", "Game Finished").ShowAsync();
            this.Frame.Navigate(typeof(MainPage));
        }

        void CoreWindow_SetCheatMode(CoreWindow sender, KeyEventArgs args)
        {
            if (args.VirtualKey == VirtualKey.W)
            {

                if (Window.Current.CoreWindow.GetKeyState(VirtualKey.Q).HasFlag(CoreVirtualKeyStates.Down))
                {
                    cheatModeOn = !cheatModeOn;
                    SetCheatMode(cheatModeOn);
                }
            }
        }

        private void SetCheatMode(bool cheatModeOn)
        {
            for (int r = 0; r < AIGrid.RowDefinitions.Count; r++)
            {
                for (int c = 0; c < AIGrid.ColumnDefinitions.Count; c++)
                {
                    rect = (Rectangle)GetAIGridRectangle(r, c);
                    if ((rect.DataContext as Cell).cellType == Cell.CellType.Ship)
                    {
                        if (cheatModeOn)
                        {
                            rect.SetValue(Rectangle.FillProperty, new SolidColorBrush(Colors.Gray));
                        }
                        else
                        {
                            rect.SetValue(Rectangle.FillProperty, new SolidColorBrush(Colors.SkyBlue));
                        }
                    }
                }
            }
        } // q + w
    }
}