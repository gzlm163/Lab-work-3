using System;

public class SquareMatrix {
  public int[,] matrix;

  public int Size {
    get { return matrix.GetLength(0); }
  }

  public SquareMatrix(int size) {
    if (size <= 0)
      throw new MatrixSizeException("Matrix size must be positive");

    matrix = new int[size, size];
    InputMatrix();
  }

  private SquareMatrix(int size, bool empty) {
    if (size <= 0)
      throw new MatrixSizeException("Matrix size must be positive");
    matrix = new int[size, size];
  }

  public SquareMatrix(SquareMatrix original) {
    if (original == null)
      throw new ArgumentNullException(nameof(original));

    this.matrix = new int[original.Size, original.Size];
    for (int rowIndex = 0; rowIndex < original.Size; ++rowIndex) {
      for (int columnIndex = 0; columnIndex < original.Size; ++columnIndex) {
        this.matrix[rowIndex, columnIndex] = original.matrix[rowIndex, columnIndex];
      }
    }
  }

  private void InputMatrix() {
    Console.WriteLine("Enter the matrix elements: ");
    for (int rowIndex = 0; rowIndex < Size; ++rowIndex) {
      for (int columnIndex = 0; columnIndex < Size; ++columnIndex) {
        Console.Write($"Matrix[{rowIndex},{columnIndex}] = ");
        matrix[rowIndex, columnIndex] = int.Parse(Console.ReadLine());
      }
    }
  }

  public override string ToString() {
    string result = "";
    for (int rowIndex = 0; rowIndex < Size; ++rowIndex) {
      for (int columnIndex = 0; columnIndex < Size; ++columnIndex) {
        result += matrix[rowIndex, columnIndex] + "\t";
      }
      result += "\n";
    }
    return result;
  }

  public double Determinant() {
    if (Size == 1) return matrix[0, 0];
    if (Size == 2) return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];

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
    if (firstMatrix == null || secondMatrix == null)
      throw new ArgumentNullException("Operands cannot be null");

    if (firstMatrix.Size != secondMatrix.Size)
      throw new MatrixSizeException("Matrices must have the same size for addition");

    SquareMatrix resultMatrix = new SquareMatrix(firstMatrix.Size, true);

    for (int rowIndex = 0; rowIndex < firstMatrix.Size; ++rowIndex) {
      for (int columnIndex = 0; columnIndex < firstMatrix.Size; ++columnIndex) {
        resultMatrix.matrix[rowIndex, columnIndex] = firstMatrix.matrix[rowIndex, columnIndex] + secondMatrix.matrix[rowIndex, columnIndex];
      }
    }

    return resultMatrix;
  }

  public static SquareMatrix operator *(SquareMatrix firstMatrix, SquareMatrix secondMatrix) {
    if (firstMatrix == null || secondMatrix == null)
      throw new ArgumentNullException("Operands cannot be null");

    if (firstMatrix.Size != secondMatrix.Size)
      throw new MatrixSizeException("Matrices must have the same size for multiplication");

    SquareMatrix resultMatrix = new SquareMatrix(firstMatrix.Size, true);

    for (int rowIndex = 0; rowIndex < firstMatrix.Size; ++rowIndex) {
      for (int columnIndex = 0; columnIndex < firstMatrix.Size; ++columnIndex) {
        int sum = 0;
        for (int kIndex = 0; kIndex < firstMatrix.Size; ++kIndex) {
          sum += firstMatrix.matrix[rowIndex, kIndex] * secondMatrix.matrix[kIndex, columnIndex];
        }
        resultMatrix.matrix[rowIndex, columnIndex] = sum;
      }
    }

    return resultMatrix;
  }

  public override bool Equals(object obj) {
    if (obj == null || !(obj is SquareMatrix)) return false;
    SquareMatrix otherMatrix = (SquareMatrix)obj;

    if (this.Size != otherMatrix.Size) return false;

    for (int rowIndex = 0; rowIndex < Size; ++rowIndex) {
      for (int columnIndex = 0; columnIndex < Size; ++columnIndex) {
        if (this.matrix[rowIndex, columnIndex] != otherMatrix.matrix[rowIndex, columnIndex]) return false;
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
    if (otherMatrix == null) return 1;

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
    if (ReferenceEquals(firstMatrix, null) && ReferenceEquals(secondMatrix, null)) return true;
    if (ReferenceEquals(firstMatrix, null) || ReferenceEquals(secondMatrix, null)) return false;
    return firstMatrix.Equals(secondMatrix);
  }

  public static bool operator !=(SquareMatrix firstMatrix, SquareMatrix secondMatrix) {
    return !(firstMatrix == secondMatrix);
  }

  public static bool operator >(SquareMatrix firstMatrix, SquareMatrix secondMatrix) {
    if (firstMatrix == null || secondMatrix == null) return false;
    return firstMatrix.CompareTo(secondMatrix) > 0;
  }

  public static bool operator <(SquareMatrix firstMatrix, SquareMatrix secondMatrix) {
    if (firstMatrix == null || secondMatrix == null) return false;
    return firstMatrix.CompareTo(secondMatrix) < 0;
  }

  public static bool operator >=(SquareMatrix firstMatrix, SquareMatrix secondMatrix) {
    if (firstMatrix == null || secondMatrix == null) return false;
    return firstMatrix.CompareTo(secondMatrix) >= 0;
  }

  public static bool operator <=(SquareMatrix firstMatrix, SquareMatrix secondMatrix) {
    if (firstMatrix == null || secondMatrix == null) return false;
    return firstMatrix.CompareTo(secondMatrix) <= 0;
  }

  public static bool operator true(SquareMatrix matrix) {
    if (matrix == null) return false;

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
    if (matrix == null) return true;

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
    if (matrix == null)
      throw new ArgumentNullException(nameof(matrix));

    return matrix.Determinant();
  }

  public static SquareMatrix operator ~(SquareMatrix matrix) {
    if (matrix == null)
      throw new ArgumentNullException(nameof(matrix));

    double determinant = matrix.Determinant();

    if (determinant == 0)
      throw new SingularMatrixException("Cannot find inverse matrix for singular matrix");

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

      minors[0, 0] = matrix.Minor2x2(matrix.matrix[1, 1], matrix.matrix[1, 2], matrix.matrix[2, 1], matrix.matrix[2, 2]);
      minors[0, 1] = matrix.Minor2x2(matrix.matrix[1, 0], matrix.matrix[1, 2], matrix.matrix[2, 0], matrix.matrix[2, 2]);
      minors[0, 2] = matrix.Minor2x2(matrix.matrix[1, 0], matrix.matrix[1, 1], matrix.matrix[2, 0], matrix.matrix[2, 1]);

      minors[1, 0] = matrix.Minor2x2(matrix.matrix[0, 1], matrix.matrix[0, 2], matrix.matrix[2, 1], matrix.matrix[2, 2]);
      minors[1, 1] = matrix.Minor2x2(matrix.matrix[0, 0], matrix.matrix[0, 2], matrix.matrix[2, 0], matrix.matrix[2, 2]);
      minors[1, 2] = matrix.Minor2x2(matrix.matrix[0, 0], matrix.matrix[0, 1], matrix.matrix[2, 0], matrix.matrix[2, 1]);

      minors[2, 0] = matrix.Minor2x2(matrix.matrix[0, 1], matrix.matrix[0, 2], matrix.matrix[1, 1], matrix.matrix[1, 2]);
      minors[2, 1] = matrix.Minor2x2(matrix.matrix[0, 0], matrix.matrix[0, 2], matrix.matrix[1, 0], matrix.matrix[1, 2]);
      minors[2, 2] = matrix.Minor2x2(matrix.matrix[0, 0], matrix.matrix[0, 1], matrix.matrix[1, 0], matrix.matrix[1, 1]);

      SquareMatrix cofactorMatrix = new SquareMatrix(3, true);
      cofactorMatrix.matrix[0, 0] = (int)minors[0, 0];
      cofactorMatrix.matrix[0, 1] = -(int)minors[0, 1];
      cofactorMatrix.matrix[0, 2] = (int)minors[0, 2];
      cofactorMatrix.matrix[1, 0] = -(int)minors[1, 0];
      cofactorMatrix.matrix[1, 1] = (int)minors[1, 1];
      cofactorMatrix.matrix[1, 2] = -(int)minors[1, 2];
      cofactorMatrix.matrix[2, 0] = (int)minors[2, 0];
      cofactorMatrix.matrix[2, 1] = -(int)minors[2, 1];
      cofactorMatrix.matrix[2, 2] = (int)minors[2, 2];

      SquareMatrix adjugateMatrix = new SquareMatrix(3, true);
      for (int rowIndex = 0; rowIndex < 3; ++rowIndex)
        for (int columnIndex = 0; columnIndex < 3; ++columnIndex)
          adjugateMatrix.matrix[rowIndex, columnIndex] = cofactorMatrix.matrix[columnIndex, rowIndex];

      SquareMatrix resultMatrix = new SquareMatrix(3, true);
      for (int rowIndex = 0; rowIndex < 3; ++rowIndex)
        for (int columnIndex = 0; columnIndex < 3; ++columnIndex)
          resultMatrix.matrix[rowIndex, columnIndex] = (int)(adjugateMatrix.matrix[rowIndex, columnIndex] / determinant);

      return resultMatrix;
    }
    else {
      throw new NotSupportedMatrixOperationException($"Inverse matrix for size {matrix.Size}x{matrix.Size} is not implemented");
    }
  }
}

public class MatrixSizeException : Exception {
  public MatrixSizeException() : base("Error: invalid matrix size") { }
  public MatrixSizeException(string message) : base(message) { }
}

public class SingularMatrixException : Exception {
  public SingularMatrixException() : base("Error: matrix is singular (determinant = 0)") { }
  public SingularMatrixException(string message) : base(message) { }
}

public class NotSupportedMatrixOperationException : Exception {
  public NotSupportedMatrixOperationException() : base("Operation is not supported for this type of matrix") { }
  public NotSupportedMatrixOperationException(string message) : base(message) { }
}

class Program {
  static void Main(string[] args) {
    SquareMatrix matrixA = null;
    SquareMatrix matrixB = null;
    bool exitProgram = false;

    while (!exitProgram) {
      Console.WriteLine("\n========== MATRIX CALCULATOR ==========");
      Console.WriteLine("1. Create matrix A");
      Console.WriteLine("2. Create matrix B");
      Console.WriteLine("3. A + B");
      Console.WriteLine("4. A * B");
      Console.WriteLine("5. A > B");
      Console.WriteLine("6. A < B");
      Console.WriteLine("7. A >= B");
      Console.WriteLine("8. A <= B");
      Console.WriteLine("9. A == B");
      Console.WriteLine("10. A != B");
      Console.WriteLine("11. Determinant of A");
      Console.WriteLine("12. Determinant of B");
      Console.WriteLine("13. Inverse matrix of A");
      Console.WriteLine("14. Inverse matrix of B");
      Console.WriteLine("15. Create copy of A");
      Console.WriteLine("16. Create copy of B");
      Console.WriteLine("17. Show all matrices");
      Console.WriteLine("0. Exit");
      Console.Write("Choose action: ");

      string userChoice = Console.ReadLine();

      try {
        switch (userChoice) {
          case "1":
            Console.Write("Enter size of matrix A: ");
            int sizeA = int.Parse(Console.ReadLine());
            matrixA = new SquareMatrix(sizeA);
            break;

          case "2":
            Console.Write("Enter size of matrix B: ");
            int sizeB = int.Parse(Console.ReadLine());
            matrixB = new SquareMatrix(sizeB);
            break;

          case "3":
            if (matrixA == null || matrixB == null)
              Console.WriteLine("Create both matrices first!");
            else
              Console.WriteLine("A + B = \n" + (matrixA + matrixB));
            break;

          case "4":
            if (matrixA == null || matrixB == null)
              Console.WriteLine("Create both matrices first!");
            else
              Console.WriteLine("A * B = \n" + (matrixA * matrixB));
            break;

          case "5":
            if (matrixA == null || matrixB == null)
              Console.WriteLine("Create both matrices first!");
            else
              Console.WriteLine($"A > B: {matrixA > matrixB}");
            break;

          case "6":
            if (matrixA == null || matrixB == null)
              Console.WriteLine("Create both matrices first!");
            else
              Console.WriteLine($"A < B: {matrixA < matrixB}");
            break;

          case "7":
            if (matrixA == null || matrixB == null)
              Console.WriteLine("Create both matrices first!");
            else
              Console.WriteLine($"A >= B: {matrixA >= matrixB}");
            break;

          case "8":
            if (matrixA == null || matrixB == null)
              Console.WriteLine("Create both matrices first!");
            else
              Console.WriteLine($"A <= B: {matrixA <= matrixB}");
            break;

          case "9":
            if (matrixA == null || matrixB == null)
              Console.WriteLine("Create both matrices first!");
            else
              Console.WriteLine($"A == B: {matrixA == matrixB}");
            break;

          case "10":
            if (matrixA == null || matrixB == null)
              Console.WriteLine("Create both matrices first!");
            else
              Console.WriteLine($"A != B: {matrixA != matrixB}");
            break;

          case "11":
            if (matrixA == null)
              Console.WriteLine("Create matrix A first!");
            else
              Console.WriteLine($"Determinant of A = {(double)matrixA}");
            break;

          case "12":
            if (matrixB == null)
              Console.WriteLine("Create matrix B first!");
            else
              Console.WriteLine($"Determinant of B = {(double)matrixB}");
            break;

          case "13":
            if (matrixA == null)
              Console.WriteLine("Create matrix A first!");
            else
              Console.WriteLine("Inverse matrix of A:\n" + (~matrixA));
            break;

          case "14":
            if (matrixB == null)
              Console.WriteLine("Create matrix B first!");
            else
              Console.WriteLine("Inverse matrix of B:\n" + (~matrixB));
            break;

          case "15":
            if (matrixA == null)
              Console.WriteLine("Create matrix A first!");
            else {
              SquareMatrix copyOfA = matrixA.Clone();
              Console.WriteLine("Copy of A:\n" + copyOfA);
            }
            break;

          case "16":
            if (matrixB == null)
              Console.WriteLine("Create matrix B first!");
            else {
              SquareMatrix copyOfB = matrixB.Clone();
              Console.WriteLine("Copy of B:\n" + copyOfB);
            }
            break;

          case "17":
            if (matrixA == null || matrixB == null)
              Console.WriteLine("Create both matrices first!");
            else {
              Console.WriteLine("Matrix A:\n" + matrixA);
              Console.WriteLine("Matrix B:\n" + matrixB);
            }
            break;

          case "0":
            exitProgram = true;
            break;

          default:
            Console.WriteLine("Invalid choice!");
            break;
        }
      }
      catch (Exception error) {
        Console.WriteLine($"Error: {error.Message}");
      }
    }
  }
}
