using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MoneyConverter
{
    private enum Type {Scientific, Normal, Simple}

    private static Type _type = Type.Normal;
    private static Dictionary<int, string> _normalDisctionary;
    private static Dictionary<int, string> _simpleDictionary;

    private static void Start()
    {
        _normalDisctionary = new Dictionary<int, string>
        {
            {6, "Mil"},
            {9, "Bil"},
            {12, "Tril"},
            {15, "Qa"},
            {18, "Qi"},
            {21, "Sx"},
            {24, "Sp"},
            {27, "Oct"},
            {30, "No"},
            {33, "Dc"},
            {36, "Udc"},
            {39, "Ddc"}
        };

        _simpleDictionary = new Dictionary<int, string>
        {
            {6, "A"},
            {9, "B"},
            {12, "C"},
            {15, "D"},
            {18, "E"},
            {21, "F"},
            {24, "G"},
            {27, "H"},
            {30, "I"},
            {33, "J"},
            {36, "K"},
            {39, "L"}
        };
    }

    public static string ConvertNumber(double value)
    {
        if (_normalDisctionary == null || _simpleDictionary == null)
            Start();

        if (value >= 1000000)
        {
            int exponent = (int)Math.Floor(Math.Log10(value));
            double mantissa = Math.Round(value / Math.Pow(10, exponent), 3);

            if (_type.Equals(Type.Scientific))
            {
                return ScientificNotation(mantissa, exponent);
            }

            if (_type.Equals(Type.Normal) || _type.Equals(Type.Simple))
            {
                return NormalNotation(mantissa, exponent);
            }

            throw new Exception("Proper MoneyConverter type isn't set");
        }

        var stringBuilder = new StringBuilder();
        string temp = value.ToString();
        var stack = new Stack<char>();

        foreach (var element in temp)
        {
            stack.Push(element);
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

        if (_type.Equals(Type.Normal))
        {
            stringBuilder.Append(_normalDisctionary[exponent]);
        }
        else if (_type.Equals(Type.Simple))
        {
            stringBuilder.Append(_simpleDictionary[exponent]);
        }

        return stringBuilder.ToString();
    }
}
