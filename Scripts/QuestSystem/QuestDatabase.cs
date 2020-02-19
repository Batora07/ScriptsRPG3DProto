using System.Collections;
using System.Collections.Generic;   // lets us use lists
using UnityEngine;
using System.Xml;                   // basic XML attributes
using System.Xml.Serialization;     // access xmlSerialiser

[System.Serializable]
public class QuestDatabase
{
	[XmlArray("Quests_List")]
	public List<Quest> list = new List<Quest>();
}

