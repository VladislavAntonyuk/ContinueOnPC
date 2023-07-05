namespace ContinueOnPC.Models;

using System.Diagnostics.CodeAnalysis;

public class HasErrorResult<T>
{
	[MemberNotNullWhen(true, nameof(Result))]
	public bool IsSuccessful => !Errors.Any();

	public T? Result { get; set; }

	public ICollection<string> Errors { get; set; } = new List<string>();

	public HasErrorResult<T> WithError(string error)
	{
		Errors.Add(error);
		return this;
	}

	public HasErrorResult<T> WithResult(T result)
	{
		Result = result;
		return this;
	}
}