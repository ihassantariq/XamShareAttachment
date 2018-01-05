using System;
using System.Threading.Tasks;

namespace MessagingSample
{
	/// <summary>
	/// Interface for Share
	/// </summary>
	public interface IShare
	{

		/// <summary>
		/// Share a message with compatible services
		/// </summary>
		/// <param name="message">Message to share</param>
		/// <param name="options">Platform specific options</param>
		/// <returns>True if the operation was successful, false otherwise</returns>
		Task<bool> Share(ShareMessage message);


	}

}
