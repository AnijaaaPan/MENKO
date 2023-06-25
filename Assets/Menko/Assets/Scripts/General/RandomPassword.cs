using System;
using System.Linq;

public static class RandomPassword
{

    private const string ASCII_NUMBER = "0123456789";
    private const string ASCII_UPPER_ALPHA = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string ASCII_MARK = "\\!#$%&()?@[]";

    private const string ALL_CONTENT = ASCII_NUMBER + ASCII_UPPER_ALPHA + ASCII_MARK;

    private static readonly Random rng = new();

    public static string Generate(int length)
    {
        string password = Choice(ASCII_NUMBER) + Choice(ASCII_UPPER_ALPHA) + Choice(ASCII_MARK);

        int cnt = length - password.Length;
        for (int i = 0; i < cnt; i++)
        {
            password += Choice(ALL_CONTENT);
        }

        return string.Join("", password.OrderBy(n => rng.Next()));
    }

    private static string Choice(string source)
    {
        return source[rng.Next(0, source.Length - 1)].ToString();
    }
}