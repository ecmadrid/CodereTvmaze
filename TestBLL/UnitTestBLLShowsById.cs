using CodereTvmaze.BLL;

namespace TestBLLAndWebApi
{
    public class TestsBLLShowsById
    {

        [SetUp]
        public void Setup()
        {

        }

        [TestCase(0)]
        [TestCase(-1)]
        public void TestGetMainInfoById_LowerThanOne(long value)
        {
            var result = MainInfo.GetById(value);
            Assert.That(result, Is.Null, "Value lower than 1 is not allowed.");
        }
        [TestCase(1)]
        [TestCase(72)]
        public void TestGetMainInfoById_GresterThanZero(long value)
        {
            var result = MainInfo.GetById(value);
            Assert.That(result, Is.Not.Null, "Value greater than 0 is allowed.");
        }

        [TestCase(1234567890)]
        public void TestGetMainInfoById_Excessivelylarge(long value)
        {
            var result = MainInfo.GetById(value);
            Assert.That(result, Is.Null, "Value excessively large is not allowed.");
        }
    }
}