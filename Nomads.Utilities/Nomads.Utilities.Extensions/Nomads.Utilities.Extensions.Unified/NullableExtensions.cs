using System;

namespace Nomads.Utilities.Extensions
{
	public static class NullableExtensions
	{
		public static int Int32Value (this nint? n)
		{
			return Convert.ToInt32(n.Value);
		}
	}
}