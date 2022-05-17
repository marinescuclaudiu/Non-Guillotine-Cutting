using System;
using System.Collections.Generic;
using System.Text;

namespace Non_Guillotine_Cutting
{
    class Individual
    {
        public List<Piece> Pieces { get; set; }

        public int StockRectangleLength { get; set; }

        public int StockRectangleWidth { get; set; }

        public double Fitness { get; set; }

        public double Unfitness { get; set; }

        public Individual(List<Tuple<int, int, int, int, int>> piecesInfo, int stockRectangleLength, int stockRectangleWidth)
        {
            Pieces = new List<Piece>();

            StockRectangleLength = stockRectangleLength;
            StockRectangleWidth = stockRectangleWidth;

            for (int index = 0; index < piecesInfo.Count; index++)
            {
                int noOfPieces = piecesInfo[index].Item1;
                int type = piecesInfo[index].Item2;
                int value = piecesInfo[index].Item3;
                int length = piecesInfo[index].Item4;
                int width = piecesInfo[index].Item5;
                GeneratePieces(noOfPieces, type, value, length, width);
            }

            CalculateFitness();
            CalculateUnfitness();

        }

        public Individual(Individual parent1, Individual parent2)
        {
            Pieces = new List<Piece>();
            StockRectangleLength = parent1.StockRectangleLength;
            StockRectangleWidth = parent1.StockRectangleWidth;

            Random random = new Random();

            for (int index = 0; index < parent1.Pieces.Count; index++)
            {
                double probability = random.NextDouble();

                if (probability <= 0.5)
                {
                    Piece pieceToAdd = new Piece(parent1.Pieces[index]);
                    Pieces.Add(pieceToAdd);
                }
                else
                {
                    Piece pieceToAdd = new Piece(parent2.Pieces[index]);
                    Pieces.Add(pieceToAdd);
                }
            }

            CalculateFitness();
            CalculateUnfitness();
        }

        public Individual()
        {
            Pieces = new List<Piece>();
            Fitness = 0;
        }

        private void GeneratePieces(int noOfPieces, int type, int value, int length, int width)
        {
            for (int indexPiece = 0; indexPiece < noOfPieces; indexPiece++)
            {
                Piece piece = new Piece(type, value, length, width, StockRectangleLength, StockRectangleWidth);
                Pieces.Add(piece);
            }
        }

        private void CalculateFitness()
        {
            Fitness = 0;

            foreach (Piece piece in Pieces)
            {
                if (piece.Z == true)
                {
                    Fitness += piece.Value;
                }
            }
        }

        private void CalculateUnfitness()
        {
            Unfitness = 0;

            for (int index = 0; index < Pieces.Count - 1; index++)
            {
                if (Pieces[index].Z == true)
                {
                    for (int index2 = index + 1; index2 < Pieces.Count; index2++)
                    {
                        if (Pieces[index2].Z == true)
                        {
                            double firstValue = Math.Abs((double)(Pieces[index].X - Pieces[index2].X));
                            firstValue -= ((double)(Pieces[index].Length + Pieces[index2].Length) / 2);

                            double secondValue = Math.Abs((double)(Pieces[index].Y - Pieces[index2].Y));
                            secondValue -= ((double)(Pieces[index].Width + Pieces[index2].Width) / 2);

                            double maxValue = Math.Max(firstValue, secondValue);

                            Unfitness += Math.Max(0, -maxValue);
                        }
                    }
                }
            }
        }

        public void SortPieces()
        {
            Comparator comparator = new Comparator();
            Pieces.Sort(comparator);
        }

        public void Mutation()
        {
            Random random = new Random();
            int indexPiece = random.Next(0, Pieces.Count);

            Pieces[indexPiece].Z = false;

            CalculateFitness();
            CalculateUnfitness();
        }

        public void Improve()
        {
            if (Unfitness == 0)
                return;

            double oldUnfitness;

            do
            {
                oldUnfitness = Unfitness;

                for (int index = 0; index < Pieces.Count; index++)
                {
                    if (Pieces[index].Z == true)
                    {
                        int pieceLength = Pieces[index].Length;
                        int pieceWidth = Pieces[index].Width;

                        //move left
                        if ((double)pieceLength / 2 < Pieces[index].X)
                        {
                            double unfitnessBeforeMove = Unfitness;

                            Pieces[index].X -= 0.5;

                            CalculateUnfitness();

                            //undo move
                            if (Unfitness > unfitnessBeforeMove)
                            {
                                Pieces[index].X += 0.5;
                                Unfitness = unfitnessBeforeMove;
                            }
                        }

                        //move down
                        if ((double)pieceWidth / 2 < Pieces[index].Y)
                        {
                            double unfitnessBeforeMove = Unfitness;

                            Pieces[index].Y -= 0.5;

                            CalculateUnfitness();

                            //undo move
                            if (Unfitness > unfitnessBeforeMove)
                            {
                                Pieces[index].Y += 0.5;
                                Unfitness = unfitnessBeforeMove;
                            }
                        }

                        //move right
                        if(Pieces[index].X + (double)Pieces[index].Length / 2 < StockRectangleLength)
                        {
                            double unfitnessBeforeMove = Unfitness;

                            Pieces[index].X += 0.5;

                            CalculateUnfitness();

                            //undo move
                            if (Unfitness >= unfitnessBeforeMove)
                            {
                                Pieces[index].X -= 0.5;
                                Unfitness = unfitnessBeforeMove;
                            }
                        }

                        //move up
                        if (Pieces[index].Y + (double)Pieces[index].Width / 2 < StockRectangleWidth)
                        {
                            double unfitnessBeforeMove = Unfitness;

                            Pieces[index].Y += 0.5;

                            CalculateUnfitness();

                            //undo move
                            if (Unfitness >= unfitnessBeforeMove)
                            {
                                Pieces[index].Y -= 0.5;
                                Unfitness = unfitnessBeforeMove;
                            }
                        }
                    }
                }

            } while (oldUnfitness != Unfitness);

        }

        public bool IsEqualTo(Individual individual)
        {
            for (int index = 0; index < Pieces.Count; index++)
            {
                if(Pieces[index].IsEqualTo(individual.Pieces[index]) == false)
                    return false;
            }

            return true;
        }

        public void PrintInfo()
        {
            Console.WriteLine("Fitness: " + Fitness);
            foreach (Piece piece in Pieces)
            {
                Console.Write("Type: " + piece.Type);
                Console.Write(" Value: " + piece.Value);
                Console.Write(" Z: " + piece.Z);
                Console.Write(" Length: " + piece.Length);
                Console.Write(" Width: " + piece.Width);
                Console.Write(" X: " + piece.X);
                Console.WriteLine(" Y: " + piece.Y);
            }
        }

        public void Copy(Individual i)
        {
            Pieces.Clear();

            StockRectangleLength = i.StockRectangleLength;
            StockRectangleWidth = i.StockRectangleWidth;

            for (int index = 0; index < i.Pieces.Count; index++)
            {
                Piece pieceToAdd = new Piece(i.Pieces[index]);
                Pieces.Add(pieceToAdd);
            }

            Fitness = i.Fitness;
            Unfitness = i.Unfitness;
        }
    }
}