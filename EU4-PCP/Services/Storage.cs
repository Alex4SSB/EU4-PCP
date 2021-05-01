using EU4_PCP.Converters;
using System;

namespace EU4_PCP.Services
{
	class Storage
	{
		public static string RetrieveValue(Enum value) => RetrieveValue(value.ToString())?.ToString();

		public static string RetrieveValue(Type value) => RetrieveValue(value.Name)?.ToString();

		public static string RetrieveValue(object tag) => RetrieveValue((string)tag)?.ToString();

		public static object RetrieveValue(string keyName)
		{
			return App.Current.Properties[keyName];
		}

		public static void StoreValue(string value, Enum keyName) => StoreValue((object)value, keyName.ToString());
		public static void StoreValue(string value, object tag) => StoreValue((object)value, (string)tag);

		public static void StoreValue(string value, string keyName) => StoreValue((object)value, keyName);

		public static void StoreValue(object value, string keyName)
		{
			if (value is null || 
				(value is string strVal && string.IsNullOrEmpty(strVal))) return;
			App.Current.Properties[keyName] = value;
		}

		public static bool RetrieveBool(Enum value) => RetrieveBool(value.ToString());

		public static bool RetrieveBool(object tag) => RetrieveBool((string)tag);

		public static bool RetrieveBool(string keyName)
		{
			return RetrieveValue(keyName) switch
			{
				string value when !string.IsNullOrEmpty(value) => bool.Parse(value),
				_ => keyName.GetDefault() == 1
			};
		}

		public static bool RetrieveBoolEnum(Enum value)
		{
			var index = RetrieveGroup(value.GetType().Name);
			return EnumToLong.GetIndex(value.GetType().Name, value.ToString()) == index;
		}

		public static long RetrieveGroup(string groupName)
		{
			var obj = RetrieveValue(groupName);

			// If there was no value - get the default value, otherwise get the index as a long
			return obj switch
			{
				null => SaveDefault(groupName),
				long value => value,
				_ => EnumToLong.GetIndex(groupName, obj.ToString())
			};
		}

		public static long SaveDefault(string groupName)
		{
			long index = groupName.GetDefault();
			Type groupEnum = groupName.ToEnum();
			StoreValue(Enum.Parse(groupEnum, Enum.GetName(groupEnum, index)), groupName);
			
			return index;
		}
	}
}
