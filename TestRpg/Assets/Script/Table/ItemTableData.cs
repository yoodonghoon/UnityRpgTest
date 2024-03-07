using System.Collections.Generic;
using System.Linq;

public class ItemTableData
{
    public int ItemIndex;
    public string Name;
    public string ImagePath;
    public int Type;
}

public class ItemTable : SingletonCommon<ItemTable>
{
    public List<ItemTableData> Datas = new();

    public void LoadCsv()
    {
        List<Dictionary<string, object>> dataList = CSVReader.Read("ItemTable");

        for(int i = 0; i < dataList.Count; ++i)
        {
            ItemTableData data = new ();
            data.ItemIndex = (int)dataList[i]["ItemIndex"];
            data.Name = dataList[i]["Name"].ToString();
            data.ImagePath = dataList[i]["ImagePath"].ToString();
            data.Type = (int)dataList[i]["Type"];
            Datas.Add(data);
        }
    }

    public ItemTableData GetData(int itemIndex)
    {
        for(int i = 0; i < Datas.Count; ++i)
        {
            if (Datas[i].ItemIndex == itemIndex)
                return Datas[i];
        }

        return null;
    }
}
