using System;

public class SquareMatrix {
  public int[,] matrix;
  private SquareMatrix _savedCopy;

  public int Size {
    get {
      return matrix.GetLength(0);
    }
  }

  public SquareMatrix(int size) {
    if (size <= 0) {
      throw new MatrixSizeException(" Matrix size must be positive ");
    }

    matrix = new int[size, size];

    InputMatrix();
  }

  private SquareMatrix(int size, bool empty) {
    if (size <= 0) {
      throw new MatrixSizeException(" Matrix size must be positive ");
    }

    matrix = new int[size, size];
  }

  public SquareMatrix(SquareMatrix original) {
    if (original == null) {
      throw new ArgumentNullException(nameof(original));
    }

    this.matrix = new int[original.Size, original.Size];

    for (int rowIndex = 0; rowIndex < original.Size; ++rowIndex) {
      for (int columnIndex = 0; columnIndex < original.Size; ++columnIndex) {
        this.matrix[rowIndex, columnIndex] = original.matrix[rowIndex, columnIndex];
      }
    }
  }

  private void InputMatrix() {
    Console.WriteLine(" Enter the matrix elements: ");
    for (int rowIndex = 0; rowIndex < Size; ++rowIndex) {
      for (int columnIndex = 0; columnIndex < Size; ++columnIndex) {
        Console.Write($" Matrix[{rowIndex},{columnIndex}] = ");

        matrix[rowIndex, columnIndex] = int.Parse(Console.ReadLine());
      }
    }
  }

  public override string ToString() {
    string result = "";
    for (int rowIndex = 0; rowIndex < Size; ++rowIndex) {
      for (int columnIndex = 0; columnIndex < Size; ++columnIndex) {
        result += matrix[rowIndex, columnIndex].ToString().PadLeft(4);
      }

      result += "\n";
    }

    return result;
  }

  public double Determinant() {
    if (Size == 1) {
      return matrix[0, 0];
    }

    if (Size == 2) {
      return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];
    }

    double determinant = 0;
    int sign = 1;

    for (int columnIndex = 0; columnIndex < Size; ++columnIndex) {
      int[,] minorArray = new int[Size - 1, Size - 1];

      for (int rowIndex = 1; rowIndex < Size; ++rowIndex) {
        for (int minorColumnIndex = 0; minorColumnIndex < Size; ++minorColumnIndex) {
          if (minorColumnIndex < columnIndex) {
            minorArray[rowIndex - 1, minorColumnIndex] = matrix[rowIndex, minorColumnIndex];
          }

          else if (minorColumnIndex > columnIndex) {
            minorArray[rowIndex - 1, minorColumnIndex - 1] = matrix[rowIndex, minorColumnIndex];
          }
        }
      }

      SquareMatrix minor = new SquareMatrix(Size - 1, true);

      for (int rowIndex = 0; rowIndex < Size - 1; ++rowIndex) {
        for (int columnIndexMinor = 0; columnIndexMinor < Size - 1; ++columnIndexMinor) {
          minor.matrix[rowIndex, columnIndexMinor] = minorArray[rowIndex, columnIndexMinor];
        }
      }

      determinant += sign * matrix[0, columnIndex] * minor.Determinant();
      sign = -sign;
    }

    return determinant;
  }

  private double Minor2x2(int elementA, int elementB, int elementC, int elementD) {
    return elementA * elementD - elementB * elementC;
  }

  public SquareMatrix Clone() {
    return new SquareMatrix(this);
  }

  public static SquareMatrix operator +(SquareMatrix firstMatrix, SquareMatrix secondMatrix) {
    if (firstMatrix == null || secondMatrix == null) {
      throw new ArgumentNullException(" Operands cannot be null ");
    }

    if (firstMatrix.Size != secondMatrix.Size) {
      throw new MatrixSizeException(" Matrices must have the same size for addition ");
    }

    SquareMatrix resultMatrix = new SquareMatrix(firstMatrix.Size, true);

    for (int rowIndex = 0; rowIndex < firstMatrix.Size; ++rowIndex) {
      for (int columnIndex = 0; columnIndex < firstMatrix.Size; ++columnIndex) {
        resultMatrix.matrix[rowIndex, columnIndex] = firstMatrix.matrix[rowIndex, columnIndex] + secondMatrix.matrix[rowIndex, columnIndex];
      }
    }

    return resultMatrix;
  }

  public static SquareMatrix operator *(SquareMatrix firstMatrix, SquareMatrix secondMatrix) {
    if (firstMatrix == null || secondMatrix == null) {
      throw new ArgumentNullException(" Operands cannot be null ");
    }

    if (firstMatrix.Size != secondMatrix.Size) {
      throw new MatrixSizeException(" Matrices must have the same size for multiplication ");
    }

    SquareMatrix resultMatrix = new SquareMatrix(firstMatrix.Size, true);

    for (int rowIndex = 0; rowIndex < firstMatrix.Size; ++rowIndex) {
      for (int columnIndex = 0; columnIndex < firstMatrix.Size; ++columnIndex) {
        int sum = 0;
        for (int multiplicationIndex = 0; multiplicationIndex < firstMatrix.Size; ++multiplicationIndex) {
          sum += firstMatrix.matrix[rowIndex, multiplicationIndex] * secondMatrix.matrix[multiplicationIndex, columnIndex];
        }
        resultMatrix.matrix[rowIndex, columnIndex] = sum;
      }
    }

    return resultMatrix;
  }

  public override bool Equals(object obj) {
    if (obj == null || !(obj is SquareMatrix)) {
      return false;
    }

    SquareMatrix otherMatrix = (SquareMatrix)obj;

    if (this.Size != otherMatrix.Size) { return false; }

    for (int rowIndex = 0; rowIndex < Size; ++rowIndex) {
      for (int columnIndex = 0; columnIndex < Size; ++columnIndex) {
        if (this.matrix[rowIndex, columnIndex] != otherMatrix.matrix[rowIndex, columnIndex]) {
          return false;
        }
      }
    }
    return true;
  }

  public override int GetHashCode() {
    int hashCode = 0;
    for (int rowIndex = 0; rowIndex < Size; ++rowIndex) {
      for (int columnIndex = 0; columnIndex < Size; ++columnIndex) {
        hashCode += matrix[rowIndex, columnIndex].GetHashCode();
      }
    }

    return hashCode;
  }

  public int CompareTo(SquareMatrix otherMatrix) {
    if (otherMatrix == null) {
      return 1;
    }

    int sumThisMatrix = 0;
    int sumOtherMatrix = 0;

    for (int rowIndex = 0; rowIndex < Size; ++rowIndex) {
      for (int columnIndex = 0; columnIndex < Size; ++columnIndex) {
        sumThisMatrix += this.matrix[rowIndex, columnIndex];
        sumOtherMatrix += otherMatrix.matrix[rowIndex, columnIndex];
      }
    }

    return sumThisMatrix.CompareTo(sumOtherMatrix);
  }

  public static bool operator ==(SquareMatrix firstMatrix, SquareMatrix secondMatrix) {
    if (ReferenceEquals(firstMatrix, null) && ReferenceEquals(secondMatrix, null)) {
      return true;
    }

    if (ReferenceEquals(firstMatrix, null) || ReferenceEquals(secondMatrix, null)) {
      return false;
    }

    return firstMatrix.Equals(secondMatrix);
  }

  public static bool operator !=(SquareMatrix firstMatrix, SquareMatrix secondMatrix) {
    return !(firstMatrix == secondMatrix);
  }

  public static bool operator >(SquareMatrix firstMatrix, SquareMatrix secondMatrix) {
    if (firstMatrix == null || secondMatrix == null) {
      return false;
    }

    return firstMatrix.CompareTo(secondMatrix) > 0;
  }

  public static bool operator <(SquareMatrix firstMatrix, SquareMatrix secondMatrix) {
    if (firstMatrix == null || secondMatrix == null) {
      return false;
    }

    return firstMatrix.CompareTo(secondMatrix) < 0;
  }

  public static bool operator >=(SquareMatrix firstMatrix, SquareMatrix secondMatrix) {
    if (firstMatrix == null || secondMatrix == null) {
      return false;
    }

    return firstMatrix.CompareTo(secondMatrix) >= 0;
  }

  public static bool operator <=(SquareMatrix firstMatrix, SquareMatrix secondMatrix) {
    if (firstMatrix == null || secondMatrix == null) {
      return false;
    }

    return firstMatrix.CompareTo(secondMatrix) <= 0;
  }

  public static bool operator true(SquareMatrix matrix) {
    if (matrix == null) {
      return false;
    }

    for (int rowIndex = 0; rowIndex < matrix.Size; ++rowIndex) {
      for (int columnIndex = 0; columnIndex < matrix.Size; ++columnIndex) {
        if (matrix.matrix[rowIndex, columnIndex] != 0) {
          return true;
        }
      }
    }

    return false;
  }

  public static bool operator false(SquareMatrix matrix) {
    if (matrix == null) {
      return true;
    }

    for (int rowIndex = 0; rowIndex < matrix.Size; ++rowIndex) {
      for (int columnIndex = 0; columnIndex < matrix.Size; ++columnIndex) {
        if (matrix.matrix[rowIndex, columnIndex] != 0) {
          return false;
        }
      }
    }
    return true;
  }

  public static explicit operator double(SquareMatrix matrix) {
    if (matrix == null) {
      throw new ArgumentNullException(nameof(matrix));
    }

    return matrix.Determinant();
  }

  public static SquareMatrix operator ~(SquareMatrix matrix) {
    if (matrix == null) {
      throw new ArgumentNullException(nameof(matrix));
    }

    double determinant = matrix.Determinant();

    if (determinant == 0) {
      throw new SingularMatrixException(" Cannot find inverse matrix for singular matrix ");
    }

    if (matrix.Size == 2) {
      SquareMatrix resultMatrix = new SquareMatrix(2, true);

      resultMatrix.matrix[0, 0] = matrix.matrix[1, 1];
      resultMatrix.matrix[0, 1] = -matrix.matrix[0, 1];
      resultMatrix.matrix[1, 0] = -matrix.matrix[1, 0];
      resultMatrix.matrix[1, 1] = matrix.matrix[0, 0];

      return resultMatrix;
    }
    else if (matrix.Size == 3) {
      double[,] minors = new double[3, 3];

      for (int rowIndex = 0; rowIndex < 3; ++rowIndex) {
        for (int columnIndex = 0; columnIndex < 3; ++columnIndex) {
          int[,] minorElements = new int[2, 2];

          int minorRowIndex = 0;

          for (int sourceRowIndex = 0; sourceRowIndex < 3; ++sourceRowIndex) {
            if (sourceRowIndex == rowIndex) {
              continue;
            }

            int minorColumnIndex = 0;

            for (int sourceColumnIndex = 0; sourceColumnIndex < 3; ++sourceColumnIndex) {
              if (sourceColumnIndex == columnIndex) {
                continue;
              }

              minorElements[minorRowIndex, minorColumnIndex] = matrix.matrix[sourceRowIndex, sourceColumnIndex];

              ++minorColumnIndex;
            }

            ++minorRowIndex;
          }

          minors[rowIndex, columnIndex] = matrix.Minor2x2(
            minorElements[0, 0], minorElements[0, 1],
            minorElements[1, 0], minorElements[1, 1]
          );
        }
      }

      SquareMatrix cofactorMatrix = new SquareMatrix(3, true);

      for (int rowIndex = 0; rowIndex < 3; ++rowIndex) {
        for (int columnIndex = 0; columnIndex < 3; ++columnIndex) {
          int sign = ((rowIndex + columnIndex) % 2 == 0) ? 1 : -1;

          cofactorMatrix.matrix[rowIndex, columnIndex] = sign * (int)minors[rowIndex, columnIndex];
        }
      }

      SquareMatrix adjugateMatrix = new SquareMatrix(matrix.Size, true);

      for (int rowIndex = 0; rowIndex < matrix.Size; ++rowIndex) {
        for (int columnIndex = 0; columnIndex < matrix.Size; ++columnIndex) {
          adjugateMatrix.matrix[rowIndex, columnIndex] = cofactorMatrix.matrix[columnIndex, rowIndex];
        }
      }

      SquareMatrix resultMatrix = new SquareMatrix(matrix.Size, true);

      for (int rowIndex = 0; rowIndex < matrix.Size; ++rowIndex) {
        for (int columnIndex = 0; columnIndex < matrix.Size; ++columnIndex) {
          resultMatrix.matrix[rowIndex, columnIndex] = (int)(adjugateMatrix.matrix[rowIndex, columnIndex] / determinant);
        }
      }

      return resultMatrix;
    }
    else {
      throw new NotSupportedMatrixOperationException($" Inverse matrix for size {matrix.Size}x{matrix.Size} is not implemented ");
    }
  }

  public void MultiplyByNumber(int number) {
    for (int rowIndex = 0; rowIndex < Size; ++rowIndex) {
      for (int columnIndex = 0; columnIndex < Size; ++columnIndex) {
        this.matrix[rowIndex, columnIndex] *= number;
      }
    }
  }

  public void SaveState() {
    _savedCopy = new SquareMatrix(this);

    Console.WriteLine(" State saved! ");
  }

  public void RestoreState() {
    if (_savedCopy == null) {
      Console.WriteLine(" No saved state! ");
      return;
    }

    this.matrix = new int[_savedCopy.Size, _savedCopy.Size];

    for (int rowIndex = 0; rowIndex < _savedCopy.Size; ++rowIndex) {
      for (int columnIndex = 0; columnIndex < _savedCopy.Size; ++columnIndex) {
        this.matrix[rowIndex, columnIndex] = _savedCopy.matrix[rowIndex, columnIndex];
      }
    }

    Console.WriteLine(" State restored! ");
  }
}

public class MatrixSizeException : Exception {
  public MatrixSizeException() : base(" Error: invalid matrix size ") { }
  public MatrixSizeException(string message) : base(message) { }
}

public class SingularMatrixException : Exception {
  public SingularMatrixException() : base(" Error: matrix is singular ") { }
  public SingularMatrixException(string message) : base(message) { }
}

public class NotSupportedMatrixOperationException : Exception {
  public NotSupportedMatrixOperationException() : base(" Operation is not supported for this type of matrix ") { }
  public NotSupportedMatrixOperationException(string message) : base(message) { }
}

class Program {
  static void Main(string[] args) {
    SquareMatrix matrixA = null;
    SquareMatrix matrixB = null;
    bool exitProgram = false;

    while (!exitProgram) {
      Console.WriteLine(" \n========== MATRIX CALCULATOR ========== ");
      Console.WriteLine(" 1. Create matrix A ");
      Console.WriteLine(" 2. Create matrix B ");
      Console.WriteLine(" 3. A + B ");
      Console.WriteLine(" 4. A * B ");
      Console.WriteLine(" 5. A > B ");
      Console.WriteLine(" 6. A < B ");
      Console.WriteLine(" 7. A >= B ");
      Console.WriteLine(" 8. A <= B ");
      Console.WriteLine(" 9. A == B ");
      Console.WriteLine(" 10. A != B ");
      Console.WriteLine(" 11. Determinant of A ");
      Console.WriteLine(" 12. Determinant of B ");
      Console.WriteLine(" 13. Inverse matrix of A ");
      Console.WriteLine(" 14. Inverse matrix of B ");
      Console.WriteLine(" 15. Show all matrices ");
      Console.WriteLine(" 16. Restore state of A ");
      Console.WriteLine(" 17. Restore state of B ");
      Console.WriteLine(" 18. Multiply A by number ");
      Console.WriteLine(" 19. Multiply B by number ");
      Console.WriteLine(" 0. Exit ");
      Console.Write(" Choose action: ");

      string userChoice = Console.ReadLine();

      try {
        switch (userChoice) {
          case "1":
            Console.Write(" Enter size of matrix A: ");

            int sizeA = int.Parse(Console.ReadLine());

            matrixA = new SquareMatrix(sizeA);

            break;

          case "2":
            Console.Write(" Enter size of matrix B: ");

            int sizeB = int.Parse(Console.ReadLine());

            matrixB = new SquareMatrix(sizeB);

            break;

          case "3":
            if (matrixA == null || matrixB == null) {
              Console.WriteLine(" Create both matrices first! ");
            }
            else {
              matrixA = matrixA + matrixB;

              Console.WriteLine(" A + B = \n " + matrixA);
            }

            break;

          case "4":
            if (matrixA == null || matrixB == null) {
              Console.WriteLine(" Create both matrices first! ");
            }
            else {
              matrixA = matrixA * matrixB;

              Console.WriteLine(" A * B = \n " + matrixA);
            }

            break;

          case "5":
            if (matrixA == null || matrixB == null) {
              Console.WriteLine(" Create both matrices first! ");
            }
            else {
              Console.WriteLine($" A > B: {matrixA > matrixB} ");
            }

            break;

          case "6":
            if (matrixA == null || matrixB == null) {
              Console.WriteLine(" Create both matrices first! ");
            }
            else {
              Console.WriteLine($" A < B: {matrixA < matrixB} ");
            }

            break;

          case "7":
            if (matrixA == null || matrixB == null) {
              Console.WriteLine(" Create both matrices first! ");
            }
            else {
              Console.WriteLine($" A >= B: {matrixA >= matrixB} ");
            }

            break;

          case "8":
            if (matrixA == null || matrixB == null) {
              Console.WriteLine(" Create both matrices first! ");
            }
            else {
              Console.WriteLine($" A <= B: {matrixA <= matrixB} ");
            }

            break;

          case "9":
            if (matrixA == null || matrixB == null) {
              Console.WriteLine(" Create both matrices first! ");
            }
            else {
              Console.WriteLine($" A == B: {matrixA == matrixB} ");
            }

            break;

          case "10":
            if (matrixA == null || matrixB == null) {
              Console.WriteLine(" Create both matrices first! ");
            }
            else {
              Console.WriteLine($" A != B: {matrixA != matrixB} ");
            }

            break;

          case "11":
            if (matrixA == null) {
              Console.WriteLine(" Create matrix A first! ");
            }
            else {
              Console.WriteLine($" Determinant of A = {(double)matrixA} ");
            }

            break;

          case "12":
            if (matrixB == null) {
              Console.WriteLine(" Create matrix B first! ");
            }
            else {
              Console.WriteLine($" Determinant of B = {(double)matrixB} ");
            }

            break;

          case "13":
            if (matrixA == null) {
              Console.WriteLine(" Create matrix A first! ");
            }
            else {
              matrixA = ~matrixA;

              Console.WriteLine(" Inverse matrix of A:\n " + matrixA);
            }

            break;

          case "14":
            if (matrixB == null) {
              Console.WriteLine(" Create matrix B first! ");
            }
            else {
              matrixB = ~matrixB;

              Console.WriteLine(" Inverse matrix of B:\n" + matrixB);
            }

            break;

          case "15":
            if (matrixA == null || matrixB == null) {
              Console.WriteLine(" Create both matrices first! ");
            }
            else {
              Console.WriteLine(" Matrix A:\n" + matrixA);
              Console.WriteLine(" Matrix B:\n" + matrixB);
            }

            break;

          case "16":
            if (matrixA == null) {
              Console.WriteLine(" Create matrix A first! ");
            }
            else {
              matrixA.RestoreState();

              Console.WriteLine(" A after restore:\n" + matrixA);
            }

            break;

          case "17":
            if (matrixB == null) {
              Console.WriteLine(" Create matrix B first! ");
            }
            else {
              matrixB.RestoreState();

              Console.WriteLine(" B after restore:\n" + matrixB);
            }

            break;

          case "18":
            if (matrixA == null) {
              Console.WriteLine(" Create matrix A first! ");
            }
            else {
              Console.Write(" Enter multiplier for A: ");
              int multiplierA = int.Parse(Console.ReadLine());

              matrixA.SaveState();
              Console.WriteLine(" Original A saved as prototype ");
              Console.WriteLine(" Before: \n" + matrixA);

              matrixA.MultiplyByNumber(multiplierA);
              Console.WriteLine($" After multiplying by {multiplierA}: \n" + matrixA);
              Console.WriteLine(" Choose 16 to restore original A ");
            }
            break;

          case "19":
            if (matrixB == null) {
              Console.WriteLine(" Create matrix B first! ");
            }
            else {
              Console.Write(" Enter multiplier for B: ");
              int multiplierB = int.Parse(Console.ReadLine());

              matrixB.SaveState();
              Console.WriteLine(" Original B saved as prototype ");
              Console.WriteLine(" Before: \n" + matrixB);

              matrixB.MultiplyByNumber(multiplierB);
              Console.WriteLine($" After multiplying by {multiplierB}: \n" + matrixB);
              Console.WriteLine(" Choose 17 to restore original B ");
            }
            break;

          case "0":
            exitProgram = true;

            break;

          default:
            Console.WriteLine(" Invalid choice! ");

            break;
        }
      }
      catch (Exception error) {
        Console.WriteLine($" Error: {error.Message} ");
      }
    }
  }
}