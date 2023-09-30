
//TextWriter textWriter = new StreamWriter("C:\\dev\\labs", true);
using System.Collections.Generic;

Console.WriteLine("Inform the picture size:");
int pictureCount = Convert.ToInt32(Console.ReadLine().Trim());

List<string> picture = new List<string>();

for (int i = 0; i < pictureCount; i++)
{
    Console.WriteLine("Inform the picture in a row:");
    string pictureItem = Console.ReadLine();
    picture.Add(pictureItem);
}

int result = Result.strokesRequired(picture);

Console.WriteLine(result);

/*textWriter.WriteLine(result);

textWriter.Flush();
textWriter.Close();*/

class Result
{

    /*
     * Complete the 'strokesRequired' function below.
     *
     * The function is expected to return an INTEGER.
     * The function accepts STRING_ARRAY picture as parameter.
     */

    public static int strokesRequired(List<string> picture)
    {
        //Check if the picture list is empty or null
        if (picture == null || picture.Count == 0)
        {
            return 0;
        }

        int numRows = picture.Count;
        int numCols = picture[0].Length;
        List<MatrixItem> matrixItemList = generateMatrixItems(numRows, numCols, picture);

        int totalStrokes = 0;

        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {
                int currentIndex = row * numCols + col;
                MatrixItem currentMatrixItem = matrixItemList[currentIndex];

                // If this item was not painted, let's paint it and check for adjacents equals
                if (!currentMatrixItem.IsPainted)
                {
                    searchForAdjacents(ref matrixItemList, currentMatrixItem, numCols);
                    totalStrokes++;
                }
            }
        }

        return totalStrokes;
    }

    public static void searchForAdjacents(ref List<MatrixItem> matrixItemList, MatrixItem currentMatrixItem, int numCols)
    {
        if (!matrixItemList.Any() || currentMatrixItem is null || currentMatrixItem.Row < 0 || currentMatrixItem.Col < 0 || currentMatrixItem.IsPainted)
        {
            return;
        }

        try
        {
            int currentIndex = currentMatrixItem.Row * numCols + currentMatrixItem.Col;
            matrixItemList[currentIndex].IsPainted = true;

            char targetColor = currentMatrixItem.Value[0];

            // Check for the upper item
            if (currentMatrixItem.Row > 0)
            {
                currentIndex = (currentMatrixItem.Row - 1) * numCols + currentMatrixItem.Col;
                if (matrixItemList[currentIndex].Value[0] == targetColor)
                {
                    searchForAdjacents(ref matrixItemList, matrixItemList[currentIndex], numCols);
                }
            }

            // Check for the lower item
            if (currentMatrixItem.Row < matrixItemList.Count / numCols - 1)
            {
                currentIndex = (currentMatrixItem.Row + 1) * numCols + currentMatrixItem.Col;
                if (matrixItemList[currentIndex].Value[0] == targetColor)
                {
                    searchForAdjacents(ref matrixItemList, matrixItemList[currentIndex], numCols);
                }
            }

            // Check for the item at the left
            if (currentMatrixItem.Col > 0)
            {
                currentIndex = currentMatrixItem.Row * numCols + (currentMatrixItem.Col - 1);
                if (matrixItemList[currentIndex].Value[0] == targetColor)
                {
                    searchForAdjacents(ref matrixItemList, matrixItemList[currentIndex], numCols);
                }
            }

            // Check for the item at the right
            if (currentMatrixItem.Col < numCols - 1)
            {
                currentIndex = currentMatrixItem.Row * numCols + (currentMatrixItem.Col + 1);
                if (matrixItemList[currentIndex].Value[0] == targetColor)
                {
                    searchForAdjacents(ref matrixItemList, matrixItemList[currentIndex], numCols);
                }
            }
        }
        catch (Exception ex)
        {
            string msg = $"Error: {ex.Message} \r\n Row: {currentMatrixItem.Row} \r\n Column: {currentMatrixItem.Col}";
        }
    }

    /// <summary>
    /// Class that represents one item in the Matrix of items to be painted
    /// </summary>
    public class MatrixItem
    {
        public string Value { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public bool IsPainted { get; set; } = false;
    }

    /// <summary>
    /// Generates a List of MatrixItem with the row/column coordinate and a boolean value that represent if the item was painted or not.
    /// </summary>
    /// <param name="numRows">The number of rows</param>
    /// <param name="numCols">The number of columns</param>
    /// <returns>A list of MatrixItem objects</returns>
    public static List<MatrixItem> generateMatrixItems(int numRows, int numCols, List<string> picture)
    {
        List<MatrixItem> result = new List<MatrixItem>();

        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                result.Add(new MatrixItem
                {
                    Row = i,
                    Col = j,
                    Value = picture[i],
                });
            }
        }

        return result;
    }

}
