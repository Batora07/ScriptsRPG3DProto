using System.Collections;
using System.Collections.Generic;   // lets us use lists
using UnityEngine;
using System.Xml;                   // basic XML attributes
using System.Xml.Serialization;     // access xmlSerialiser
using System.IO;                    // file management

public class XMLManager : MonoBehaviour
{
	public static XMLManager instance;

	private void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
	}

	/// <summary>
	/// lists of quests
	/// </summary>
	public QuestDatabase questsDB;

	// save function
	public void SaveQuestItems()
	{
		// open a new xml file
		XmlSerializer serializer = new XmlSerializer(typeof(QuestDatabase));
		/**
		 * classical way of serializing streams
		 * */
		/* FileStream stream = new FileStream(Application.dataPath + "/StreamingAssets/Data/XML/item_data.xml", FileMode.Create);
		   serializer.Serialize(stream, itemDB); */

		// this method prevents errors with encoding streams
		var encoding = System.Text.Encoding.GetEncoding("UTF-8");
		using(var stream = new StreamWriter(Application.dataPath + "/StreamingAssets/XML/Data/Quests/quests_data.xml", false, encoding))
		{
			serializer.Serialize(stream, questsDB);
			stream.Close();
		}
		// stream.Close();
	}

	// load function
	public void LoadQuestItems()
	{
		XmlSerializer serializer = new XmlSerializer(typeof(QuestDatabase));
		FileStream stream = new FileStream(Application.dataPath + "/StreamingAssets/XML/Data/Quests/quests_data.xml", FileMode.Open);
		questsDB = (QuestDatabase)serializer.Deserialize(stream);
		stream.Close();
	}
}
