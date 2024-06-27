using CodereTvmaze.BLL;

namespace TestBLLAndWebApi
{
    public class TestsBLLShowsByPage
    {

        [SetUp]
        public void Setup()
        {

        }

        [TestCase(-1)]
        [TestCase(-2)]
        public void TestGetMainInfoByPage_LowerThanZero(long value)
        {
            var result = MainInfo.GetByPage(value);
            Assert.That(result, Is.Null, "Value lower than 0 is not allowed.");
        }
        [TestCase(0)]
        [TestCase(5)]
        public void TestGetMainInfoByPage_EqualToOrGresterThanZero(long value)
        {
            var result = MainInfo.GetByPage(value);
            Assert.That(result, Is.Not.Null, "Value equal to o greater than 0 is allowed.");
        }

        [TestCase(1234567890)]
        public void TestGetMainInfoByPage_Excessivelylarge(long value)
        {
            var result = MainInfo.GetByPage(value);
            Assert.That(result, Is.Null, "Value excessively large is not allowed.");
        }
    }
}