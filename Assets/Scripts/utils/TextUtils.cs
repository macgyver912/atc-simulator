using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class TextUtils
{
    public static string Text2SpellFormat(string str_in)
    {
        string str_out = string.Empty;
        string phonetic = string.Empty;
        
        char[] array = str_in.ToUpper().ToCharArray();  // converto to upper for compare below
        foreach (char c in array)
        {
            switch (c)
            {
                case 'A':
                    phonetic = "Alpha";
                    break;
                case 'B':
                    phonetic = "Bravo";
                    break;
                case 'C':
                    phonetic = "Charlie";
                    break;
                case 'D':
                    phonetic = "Delta";
                    break;
                case 'E':
                    phonetic = "Echo";
                    break;
                case 'F':
                    phonetic = "Foxtrot";
                    break;
                case 'G':
                    phonetic = "Golf";
                    break;
                case 'H':
                    phonetic = "Hotel";
                    break;
                case 'I':
                    phonetic = "India";
                    break;
                case 'J':
                    phonetic = "Juliet";
                    break;
                case 'K':
                    phonetic = "Kilo";
                    break;
                case 'L':
                    phonetic = "Lima";
                    break;
                case 'M':
                    phonetic = "Mike";
                    break;
                case 'N':
                    phonetic = "November";
                    break;
                case 'O':
                    phonetic = "Oscar";
                    break;
                case 'P':
                    phonetic = "Papa";
                    break;
                case 'Q':
                    phonetic = "Quebec";
                    break;
                case 'R':
                    phonetic = "Romeo";
                    break;
                case 'S':
                    phonetic = "Sierra";
                    break;
                case 'T':
                    phonetic = "Tango";
                    break;
                case 'U':
                    phonetic = "Uniform";
                    break;
                case 'V':
                    phonetic = "Victor";
                    break;
                case 'W':
                    phonetic = "Whiskey";
                    break;
                case 'X':
                    phonetic = "X-Ray";
                    break;
                case 'Y':
                    phonetic = "Yankee";
                    break;
                case 'Z':
                    phonetic = "Zulu";
                    break;
                default:
                    phonetic = string.Empty;
                    break;
            }
            str_out += (phonetic != string.Empty ? phonetic : c) + " ";
        }
        return str_out.TrimEnd();
    }
}
