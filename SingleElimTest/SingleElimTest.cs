using Microsoft.VisualStudio.TestTools.UnitTesting;
using SingleElimDecisionAssist.Enums;
using SingleElimDecisionAssist.Models;
using System;

namespace SingleElimTest
{
    [TestClass]
    public class SingleElimTest
    {
        private string seq = "";
        private char winner;
        private readonly SingleElim<char> elim;

        public SingleElimTest()
        {
            elim = new SingleElim<char>()
            {
                NextCallback = x =>
                {
                    if (seq != "")
                        seq += ",";
                    seq += x.Item1;
                    seq += x.Item2;
                },
                WinnerCallback = x => winner = x,
                Shuffle = false
            };
        }

        [TestMethod]
        public void EvenNumberElements()
        {
            char[] items = { 'A', 'B', 'C', 'D' };

            elim.LoadNew(items); //CD
            elim.Choose(ElimChoice.First); // AB
            elim.Choose(ElimChoice.First); // AC
            elim.Choose(ElimChoice.First); // C win

            var expectedSeq = "CD,AB,CA";
            Assert.AreEqual(expectedSeq, seq);
            Assert.IsTrue(elim.HasWinner);
            Assert.AreEqual('C', winner);
        }

        [TestMethod]
        public void OddNumberElements()
        {
            char[] items = { 'A', 'B', 'C', 'D', 'E' };

            elim.LoadNew(items); //next CD
            elim.Choose(ElimChoice.First); // Pick C, next AB
            elim.Choose(ElimChoice.First); // Pick A, next EC
            elim.Choose(ElimChoice.First); // Pick E, next EA
            elim.Choose(ElimChoice.First); // Pick A, win 

            var expectedSeq = "CD,AB,EC,AE";
            Assert.AreEqual(expectedSeq, seq);
            Assert.IsTrue(elim.HasWinner);
            Assert.AreEqual('A', winner);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ZeroElementsThrowsException()
        {
            char[] items = { };

            elim.LoadNew(items);
        }

        [TestMethod]
        public void OneElementInstaWins()
        {
            char[] items = { 'A' };

            elim.LoadNew(items);

            Assert.IsTrue(elim.HasWinner);
            Assert.AreEqual('A', winner);
        }

        [TestMethod]
        public void ChooseAfterWinIsImpotent()
        {
            char[] items = { 'A', 'B' };

            elim.LoadNew(items);
            elim.Choose(ElimChoice.First); // A win
            elim.Choose(ElimChoice.First);
            elim.Choose(ElimChoice.Second);


            var expectedSeq = "AB";
            Assert.AreEqual(expectedSeq, seq);
            Assert.IsTrue(elim.HasWinner);
            Assert.AreEqual('A', winner);
        }
    }
}
