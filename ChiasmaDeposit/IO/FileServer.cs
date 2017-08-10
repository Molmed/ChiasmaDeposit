using System;
using System.Data;
using System.IO;

namespace Molmed.ChiasmaDep.IO
{
    public class FileServer
    {
        public FileServer()
        {
        }

        public bool FileExists(string filePath)
        {
            //Returns true if the file exists, otherwise false.
            return new FileInfo(filePath).Exists;
        }

        public void FillFromFile(String filePath, DataTable table)
        {
            String[][] textData;
            Int32 i;
            Int32 j;
            String[] tempTextRow;
            DataRow row;

            try
            {
                textData = ReadMultipleColumns(filePath, table.Columns.Count);
                table.Rows.Clear();

                // Go through all rows.
                for (i = 0; i <= textData.GetUpperBound(0); i++)
                {
                    tempTextRow = textData[i];
                    row = table.NewRow();
                    // Go through all columns.
                    for (j = 0; j < tempTextRow.Length; j++)
                    {
                        row[j] = tempTextRow[j];
                    }
                    table.Rows.Add(row);
                }
            }
            catch
            {
                // Clear the table on error (otherwise could show half the file).
                table.Rows.Clear();
                throw;
            }
        }

        public string[][] ReadMultipleColumns(string filePath, int numberOfColumns)
        {
            //Returns rows (index 0) and columns (index 1) from a tab delimited text file.
            StreamReader sr = null;
            string textLine;
            int lineCount;
            string[][] colArray;
            int i;
            char[] delimiter;

            try
            {

                delimiter = new Char[1];

                delimiter[0] = (char)9;

                //Count the number of items.
                sr = new StreamReader(filePath);
                lineCount = 0;
                while ((textLine = sr.ReadLine()) != null)
                {
                    if (textLine.Trim() != "")
                    {
                        lineCount++;
                    }
                }
                sr.Close();
                //Read the items.
                colArray = new string[lineCount][];
                sr = new StreamReader(filePath);
                i = 0;
                while ((textLine = sr.ReadLine()) != null)
                {
                    textLine = textLine.Trim();
                    if (textLine != "")
                    {
                        colArray[i] = textLine.Split(delimiter);
                        if (colArray[i] != null)
                        {
                            if (colArray[i].GetLength(0) != numberOfColumns)
                            {
                                throw new Exception("Inappropriate number of columns on text row " + (i + 1).ToString() + ".");
                            }
                        }
                        i++;
                    }
                }
                sr.Close();
                return colArray;
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
            }
        }

        public string[] ReadSingleColumn(string filePath)
        {
            //Returns trimmed text lines from a file.
            StreamReader sr;
            string textLine;
            int lineCount;
            string[] stringArray;
            int i;

            //Count the number of items.
            sr = new StreamReader(filePath);
            lineCount = 0;
            while ((textLine = sr.ReadLine()) != null)
            {
                if (textLine.Trim() != "")
                {
                    lineCount++;
                }
                //Check for inappropriate tabs.
                for (i = 0; i < textLine.Length; i++)
                {
                    if (textLine.Trim()[i] == (char)9)
                    {
                        throw (new Exception("File format error."));
                    }
                }
            }
            sr.Close();
            //Read the items.
            stringArray = new string[lineCount];
            sr = new StreamReader(filePath);
            i = 0;
            while ((textLine = sr.ReadLine()) != null)
            {
                if (textLine.Trim() != "")
                {
                    stringArray[i] = textLine.Trim();
                    i++;
                }
            }
            sr.Close();
            return stringArray;
        }

        public bool SaveString(string text, string filePath)
        {
            //Saves the text string to the specified file. Returns true on success.
            StreamWriter sw;

            sw = new StreamWriter(filePath, false);
            sw.Write(text);
            sw.Flush();
            sw.Close();
            return true;
        }
    }
}
