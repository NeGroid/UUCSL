using Xunit;

namespace UUCSL.Core.Tests
{
	public class SVVectorTests
	{
		[Fact]
		public void Create_SVVector_from_SV_string()
		{
			var svString = "[SV 0 0 0 0 0 0 1 0 1 1 1 0 1]";
			var vector = SVVector.FromSV(svString);
			Assert.Equal(svString, vector.ToString());
		}
	}
}
