using System.Collections.Generic;

namespace Balea.Store
{
	/// <summary>
	///     Represents the result of an access control operation
	/// </summary>
	public class AccessResult
	{
		private static readonly AccessResult _success = new AccessResult(true);

		/// <summary>
		///     Failure constructor that takes error messages
		/// </summary>
		/// <param name="errors"></param>
		public AccessResult(params string[] errors) : this((IEnumerable<string>)errors)
		{
		}

		/// <summary>
		///     Failure constructor that takes error messages
		/// </summary>
		/// <param name="errors"></param>
		public AccessResult(IEnumerable<string> errors)
		{
			if (errors == null)
			{
				errors = new[] { "Something went wrong" };
			}
			Succeeded = false;
			Errors = errors;
		}

		/// <summary>
		/// Constructor that takes whether the result is successful
		/// </summary>
		/// <param name="success"></param>
		protected AccessResult(bool success)
		{
			Succeeded = success;
			Errors = new string[0];
		}

		/// <summary>
		///     True if the operation was successful
		/// </summary>
		public bool Succeeded { get; private set; }

		/// <summary>
		///     List of errors
		/// </summary>
		public IEnumerable<string> Errors { get; private set; }

		/// <summary>
		///     Static success result
		/// </summary>
		/// <returns></returns>
		public static AccessResult Success
		{
			get { return _success; }
		}

		/// <summary>
		///     Failed helper method
		/// </summary>
		/// <param name="errors"></param>
		/// <returns></returns>
		public static AccessResult Failed(params string[] errors)
		{
			return new AccessResult(errors);
		}
	}
}
