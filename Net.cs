using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroWFA
{
    class Net
    {
        public int[,] input,muls, weights;
        public int sum;
        public int limit, sx, sy;
        public bool good;

        public Net(int sizex, int sizey)
        {
            weights = new int[sizex, sizey];
            muls = new int[sizex, sizey];
            input = new int[sizex, sizey];

            limit = 9;
            sx = sizex;
            sy = sizey;

            good = true;

            for (int x = 0; x < sizex; x++)
                for (int y = 0; y < sizey; y++)
                    weights[x,y] = 1;
        }

        public void setInput(int[,] inp)
        {
            input = inp;
        }

        public void MulAndSum()
        {
            sum = 0;
            for (int x = 0; x < sx; x++)
                for (int y = 0; y < sy; y++)
                {
                    sum += weights[x, y] * input[x, y];
                    muls[x, y] = weights[x, y] * input[x, y];
                }
        }

        public bool Compare()
        {
            good = (sum >= limit);
            return good;
        }

        public void Fail()
        {
            for (int x = 0; x < sx; x++)
                for (int y = 0; y < sy; y++)
                    if (good) weights[x, y] -= input[x, y];
                    else weights[x, y] += input[x, y];
        }
    }
}
