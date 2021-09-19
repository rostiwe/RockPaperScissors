using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTables;

namespace RockPaperScissors
{
    static class TableGeneratorClass
    {
        public static ConsoleTable TableGenerate(int length)
        {

            string[] TableString = new string[length + 1];
            TableString[0] = "";
            for (int i = 1; i <= length; i++)
            {
                TableString[i] = i.ToString();
            }
            ConsoleTable returnedTable = new ConsoleTable(TableString);
            for (int i = 1; i <= length; i++)
            {
                TableString = new string[length + 1];
                for (int j = 0; j <= length; j++)
                {
                    if (j == 0) TableString[0] = i.ToString();
                    else
                    {
                        TableString[j] = WinnerDeterminingClass.WinnerDetermining(i, j, length);
                    }
                }
                returnedTable.AddRow(TableString);
            }
            return returnedTable;
        }
    }
}