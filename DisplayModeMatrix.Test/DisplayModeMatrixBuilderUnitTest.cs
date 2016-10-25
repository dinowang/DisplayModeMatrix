using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DisplayModeMatrix.Test
{
    [TestClass]
    public class DisplayModeMatrixBuilderUnitTest
    {
        [TestMethod]
        public void Test_DisplayModeMatrixBuilder_Could_Provide_Right_Combonations()
        {
            //Arrange
            var builder = new DisplayModeMatrixBuilder();

            var matrix = builder
                            .AddOptionalLayer("Device", l => l.Suffix("Mobile", x => true).Suffix("Tablet", x => true))
                            .AddOptionalLayer("Theme", l => l.Suffix("Dark", x => true))
                            .AddOptionalLayer("Preview", l => l.Suffix("Preview", x => true))
                            .Build();

            //Act
            var result = matrix.Select(x => x.Name).ToList();

            foreach (var x in result)
            {
                Console.WriteLine(x);
            }

            //Assert
            var expected = new[]
            {
                "Mobile-Dark-Preview",
                "Tablet-Dark-Preview",
                "Mobile-Dark",
                "Tablet-Dark",
                "Mobile-Preview",
                "Tablet-Preview",
                "Dark-Preview",
                "Dark-Preview",
                "Mobile",
                "Tablet",
                "Dark",
                "Preview",
            };

            CollectionAssert.AreEqual(expected, result);
        }
    }
}
