using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleLinkedListInAList
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    class ListInAList<T>
    {
        Dictionary<int, Pos> pos;


        class SimpleSingleLinkedList<E>
        {
            private Node first;
            private int count;

            public SimpleSingleLinkedList()
            {
                count = 0;
            }

            public void Add(E item)
            {
                if (first == null)
                {
                    first = new Node(item);
                }
                else
                {
                    Node last = first;
                    while (last.Next != null)
                    {
                        last = last.Next;
                    }
                    last.Next = new Node(item);
                }
                count++;
            }

            public E GetAt(int index)
            {
                if (first == null || index >= count)
                    throw new IndexOutOfRangeException($"index is out of range: {index} with a count of {count} items in the collection");
                Node last = first;
                for (int i = 0; i < index; i++)
                {
                    last = last.Next;
                }
                return last.Element;
            }

            public int Count { get { return count; } private set { count = value; } }

            class Node
            {
                private E element;
                private Node next;

                public Node(E e)
                {
                    element = e;
                }

                public Node Next { get { return next; } set { next = value; } }

                public E Element { get { return element; } set { element = value; } }
            }
        }

        class Pos
        {
            private int posOuter;
            private int posInner;

            public Pos()
            {

            }

            public Pos(int posOuter, int posInner)
            {
                this.posOuter = posOuter;
                this.posInner = posInner;
            }

            public int PosInner
            {
                get { return posInner; }
                set { posInner = value; }
            }

            public int PosOuter
            {
                get { return posOuter; }
                set { posOuter = value; }
            }
        }
    }
}
