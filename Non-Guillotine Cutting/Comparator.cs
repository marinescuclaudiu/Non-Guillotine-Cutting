using System;
using System.Collections.Generic;
using System.Text;

namespace Non_Guillotine_Cutting
{
    class Comparator : IComparer<Piece>
    {
        public int Compare(Piece p1, Piece p2)
        {
            if (p1.Type == p2.Type)
            {
                if (p1.Z == p2.Z)
                {
                    if (p1.X == p2.X)
                    {
                        if (p1.Y <= p2.Y)
                            return -1;

                        return 1;
                    }

                    if (p1.X < p2.X)
                        return -1;

                    return 1;
                }

                if (p1.Z == true && p2.Z == false)
                    return -1;

                return 1;

            }

            if (p1.Type < p2.Type)
                return -1;

            return 1;
        }
    }
}
