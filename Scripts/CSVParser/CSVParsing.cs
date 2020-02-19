using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

public class CSVParsing : MonoBehaviour 
{
	public TextAsset csvFile;
	public Text contentArea; // Reference of contentArea where records are displayed

	private char lineSeperater = '\n'; // It defines line seperate character
	private char fieldSeperator = ';'; // It defines field seperate chracter

	void Start () 
	{
		string[,] grid = SplitCsvGrid(csvFile.text);
		Debug.Log("size = " + (1 + grid.GetUpperBound(0)) + fieldSeperator + (1 + grid.GetUpperBound(1)));

		// split the text to separate the items with ;
		SplitData(DebugOutputGrid(grid)); 
	}

	// outputs the content of a 2D array, useful for checking the importer
	static public string DebugOutputGrid(string[,] grid)
	{
		string textOutput = "";
		for(int y = 0; y < grid.GetUpperBound(1); y++)
		{
			for(int x = 0; x < grid.GetUpperBound(0); x++)
			{

				textOutput += grid[x, y];
				//textOutput += "\n";
			}
			textOutput += "\n";
		}
		Debug.Log(textOutput);
		return textOutput;
	}

	// splits a CSV file into a 2D string array
	static public string[,] SplitCsvGrid(string csvText)
	{
		string[] lines = csvText.Split("\n"[0]);

		// finds the max width of row
		int width = 0;
		for(int i = 0; i < lines.Length; i++)
		{
			string[] row = SplitCsvLine(lines[i]);
			width = Mathf.Max(width, row.Length);
		}

		// creates new 2D string grid to output to
		string[,] outputGrid = new string[width + 1, lines.Length + 1];
		for(int y = 0; y < lines.Length; y++)
		{
			string[] row = SplitCsvLine(lines[y]);
			for(int x = 0; x < row.Length; x++)
			{
				outputGrid[x, y] = row[x];

				// This line was to replace "" with " in my output. 
				// Include or edit it as you wish.
				outputGrid[x, y] = outputGrid[x, y].Replace("\"\"", "\"");
			}
		}

		return outputGrid;
	}

	// splits a CSV row 
	static public string[] SplitCsvLine(string line)
	{
		return (from System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(line,
		@"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)",
		System.Text.RegularExpressions.RegexOptions.ExplicitCapture)
				select m.Groups[1].Value).ToArray();
	}

	public void SplitData(string record)
	{
		string[] fields = record.Split(fieldSeperator);
		foreach(string field in fields)
		{
			contentArea.text += field + "\t";
		}
		contentArea.text += '\n';
	}
	
/*
// Add data to CSV file
public void addData()
{
	// Following line adds data to CSV file
	File.AppendAllText(getPath() + "/Assets/StudentData.csv",lineSeperater + rollNoInputField.text + fieldSeperator +nameInputField.text);
	// Following lines refresh the edotor and print data
	rollNoInputField.text = "";
	nameInputField.text = "";
	contentArea.text = "";
	#if UNITY_EDITOR
		UnityEditor.AssetDatabase.Refresh ();
	#endif
	readData();
}
*/
// Get path for given CSV file
	private static string getPath(){
		#if UNITY_EDITOR
		return Application.dataPath;
	/*	#elif UNITY_ANDROID
		return Application.persistentDataPath;// +fileName;
		#elif UNITY_IPHONE
		return GetiPhoneDocumentsPath();// +"/"+fileName;*/
		#else
		return Application.dataPath;// +"/"+ fileName;
		#endif
	}

	/*
	// Get the path in iOS device
	private static string GetiPhoneDocumentsPath()
	{
		string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
		path = path.Substring(0, path.LastIndexOf('/'));
		return path + "/Documents";
	}
	*/
}