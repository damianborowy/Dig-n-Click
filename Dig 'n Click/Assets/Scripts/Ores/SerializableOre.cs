using System;
using System.Collections.Generic;

public class SerializableOre
{
    public static Dictionary<string, int> ConvertToSerializable(SortedList<Ore, int> itemsList)
    {
        var serialized = new Dictionary<string, int>();

        foreach(KeyValuePair<Ore, int> pair in itemsList)
        {
            serialized.Add(pair.Key.Name, pair.Value);
        }

        return serialized;
    }

    public static void DeserializeOres(Dictionary<string, int> serialized)
    {
        var oresList = OresList.Instance.Ores;
        
        foreach(var ore in oresList)
        {
            if (serialized.ContainsKey(ore.Name))
            {
                EquipmentController.Instance.Items.Add(ore, serialized[ore.Name]);
            }
        }
    }
}