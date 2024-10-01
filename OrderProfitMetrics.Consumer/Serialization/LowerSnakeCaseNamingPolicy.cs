using System;
using System.Text.Json;

namespace GettingStarted.Serialization;

public class LowerSnakeCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        Span<char> lowerSnakeCaseName = stackalloc char[name.Length * 2];
        var lowerSnakeCaseIndex = 0;
        for (var i = 0; i < name.Length; i++)
        {
            if (char.IsUpper(name[i]) && i > 0)
            {
                lowerSnakeCaseName[lowerSnakeCaseIndex] = '_';
                lowerSnakeCaseIndex++;
            }

            lowerSnakeCaseName[lowerSnakeCaseIndex] = char.ToLower(name[i]);
            lowerSnakeCaseIndex++;
        }
        return lowerSnakeCaseName[..lowerSnakeCaseIndex].ToString();
    }
}
