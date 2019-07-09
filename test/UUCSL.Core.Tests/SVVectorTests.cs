using System.Text;
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

			var sb = new StringBuilder();
			for(var i = 0; i < vector.Mask.Length; i++)
			{
				sb.Append(vector.Mask[i] ? '1' : '0');
			}
			var mask = sb.ToString();

			Assert.Equal(svString, vector.ToString());
			Assert.Equal("0000001011101", mask);
		}
	}
}
