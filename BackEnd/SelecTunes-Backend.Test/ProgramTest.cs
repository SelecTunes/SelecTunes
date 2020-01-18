using NUnit.Framework;

namespace SelecTunes.Test
{
    public class ProgramTest
    {
        [Test]
        public void AssertThatInstantiatedClassIsInstanceOfTheClass()
        {
            Program program = new Program();

            Assert.IsInstanceOf<Program>(program);
        }
    }
}