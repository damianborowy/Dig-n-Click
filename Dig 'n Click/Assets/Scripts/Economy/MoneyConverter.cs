using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MoneyConverter
{

    public const int SCIENTIFIC = 0;
    public const int NORMAL = 1;
    public const int SIMPLE = 2;

    private static int _type = SCIENTIFIC;

    private static Dictionary<int, string> normalDisctionary;
    private static Dictionary<int, string> simpleDictionary;

    private static void Start()
    {
        normalDisctionary = new Dictionary<int, string>();
        normalDisctionary.Add(6, "Mil");
        normalDisctionary.Add(9, "Bil");
        normalDisctionary.Add(12, "Tril");
        normalDisctionary.Add(15, "Qa");
        normalDisctionary.Add(18, "Qi");
        normalDisctionary.Add(21, "Sx");
        normalDisctionary.Add(24, "Sp");
        normalDisctionary.Add(27, "Oct");
        normalDisctionary.Add(30, "No");
        normalDisctionary.Add(33, "Dc");
        normalDisctionary.Add(36, "Udc");
        normalDisctionary.Add(39, "Ddc");

        simpleDictionary = new Dictionary<int, string>();
        simpleDictionary.Add(6, "A");
        simpleDictionary.Add(9, "B");
        simpleDictionary.Add(12, "C");
        simpleDictionary.Add(15, "D");
        simpleDictionary.Add(18, "E");
        simpleDictionary.Add(21, "F");
        simpleDictionary.Add(24, "G");
        simpleDictionary.Add(27, "H");
        simpleDictionary.Add(30, "I");
        simpleDictionary.Add(33, "J");
        simpleDictionary.Add(36, "K");
        simpleDictionary.Add(39, "L");
    }

    public static string ConvertNumber(double value)
    {
        if (normalDisctionary == null || simpleDictionary == null)
            Start();

        if (value >= 1000000)
        {
            int exponent = (int)Math.Floor(Math.Log10(value));
            double mantissa = Math.Round(value / Math.Pow(10, exponent), 3);

            if (_type == SCIENTIFIC)
            {
                return ScientificNotation(mantissa, exponent);
            }
            else if (_type == NORMAL || _type == SIMPLE)
            {
                return NormalNotation(mantissa, exponent);
            }
            else
            {
                throw new Exception("Proper MoneyConverter type isn't set");
            }
        }
        else
        {
            var stringBuilder = new StringBuilder();
            string temp = value.ToString();
            var stack = new Stack<char>();

            for(int i = 0; i < temp.Length; i++)
            {
                stack.Push(temp[i]);
            }

            int iterator = 1;

            while(stack.Count > 0)
            {
                if(iterator % 4 != 0)
                {
                    stringBuilder.Insert(0, stack.Pop());
                }
                else
                {
                    stringBuilder.Insert(0, " ");
                }
                iterator++;
            }

            return stringBuilder.ToString();
        }
    }

    private static string ScientificNotation(double mantissa, int exponent)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.Append(mantissa);
        stringBuilder.Append("e");
        stringBuilder.Append(exponent);

        return stringBuilder.ToString();
    }

    private static string NormalNotation(double mantissa, int exponent)
    {
        var stringBuilder = new StringBuilder();

        double newMantissa = mantissa * Math.Pow(10, exponent % 3);
        exponent -= exponent % 3;

        stringBuilder.Append(newMantissa + " ");

        if (_type == NORMAL)
            stringBuilder.Append(normalDisctionary[exponent]);
        else if (_type == SIMPLE)
            stringBuilder.Append(simpleDictionary[exponent]);

        return stringBuilder.ToString();
    }
}
