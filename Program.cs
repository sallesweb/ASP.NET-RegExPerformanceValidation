// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.Text.RegularExpressions;

new RegexTest();

public class RegexTest
{
    public Regex _rgx;
    private const string DEFAULT_REGEX = @"\\t|\\n|\\r";
    private const string DEFAULT_TEXT = @"Lorem Ipsum is simply dummy text of the printing and typesetting industry.
                                  Lorem Ipsum has been the industry's standard dummy text ever since the 1500s,
                                  when an unknown printer took a galley of type and scrambled it to make a type
                                  specimen book. It has survived not only five centuries, but also the leap into
                                  electronic typesetting, remaining essentially unchanged. It was popularised in
                                  the 1960s with the release of Letraset sheets containing Lorem Ipsum passages,
                                  and more recently with desktop publishing software like Aldus PageMaker
                                  including versions of Lorem Ipsum.";

    public RegexTest()
    {
        _rgx = new(DEFAULT_REGEX);
        Main();
    }

    private void Main()
    {
        LogHeader("1");
        ExecRegexInLoop(1, ERegexExecutionType.MultipleReferences);
        ExecRegexInLoop(1, ERegexExecutionType.Static);
        ExecRegexInLoop(1, ERegexExecutionType.OnlyOneReference);
        LogFooter();

        LogHeader("1K");
        ExecRegexInLoop(1000, ERegexExecutionType.MultipleReferences);
        ExecRegexInLoop(1000, ERegexExecutionType.Static);
        ExecRegexInLoop(1000, ERegexExecutionType.OnlyOneReference);
        LogFooter();

        LogHeader("1M");
        ExecRegexInLoop(1000000, ERegexExecutionType.MultipleReferences);
        ExecRegexInLoop(1000000, ERegexExecutionType.Static);
        ExecRegexInLoop(1000000, ERegexExecutionType.OnlyOneReference);
        LogFooter();

        LogHeader("10M");
        ExecRegexInLoop(10000000, ERegexExecutionType.MultipleReferences);
        ExecRegexInLoop(10000000, ERegexExecutionType.Static);
        ExecRegexInLoop(10000000, ERegexExecutionType.OnlyOneReference);
        LogFooter();

        LogHeader("100M");
        ExecRegexInLoop(100000000, ERegexExecutionType.Static);
        ExecRegexInLoop(100000000, ERegexExecutionType.OnlyOneReference);
        LogFooter();

        LogHeader("1B");
        ExecRegexInLoop(1000000000, ERegexExecutionType.Static);
        ExecRegexInLoop(1000000000, ERegexExecutionType.OnlyOneReference);
        LogFooter();
    }

    private void ExecRegexInLoop(int loop, ERegexExecutionType type)
    {
        var sw = new Stopwatch();

        sw.Reset();
        sw.Start();

        for (int i = 0; i < loop; i++)
        {
            switch (type)
            {
                case ERegexExecutionType.MultipleReferences:
                    ExecMultipleReferences();
                    break;
                case ERegexExecutionType.Static:
                    ExecStatic();
                    break;
                case ERegexExecutionType.OnlyOneReference:
                    ExecOnlyOneReference();
                    break;
                default:
                    break;
            }
        }

        sw.Stop();
        LogResults(type, sw.Elapsed);
    }

    private void LogHeader(string loop)
    {
        var maxLength = 20;
        var loopLength = loop.Length;
        var length = Math.Abs((maxLength - loopLength) / 2);

        Console.WriteLine($"{new string('-', length)}{loop}{new string('-', length)}");
    }

    private void LogFooter()
    {
        Console.WriteLine();
    }

    private void LogResults(ERegexExecutionType type, TimeSpan elapsed) =>
        Console.WriteLine($"{type.ToString().PadRight(20, '_')} {elapsed}");

    private static void ExecMultipleReferences()
    {
        var rgx = new Regex(DEFAULT_REGEX);
        _ = rgx.Replace(DEFAULT_TEXT, " ");
    }

    private void ExecStatic() =>
        Regex.Replace(DEFAULT_TEXT, DEFAULT_REGEX, " ");

    private void ExecOnlyOneReference() =>
        _rgx.Replace(DEFAULT_TEXT, " ");
}

public enum ERegexExecutionType
{
    MultipleReferences = 1,
    Static = 2,
    OnlyOneReference = 3
}