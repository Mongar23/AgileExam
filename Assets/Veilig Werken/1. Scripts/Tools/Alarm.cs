using MBevers;

namespace VeiligWerken.Prototyping
{
	/// <summary>
	///     <para>
	///         Created by: Mathias on 6/5/2021 3:31:06 PM.
	///     </para>
	/// </summary>
	public class Alarm
	{
		public enum AlarmType
		{
			Bells,
			Horns
		}

		public AlarmType Type { get; }
		public int Hundred { get; }
		public int One { get; }

		public Alarm(int hundred, int one, AlarmType type)
		{
			Hundred = hundred;
			One = one;
			Type = type;
		}

		public override string ToString() => $"{{\n\tHundred: {Hundred}\n\tOne: {One}\n\tType: {Type.Name((int) Type)}\n}}";
	}
}