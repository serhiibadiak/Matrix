using System;

namespace MatrixLibrary
{    
    public class MatrixException : Exception
    {
        public MatrixException(string message)
            :base(message)
        { }
        public MatrixException()
            :base()
        { }
    }
    public class Matrix : ICloneable
    {
        public int Rows
        {
            set
            {
                rows = value;
            }
            get => rows;
        }
        private int rows { set; get; }
        public int Columns
        {
            set
            {
                columns = value;                
            }
            get => columns;
        }
        private int columns;
        public double[,] Array
        {
            set
            {
                array = value;
            }
            get => array;
        }
        private double[,] array;
        
        public Matrix(int rows, int columns)
        {
            if (rows < 1 || columns < 1) throw new ArgumentOutOfRangeException();
            else
            {
                this.Rows = rows;
                this.Columns = columns;
                Array = new double[rows, columns];
            }
        }
        /// <exception cref="ArgumentNullException"></exception>
        public Matrix(double[,] array)
        {
            if (array is null) throw new ArgumentNullException();
            else
            {
                this.Array = array;
                this.Rows = Array.GetUpperBound(0) + 1;
                if (Rows < 1) columns = 0;
                else this.Columns = Array.Length / this.Rows;
            }
        }
        public double this[int row, int column]
        {
            get
            {
                if (row >= Rows || row < 0) throw new ArgumentException();
                if (column >= Columns || Columns < 0) throw new ArgumentException();
                return Array[row, column];
            }
            set
            {
                if (row < 0) throw new ArgumentException();
                if (column < 0) throw new ArgumentException();
                Array[row, column] = value;
            }
        }
        public object Clone() //=> new Matrix(Array.Clone() as double[,]);
        {
            double[,] arr = new double[Rows, Columns];
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    arr[i, j] = Array[i, j];
                }
            }
            Matrix m = new Matrix(arr);
            m.Array = arr;
            m.Rows = arr.GetUpperBound(0) + 1;
            m.Columns = arr.Length / m.Rows;
            return m;
        }
        public static Matrix operator +(Matrix matrix1, Matrix matrix2)
        {
            if (matrix1 is null)
            {
                throw new ArgumentNullException(nameof(matrix1), "is null");
            }
            else if (matrix2 is null)
            {
                throw new ArgumentNullException(nameof(matrix2), "is null");
            }
            else if(matrix1.Columns != matrix2.Columns || matrix1.Rows != matrix2.Rows)
            {
                throw new MatrixException();
            }
            else
            {
                return matrix1.Add(matrix2);
            }
        }
        public static Matrix operator -(Matrix matrix1, Matrix matrix2)
        {
            if (matrix1 is null)
            {
                throw new ArgumentNullException(nameof(matrix1), "is null");
            }
            else if (matrix2 is null)
            {
                throw new ArgumentNullException(nameof(matrix2), "is null");
            }
            else if (matrix1.Columns != matrix2.Columns || matrix1.Rows != matrix2.Rows)
            {
                throw new MatrixException();
            }
            else
            {
                return matrix1.Subtract(matrix2);
            }
        }
        public static Matrix operator *(Matrix matrix1, Matrix matrix2) 
        {
            if (matrix1 is null)
            {
                throw new ArgumentNullException(nameof(matrix1), "is null");
            }
            else if (matrix2 is null)
            {
                throw new ArgumentNullException(nameof(matrix2), "is null");
            }
            else if (matrix1.Columns != matrix2.Rows)
            {
                throw new MatrixException();
            }
            else
            {
               return matrix1.Multiply(matrix2);
            }
        }
        public Matrix Add(Matrix matrix)
        {
            if (matrix is null) throw new ArgumentNullException(nameof(matrix), "matrix to add is null");
            if ((matrix.Columns <= 0 || matrix.Columns != this.Columns) || (this.Rows <= 0 || matrix.Rows != this.Rows)) throw new MatrixException();
            else
            {
                Matrix result = new Matrix(this.Rows, this.Columns);
                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Columns; j++)
                    {
                        result[i, j] = this[i, j] + matrix[i, j];
                    }
                }
                return result;
            }
        }
        public Matrix Subtract(Matrix matrix)
        {
            if (matrix is null) throw new ArgumentNullException(nameof(matrix), "matrix to add is null");
            if ((matrix.Columns <= 0 || matrix.Columns != this.Columns) || (this.Rows <= 0 || matrix.Rows != this.Rows)) throw new MatrixException();
            else
            {
                Matrix result = new Matrix(this.Rows, this.Columns);
                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Columns; j++)
                    {
                        result[i, j] = this[i, j] - matrix[i, j];
                    }
                }
                return result;
            }
        }
        public Matrix Multiply(Matrix matrix)
        {
            if (matrix is null) throw new ArgumentNullException(nameof(matrix), "matrix to multiply is null");
            else if(this.Rows != matrix.Columns) throw new MatrixException();
            else if (matrix.Array.Length == 0) throw new MatrixException();
            else if (matrix.Columns <= 0) throw new MatrixException();
            else if (matrix.Rows <= 0) throw new MatrixException();
             
            else
            {
                Matrix result = new Matrix(this.Rows, matrix.Columns);
                for (int i = 0; i < this.Rows; i++)
                {
                    for (int j = 0; j < matrix.Columns; j++)
                    {
                        for (int k = 0; k < this.Columns; k++)
                        {
                            result[i, j] += (this[i, k] * matrix[k, j]);
                        }
                    }
                }
                return result;
            }
        }
        public override bool Equals(object obj)
        {
            if(!(obj is Matrix) || obj is null)
            {
                return false;
            }
            var matrix = obj as Matrix;
            if(matrix.Rows != Rows && matrix.Columns != Columns)
            {
                return false;
            }
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    try
                    {
                        if (matrix[i, j] != this[i, j]) return false;
                    }
                    catch
                    {
                        return false;
                    }
                }

            }
            return true;
        }

        public override int GetHashCode() => base.GetHashCode();
    }
}
