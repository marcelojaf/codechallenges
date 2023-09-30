
Console.WriteLine("Inform the picture size:");
int pictureCount = Convert.ToInt32(Console.ReadLine().Trim());

List<string> picture = new List<string>();

for (int i = 0; i < pictureCount; i++)
{
    Console.WriteLine("Inform the picture in a row:");
    string pictureItem = Console.ReadLine();
    picture.Add(pictureItem);
}

int result = Result.StrokesRequired(picture);

Console.WriteLine(result);

class Result
{

    /*
     * Complete the 'strokesRequired' function below.
     *
     * The function is expected to return an INTEGER.
     * The function accepts STRING_ARRAY picture as parameter.
     */

    /// <summary>
    /// Calculates the minimum number of strokes required to completely repaint the picture.
    /// </summary>
    /// <param name="picture">The picture represented as a list of strings where each string is a row of colors.</param>
    /// <returns>The minimum number of strokes required.</returns>
    public static int StrokesRequired(List<string> picture)
    {
        if (picture == null || picture.Count == 0)
        {
            return 0;
        }

        int numRows = picture.Count;
        int numCols = picture[0].Length;
        List<MatrixItem> matrix = GenerateMatrix(numRows, numCols, picture);

        int totalStrokes = 0;

        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {
                int currentIndex = row * numCols + col;
                MatrixItem currentMatrixItem = matrix[currentIndex];

                if (!currentMatrixItem.IsPainted)
                {
                    PaintAdjacent(matrix, currentMatrixItem, numCols);
                    totalStrokes++;
                }
            }
        }

        return totalStrokes;
    }

    /// <summary>
    /// Recursively paints adjacent cells with the same color.
    /// </summary>
    /// <param name="matrix">The matrix representing the picture.</param>
    /// <param name="currentMatrixItem">The current matrix item being processed.</param>
    /// <param name="numCols">The number of columns in the matrix.</param>
    private static void PaintAdjacent(List<MatrixItem> matrix, MatrixItem currentMatrixItem, int numCols)
    {
        if (!matrix.Any() || currentMatrixItem == null || currentMatrixItem.Row < 0 || currentMatrixItem.Col < 0 || currentMatrixItem.IsPainted)
        {
            return;
        }

        try
        {
            int currentIndex = currentMatrixItem.Row * numCols + currentMatrixItem.Col;
            matrix[currentIndex].IsPainted = true;

            char targetColor = currentMatrixItem.Value;

            // Check for the upper item
            if (currentMatrixItem.Row > 0)
            {
                currentIndex = (currentMatrixItem.Row - 1) * numCols + currentMatrixItem.Col;
                if (currentIndex >= 0 && matrix[currentIndex].Value == targetColor)
                {
                    PaintAdjacent(matrix, matrix[currentIndex], numCols);
                }
            }

            // Check for the lower item
            if (currentMatrixItem.Row < matrix.Count / numCols - 1)
            {
                currentIndex = (currentMatrixItem.Row + 1) * numCols + currentMatrixItem.Col;
                if (currentIndex >= 0 && matrix[currentIndex].Value == targetColor)
                {
                    PaintAdjacent(matrix, matrix[currentIndex], numCols);
                }
            }

            // Check for the item on the left
            if (currentMatrixItem.Col > 0)
            {
                currentIndex = currentMatrixItem.Row * numCols + (currentMatrixItem.Col - 1);
                if (currentIndex >= 0 && matrix[currentIndex].Value == targetColor)
                {
                    PaintAdjacent(matrix, matrix[currentIndex], numCols);
                }
            }

            // Check for the item on the right
            if (currentMatrixItem.Col < numCols - 1)
            {
                currentIndex = currentMatrixItem.Row * numCols + (currentMatrixItem.Col + 1);
                if (currentIndex >= 0 && matrix[currentIndex].Value == targetColor)
                {
                    PaintAdjacent(matrix, matrix[currentIndex], numCols);
                }
            }
        }
        catch (Exception ex)
        {
            string msg = $"Error: {ex.Message} \r\n Row: {currentMatrixItem.Row} \r\n Column: {currentMatrixItem.Col}";
        }
    }

    /// <summary>
    /// Represents an item in the matrix of colors.
    /// </summary>
    public class MatrixItem
    {
        /// <summary>
        /// Gets or sets the color value of the item.
        /// </summary>
        public char Value { get; set; }

        /// <summary>
        /// Gets or sets the row index of the item.
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// Gets or sets the column index of the item.
        /// </summary>
        public int Col { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item has been painted.
        /// </summary>
        public bool IsPainted { get; set; } = false;
    }

    /// <summary>
    /// Generates a matrix of items representing the picture.
    /// </summary>
    /// <param name="numRows">The number of rows in the picture.</param>
    /// <param name="numCols">The number of columns in the picture.</param>
    /// <param name="picture">The picture represented as a list of strings.</param>
    /// <returns>The matrix of items representing the picture.</returns>
    private static List<MatrixItem> GenerateMatrix(int numRows, int numCols, List<string> picture)
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
                    Value = picture[i][j],
                });
            }
        }

        return result;
    }




}
