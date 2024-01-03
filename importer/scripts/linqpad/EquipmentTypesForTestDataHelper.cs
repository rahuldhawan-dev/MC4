void Main()
{
	var sapTypes = new Dictionary<int, string>();
	using (var conn = new SqlConnection("Data Source=localhost;Initial Catalog=mapcalldev;Integrated Security=sspi"))
	{
		conn.Open();
		var cmd = new SqlCommand("SELECT Id, Description FROM EquipmentTypes WHERE Id IN (SELECT EquipmentTypeId FROM EquipmentPurposes)", conn);
		using (var results = cmd.ExecuteReader())
		{
			while (results.Read())
			{
				sapTypes[results.GetInt32(0)] = results.GetString(1);
			}
		}

		foreach (var type in sapTypes)
		{
			Console.WriteLine("case \"" + type.Value + "\":");
			cmd = new SqlCommand(@"
select
	'INSERT INTO EquipmentPurposes (EquipmentPurposeID, Description, Abbreviation, EquipmentTypeId) VALUES (' +
	cast(EquipmentPurposeId as varchar) + ', ' +
	'''' + replace(Description, '""', '""""') + ''', ' +
	'''' + Abbreviation + ''', ' +
	cast(EquipmentTypeId as varchar) + ');',
* from EquipmentPurposes
WHERE EquipmentTypeId = " + type.Key.ToString(), conn);
			var inserts = new List<string>();
			using (var results = cmd.ExecuteReader())
			{
				while (results.Read())
				{
					inserts.Add(results.GetString(0));
				}
			}

			Console.WriteLine("return @\"");
			Console.WriteLine(string.Join("\n", inserts.ToArray()) + "\n\";");
		}

		conn.Close();
	}
}

// Define other methods and classes here
