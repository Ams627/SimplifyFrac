﻿using Rationals;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace SimplifyFrac;

internal record FractionProblem(Rational F1, Rational F2, char Op)
{
    public Rational Answer => Op switch
    {
        '+' => F1 + F2,
        '-' => F1 - F2,
        '*' => F1 * F2,
        '/' => F1 / F2,
        _ => throw new ArgumentException($"Bad operator ('{Op}')specified for fractions.")
    };

    public char PrintOp => Op switch
    {
        '+' => '+',
        '-' => '\u2212',
        '*' => '\u00B7',
        '/' => '\u2236',
        _ => throw new ArgumentException($"Bad operator ('{Op}')specified for fractions.")
    };

    //private (Rational f1, Rational f2, Rational answer, char op) Deconstruct()
    //{
    //    return (F1, F2, Answer, Op);
    //}

}

internal class Program
{
    public static void Shuffle<T>(IList<T> list)
    {
        var rnd = new Random();

        for (int i = list.Count() - 1; i > 0; i--)
        {
            // random from zero to i:
            var j = rnd.Next(i + 1);

            // swap:
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
    private static void Main(string[] args)
    {

        try
        {
            MakePercentSheet();
            Environment.Exit(0);
            MakeFracCombinedSheet();
            MakeDecimalSheet();
            MakeGcdSheet();
            MakeFracDivideSheet();
            MakeLowestCommonMultipleSheet();
            MakeFracAddSheet();
            MakeFracOfSheet();
            MakeFracSimplifySheet();
            MakeFracImproperSheet();
        }
        catch (Exception ex)
        {
            var fullname = System.Reflection.Assembly.GetEntryAssembly().Location;
            var progname = Path.GetFileNameWithoutExtension(fullname);
            Console.Error.WriteLine($"{progname} Error: {ex}");
        }

    }

    private static void MakePercentSheet()
    {
        var problems = (from before in Enumerable.Range(10, 200)
                        from percentage in Enumerable.Range(1, 98)
                        where percentage is not 0
                        where before % 100 != 0
                        where (before * percentage) % 100 == 0
                        select (before, percentage, answer: before + before * percentage / 100)
                      ).ToList();
        Shuffle(problems);

        using (var strm = new StreamWriter("percent-sheet.html"))
        {
            HtmlHelpers.WriteHead(strm);
            int count = 0;
            strm.WriteLine("<h1>Price rises</h1>");
            strm.WriteLine("<p>In each case calculate the percentage increase. <b>In these questions, THIS IS ALWAYS A WHOLE NUMBER (celé číslo)</b></p>");
            strm.WriteLine(@"<table class=""sumstable-two"">");
            int cols = 2;
            foreach (var num in problems.Take(20))
            {
                if (count % cols == 0)
                {
                    strm.WriteLine($"<tr>");
                }
                strm.WriteLine("<td>");

                strm.WriteLine($"<div class=\"qn\">{count + 1}</div>");
                strm.WriteLine($"<div class=\"eqn\">Original price: {num.before}");
                strm.WriteLine($"<br>Current price: {num.answer}</div>");
                strm.WriteLine("</td>");
                if (count % cols == cols - 1)
                {
                    strm.WriteLine($"</tr>");
                }
                count++;
            }
            strm.WriteLine("</table>");

            strm.WriteLine(@"<div class=""pageDivider"">");

            count = 0;
            strm.WriteLine("<table class=\"sumstable\">");
            foreach (var num in problems.Take(20))
            {
                if (count % cols == 0)
                {
                    strm.WriteLine($"<tr>");
                }
                strm.WriteLine("<td>");

                strm.WriteLine($"<div class=\"qn\">{count + 1}</div>");
                strm.WriteLine($"<div class=\"eqn\">{num.percentage}</div>");
                strm.WriteLine("</td>");
                if (count % cols == cols - 1)
                {
                    strm.WriteLine($"</tr>");
                }
                count++;
            }
            strm.WriteLine("</table>");

            strm.WriteLine("</body></html>");
        }

    }

    private static void MakeDecimalSheet()
    {
        var nums = (from x in Enumerable.Range(11, 88) select x).Where(x => x % 10 != 0).ToList();
        Shuffle(nums);

        using (var strm = new StreamWriter("decimal-square-sheet.html"))
        {
            HtmlHelpers.WriteHead(strm);
            int count = 0;

            strm.WriteLine("<table class=\"sumstable\">");
            int cols = 3;
            foreach (var num in nums.Take(36))
            {
                if (count % cols == 0)
                {
                    strm.WriteLine($"<tr>");
                }
                strm.WriteLine("<td>");
                var doc = new XDocument(
                    new XElement("math",
                        new XElement("mstyle", new XAttribute("displaystyle", "true"),
                                new XElement("mn", Format(0.1 * num)),
                                new XElement("mo", '\u00B7'),
                                new XElement("mn", Format(0.1 * num))
                            )));

                strm.WriteLine($"<div class=\"qn\">{count + 1}</div>");
                strm.WriteLine($"<div class=\"eqn\">{doc}</div>");
                strm.WriteLine("</td>");
                if (count % cols == cols - 1)
                {
                    strm.WriteLine($"</tr>");
                }
                count++;
            }
            strm.WriteLine("</table>");

            strm.WriteLine(@"<div class=""pageDivider"">");

            count = 0;
            strm.WriteLine("<table class=\"sumstable\">");
            foreach (var num in nums.Take(36))
            {
                if (count % cols == 0)
                {
                    strm.WriteLine($"<tr>");
                }
                strm.WriteLine("<td>");
                var doc = new XDocument(
                    new XElement("math",
                        new XElement("mstyle", new XAttribute("displaystyle", "true"),
                                new XElement("mn", Format(0.1 * num)),
                                new XElement("mo", '\u00B7'),
                                new XElement("mn", Format(0.1 * num)),
                                new XElement("mo", "="),
                                new XElement("mn", Format2(0.01 * num * num)))
                            ));

                strm.WriteLine($"<div class=\"qn\">{count + 1}</div>");
                strm.WriteLine($"<div class=\"eqn\">{doc}</div>");
                strm.WriteLine("</td>");
                if (count % cols == cols - 1)
                {
                    strm.WriteLine($"</tr>");
                }
                count++;
            }
            strm.WriteLine("</table>");

            strm.WriteLine("</body></html>");
        }
    }

    private static object Format2(double v)
    {
        return string.Format(new System.Globalization.CultureInfo("de-de"), "{0:0.00}", v);
    }

    private static string Format(double v)
    {
        return string.Format(new System.Globalization.CultureInfo("de-de"), "{0:0.0}", v);
    }

    private static void MakeGcdSheet()
    {
        // list of powers to which to raise each prime:
        var powerList = from i in Enumerable.Range(0, 7)      // 2
                        from j in Enumerable.Range(0, 6)      // 3
                        from k in Enumerable.Range(0, 5)      // 5
                        from l in Enumerable.Range(0, 3)      // 7
                        from m in Enumerable.Range(0, 2)      // 11
                        from n in Enumerable.Range(0, 2)      // 13
                        select new[] { i, j, k, l, m, n };

        var listOfComposedInts = powerList.Select(x => new ComposedInteger(x)).Where(x => x.Result < 3600).ToList();

        var gcdProblems = new List<GcdProblem>();

        var r1 = new Random();
        for (int i = 0; i < 100000; i++)
        {
            var q1 = listOfComposedInts[r1.Next(listOfComposedInts.Count)];
            var q2 = listOfComposedInts[r1.Next(listOfComposedInts.Count)];
            var q3 = listOfComposedInts[r1.Next(listOfComposedInts.Count)];

            if (q1.Result == q2.Result || q2.Result == q3.Result || q3.Result == q1.Result)
            {
                continue;
            }

            var gcdProblem = new GcdProblem(q1, q2, q3);
            if (gcdProblem.GetGcd() < 6)
            {
                continue;
            }

            gcdProblems.Add(gcdProblem);
        }

        gcdProblems.Sort((x, y) => x.GetGcd().CompareTo(y.GetGcd()));

        var lk = gcdProblems.ToLookup(x => x.GetGcd());
        var goodProblems = new List<GcdProblem>();
        foreach (var kv in lk)
        {
            var prob = kv.First();
            var commonPrimes = prob.GetCommonPrimeFactors();
            if (commonPrimes.Sum() < 3 || commonPrimes.Count(x => x == 0) > 4)
            {
                continue;
            }

            goodProblems.Add(kv.First());
        }

        Shuffle(goodProblems);


        using (var strm = new StreamWriter("gcd.html"))
        {
            HtmlHelpers.WriteHead(strm);
            int count = 0;

            strm.WriteLine(@"<p>Calculate the Greatest Common Divisor (Největší společný dělitel) for each set of numbers:</p>");

            strm.WriteLine("<table class=\"gcdtable\">");
            int cols = 2;
            foreach (var prob in goodProblems)
            {
                if (count % cols == 0)
                {
                    strm.WriteLine($"<tr>");
                }
                strm.WriteLine("<td>");

                var numbers = string.Join("&nbsp;&nbsp;&nbsp;", prob.Integers.Select(x => x.Result.ToString()));
                strm.WriteLine($"<div class=\"qn\">{count + 1}</div>");
                strm.WriteLine($"<div class=\"eqn\">{numbers}</div>");
                strm.WriteLine("</td>");
                if (count % cols == cols - 1)
                {
                    strm.WriteLine($"</tr>");
                }
                count++;
            }
            strm.WriteLine("</table>");


            count = 0;
            strm.WriteLine("<table class=\"answerstable\">");
            foreach (var prob in goodProblems)
            {
                if (count % cols == 0)
                {
                    strm.WriteLine($"<tr>");
                }
                strm.WriteLine("<td>");

                var numbers = string.Join("&nbsp;&nbsp;&nbsp;", prob.Integers.Select(x => x.Result.ToString()));
                strm.WriteLine($"<div class=\"qn\">{count + 1}</div>");
                strm.WriteLine($"<div class=\"eqn\">{numbers}</div>");
                strm.WriteLine($"<div class=\"eqn\">{prob.GetGcd()}</div>");

                var primes = new[] { 2, 3, 5, 7, 11, 13 };

                foreach (var entry in prob.Integers.Select(x => x.PrimeFactorPowers))
                {
                    var z = entry.Zip(primes);

                    var doc = new XDocument(new XElement("div",
                        new XElement("math",
                            new XElement("mstyle", new XAttribute("displaystyle", "true"),
                            JoinXElements(new XElement("mo", "\u22C5"), z.Select(x => new XElement("msup",
                                                        new XElement("mi", x.Second),
                                                        new XElement("mn", x.First)))
                            )))));
                    strm.WriteLine(doc);
                }

                strm.WriteLine("<br/>");
                strm.WriteLine("<br/>");

                strm.WriteLine("</td>");
                if (count % cols == cols - 1)
                {
                    strm.WriteLine($"</tr>");
                }
                count++;
            }
            strm.WriteLine("</table>");

            strm.WriteLine("</body></html>");
        }
    }

    private static IEnumerable<XElement> JoinXElements(XElement separator, IEnumerable<XElement> xElements)
    {
        bool first = true;
        foreach (var xElement in xElements)
        {
            if (!first)
            {
                yield return separator;
            }
            first = false;

            yield return xElement;
        }
    }

    private static void MakeFracDivideSheet()
    {
        var al = Enumerable.Range(1, 11);
        var bl = Enumerable.Range(2, 11);
        var cl = Enumerable.Range(1, 11);
        var dl = Enumerable.Range(2, 11);

        var problems = (from a in al
                        from b in bl
                        from c in cl
                        from d in dl
                        let rNum = a * d
                        let rDenom = b * c
                        where
                          a < b && !HaveCommonPrimeFactors(a, b) &&
                          c < d && !HaveCommonPrimeFactors(c, d) &&
                          HaveCommonPrimeFactors(rNum, rDenom)
                        select (a, b, c, d)).Take(100_000).ToList();
        Shuffle(problems);

        using (var strm = new StreamWriter("divide-frac.html"))
        {
            HtmlHelpers.WriteHead(strm);
            int count = 0;


            strm.WriteLine("<h1>Instructions</h1>");

            strm.WriteLine(@"<p>Calculate the results as fractions and <b>simplify each result</b>. Where the result is great than 1, write the result as 
a whole number and a fraction. For example:</p>");

            var introDoc = new XDocument(
                new XElement("math",
                    new XElement("mstyle", new XAttribute("displaystyle", "true"),
                        new XElement("mfrac",
                            new XElement("mn", 5),
                            new XElement("mn", 6)),
                        new XElement("mspace", new XAttribute("width", "10px")),
                        new XElement("mtext", ":"),
                        new XElement("mspace", new XAttribute("width", "10px")),
                        new XElement("mspace"),
                        new XElement("mfrac",
                            new XElement("mn", 5),
                            new XElement("mn", 11)),
                        new XElement("mo", "="),
                        new XElement("mfrac",
                            new XElement("mn", 11),
                            new XElement("mn", 6)),
                        new XElement("mo", "="),
                        new XElement("mn", 1),
                        new XElement("mfrac",
                            new XElement("mn", 5),
                            new XElement("mn", 6))
                        )));

            strm.WriteLine(introDoc);

            strm.WriteLine("<br>");
            strm.WriteLine("<br>");

            strm.WriteLine("<table class=\"sumstable\">");
            int cols = 3;
            foreach (var (a, b, c, d) in problems)
            {
                if (count % cols == 0)
                {
                    strm.WriteLine($"<tr>");
                }
                strm.WriteLine("<td>");

                var doc = new XDocument(
                    new XElement("math",
                        new XElement("mstyle", new XAttribute("displaystyle", "true"),
                            new XElement("mfrac",
                                new XElement("mn", a),
                                new XElement("mn", b)),
                            new XElement("mspace", new XAttribute("width", "10px")),
                            new XElement("mtext", ":"),
                            new XElement("mspace", new XAttribute("width", "10px")),
                            new XElement("mspace"),
                            new XElement("mfrac",
                                new XElement("mn", c),
                                new XElement("mn", d))
                            )));

                strm.WriteLine($"<div class=\"qn\">{count + 1}</div>");
                strm.WriteLine($"<div class=\"eqn\">{doc}</div>");
                strm.WriteLine("</td>");
                if (count % cols == cols - 1)
                {
                    strm.WriteLine($"</tr>");
                }
                count++;
            }
            strm.WriteLine("</table>");
            strm.WriteLine("</body></html>");
        }
    }

    private static bool HaveCommonPrimeFactors(int a, int b)
    {
        int[] primes = new[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97 };
        var aPrimeFactors = primes.Where(x => a % x == 0).ToHashSet();
        var bPrimeFactors = primes.Where(x => b % x == 0).ToHashSet();
        return aPrimeFactors.Intersect(bPrimeFactors).Count() > 0;
    }

    private static void MakeFracOfSheet()
    {
        var fracs = (from d in Enumerable.Range(2, 11)
                     from n in Enumerable.Range(2, 11)
                     from mult in Enumerable.Range(2, 11)
                     select (n, d, number: mult * d)
                     ).ToList();
        var goodFracs = fracs.Where(x => x.n < x.d).ToList();
        Shuffle(goodFracs);
        using (var strm = new StreamWriter("fraction-of-sheet.html"))
        {
            HtmlHelpers.WriteHead(strm);
            int count = 0;

            strm.WriteLine("<table class=\"sumstable\">");
            int cols = 3;
            foreach (var (n, d, number) in goodFracs)
            {
                if (count % cols == 0)
                {
                    strm.WriteLine($"<tr>");
                }
                strm.WriteLine("<td>");

                var doc = new XDocument(
                    new XElement("math",
                        new XElement("mstyle", new XAttribute("displaystyle", "true"),
                            new XElement("mfrac",
                                new XElement("mn", n),
                                new XElement("mn", d)),
                            new XElement("mo", " of "),
                            new XElement("mn", number)
                            )));

                strm.WriteLine($"<div class=\"qn\">{count + 1}</div>");
                strm.WriteLine($"<div class=\"eqn\">{doc}</div>");
                strm.WriteLine("</td>");
                if (count % cols == cols - 1)
                {
                    strm.WriteLine($"</tr>");
                }
                count++;
            }
            strm.WriteLine("</table>");
            strm.WriteLine("</body></html>");
        }

        Console.WriteLine();
    }

    private static int GetGcd(int a, int b)
    {
        while (b != 0)
        {
            var t = b;
            b = a % b;
            a = t;
        }
        return a;
    }

    private static int GetLcm(int a, int b)
    {
        return a * b / GetGcd(a, b);
    }


    private static void MakeLowestCommonMultipleSheet()
    {
        var pairs = (from d1 in Enumerable.Range(2, 120)
                     from d2 in Enumerable.Range(2, 120)
                     let lcm = GetLcm(d1, d2)
                     where d1 != d2 && d1 * d2 < 144
                     && lcm != 1 && lcm != d1 && lcm != d2 && lcm != d1 * d2
                     select (d1: d1, d2: d2, lcm: lcm)
                     ).ToList();
        Shuffle(pairs);

        using (var strm = new StreamWriter("lcm-sheet.html"))
        {
            HtmlHelpers.WriteHead(strm);
            int count = 0;

            strm.WriteLine(@"<p class=""header"">Find the lowest common multiple (nejnižší společný násobek)</p>");
            strm.WriteLine("<table class=\"sumstable\">");
            int cols = 3;
            foreach (var (d1, d2, lcm) in pairs)
            {
                if (count % cols == 0)
                {
                    strm.WriteLine($"<tr>");
                }
                strm.WriteLine("<td>");

                var doc = $"{d1} and {d2}";

                strm.WriteLine($"<div class=\"qn\">{count + 1}</div>");
                strm.WriteLine($"<div class=\"eqn\">{doc}</div>");
                strm.WriteLine("</td>");
                if (count % cols == cols - 1)
                {
                    strm.WriteLine($"</tr>");
                }
                count++;
            }
            strm.WriteLine("</table>");
            strm.WriteLine("</body></html>");
        }
    }


    private static void MakeFracAddSheet()
    {
        var fracs = (from d1 in Enumerable.Range(2, 11)
                     from d2 in Enumerable.Range(2, 11)
                     where d1 != d2 && d1 != 11 && d2 != 11
                     from n1 in Enumerable.Range(1, d1 - 1)
                     where n1 == 1 || GetGcd(n1, d1) == 1      // ensure you can't simplify fraction 1 in question
                     from n2 in Enumerable.Range(1, d2 - 1)
                     where n2 == 1 || GetGcd(n2, d2) == 1      // ensure you can't simplify fraction 2 in question
                     from op in new char[] { '+', '-' }
                     select (n1, n2, d1, d2, op)).ToList();
        var goodFracs = fracs.Where(x => x.n1 * x.d2 + x.n2 * x.d1 < x.d1 * x.d2).ToList();
        Shuffle(goodFracs);

        using (var strm = new StreamWriter("frac-add-sheet.html"))
        {
            HtmlHelpers.WriteHead(strm);
            int count = 0;

            strm.WriteLine("<table class=\"sumstable\">");
            int cols = 3;
            foreach (var (n1, n2, d1, d2, op) in goodFracs)
            {
                Rational f1 = (Rational)n1 / d1;
                Rational f2 = (Rational)n2 / d2;

                Rational result = op == '+' ? (f1 + f2).CanonicalForm : (f1 - f2).CanonicalForm;

                if (count % cols == 0)
                {
                    strm.WriteLine($"<tr>");
                }
                strm.WriteLine("<td>");
                var doc = new XDocument(
                    new XElement("math",
                        new XElement("mstyle", new XAttribute("displaystyle", "true"),
                            new XElement("mfrac",
                                new XElement("mn", n1),
                                new XElement("mn", d1)),
                            new XElement("mo", op),
                            new XElement("mfrac",
                                new XElement("mn", n2),
                                new XElement("mn", d2)),
                            new XElement("mo", "="),
                            new XElement("mfrac",
                                new XElement("mn", result.Numerator),
                                new XElement("mn", result.Denominator))
                            )));

                strm.WriteLine($"<div class=\"qn\">{count + 1}</div>");
                strm.WriteLine($"<div class=\"eqn\">{doc}</div>");
                strm.WriteLine("</td>");
                if (count % cols == cols - 1)
                {
                    strm.WriteLine($"</tr>");
                }
                count++;
            }
            strm.WriteLine("</table>");
            strm.WriteLine("</body></html>");
        }
    }

    private static void MakeFracCombinedSheet()
    {
        var adds = from d1 in Enumerable.Range(2, 11)
                   from d2 in Enumerable.Range(2, 11)
                   where d1 != d2 && d1 != 11 && d2 != 11
                   from n1 in Enumerable.Range(1, d1 - 1)
                   where n1 == 1 || GetGcd(n1, d1) == 1      // ensure you can't simplify fraction 1 in question
                   from n2 in Enumerable.Range(1, d2 - 1)
                   where n2 == 1 || GetGcd(n2, d2) == 1      // ensure you can't simplify fraction 2 in question
                   from op in new char[] { '+', '-' }
                   let problem = new FractionProblem((Rational)n1 / d1, (Rational)n2 / d2, op)
                   select problem;


        var products = from d1 in Enumerable.Range(2, 20)
                       from d2 in Enumerable.Range(2, 20)
                       where d1 != d2
                       from n1 in Enumerable.Range(1, d1 - 1)
                       where n1 == 1 || GetGcd(n1, d1) == 1      // ensure you can't simplify fraction 1 in question
                       from n2 in Enumerable.Range(1, d2 - 1)
                       where n2 == 1 || GetGcd(n2, d2) == 1      // ensure you can't simplify fraction 2 in question
                       from op in new char[] { '/', '*' }
                       let problem = new FractionProblem((Rational)n1 / d1, (Rational)n2 / d2, op)
                       where problem.Answer.CanonicalForm.Denominator < 200
                             && problem.Answer.CanonicalForm.Numerator < 100
                             && problem.Answer.Denominator != problem.Answer.CanonicalForm.Denominator
                       select problem;

        var plus = adds.Where(x => x.Op == '+').ToList();
        var minus = adds.Where(x => x.Op == '-').ToList();
        var multiply = products.Where(x => x.Op == '*').ToList();
        var divide = products.Where(x => x.Op == '/').ToList();

        Shuffle(plus);
        Shuffle(minus);
        Shuffle(multiply);
        Shuffle(divide);

        var countMin = new[] { plus.Count, minus.Count, multiply.Count, divide.Count }.Min();

        var combined = new List<FractionProblem>();
        for (int i = 0; i < countMin; i++)
        {
            combined.Add(plus[i]);
            combined.Add(minus[i]);
            combined.Add(multiply[i]);
            combined.Add(divide[i]);
        }

        using (var strm = new StreamWriter("frac-combined-sheet.html"))
        {
            HtmlHelpers.WriteHead(strm);
            int count = 0;

            strm.WriteLine("<table class=\"sumstable\">");
            int cols = 3;
            foreach (var problem in combined.Take(36))
            {
                if (count % cols == 0)
                {
                    strm.WriteLine($"<tr>");
                }
                strm.WriteLine("<td>");
                var doc = new XDocument(
                    new XElement("math",
                        new XElement("mstyle", new XAttribute("displaystyle", "true"),
                            new XElement("mfrac",
                                new XElement("mn", problem.F1.Numerator),
                                new XElement("mn", problem.F1.Denominator)),
                            new XElement("mo", problem.PrintOp),
                            new XElement("mfrac",
                                new XElement("mn", problem.F2.Numerator),
                                new XElement("mn", problem.F2.Denominator))
                            )));

                strm.WriteLine($"<div class=\"qn\">{count + 1}</div>");
                strm.WriteLine($"<div class=\"eqn\">{doc}</div>");
                strm.WriteLine("</td>");
                if (count % cols == cols - 1)
                {
                    strm.WriteLine($"</tr>");
                }
                count++;
            }
            strm.WriteLine("</table>");

            strm.WriteLine(@"<div class=""pageDivider"">");

            count = 0;
            strm.WriteLine("<table class=\"sumstable\">");
            foreach (var problem in combined.Take(36))
            {
                if (count % cols == 0)
                {
                    strm.WriteLine($"<tr>");
                }
                strm.WriteLine("<td>");
                var doc = new XDocument(
                    new XElement("math",
                        new XElement("mstyle", new XAttribute("displaystyle", "true"),
                            new XElement("mfrac",
                                new XElement("mn", problem.F1.Numerator),
                                new XElement("mn", problem.F1.Denominator)),
                            new XElement("mo", problem.PrintOp),
                            new XElement("mfrac",
                                new XElement("mn", problem.F2.Numerator),
                                new XElement("mn", problem.F2.Denominator)),
                            new XElement("mo", "="),
                            new XElement("mfrac",
                                new XElement("mn", problem.Answer.CanonicalForm.Numerator),
                                new XElement("mn", problem.Answer.CanonicalForm.Denominator))
                            )));

                strm.WriteLine($"<div class=\"qn\">{count + 1}</div>");
                strm.WriteLine($"<div class=\"eqn\">{doc}</div>");
                strm.WriteLine("</td>");
                if (count % cols == cols - 1)
                {
                    strm.WriteLine($"</tr>");
                }
                count++;
            }
            strm.WriteLine("</table>");

            strm.WriteLine("</body></html>");
        }
    }

    private static Rational GetProductAnswer(int n1, int d1, int n2, int d2, char op)
    {
        Rational f1 = (Rational)n1 / d2;
        Rational f2 = (Rational)n2 / d2;

        var answer = op == '*' ? f1 * f2 : f1 / f2;
        return answer;
    }

    private static void MakeFracImproperSheet()
    {
    }

    private static void MakeFracSimplifySheet()
    {
        var nonPrimes = new[] { 4, 6, 8, 9, 10, 12, 14, 15, 16, 18, 20, 21, 22, 24 }; //25, 26, 28, 32, 33, 34, 36 };

        var dict = new Dictionary<int, HashSet<int>>();
        foreach (var n in nonPrimes)
        {
            for (int i = 2; i <= Math.Sqrt(n); i++)
            {
                if (n % i == 0)
                {
                    if (!dict.TryGetValue(n, out var list))
                    {
                        list = new HashSet<int>();
                        dict.Add(n, list);
                    }
                    list.Add(i);
                    list.Add(n / i);
                }
            }
        }

        var flatList = (from entry in dict from setEntry in entry.Value select (denominator: entry.Key, numerator: setEntry)).ToList();
        Shuffle(flatList);

        using (var strm = new StreamWriter("frac-sheet.html"))
        {
            HtmlHelpers.WriteHead(strm);
            int count = 0;

            strm.WriteLine("<table class=\"sumstable\">");
            int cols = 3;
            foreach (var (d, n) in flatList)
            {
                if (count % cols == 0)
                {
                    strm.WriteLine($"<tr>");
                }
                strm.WriteLine("<td>");
                var doc = new XDocument(
                    new XElement("math",
                        new XElement("mstyle", new XAttribute("displaystyle", "true"),
                            new XElement("mfrac",
                                new XElement("mn", n),
                                new XElement("mn", d)))));

                strm.WriteLine($"<div class=\"qn\">{count + 1}</div>");
                strm.WriteLine($"<div class=\"eqn\">{doc}</div>");
                strm.WriteLine("</td>");
                if (count % cols == cols - 1)
                {
                    strm.WriteLine($"</tr>");
                }
                count++;
            }
            strm.WriteLine("</table>");
            strm.WriteLine("</body></html>");
        }
    }
}
