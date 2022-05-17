using System;
using System.Collections.Generic;
using System.Text;

namespace Non_Guillotine_Cutting
{
    class Piece
    {
        public int Type { get; set; }

        public int Value { get; set; }

        public int Length { get; set; }

        public int Width { get; set; }

        public bool Z { get; set; }

        public double X { get; set; }

        public double Y { get; set; }

        public Piece(int type, int value, int length, int width, int stockRectangleLength, int stockRectangleWidth)
        {
            Type = type;
            Value = value;
            Length = length;
            Width = width;

            Z = true;

            GenerateCoordinates(stockRectangleLength, stockRectangleWidth);
        }

        public Piece(Piece p)
        {
            Type = p.Type;
            Value = p.Value;
            Length = p.Length;
            Width = p.Width;
            Z = p.Z;
            X = p.X;
            Y = p.Y;
        }

        public Piece (bool z, double x, double y)
        {
            Z = z;
            X = x;
            Y = y;
        }

        private void GenerateCoordinates(int stockRectangleLength, int stockRectangleWidth)
        {
            Random random = new Random();

            int lowerBoundLength = (int)Math.Truncate(((double)Length / 2 + 0.5));

            int upperBoundLength = stockRectangleLength - lowerBoundLength;

            X = random.Next(lowerBoundLength, upperBoundLength + 1);

            int lowerBoundWidth = (int)Math.Truncate(((double)Width / 2 + 0.5));

            int upperBoundWidth = stockRectangleWidth - lowerBoundWidth;

            Y = random.Next(lowerBoundWidth, upperBoundWidth + 1);
        }

        public bool IsEqualTo(Piece p)
        {
            if (Z != p.Z)
                return false;

            if (X != p.X)
                return false;

            if (Y != p.Y)
                return false;

            return true;
        }
    }
}
