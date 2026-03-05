using System;

public class SquareMatrix {
  public int[,] matrix;

  public int Size {
    get { return matrix.GetLength(0); }
  }

  public SquareMatrix(int n) {
    if (n <= 0)
      throw new MatrixSizeException("Размер матрицы должен быть положительным числом");

    matrix = new int[n, n];
    InputMatrix();
  }

  private SquareMatrix(int n, bool empty) {
    if (n <= 0)
      throw new MatrixSizeException("Размер матрицы должен быть положительным числом");
    matrix = new int[n, n];
  }

  public SquareMatrix(SquareMatrix original) {
    if (original == null)
      throw new ArgumentNullException(nameof(original));

    this.matrix = new int[original.Size, original.Size];
    for (int i = 0; i < original.Size; i++) {
      for (int j = 0; j < original.Size; j++) {
        this.matrix[i, j] = original.matrix[i, j];
      }
    }
  }

  private void InputMatrix() {
    Console.WriteLine($" Enter the matrix elements: ");
    for (int i = 0; i < Size; i++) {
      for (int j = 0; j < Size; j++) {
        Console.Write($" Matrix: ");
        matrix[i, j] = int.Parse(Console.ReadLine());
      }
    }
  }

  public override string ToString() {
    string result = "";
    for (int i = 0; i < Size; i++) {
      for (int j = 0; j < Size; j++) {
        result += matrix[i, j] + "\t";
      }
      result += "\n";
    }
    return result;
  }

  public double Determinant() {
    if (Size == 1) return matrix[0, 0];
    if (Size == 2) return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];

    double det = 0;
    int sign = 1;

    for (int j = 0; j < Size; j++) {
      int[,] minorArray = new int[Size - 1, Size - 1];

      for (int i = 1; i < Size; i++) {
        for (int k = 0; k < Size; k++) {
          if (k < j) {
            minorArray[i - 1, k] = matrix[i, k];
          }
          else if (k > j) {
            minorArray[i - 1, k - 1] = matrix[i, k];
          }
        }
      }

      SquareMatrix minor = new SquareMatrix(Size - 1, true);
      for (int i = 0; i < Size - 1; i++) {
        for (int k = 0; k < Size - 1; k++) {
          minor.matrix[i, k] = minorArray[i, k];
        }
      }

      det += sign * matrix[0, j] * minor.Determinant();
      sign = -sign;
    }

    return det;
  }

  private double Minor2x2(int a, int b, int c, int d) {
    return a * d - b * c;
  }

  public SquareMatrix Clone() {
    return new SquareMatrix(this);
  }

  public static SquareMatrix operator +(SquareMatrix a, SquareMatrix b) {
    if (a == null || b == null)
      throw new ArgumentNullException("Операнды не могут быть null");

    if (a.Size != b.Size)
      throw new MatrixSizeException("Матрицы должны быть одинакового размера для сложения");

    SquareMatrix result = new SquareMatrix(a.Size, true);

    for (int i = 0; i < a.Size; i++) {
      for (int j = 0; j < a.Size; j++) {
        result.matrix[i, j] = a.matrix[i, j] + b.matrix[i, j];
      }
    }

    return result;
  }

  public static SquareMatrix operator *(SquareMatrix a, SquareMatrix b) {
    if (a == null || b == null)
      throw new ArgumentNullException("Операнды не могут быть null");

    if (a.Size != b.Size)
      throw new MatrixSizeException("Матрицы должны быть одинакового размера для умножения");

    SquareMatrix result = new SquareMatrix(a.Size, true);

    for (int i = 0; i < a.Size; i++) {
      for (int j = 0; j < a.Size; j++) {
        int sum = 0;
        for (int k = 0; k < a.Size; k++) {
          sum += a.matrix[i, k] * b.matrix[k, j];
        }
        result.matrix[i, j] = sum;
      }
    }

    return result;
  }

  public override bool Equals(object obj) {
    if (obj == null || !(obj is SquareMatrix)) return false;
    SquareMatrix other = (SquareMatrix)obj;

    if (this.Size != other.Size) return false;

    for (int i = 0; i < Size; i++) {
      for (int j = 0; j < Size; j++) {
        if (this.matrix[i, j] != other.matrix[i, j]) return false;
      }
    }
    return true;
  }

  public override int GetHashCode() {
    int hash = 0;
    for (int i = 0; i < Size; i++) {
      for (int j = 0; j < Size; j++) {
        hash += matrix[i, j].GetHashCode();
      }
    }
    return hash;
  }

  public int CompareTo(SquareMatrix other) {
    if (other == null) return 1;

    int sumThis = 0;
    int sumOther = 0;

    for (int i = 0; i < Size; i++) {
      for (int j = 0; j < Size; j++) {
        sumThis += this.matrix[i, j];
        sumOther += other.matrix[i, j];
      }
    }

    return sumThis.CompareTo(sumOther);
  }

  public static bool operator ==(SquareMatrix a, SquareMatrix b) {
    if (ReferenceEquals(a, null) && ReferenceEquals(b, null)) return true;
    if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
    return a.Equals(b);
  }

  public static bool operator !=(SquareMatrix a, SquareMatrix b) {
    return !(a == b);
  }

  public static bool operator >(SquareMatrix a, SquareMatrix b) {
    if (a == null || b == null) return false;
    return a.CompareTo(b) > 0;
  }

  public static bool operator <(SquareMatrix a, SquareMatrix b) {
    if (a == null || b == null) return false;
    return a.CompareTo(b) < 0;
  }

  public static bool operator >=(SquareMatrix a, SquareMatrix b) {
    if (a == null || b == null) return false;
    return a.CompareTo(b) >= 0;
  }

  public static bool operator <=(SquareMatrix a, SquareMatrix b) {
    if (a == null || b == null) return false;
    return a.CompareTo(b) <= 0;
  }

  public static bool operator true(SquareMatrix m) {
    if (m == null) return false;

    for (int i = 0; i < m.Size; i++) {
      for (int j = 0; j < m.Size; j++) {
        if (m.matrix[i, j] != 0) {
          return true;
        }
      }
    }
    return false;
  }

  public static bool operator false(SquareMatrix m) {
    if (m == null) return true;

    for (int i = 0; i < m.Size; i++) {
      for (int j = 0; j < m.Size; j++) {
        if (m.matrix[i, j] != 0) {
          return false;
        }
      }
    }
    return true;
  }

  public static explicit operator double(SquareMatrix m) {
    if (m == null)
      throw new ArgumentNullException(nameof(m));

    return m.Determinant();
  }

  public static SquareMatrix operator ~(SquareMatrix m) {
    if (m == null)
      throw new ArgumentNullException(nameof(m));

    double det = m.Determinant();

    if (det == 0)
      throw new SingularMatrixException("Нельзя найти обратную матрицу для вырожденной матрицы");

    if (m.Size == 2) {
      SquareMatrix result = new SquareMatrix(2, true);
      result.matrix[0, 0] = m.matrix[1, 1];
      result.matrix[0, 1] = -m.matrix[0, 1];
      result.matrix[1, 0] = -m.matrix[1, 0];
      result.matrix[1, 1] = m.matrix[0, 0];
      return result;
    }
    else if (m.Size == 3) {
      double[,] minors = new double[3, 3];

      minors[0, 0] = m.Minor2x2(m.matrix[1, 1], m.matrix[1, 2], m.matrix[2, 1], m.matrix[2, 2]);
      minors[0, 1] = m.Minor2x2(m.matrix[1, 0], m.matrix[1, 2], m.matrix[2, 0], m.matrix[2, 2]);
      minors[0, 2] = m.Minor2x2(m.matrix[1, 0], m.matrix[1, 1], m.matrix[2, 0], m.matrix[2, 1]);

      minors[1, 0] = m.Minor2x2(m.matrix[0, 1], m.matrix[0, 2], m.matrix[2, 1], m.matrix[2, 2]);
      minors[1, 1] = m.Minor2x2(m.matrix[0, 0], m.matrix[0, 2], m.matrix[2, 0], m.matrix[2, 2]);
      minors[1, 2] = m.Minor2x2(m.matrix[0, 0], m.matrix[0, 1], m.matrix[2, 0], m.matrix[2, 1]);

      minors[2, 0] = m.Minor2x2(m.matrix[0, 1], m.matrix[0, 2], m.matrix[1, 1], m.matrix[1, 2]);
      minors[2, 1] = m.Minor2x2(m.matrix[0, 0], m.matrix[0, 2], m.matrix[1, 0], m.matrix[1, 2]);
      minors[2, 2] = m.Minor2x2(m.matrix[0, 0], m.matrix[0, 1], m.matrix[1, 0], m.matrix[1, 1]);

      SquareMatrix cofactor = new SquareMatrix(3, true);
      cofactor.matrix[0, 0] = (int)minors[0, 0];
      cofactor.matrix[0, 1] = -(int)minors[0, 1];
      cofactor.matrix[0, 2] = (int)minors[0, 2];
      cofactor.matrix[1, 0] = -(int)minors[1, 0];
      cofactor.matrix[1, 1] = (int)minors[1, 1];
      cofactor.matrix[1, 2] = -(int)minors[1, 2];
      cofactor.matrix[2, 0] = (int)minors[2, 0];
      cofactor.matrix[2, 1] = -(int)minors[2, 1];
      cofactor.matrix[2, 2] = (int)minors[2, 2];

      SquareMatrix adjugate = new SquareMatrix(3, true);
      for (int i = 0; i < 3; i++)
        for (int j = 0; j < 3; j++)
          adjugate.matrix[i, j] = cofactor.matrix[j, i];

      SquareMatrix result = new SquareMatrix(3, true);
      for (int i = 0; i < 3; i++)
        for (int j = 0; j < 3; j++)
          result.matrix[i, j] = (int)(adjugate.matrix[i, j] / det);

      return result;
    }
    else {
      throw new NotSupportedMatrixOperationException($"Обратная матрица для размера {m.Size}x{m.Size} не реализована");
    }
  }
}

public class MatrixSizeException : Exception {
  public MatrixSizeException() : base("Ошибка: некорректный размер матрицы") { }
  public MatrixSizeException(string message) : base(message) { }
}

public class SingularMatrixException : Exception {
  public SingularMatrixException() : base("Ошибка: матрица вырожденная (детерминант = 0)") { }
  public SingularMatrixException(string message) : base(message) { }
}

public class NotSupportedMatrixOperationException : Exception {
  public NotSupportedMatrixOperationException() : base("Операция не поддерживается для данного типа матриц") { }
  public NotSupportedMatrixOperationException(string message) : base(message) { }
}

class Program {
  static void Main(string[] args) {
    SquareMatrix a = null;
    SquareMatrix b = null;
    bool exit = false;

    while (!exit) {
      Console.WriteLine("\n========== МАТРИЧНЫЙ КАЛЬКУЛЯТОР ==========");
      Console.WriteLine("1. Создать матрицу A");
      Console.WriteLine("2. Создать матрицу B");
      Console.WriteLine("3. A + B");
      Console.WriteLine("4. A * B");
      Console.WriteLine("5. Сравнить A и B");
      Console.WriteLine("6. Детерминант A");
      Console.WriteLine("7. Детерминант B");
      Console.WriteLine("8. Обратная матрица A");
      Console.WriteLine("9. Обратная матрица B");
      Console.WriteLine("10. Показать копии A и B");
      Console.WriteLine("11. Показать все матрицы");
      Console.WriteLine("0. Выход");
      Console.Write("Выберите действие: ");

      string choice = Console.ReadLine();

      try {
        switch (choice) {
          case "1":
            Console.Write("Введите размер матрицы A: ");
            int n1 = int.Parse(Console.ReadLine());
            a = new SquareMatrix(n1);
            break;

          case "2":
            Console.Write("Введите размер матрицы B: ");
            int n2 = int.Parse(Console.ReadLine());
            b = new SquareMatrix(n2);
            break;

          case "3":
            if (a == null || b == null)
              Console.WriteLine("Сначала создайте обе матрицы!");
            else
              Console.WriteLine("A + B = \n" + (a + b));
            break;

          case "4":
            if (a == null || b == null)
              Console.WriteLine("Сначала создайте обе матрицы!");
            else
              Console.WriteLine("A * B = \n" + (a * b));
            break;

          case "5":
            if (a == null || b == null)
              Console.WriteLine("Сначала создайте обе матрицы!");
            else {
              Console.WriteLine($"A > B: {a > b}");
              Console.WriteLine($"A < B: {a < b}");
              Console.WriteLine($"A == B: {a == b}");
            }
            break;

          case "6":
            if (a == null)
              Console.WriteLine("Сначала создайте матрицу A!");
            else
              Console.WriteLine($"Детерминант A = {(double)a}");
            break;

          case "7":
            if (b == null)
              Console.WriteLine("Сначала создайте матрицу B!");
            else
              Console.WriteLine($"Детерминант B = {(double)b}");
            break;

          case "8":
            if (a == null)
              Console.WriteLine("Сначала создайте матрицу A!");
            else
              Console.WriteLine("Обратная матрица A:\n" + (~a));
            break;

          case "9":
            if (b == null)
              Console.WriteLine("Сначала создайте матрицу B!");
            else
              Console.WriteLine("Обратная матрица B:\n" + (~b));
            break;

          case "10":
            if (a == null || b == null)
              Console.WriteLine("Сначала создайте обе матрицы!");
            else {
              SquareMatrix aCopy = a.Clone();
              SquareMatrix bCopy = b.Clone();
              Console.WriteLine("Копия A:\n" + aCopy);
              Console.WriteLine("Копия B:\n" + bCopy);
            }
            break;

          case "11":
            if (a == null || b == null)
              Console.WriteLine("Сначала создайте обе матрицы!");
            else {
              Console.WriteLine("Матрица A:\n" + a);
              Console.WriteLine("Матрица B:\n" + b);
            }
            break;

          case "0":
            exit = true;
            Console.WriteLine("До свидания!");
            break;

          default:
            Console.WriteLine("Неверный выбор!");
            break;
        }
      }
      catch (Exception ex) {
        Console.WriteLine($"Ошибка: {ex.Message}");
      }
    }
  }
}