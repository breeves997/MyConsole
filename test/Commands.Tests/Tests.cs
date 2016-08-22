using System;
using Xunit;
using System.Linq;
using System.Collections;
using MyConsole;

namespace MyConsole.UnitTests.Tests
{
    public class Tests
    {
        [Fact]
        public void DirectionReduction_NoReduction()
        {
            string[] a = new string[] { "NORTH", "WEST", "SOUTH", "EAST" };
            string[] b = new string[] { "NORTH", "WEST", "SOUTH", "EAST" };
            Assert.True(b.SequenceEqual(Commands.StringChallenges.dirReduc(a)));

        }

				[Fact]
        public void DirectionReduction_Reduction()
        {
            string[] a = new string[] { "NORTH", "SOUTH", "SOUTH", "EAST", "WEST", "NORTH", "WEST" };
            string[] b = new string[] { "WEST" };
            Assert.True(b.SequenceEqual(Commands.StringChallenges.dirReduc(a)));

        }

				[Fact]
        public void DirectionReductionStack_Reduction()
        {
            string[] a = new string[] { "NORTH", "SOUTH", "SOUTH", "EAST", "WEST", "NORTH", "WEST" };
            string[] b = new string[] { "WEST" };
            Assert.True(b.SequenceEqual(Commands.StringChallenges.dirReducSolution(a)));

        }

				[Fact]
        public void ArrayDecompositionTest()
        {
            string ans = Commands.MathGames.ArrayDecomposition(0);
            Assert.Equal(ans, "[][0]");

            ans = Commands.MathGames.ArrayDecomposition(4);
            Assert.Equal(ans, "[2][0]");

            ans = Commands.MathGames.ArrayDecomposition(9);
            Assert.Equal(ans, "[3][1]");

            ans = Commands.MathGames.ArrayDecomposition(25);
            Assert.Equal(ans, "[4, 2][0]");

            ans = Commands.MathGames.ArrayDecomposition(8330475);
            Assert.Equal(ans, "[22, 13, 10, 8, 7, 6, 6, 5, 5, 5, 4, 4, 4, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2][0]");
        }
    }
}
