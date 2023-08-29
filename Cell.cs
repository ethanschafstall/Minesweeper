/*
 * t-P_Prog-Schafstall-Ethan-Demineur
 * Ethan Schafstall
 * 09.03.2023
 * CIN1B
 * ETML
 */

namespace t_P_Prog_Schafstall_Ethan_Demineur
{
    /// <summary>
    /// Cell class responsible for making cell objects that represent each indiviual cell that makes up the gameboard.
    /// This class manages all interactions, either by a user, or methods using game creation.
    /// Each cell can be in 1 of 9 state values dependant on the game generation rng/user interaction which said cell.
    /// Each cell has a fixed (visual) location.
    /// Each cell can hold the number of bordering mine value cells next to it. (Calculated based on method from Program class)
    /// Each cell is capable of changing it's value based on user interaction, and displaying it's value based using different chars, colors.
    /// </summary>
    internal class Cell
    {
        enum CellStates
        {
            coveredEmpty = 0,
            coveredNumber = 1,
            coveredMine = 2,
            flaggedEmpty = 3,
            flaggedNumber = 4,
            flaggedMine = 5,
            exposedEmpty = 6,
            exposedNumber = 7,
            exposedMine = 8,
        }
        private int cellState;
        private int xPosition;
        private int yPosition;
        private int borderingMines;
        public int CellState
        {
            get { return cellState; }
            set { cellState = value; }
        }
        public int XPosition
        {
            get { return xPosition; }
            set { xPosition = value; }
        }
        public int YPosition
        {
            get { return yPosition; }
            set { yPosition = value; }
        }
        public int BorderingMines
        {
            get { return borderingMines; }
            set { borderingMines = value; }
        }
        public Cell() { }
        public int SetNumber
        {
            get { return borderingMines; }
            set { borderingMines = value; }
        }

        /// <summary>
        /// Method that changes the cell's state value based on it's current value. Responsible for "flagging"/changing the cell's value.
        /// </summary>
        public void FlagCell(bool canFlag)
        {
            if (!canFlag)
            {
                switch (cellState)
                {
                    case (int)CellStates.flaggedEmpty:
                        cellState = (int)CellStates.coveredEmpty;
                        break;
                    case (int)CellStates.flaggedMine:
                        cellState = (int)CellStates.coveredMine;
                        break;
                    case (int)CellStates.flaggedNumber:
                        cellState = (int)CellStates.coveredNumber;
                        break;
                }
            }
            if (canFlag)
            {
                if (cellState < 3)
                {
                    switch (cellState)
                    {
                        case (int)CellStates.coveredEmpty:
                            cellState = (int)CellStates.flaggedEmpty;
                            break;
                        case (int)CellStates.coveredMine:
                            cellState = (int)CellStates.flaggedMine;
                            break;
                        case (int)CellStates.coveredNumber:
                            cellState = (int)CellStates.flaggedNumber;
                            break;
                    }
                }
                else
                {
                    switch (cellState)
                    {
                        case (int)CellStates.flaggedEmpty:
                            cellState = (int)CellStates.coveredEmpty;
                            break;
                        case (int)CellStates.flaggedMine:
                            cellState = (int)CellStates.coveredMine;
                            break;
                        case (int)CellStates.flaggedNumber:
                            cellState = (int)CellStates.coveredNumber;
                            break;
                    }
                }

            }
        }

        /// <summary>
        /// Method that changes the cell's state value based on it's current value. Responsible for "exposing"/seeing the cell's value.
        /// </summary>
        public void CheckCell()
        {
            switch (cellState)
            {
                case (int)CellStates.coveredEmpty:
                    cellState = (int)CellStates.exposedEmpty;
                    break;
                case (int)CellStates.coveredMine:
                    cellState = (int)CellStates.exposedMine;
                    break;
                case (int)CellStates.coveredNumber:
                    cellState = (int)CellStates.exposedNumber;
                    break;
            }
        }

        /// <summary>
        /// Method that writes the cell's current state value in the console.
        /// </summary>
        public void PrintState()
        {
            static void Flag()
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("|");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(">");
                Console.ResetColor();
            }

            Console.SetCursorPosition(yPosition, xPosition);
            Console.Write("  ");
            Console.SetCursorPosition(yPosition, xPosition);
            switch (cellState)
            {
                case (int)CellStates.coveredEmpty:
                    Console.Write('▒');
                    break;
                case (int)CellStates.coveredMine:
                    Console.Write('▒');
                    break;
                case (int)CellStates.coveredNumber:
                    Console.Write('▒');
                    break;
                case (int)CellStates.flaggedEmpty:
                    Flag();
                    break;
                case (int)CellStates.flaggedMine:
                    Flag();
                    break;
                case (int)CellStates.flaggedNumber:
                    Flag();
                    break;
                case (int)CellStates.exposedEmpty:
                    Console.Write("  ");
                    break;
                case (int)CellStates.exposedMine:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("*");
                    Console.ResetColor();
                    break;
                // If the cell's state 
                case (int)CellStates.exposedNumber:
                    switch (borderingMines)
                    {
                        case 1:
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write("1");
                            Console.ResetColor();
                            break;
                        case 2:
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("2");
                            Console.ResetColor();
                            break;
                        case 3:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("3");
                            Console.ResetColor();
                            break;
                        case 4:
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.Write("4");
                            Console.ResetColor();
                            break;
                        case 5:
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.Write("5");
                            Console.ResetColor();
                            break;
                        case 6:
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("6");
                            Console.ResetColor();
                            break;
                        case 7:
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write("7");
                            Console.ResetColor();
                            break;
                        case 8:
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.Write("8");
                            Console.ResetColor();
                            break;
                    }
                    break;
            }
        }
    }
}
