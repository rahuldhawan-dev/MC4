<Query Kind="Program" />

void Main()
{
	var sapTypes = new Dictionary<int, string>();
	using (var conn = new SqlConnection("Data Source=localhost;Initial Catalog=mapcalldev;Integrated Security=sspi"))
	{
		conn.Open();
		var cmd = new SqlCommand("SELECT Id, Description FROM SAPEquipmentTypes WHERE Id IN (SELECT SAPEquipmentTypeId FROM EquipmentTypes) ORDER BY Description", conn);
		using (var results = cmd.ExecuteReader())
		{
			while (results.Read())
			{
				sapTypes[results.GetInt32(0)] = results.GetString(1);
			}
		}

		foreach (var type in sapTypes)
		{
			Console.WriteLine($"#region {type.Value}\n");
			Console.WriteLine("case \"" + type.Value + "\":");
			cmd = new SqlCommand($@"
select
	'INSERT INTO EquipmentCharacteristicFields (Id, FieldName, SAPEquipmentTypeId, FieldTypeId, Required, IsSAPCharacteristic, IsActive, OrderBy) VALUES (' +
	{IntField("Id")}
	{StringField("FieldName")}
	{IntField("SAPEquipmentTypeId")}
	{IntField("FieldTypeId")}
	{IntField("Required")}
	{IntField("IsSAPCharacteristic")}
	{IntField("IsActive")}
	{NullableIntField("OrderBy", true)}
* from EquipmentCharacteristicFields
WHERE SAPEquipmentTypeId = " + type.Key.ToString(), conn);
			var inserts = new List<string>();
			//Console.WriteLine(cmd.CommandText);
			using (var results = cmd.ExecuteReader())
			{
				while (results.Read())
				{
					inserts.Add(results.GetString(0));
				}
			}

			Console.WriteLine("return @\"");
			Console.WriteLine(string.Join("\n", inserts.ToArray()) + "\n\";");
			Console.WriteLine("\n#endregion\n");
		}

		conn.Close();
	}
}

// Define other methods and classes here

string Ending(bool end = false)
{
	return end ? ");'," : ", ' +";
}

string IntField(string name, bool end = false)
{
	return $"cast({name} as varchar) + '" + Ending(end);
}

string StringField(string name, bool end = false)
{
	return $"'''' + {name.Replace("'", "''")} + '''" + Ending(end);
}

string NullableIntField(string name, bool end = false)
{
	return $"isnull(cast({name} as varchar), 'NULL') + '" + Ending(end);
}