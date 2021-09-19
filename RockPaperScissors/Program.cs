using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTables;
using System.Security.Cryptography;

namespace RockPaperScissors
{
    class Program
    {
        static string[] SettingsStrings;
        static int GameTurn;
        static byte[] key;
        const string NotCorrectStringsMessage = "Your version is not correct, because it does not correspond to one of the rules:\n"+
            "1) There must be an odd number of rows;\n"+
            "2) The strings must be unique;\n"+
            "For example: rock scissors paper 2 3gtrr";
        const string TryAgainMessage = "Do you want to try again? ";
        const string InfoMessage = "This is a cinde of \"Rock Paper Scissors\" game.\n"
            + "After you enter the game settings, you will be shown a table with possible actions.\n"
            + "By entering the corresponding character, you will perform a certain action.\n"
            + "Also, the computer immediately makes its move, encrypts it and shows it to the player.\n"
            + "After the player's turn, it will be displayed whether he won or not, as well as the key for decryption.\n"
            + "The game uses hmac based on SHA-256.\n"
            + "The victory is determined as follows: half of the next ones in the circle wins, half of the previous ones in the circle loses.";
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, player! This is a strange \"Rock Paper Scissors\" game. ");
            if (SettingsCheck(args))
            {
                SettingsStrings = args;
                ShowTurns();
                GameRunning();
            }
            else
            {
                Console.WriteLine(NotCorrectStringsMessage);
                if (IsWantCheck(TryAgainMessage))
                {
                    if (GetNewSettings())
                    {
                        ShowTurns();
                        GameRunning();
                    }
                }
            }

            Console.WriteLine("Thank you for playing!\nPress any key to exit.");
            Console.ReadKey();
        }
        static void GameRunning()
        {
            GameTurn = HmacClass.GetRandomNum(1, SettingsStrings.Length);
            key = HmacClass.GetKey();
            Console.WriteLine("\nMy turn Hmac is  "+HmacClass.GetHmac(key, SettingsStrings[GameTurn-1]));
            Console.WriteLine("Your turn, Mr. Player.");
            string playerTurn = Console.ReadLine();
            switch (playerTurn.ToLower())
            {
                case ("r"):
                    Console.WriteLine(InfoMessage);
                    GameRunning();
                    break;
                case ("c"):
                    GetNewSettings();
                    GameRunning();
                    break;
                case ("?"):
                    ConsoleTable table = TableGeneratorClass.TableGenerate(SettingsStrings.Length);
                    table.Write(Format.Alternative);
                    GameRunning();
                    break;
                case ("e"):
                    break;
                default:
                    if(TurnCheck(playerTurn))
                    {
                        int turn = Convert.ToInt32(playerTurn);
                        Console.WriteLine("you chouse " + SettingsStrings[Convert.ToInt32(playerTurn) - 1]);
                        Console.WriteLine("It is "+WinnerDeterminingClass.WinnerDetermining(turn, GameTurn, SettingsStrings.Length)+"!");
                        Console.WriteLine("Game turn was - " + SettingsStrings[GameTurn - 1]+ "  Key is: ");
                        BiteShow(key);
                    }
                    GameRunning();
                    break;
            }

        }
        static bool TurnCheck(string str)
        {
            try
            {
                int turn = Convert.ToInt32(str);
                if (turn >= 1 && turn <= SettingsStrings.Length) return true;
                return false;
            }
            catch
            {
                Console.WriteLine("This isn`t avilible turn!");
                ShowTurns();
                return false;
            }
        }
        static void ShowTurns()
        {
            Console.WriteLine();
            for (int i = 0; i< SettingsStrings.Length; i++)
            {
                Console.WriteLine(i+1 + " - "+ SettingsStrings[i]);
            }
            Console.WriteLine("r - rules \nc - change settings \n? - table of combinations\nE - exit");
        }
        static bool GetNewSettings ()
        {
            while(true)
            {
                Console.WriteLine("Please enter new names separated by a spaces:");
                string enterstring = Console.ReadLine();
                if (enterstring != "")
                {
                    string[] str = (from i in enterstring.Split(new char[] { ' ' }) where i != "" select i).ToArray();
                    if (SettingsCheck(str))
                    {
                        SettingsStrings = str;
                        return true;
                    }
                    else
                    {
                        Console.WriteLine(NotCorrectStringsMessage);
                        if (!IsWantCheck(TryAgainMessage)) return false;
                    }
                }
                else
                {
                    Console.WriteLine("You entered an empty row!");
                    if (!IsWantCheck(TryAgainMessage)) return false;

                }
            }
        }
        static bool IsWantCheck(string question)
        {
            Console.WriteLine(question + "  yes/no: ");
            string ans = Console.ReadLine();
            if (ans.ToLower() == "yes") return true;
            else if (ans.ToLower() == "no") return false;
            else
            {
                Console.WriteLine("Sorry, i don`t understand this answer, try again.");
                return IsWantCheck(question);
            }
        }
        static bool SettingsCheck (string[] set)
        {
            if (set.Length <3 || set.Length % 2 == 0) return false;
            
            try
            {
                var MaxNonUnique = set.GroupBy(x => x).Select(g => g.Count()).Max();
                if (MaxNonUnique < 2) return true;
                return false;
            }
            catch 
            {
                try
                {
                    bool resoult = HardSettingsCheck(set);
                    return resoult;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
        }
        static bool HardSettingsCheck(string[] set)
        {
            for (int i = 0; i< set.Length-1; i++)
            {
                for (int j = i + 1; j < set.Length; j++)
                    if (set[i] == set[j]) return false;
            }
            return true;
        }
        static void BiteShow (byte[] b)
        {
            Console.WriteLine(BitConverter.ToString(b).Replace("-", "").ToLowerInvariant());
        }
        
    }

}
