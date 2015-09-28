using System;
using System.Collections.Generic;
using System.Linq;
using FsCheck;
using NUnit.Framework;

// ReSharper disable RedundantArgumentNameForLiteralExpression

namespace CustomShrinkerTests2
{
    [TestFixture]
    public class PalindromeCheckerTests
    {
        //private static readonly Config Config = Config.VerboseThrowOnFailure.WithStartSize(20);
        //private static readonly Configuration Configuration = Config.ToConfiguration();

        [Test]
        public void PassingTest()
        {
            //Spec
            //    .For(PalindromeGen(Any.OfType<int>()), xs => PalindromeChecker.IsPalindromic(xs, introduceDeliberateBug: false))
            //    .Check(Configuration);
        }

        [Test]
        public void FailingTestWithShrinking()
        {
            //Spec
            //    .For(PalindromeGen(Any.OfType<int>()), xs => PalindromeChecker.IsPalindromic(xs, introduceDeliberateBug: true))
            //    .Check(Configuration);
        }

        [Test]
        public void FailingTestWithoutShrinking()
        {
            //Spec
            //    .For(PalindromeGen(Any.OfType<int>()), xs => PalindromeChecker.IsPalindromic(xs, introduceDeliberateBug: true))
            //    .Shrink(_ => Enumerable.Empty<IList<int>>())
            //    .Check(Configuration);
        }

        [Test]
        public void FailingTestWithCustomShrinking()
        {
            //Spec
            //    .For(PalindromeGen(Any.OfType<int>()), xs => PalindromeChecker.IsPalindromic(xs, introduceDeliberateBug: true))
            //    .Shrink(PalindromeShrinker)
            //    .Check(Configuration);
        }

        private static IEnumerable<List<T>> PalindromeShrinker<T>(IEnumerable<T> value)
        {
            var copyOfValue = new List<T>(value);

            for (;;)
            {
                var hasOddLength = copyOfValue.Count % 2 == 1;
                for (var i = 0; i < (hasOddLength ? 1 : 2); i++)
                {
                    if (copyOfValue.Count == 0) yield break;
                    copyOfValue.RemoveAt((copyOfValue.Count - 1) / 2);
                    yield return copyOfValue;
                }
            }
        }

        private static Gen<List<T>> PalindromeGen<T>(Gen<T> genT)
        {
            return Gen.Frequency(
                Tuple.Create(50, EvenLengthPalindromeGen(genT)),
                Tuple.Create(50, OddLengthPalindromeGen(genT)));
        }

        private static Gen<List<T>> EvenLengthPalindromeGen<T>(Gen<T> genT)
        {
            return
                from xs in genT.ListOf()
                select xs.Concat(xs.Reverse()).ToList();
        }

        private static Gen<List<T>> OddLengthPalindromeGen<T>(Gen<T> genT)
        {
            return
                from xs in genT.ListOf()
                from x in genT
                select xs.Concat(new[] { x }).Concat(xs.Reverse()).ToList();
        }
    }
}
