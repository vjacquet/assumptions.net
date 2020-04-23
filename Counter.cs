﻿namespace NetCore.Assumptions
{
    class Counter
    {
        private int _count;

        public int Increment() => _count++;
        public int Tally => _count;
    }
}
