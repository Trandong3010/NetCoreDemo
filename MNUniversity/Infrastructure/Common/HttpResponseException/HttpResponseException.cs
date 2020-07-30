using System;

namespace Infrastructure.Common.HttpResponseException
{
	public class HttpResponseException : Exception
	{
		public int Status { get; set; } = 500;
		public object Value { get; set; }
	}
}
