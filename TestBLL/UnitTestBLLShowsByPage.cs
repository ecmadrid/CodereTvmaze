using CodereTvmaze.BLL;

namespace TestBLLAndWebApi
{
    /// <summary>
    /// Class <c>TestsBLLShowsByPage</c> tests BLL methods for get main info by page.
    /// </summary>
    public class TestsBLLShowsByPage
    {
        /// <summary>
        /// Setup.
        /// </summary>
        [SetUp]
        public void Setup()
        {

        }

        /// <summary>
        /// Method to test bad values for getting main infos page (lower than 0).
        /// </summary>
        /// <param name="value"></param>
        [TestCase(-1)]
        [TestCase(-2)]
        public void TestGetMainInfoByPage_LowerThanZero(long value)
        {
            var result = MainInfo.GetByPage(value);
            Assert.That(result, Is.Null, "Value lower than 0 is not allowed.");
        }
        /// <summary>
        /// Method to test correct values for getting main infos page (equal to or greater than 0).
        /// </summary>
        /// <param name="value"></param>
        [TestCase(0)]
        [TestCase(5)]
        public void TestGetMainInfoByPage_EqualToOrGreaterThanZero(long value)
        {
            var result = MainInfo.GetByPage(value);
            Assert.That(result, Is.Not.Null, "Value equal to o greater than 0 is allowed.");
        }

        /// <summary>
        /// Method to test bad values for getting main info (excessively great. More than max number of pages).
        /// </summary>
        /// <param name="value"></param>
        [TestCase(1234567890)]
        public void TestGetMainInfoByPage_ExcessivelyGreat(long value)
        {
            var result = MainInfo.GetByPage(value);
            Assert.That(result, Is.Null, "Value excessively great is not allowed.");
        }
    }
}