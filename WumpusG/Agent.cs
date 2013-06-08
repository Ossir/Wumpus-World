using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WumpusG
{
    public class Agent
    {
        public struct cellA
        {
            public int isPit;
            public int isGold;
            public int isWumpus;
            public bool isSafe;
            public bool isFog;
        }
        public int wumpusCount = 0;
        public bool isDead;
        public int PositionRow;
        public int PositionCol;
        public cellA[,] field = new cellA[4, 4];

        public Agent()
        {
            PositionRow = 3;
            PositionCol = 0;
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    field[row, col].isFog = true;
                    field[row, col].isPit = 0;
                    field[row, col].isGold = 0;
                    field[row, col].isWumpus = 0;
                }
            }
            field[3, 0].isSafe = true;
        }

        public int Analys(WumpusG.MainWindow.cell [,] map, int AposRow, int AposCol)
        {
            int stateUp, stateDown, stateRight, stateLeft = 0;
            int direction = 0;
            PositionRow = AposRow;
            PositionCol = AposCol;
            stateUp = checkMove(map, PositionRow - 1, PositionCol);
            stateDown = checkMove(map, PositionRow + 1, PositionCol);
            stateLeft = checkMove(map, PositionRow, PositionCol - 1);
            stateRight = checkMove(map, PositionRow, PositionCol + 1);
            if (stateUp == 3)
                return direction = 1;
            else if (stateDown == 3)
                return direction = 2;
            else if (stateLeft == 3)
                return direction = 3;
            else if (stateRight == 3)
                return direction = 4;
            if (stateUp == 1 && map[PositionRow - 1, PositionCol].isFog)
                return direction = 1;
            else if (stateDown == 1 && map[PositionRow + 1, PositionCol].isFog)
                return direction = 2;
            else if (stateLeft == 1 && map[PositionRow, PositionCol - 1].isFog)
                return direction = 3;
            else if (stateRight == 1 && map[PositionRow, PositionCol + 1].isFog)
                return direction = 4;
            if (stateUp == 1 && !map[PositionRow - 1, PositionCol].isFog && map[PositionRow - 1, PositionCol].isSafe)
                direction = 1;
            else if (stateDown == 1 && !map[PositionRow + 1, PositionCol].isFog && map[PositionRow + 1, PositionCol].isSafe)
                direction = 2;
            else if (stateLeft == 1 && !map[PositionRow, PositionCol - 1].isFog && map[PositionRow, PositionCol - 1].isSafe)
                direction = 3;
            else if (stateRight == 1 && !map[PositionRow, PositionCol + 1].isFog && map[PositionRow, PositionCol + 1].isSafe)
                direction = 4;
            return direction;
        }

        public int checkMove(WumpusG.MainWindow.cell[,] mapL, int newRow, int newCol)
        {
            if (newRow < 0 || newRow > 3 || newCol < 0 || newCol > 3)
            {
                return 0;
            }
            else if (mapL[newRow, newCol].isPit)
            {
                return 2;
            }
            else if (mapL[newRow, newCol].isGold)
            {
                return 3;
            }
            else if (mapL[newRow, newCol].isWumpus)
            {
                return 4;
            }
            if ((newCol > 0 && mapL[newRow, newCol - 1].isPit) ||
                (newCol < 3 && mapL[newRow, newCol + 1].isPit) ||
                (newRow > 0 && mapL[newRow - 1, newCol].isPit) ||
                (newRow < 3 && mapL[newRow + 1, newCol].isPit))
            {
                if (newCol > 0 && !field[newRow, newCol - 1].isSafe)
                    field[newRow, newCol - 1].isPit = 1;
                if (newCol < 3 && !field[newRow, newCol + 1].isSafe)
                    field[newRow, newCol + 1].isPit = 1;
                if (newRow > 0 && !field[newRow - 1, newCol].isSafe)
                    field[newRow - 1, newCol].isPit = 1;
                if (newRow < 3 && !field[newRow + 1, newCol].isSafe)
                    field[newRow + 1, newCol].isPit = 1;
            }
            if ((newCol > 0 && mapL[newRow, newCol - 1].isGold) ||
                (newCol < 3 && mapL[newRow, newCol + 1].isGold) ||
                (newRow > 0 && mapL[newRow - 1, newCol].isGold) ||
                (newRow < 3 && mapL[newRow + 1, newCol].isGold))
            {
                if (newCol > 0 && !field[newRow, newCol - 1].isSafe)
                    field[newRow, newCol - 1].isGold = 1;
                if (newCol < 3 && !field[newRow, newCol + 1].isSafe)
                    field[newRow, newCol + 1].isGold = 1;
                if (newRow > 0 && !field[newRow - 1, newCol].isSafe)
                    field[newRow - 1, newCol].isGold = 1;
                if (newRow < 3 && !field[newRow + 1, newCol].isSafe)
                    field[newRow + 1, newCol].isGold = 1;
            }
            if ((newCol > 0 && mapL[newRow, newCol - 1].isWumpus) ||
                (newCol < 3 && mapL[newRow, newCol + 1].isWumpus) ||
                (newRow > 0 && mapL[newRow - 1, newCol].isWumpus) ||
                (newRow < 3 && mapL[newRow + 1, newCol].isWumpus))
            {
                if (newCol > 0 && !field[newRow, newCol - 1].isSafe)
                    field[newRow, newCol - 1].isWumpus++;
                if (newCol < 3 && !field[newRow, newCol + 1].isSafe)
                    field[newRow, newCol + 1].isWumpus++;
                if (newRow > 0 && !field[newRow - 1, newCol].isSafe)
                    field[newRow - 1, newCol].isWumpus++;
                if (newRow < 3 && !field[newRow + 1, newCol].isSafe)
                    field[newRow + 1, newCol].isWumpus++;
            }
            if ((newCol > 0 && field[newRow, newCol - 1].isWumpus == 0 && field[newRow, newCol - 1].isPit == 0))
            {
                field[newRow, newCol - 1].isSafe = true;
            }
            if ((newCol < 3 && field[newRow, newCol + 1].isWumpus == 0 && field[newRow, newCol + 1].isPit == 0))
            {
                field[newRow, newCol + 1].isSafe = true;
            }
            if ((newRow > 0 && field[newRow - 1, newCol].isWumpus == 0 && field[newRow - 1, newCol].isPit == 0))
            {
                field[newRow - 1, newCol].isSafe = true;
            }
            if ((newRow < 3 && field[newRow + 1, newCol].isWumpus == 0 && field[newRow + 1, newCol].isPit == 0))
            {
                field[newRow + 1, newCol].isSafe = true;
            }
            return 1;
        }
    }
}
