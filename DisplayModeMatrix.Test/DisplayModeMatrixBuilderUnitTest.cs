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
                            .AddOptionalFactor("Device", l => l.Evidence("Mobile", x => true).Evidence("Tablet", x => true))
                            .AddOptionalFactor("Theme", l => l.Evidence("Dark", x => true))
                            .AddOptionalFactor("Preview", l => l.Evidence("Preview", x => true))
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
                "Mobile",
                "Tablet",
                "Dark",
                "Preview",
            };

            CollectionAssert.AreEqual(expected, result);
        }
    }
}
