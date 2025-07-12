using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using StardewModdingAPI;
// ReSharper disable PossibleMultipleEnumeration
// ReSharper disable RedundantArgumentDefaultValue

namespace BETAS.Helpers;

public static class Log
{
    public static void Debug<T>(T message) => BETAS.ModMonitor.Log(
        $"{(message is not string ? "[" + message?.GetType() + "] " : string.Empty)}{message?.ToString() ?? string.Empty}",
        LogLevel.Debug);

    public static void Error<T>(T message) => BETAS.ModMonitor.Log(
        $"{(message is not string ? "[" + message?.GetType() + "] " : string.Empty)}{message?.ToString() ?? string.Empty}",
        LogLevel.Error);

    public static void Warn<T>(T message) => BETAS.ModMonitor.Log(
        $"{(message is not string ? "[" + message?.GetType() + "] " : string.Empty)}{message?.ToString() ?? string.Empty}",
        LogLevel.Warn);

    public static void Info<T>(T message) => BETAS.ModMonitor.Log(
        $"{(message is not string ? "[" + message?.GetType() + "] " : string.Empty)}{message?.ToString() ?? string.Empty}",
        LogLevel.Info);

    public static void Trace<T>(T message) => BETAS.ModMonitor.Log(
        $"{(message is not string ? "[" + message?.GetType() + "] " : string.Empty)}{message?.ToString() ?? string.Empty}",
        LogLevel.Trace);
    
    public static void Alert<T>(T message) => BETAS.ModMonitor.Log(
        $"{(message is not string ? "[" + message?.GetType() + "] " : string.Empty)}{message?.ToString() ?? string.Empty}",
        LogLevel.Alert);

    public static void ILCode(IEnumerable<CodeInstruction> code)
    {
        foreach (var instruction in code)
        {
            Debug($"{instruction.opcode} {instruction.operand}");
        }
    }

    public static void ILCode(IEnumerable<CodeInstruction> newCode, IEnumerable<CodeInstruction> originalCode)
    {
        var originalEnumerator = 0;
        foreach (var instruction in newCode)
        {
            if (originalEnumerator >= originalCode.Count())
            {
                Warn($"{instruction.opcode} {instruction.operand}");
                continue;
            }

            if (instruction.opcode != originalCode.ElementAt(originalEnumerator).opcode ||
                instruction.operand != originalCode.ElementAt(originalEnumerator).operand)
            {
                Warn($"{instruction.opcode} {instruction.operand}");
                continue;
            }

            Debug($"{instruction.opcode} {instruction.operand}");
            originalEnumerator++;
        }
    }

    public static void ILCode(CodeInstruction code)
    {
        Debug($"{code.opcode} {code.operand}");
    }

    public static void LogPairs(this IEnumerable? enumerable, int depth = 0)
    {
        if (enumerable is null)
        {
            Warn("Enumerable is null");
            return;
        }

        foreach (var item in enumerable)
        {
            if (item is KeyValuePair<string, object> kvp)
            {
                if (kvp.Value is IEnumerable innerEnumerable and not string)
                {
                    Debug($"{new string(' ', depth * 2)}{kvp.Key}:");
                    innerEnumerable.LogPairs(depth + 1);
                }
                else Debug($"{new string(' ', depth * 2)}{kvp.Key}: {kvp.Value}");
            }
            else Debug($"{new string(' ', depth * 2)}{item}");
        }
    }
}